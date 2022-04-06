using System.Globalization;

namespace Kasi_Server.Utils.Extensions
{
    public static partial class DateTimeExtensions
    {
        public static DateTime FirstDayOfYear(this DateTime dt) => dt.SetDate(dt.Year, 1, 1);

        public static DateTime FirstDayOfQuarter(this DateTime dt)
        {
            var currentQuarter = (dt.Month - 1) / 3 + 1;
            var firstDay = new DateTime(dt.Year, 3 * currentQuarter - 2, 1);
            return dt.SetDate(firstDay.Year, firstDay.Month, firstDay.Day);
        }

        public static DateTime FirstDayOfMonth(this DateTime dt) => dt.SetDay(1);

        public static DateTime FirstDayOfWeek(this DateTime dt)
        {
            var currentCulture = CultureInfo.CurrentCulture;
            var firstDayOfWeek = currentCulture.DateTimeFormat.FirstDayOfWeek;
            var offset = dt.DayOfWeek - firstDayOfWeek < 0 ? 7 : 0;
            var numberOfDaysSinceBeginningOfTheWeek = dt.DayOfWeek + offset - firstDayOfWeek;
            return dt.AddDays(-numberOfDaysSinceBeginningOfTheWeek);
        }

        public static DateTime LastDayOfYear(this DateTime dt) => dt.SetDate(dt.Year, 12, 31);

        public static DateTime LastDayOfQuarter(this DateTime dt)
        {
            var currentQuarter = (dt.Month - 1) / 3 + 1;
            var firstDay = new DateTime(dt.Year, 3 * currentQuarter - 2, 1);
            return firstDay.SetMonth(firstDay.Month + 2).LastDayOfMonth();
        }

        public static DateTime LastDayOfMonth(this DateTime dt) => dt.SetDay(DateTime.DaysInMonth(dt.Year, dt.Month));

        public static DateTime LastDayOfWeek(this DateTime dt) => dt.FirstDayOfWeek().AddDays(6);
    }
}