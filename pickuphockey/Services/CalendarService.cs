﻿using System;
using System.IO;
using System.Linq;
using System.Web;
using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Ical.Net.Serialization;
using pickuphockey.Models;
using WebGrease.Css.Extensions;

namespace pickuphockey.Services
{
    public static class CalendarService
    {
        public static void RebuildCalendar(IOrderedEnumerable<Session> sessions)
        {
            // Get the base root URL
            var baseUrl = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath.TrimEnd('/');

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
                var url = baseUrl + "/Sessions/Details/" + s.SessionId.ToString();
                var uri = new Uri(url);

                var iCalEvent = new CalendarEvent
                {
                    Summary = siteTitle,
                    Description = string.Format("{0}{1}", url, !string.IsNullOrEmpty(s.Note) ? Environment.NewLine + Environment.NewLine + s.Note : ""),
                    DtStart = new CalDateTime(s.SessionDate, tzid),
                    DtEnd = new CalDateTime(s.SessionDate.AddHours(1), tzid),
                    Url = uri,
                    Location = pickupLocation
                };

                calendar.Events.Add(iCalEvent);
            });

            var iCalSerializer = new CalendarSerializer();
            var result = iCalSerializer.SerializeToString(calendar);

            var filePath = AppDomain.CurrentDomain.BaseDirectory;
            var fileName = Path.Combine(filePath, "hockey_pickup.ics");

            var fileWriter = new StreamWriter(fileName);

            fileWriter.WriteLine(result);

            fileWriter.Dispose();
        }
    }
}
