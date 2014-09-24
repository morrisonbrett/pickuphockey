using System;
using System.ComponentModel.DataAnnotations;

namespace pickuphockey.Attributes
{
    public class FutureDateValidatorAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var pstZone = TimeZoneInfo.FindSystemTimeZoneById(System.Configuration.ConfigurationManager.AppSettings["DisplayTimeZone"]);

            return value != null && Convert.ToDateTime(value).Date >= TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, pstZone).Date;
        }
    }
}
