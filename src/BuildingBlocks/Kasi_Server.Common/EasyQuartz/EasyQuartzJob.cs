using System;
using System.Collections.Generic;
using System.Text;

namespace Kasi_Server.Common.EasyQuartz
{
    public abstract class EasyQuartzJob
    {
        public abstract string Cron { get; }
    }
}
