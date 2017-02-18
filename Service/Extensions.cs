using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public static class Extensions
    {
        public static string ToSQLite(this DateTime datetime)
        {
            string dateTimeFormat = "{0}-{1}-{2} {3}:{4}:{5}.{6}";
            return string.Format(dateTimeFormat,
            datetime.Year.ToString().PadLeft(4, '0'),
            datetime.Month.ToString().PadLeft(2, '0'),
            datetime.Day.ToString().PadLeft(2, '0'),
            datetime.Hour.ToString().PadLeft(2, '0'),
            datetime.Minute.ToString().PadLeft(2, '0'),
            datetime.Second.ToString().PadLeft(2, '0'),
            datetime.Millisecond.ToString().PadLeft(4, '0'));
        }
        public static DateTime FromSQLite(this string datetime)
        {
            string[] parts = datetime.Split(' ');
            parts = parts[0].Split('-');
            int year = int.Parse(parts[0]);
            int month = int.Parse(parts[1]);
            int day = int.Parse(parts[2]);
            return new DateTime(year, month, day);
        }
    }
}
