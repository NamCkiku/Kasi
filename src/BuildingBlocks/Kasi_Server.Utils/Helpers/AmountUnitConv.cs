using Kasi_Server.Utils.Extensions;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Kasi_Server.Utils.Helpers
{
    public static class AmountUnitConv
    {
        public static decimal ToYuan(int fen) => Conv.ToDecimal((decimal)fen / 100, 2);

        public static decimal ToYuan(int? fen) => fen == null ? 0 : Conv.ToDecimal((decimal)fen / 100, 2);

        public static int ToFen(decimal yuan) => Conv.ToInt(CutDecimalWithN(yuan, 2) * 100, 0);

        public static int ToFen(decimal? yuan) => yuan == null ? 0 : Conv.ToInt(CutDecimalWithN(yuan.Value, 2) * 100, 0);

        private static decimal CutDecimalWithN(decimal input, int digits)
        {
            var decimalStr = input.ToString(CultureInfo.InvariantCulture);
            var index = decimalStr.IndexOf(".", StringComparison.Ordinal);
            if (index == -1 || decimalStr.Length < index + digits + 1)
            {
                decimalStr = string.Format("{0:F" + digits + "}", input);
            }
            else
            {
                var length = index;
                if (digits != 0)
                    length = index + digits + 1;
                decimalStr = decimalStr.Substring(0, length);
            }
            return decimal.Parse(decimalStr);
        }

        public static string ToN2String(decimal input) => $"{input:N2}";

        public static string ToChinese(double x)
        {
            string s = x.ToString("#L#E#D#C#K#E#D#C#J#E#D#C#I#E#D#C#H#E#D#C#G#E#D#C#F#E#D#C#.0B0A");
            string d = Regex.Replace(s, @"((?<=-|^)[^1-9]*)|((?'z'0)[0A-E]*((?=[1-9])|(?'-z'(?=[F-L\.]|$))))|((?'b'[F-L])(?'z'0)[0A-L]*((?=[1-9])|(?'-z'(?=[\.]|$))))", "${b}${z}");
            return Regex.Replace(d, ".", delegate (Match m) { return "负元 零壹贰叁肆伍陆柒捌玖       分角拾佰仟萬億兆京垓秭穰"[m.Value[0] - '-'].ToString(); });
        }

        public static string ToChinese(string x)
        {
            double money = x.ToDouble();
            return ToChinese(money);
        }
    }
}