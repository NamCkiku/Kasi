using Kasi_Server.Utils.Extensions;

namespace Kasi_Server.Utils.Date
{
    public static class DateTimeFactory
    {
        public static DateTime Now() => DateTime.Now;

        public static DateTime UtcNow() => DateTime.UtcNow;

        public static DateTime Create(int year, int month, int day) => new DateTime(year, month, day);

        public static DateTime Create(int year, int month, int day, int hour, int minute, int second) =>
            new DateTime(year, month, day, hour, minute, second);

        public static DateTime Create(int year, int month, int day, int hour, int minute, int second, int millisecond) =>
            new DateTime(year, month, day, hour, minute, second, millisecond);

        public static DateTime Create(int year, int month, int day, int hour, int minute, int second, int millisecond,
            DateTimeKind kind) =>
            new DateTime(year, month, day, hour, minute, second, millisecond, kind);

        public static DateTime OffsetByWeek(int year, int month, int weekAtMonth, int dayOfWeek)
        {
            var fd = Create(year, month, 1);
            var fDayOfWeek = fd.DayOfWeek.ToInt();
            var restDayOfFdInWeek = 7 - fDayOfWeek + 1; 

            var targetDay = fDayOfWeek > dayOfWeek
                ? (weekAtMonth - 1) * 7 + dayOfWeek + restDayOfFdInWeek
                : (weekAtMonth - 2) * 7 + dayOfWeek + restDayOfFdInWeek;
            return Create(year, month, targetDay);
        }

        public static DateTime OffsetByWeek(int year, int month, int weekAtMonth, DayOfWeek dayOfWeek) =>
            OffsetByWeek(year, month, weekAtMonth, dayOfWeek.ToInt());

        public static DateTime FindLastDay(int year, int month, DayOfWeek dayOfWeek)
        {
            var resultedDay = FindDay(year, month, dayOfWeek, 5);
            if (resultedDay == DateTime.MinValue)
                resultedDay = FindDay(year, month, dayOfWeek, 4);
            return resultedDay;
        }

        public static DateTime FindNextDay(int year, int month, int day, DayOfWeek dayOfWeek)
        {
            var calculationDay = Create(year, month, day);
            return FindNextDay(calculationDay, dayOfWeek);
        }

        public static DateTime FindNextDay(DateTime dt, DayOfWeek dayOfWeek)
        {
            var daysNeeded = (int)dayOfWeek - (int)dt.DayOfWeek;
            return (int)dayOfWeek >= (int)dt.DayOfWeek
                ? dt.AddDays(daysNeeded)
                : dt.AddDays(daysNeeded + 7);
        }

        public static DateTime FindDayBefore(int year, int month, int day, DayOfWeek dayOfWeek)
        {
            var calculationDay = Create(year, month, day);
            return FindDayBefore(calculationDay, dayOfWeek);
        }

        public static DateTime FindDayBefore(DateTime dt, DayOfWeek dayOfWeek)
        {
            var daysSubtract = (int)dayOfWeek - (int)dt.DayOfWeek;
            return (int)dayOfWeek < (int)dt.DayOfWeek
                ? dt.AddDays(daysSubtract)
                : dt.AddDays(daysSubtract - 7);
        }

        public static DateTime FindDay(int year, int month, DayOfWeek dayOfWeek, int occurrence)
        {
            if (occurrence == 0 || occurrence > 5)
                throw new IndexOutOfRangeException(nameof(occurrence));
            var firstDayOfMonth = Create(year, month, 1);
            var daysNeeded = (int)dayOfWeek - (int)firstDayOfMonth.DayOfWeek;
            if (daysNeeded < 0)
                daysNeeded += 7;
            var resultedDay = daysNeeded + 1 + 7 * (occurrence - 1);
            if (resultedDay > DateTime.DaysInMonth(year, month))
                return DateTime.MinValue;
            return Create(year, month, resultedDay);
        }
    }
}