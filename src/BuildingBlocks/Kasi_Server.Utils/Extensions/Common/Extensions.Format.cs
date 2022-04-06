using System.Globalization;
using System.Text;

namespace Kasi_Server.Utils.Extensions
{
    public static partial class Extensions
    {
        public static string Description(this bool value) => value ? "是" : "否";

        public static string Description(this bool? value) => value == null ? "" : Description(value.Value);

        public static string FormatInvariant(this string format, params object[] args) => string.Format(CultureInfo.InvariantCulture, format, args);

        public static string FormatCurrent(this string format, params object[] args) =>
            string.Format(CultureInfo.CurrentCulture, format, args);

        public static string FormatCurrentUI(this string format, params object[] args) =>
            string.Format(CultureInfo.CurrentUICulture, format, args);

        public static string FormatMessage(this Exception e, bool isHideStackTrace = false)
        {
            return FormatMessage(e, string.Empty, isHideStackTrace);
        }

        public static string FormatMessage(this Exception e, string message, bool isHideStackTrace = false)
        {
            var sb = new StringBuilder();
            sb.Append(message);
            var count = 0;
            var appString = string.Empty;
            while (e != null)
            {
                if (count > 0)
                    appString += "  ";
                sb.AppendLine($"{appString}异常消息：{e.Message}");
                sb.AppendLine($"{appString}异常类型：{e.GetType().FullName}");
                sb.AppendLine($"{appString}异常方法：{(e.TargetSite == null ? null : e.TargetSite.Name)}");
                sb.AppendLine($"{appString}异常源：{e.Source}");
                if (!isHideStackTrace && e.StackTrace != null)
                    sb.AppendLine($"{appString}异常堆栈：{e.StackTrace}");
                if (e.InnerException != null)
                {
                    sb.AppendLine($"{appString}内部异常：");
                    count++;
                }
                e = e.InnerException;
            }
            return sb.ToString();
        }
    }
}