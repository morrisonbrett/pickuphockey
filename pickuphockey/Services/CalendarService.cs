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
            var zone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");

            // Base Cal object, events are added to this object
            var calendar = new Calendar();
            calendar.Name = siteTitle + " Calendar";

            sessions.ForEach(s =>
            {
                // Build the session URL
                var url = baseUrl + "/Session/" + s.SessionId.ToString();
                var uri = new Uri(url);

                var iCalEvent = new CalendarEvent
                {
                    Summary = siteTitle,
                    Description = string.Format("{0}{1}{2}", url, s.Note != null && s.Note.Length > 0 ? Environment.NewLine : "", s.Note),
                    DtStart = new CalDateTime(s.SessionDate, zone.StandardName),
                    DtEnd = new CalDateTime(s.SessionDate.AddHours(1), zone.StandardName),
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
