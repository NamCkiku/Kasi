using Kasi_Server.Utils.Extensions;

namespace Kasi_Server.Utils.Date
{
    public class DateInfo
    {
        private DateTime _internalDateTime { get; set; }

        internal DateInfo()
        { }

        public DateInfo(DateTime dt)
        {
            _internalDateTime = dt.SetTime(0, 0, 0, 0);
        }
    }
}