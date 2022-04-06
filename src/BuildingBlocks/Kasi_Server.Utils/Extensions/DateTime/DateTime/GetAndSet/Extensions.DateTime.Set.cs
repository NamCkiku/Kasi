using Kasi_Server.Utils.Date;

namespace Kasi_Server.Utils.Extensions
{
    public static partial class DateTimeExtensions
    {
        public static DateTime SetTime(this DateTime dt, int hour) => DateTimeFactory.Create(dt.Year, dt.Month, dt.Day, hour, dt.Minute, dt.Second, dt.Millisecond, dt.Kind);

        public static DateTime SetTime(this DateTime dt, int hour, int minute) => DateTimeFactory.Create(dt.Year, dt.Month, dt.Day, hour, minute, dt.Second, dt.Millisecond, dt.Kind);

        public static DateTime SetTime(this DateTime dt, int hour, int minute, int second) => DateTimeFactory.Create(dt.Year, dt.Month, dt.Day, hour, minute, second, dt.Millisecond, dt.Kind);

        public static DateTime SetTime(this DateTime dt, int hour, int minute, int second, int millisecond) => DateTimeFactory.Create(dt.Year, dt.Month, dt.Day, hour, minute, second, millisecond, dt.Kind);

        public static DateTime SetHour(this DateTime dt, int hour) => DateTimeFactory.Create(dt.Year, dt.Month, dt.Day, hour, dt.Minute, dt.Second, dt.Millisecond, dt.Kind);

        public static DateTime SetMinute(this DateTime dt, int minute) => DateTimeFactory.Create(dt.Year, dt.Month, dt.Day, dt.Hour, minute, dt.Second, dt.Millisecond, dt.Kind);

        public static DateTime SetSecond(this DateTime dt, int second) => DateTimeFactory.Create(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, second, dt.Millisecond, dt.Kind);

        public static DateTime SetMillisecond(this DateTime dt, int millisecond) => DateTimeFactory.Create(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, millisecond, dt.Kind);

        public static DateTime Midnight(this DateTime dt) => throw new NotImplementedException();

        public static DateTime Noon(this DateTime dt) => dt.SetTime(12, 0, 0, 0);

        public static DateTime SetDate(this DateTime dt, int year) => DateTimeFactory.Create(year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, dt.Millisecond, dt.Kind);

        public static DateTime SetDate(this DateTime dt, int year, int month) => DateTimeFactory.Create(year, month, dt.Day, dt.Hour, dt.Minute, dt.Second, dt.Millisecond, dt.Kind);

        public static DateTime SetDate(this DateTime dt, int year, int month, int day) => DateTimeFactory.Create(year, month, day, dt.Hour, dt.Minute, dt.Second, dt.Millisecond, dt.Kind);

        public static DateTime SetYear(this DateTime dt, int year) => DateTimeFactory.Create(year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, dt.Millisecond, dt.Kind);

        public static DateTime SetMonth(this DateTime dt, int month) => DateTimeFactory.Create(dt.Year, month, dt.Day, dt.Hour, dt.Minute, dt.Second, dt.Millisecond, dt.Kind);

        public static DateTime SetDay(this DateTime dt, int day) => DateTimeFactory.Create(dt.Year, dt.Month, day, dt.Hour, dt.Minute, dt.Second, dt.Millisecond, dt.Kind);

        public static DateTime SetKind(this DateTime dt, DateTimeKind kind) => DateTimeFactory.Create(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, dt.Millisecond, kind);
    }
}