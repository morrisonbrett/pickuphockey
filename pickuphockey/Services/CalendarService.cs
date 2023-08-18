using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Ical.Net.Serialization;
using Microsoft.Ajax.Utilities;
using pickuphockey.Models;

namespace pickuphockey.Services
{
    public class CalendarService
    {
        public static async Task RebuildCalendar(IOrderedEnumerable<Session> sessions)
        {
            // Get the base root URL
            var baseUrl = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath.TrimEnd('/') + "/";

            // Site settings
            var siteTitle = System.Configuration.ConfigurationManager.AppSettings["SiteTitle"];
            var pickupLocation = System.Configuration.ConfigurationManager.AppSettings["PickupLocation"];
            var tzid = "America/Los_Angeles";

            // Base Cal object, events are added to this object
            var calendar = new Calendar();
            calendar.AddProperty("DESCRIPTION", siteTitle + " Calendar");
            calendar.AddProperty("X-WR-CALNAME", siteTitle + " Calendar");

            var recentSessions = sessions.Where(rs => rs.SessionDate.Year >= 2023 && !(!string.IsNullOrEmpty(rs.Note) && rs.Note.Contains("CANCELLED")));

            recentSessions.ForEach(s =>
            {
                // Build the session URL
                var url = baseUrl + "/Session/" + s.SessionId.ToString();
                var uri = new Uri(url);

                var iCalEvent = new CalendarEvent
                {
                    Summary = siteTitle,
                    Description = string.Format("{0}{1}{2}", url, s.Note != null && s.Note.Length > 0 ? Environment.NewLine : "", s.Note),
                    DtStart = new CalDateTime(s.SessionDate, tzid),
                    DtEnd = new CalDateTime(s.SessionDate.AddHours(1), tzid),
                    Url = uri,
                    Location = pickupLocation
                };

                calendar.Events.Add(iCalEvent);
            });


            var iCalSerializer = new CalendarSerializer();
            string result = iCalSerializer.SerializeToString(calendar);

            var filePath = AppDomain.CurrentDomain.BaseDirectory;
            var fileName = Path.Combine(filePath, "hockeypickup.ics");

            var fileWriter = new StreamWriter(fileName);

            await fileWriter.WriteLineAsync(result);

            fileWriter.Dispose();
        }
    }
}
