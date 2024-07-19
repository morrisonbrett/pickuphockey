using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.Serialization;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net;
using System.Text;
using File = System.IO.File;
#pragma warning disable CS8601 // Possible null reference assignment.
#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CA2254 // Template should be a static expression

namespace pickuphockey.Functions
{
    public class CheckCalendar(ILoggerFactory loggerFactory)
    {
        private readonly string CalendarUrl = Environment.GetEnvironmentVariable("CalendarUrl");
        private readonly string SendGridApiKey = Environment.GetEnvironmentVariable("SendGridApiKey");
        private readonly string SendGridNotificationAddress = Environment.GetEnvironmentVariable("SendGridNotificationAddress");
        private readonly string SendGridFromAddress = Environment.GetEnvironmentVariable("SendGridFromAddress");

        private readonly ILogger _logger = loggerFactory.CreateLogger<CheckCalendar>();

        [Function("CheckCalendar")]
        public async Task RunAsync([TimerTrigger("0 0 5,17 * * *")] TimerInfo timerInfo)
        {
            _logger.LogInformation($"HTTP timer function executed.");

            await ExecuteAsync();
        }

        [Function("CheckCalendarHttp")]
        public async Task<HttpResponseData> RunHttpAsync([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("HTTP trigger function executed.");

            await ExecuteAsync();

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            await response.WriteStringAsync("Function executed successfully.");
            return response;
        }

        private static async Task<string?> GetICSFromFileSystem()
        {
            var icsFilename = "tspc.ics";

            return File.Exists(icsFilename) ? await File.ReadAllTextAsync(icsFilename) : null;
        }

        private async Task<string> GetICSFromHttp()
        {
            using HttpClient client = new();
            var icsContent = await client.GetStringAsync(CalendarUrl);

            return icsContent;
        }

        private async Task ExecuteAsync()
        {
            _logger.LogInformation($"Function executed at: {DateTime.Now}");

            try
            {
                // Determine storage file path
                var storageFilePath = GetStorageFilePath();

                _logger.LogInformation($"Storage file path: {storageFilePath}");

                // Fetch the .ics file
                var icsContent = await GetICSFromFileSystem() ?? await GetICSFromHttp();

                // Parse the .ics file
                var calendar = Calendar.Load(icsContent);
                var futureEvents = calendar.Events
                    .Where(e => e.Start.AsUtc > DateTime.UtcNow && e.Summary.Contains("John Bryan"))
                    .OrderBy(e => e.Start.AsUtc)
                    .Select(e => new CalendarEvent
                    {
                        Start = e.Start,
                        End = e.End,
                        Summary = e.Summary,
                        Location = e.Location,
                        Description = e.Description
                    })
                    .ToList();

                // Load previous state
                var previousState = File.Exists(storageFilePath) ? await File.ReadAllTextAsync(storageFilePath) : string.Empty;

                _logger.LogInformation("Previous state loaded successfully.");

                // Serialize current state
                var newCalendar = new Calendar();
                foreach (var calendarEvent in futureEvents)
                {
                    newCalendar.Events.Add(calendarEvent);
                }
                var serializer = new CalendarSerializer();
                var currentState = serializer.SerializeToString(newCalendar);

                // Compare states
                if (previousState != currentState)
                {
                    // Extract changes
                    var changes = ExtractCalendarChanges(previousState, currentState);

                    // Send notification if there are changes
                    if (!string.IsNullOrWhiteSpace(changes))
                    {
                        await SendNotificationEmail(changes);
                        // Update stored state only if email sent successfully
                        await File.WriteAllTextAsync(storageFilePath, currentState);
                    }
                    else
                    {
                        _logger.LogInformation("No changes detected since last update.");
                    }
                }
                else
                {
                    _logger.LogInformation("No changes detected since last update.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred: {ex.Message}");
            }
        }

        private async Task SendNotificationEmail(string changes)
        {
            try
            {
                var client = new SendGridClient(SendGridApiKey);
                var from = new EmailAddress(SendGridFromAddress, "Pickup Hockey - Calendar Monitor");
                var to = new EmailAddress(SendGridNotificationAddress);
                var subject = "Pickup Hockey - Calendar Update Detected";

                // Format the email body with HTML for better readability
                var htmlContent = FormatEmailBody(changes);

                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent: null, htmlContent);

                _logger.LogInformation("Sending email...");
                var response = await client.SendEmailAsync(msg);

                _logger.LogInformation($"Email send status: {response.StatusCode}");

                if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Accepted)
                {
                    var responseBody = await response.Body.ReadAsStringAsync();
                    _logger.LogError($"Failed to send email: {responseBody}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occurred while sending email: {ex.Message}");
            }
        }

        private static string FormatEmailBody(string changes)
        {
            // Format changes into HTML for better readability in the email
            var htmlBuilder = new StringBuilder();
            htmlBuilder.Append("<html><body>");
            htmlBuilder.Append("<h2>Pickup Hockey - Calendar Update Details:</h2>");
            htmlBuilder.Append("<p>");
            htmlBuilder.Append(changes.Replace("\n", "<br>"));
            htmlBuilder.Append("</p>");
            htmlBuilder.Append("</body></html>");

            return htmlBuilder.ToString();
        }

        private static string ExtractCalendarChanges(string previousState, string currentState)
        {
            // Compare previous and current states to extract changes
            if (string.IsNullOrEmpty(previousState))
                return currentState;

            // Split the states into lines for comparison
            var previousLines = previousState.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            var currentLines = currentState.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            // Compare line by line, ignoring UID and DTSTAMP
            var changes = new StringBuilder();
            for (var i = 0; i < Math.Min(previousLines.Length, currentLines.Length); i++)
            {
                // Skip comparing lines that start with UID or DTSTAMP
                if (!previousLines[i].StartsWith("UID:") && !previousLines[i].StartsWith("DTSTAMP:") &&
                    !currentLines[i].StartsWith("UID:") && !currentLines[i].StartsWith("DTSTAMP:"))
                {
                    if (previousLines[i] != currentLines[i])
                    {
                        changes.AppendLine($"Change detected:");
                        changes.AppendLine($"Previous: {previousLines[i]}");
                        changes.AppendLine($"Current: {currentLines[i]}");
                    }
                }
            }

            return changes.ToString();
        }

        private string GetStorageFilePath()
        {
            var localPath = "calendar_state.txt"; // Local development path
            var azureHomePath = Environment.GetEnvironmentVariable("HOME");

            // Determine the environment
            var isAzureEnvironment = !string.IsNullOrEmpty(azureHomePath);

            if (isAzureEnvironment)
            {
                var dataDirectory = Path.Combine(azureHomePath, "site", "data");
                var storageFilePath = Path.Combine(dataDirectory, "calendar_state.txt");

                // Create the data directory if it doesn't exist
                if (!Directory.Exists(dataDirectory))
                {
                    try
                    {
                        Directory.CreateDirectory(dataDirectory);
                        _logger.LogInformation($"Created directory: {dataDirectory}");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Error creating directory: {ex.Message}");
                        // If we can't create the directory, fall back to the root path
                        storageFilePath = Path.Combine(azureHomePath, "site", "calendar_state.txt");
                    }
                }

                return storageFilePath;
            }
            else
            {
                return localPath;
            }
        }
    }
}
#pragma warning restore CA2254 // Template should be a static expression
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8601 // Possible null reference assignment.
