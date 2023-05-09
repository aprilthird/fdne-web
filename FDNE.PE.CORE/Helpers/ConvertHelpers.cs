using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDNE.PE.CORE.Helpers
{
    public static class ConvertHelpers
    {
        public static DateTime ToUtcDateTime(this string dateString)
        {
            return DateTime.ParseExact(dateString, ConstantHelpers.FORMATS.DATE, System.Globalization.CultureInfo.InvariantCulture).ToUniversalTime();
        }

        public static DateTime ToDefaultTimeZone(this DateTime dateTime)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(dateTime, TimeZoneInfo.FindSystemTimeZoneById(ConstantHelpers.TIMEZONE_ID));
        }

        public static string ToDateFormat(this DateTime dateTime)
        {
            return dateTime.ToString(ConstantHelpers.FORMATS.DATE, CultureInfo.InvariantCulture);
        }

        public static string ToDateTimeFormat(this DateTime dateTime)
        {
            return dateTime.ToString(ConstantHelpers.FORMATS.DATETIME, CultureInfo.InvariantCulture);
        }

        public static string ToTimeFormat(this DateTime dateTime)
        {
            return dateTime.ToString(ConstantHelpers.FORMATS.DATE, CultureInfo.InvariantCulture);
        }

        public static string ToLocalDateFormat(this DateTime dateTime)
        {
            return dateTime.ToDefaultTimeZone().ToString(ConstantHelpers.FORMATS.DATE, CultureInfo.InvariantCulture);
        }

        public static string ToLocalDateTimeFormat(this DateTime dateTime)
        {
            return dateTime.ToDefaultTimeZone().ToString(ConstantHelpers.FORMATS.DATETIME, CultureInfo.InvariantCulture);
        }

        public static string ToLocalTimeFormat(this DateTime dateTime)
        {
            return dateTime.ToDefaultTimeZone().ToString(ConstantHelpers.FORMATS.TIME, CultureInfo.InvariantCulture);
        }
    }
}
