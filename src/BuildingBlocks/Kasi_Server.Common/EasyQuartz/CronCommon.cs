using System;
using System.Collections.Generic;
using System.Text;

namespace Kasi_Server.Common.EasyQuartz
{
    public static class CronCommon
    {
        public static string SecondInterval(int second)
        {
            second = second > 60 ? 60 : second;
            second = second <= 0 ? 1 : second;
            return $"*/{second} * * * * ?";
        }

        public static string MinuteInterval(int interval)
        {
            interval = interval > 60 ? 60 : interval;
            interval = interval <= 0 ? 1 : interval;
            return $"0 */{interval} * * * ?";
        }

        public static string HourInterval(int interval)
        {
            interval = interval > 23 ? 23 : interval;
            interval = interval <= 0 ? 1 : interval;
            return $"0 0 */{interval} * * ?";
        }
    }
}
