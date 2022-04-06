namespace Kasi_Server.Utils.Maths
{
    public class HexConv
    {
        private const string BaseChar = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

        public static string BinToOct(string value)
        {
            return X2X(value, 2, 8);
        }

        public static string BinToDec(string value)
        {
            return X2X(value, 2, 10);
        }

        public static string BinToHex(string value)
        {
            return X2X(value, 2, 16);
        }

        public static string OctToBin(string value)
        {
            return X2X(value, 8, 2);
        }

        public static string OctToDec(string value)
        {
            return X2X(value, 8, 10);
        }

        public static string OctToHex(string value)
        {
            return X2X(value, 8, 16);
        }

        public static string DecToBin(string value)
        {
            return X2X(value, 10, 2);
        }

        public static string DecToOct(string value)
        {
            return X2X(value, 10, 8);
        }

        public static string DecToHex(string value)
        {
            return X2X(value, 10, 16);
        }

        public static string HexToBin(string value)
        {
            return X2X(value, 16, 2);
        }

        public static string HexToOct(string value)
        {
            return X2X(value, 16, 8);
        }

        public static string HexToDec(string value)
        {
            return X2X(value, 16, 10);
        }

        public static string X2X(string value, int fromRadix, int toRadix)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(value));
            }
            if (fromRadix < 2 || fromRadix > 62)
            {
                throw new ArgumentOutOfRangeException(nameof(fromRadix));
            }
            if (toRadix < 2 || toRadix > 62)
            {
                throw new ArgumentOutOfRangeException(nameof(toRadix));
            }

            ulong num = X2H(value, fromRadix);
            return H2X(num, toRadix);
        }

        public static ulong X2H(string value, int fromRadix)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (fromRadix < 2 || fromRadix > 62)
            {
                throw new ArgumentOutOfRangeException(nameof(fromRadix));
            }

            value = value.Trim();
            var baseChar = GetBaseChar(fromRadix);
            ulong result = 0;
            for (int i = 0; i < value.Length; i++)
            {
                char @char = value[i];
                if (!baseChar.Contains(@char))
                {
                    throw new ArgumentException(string.Format("Ký tự {0} trong tham số không phải là ký tự hợp lệ trong {1} số thập lục phân.", @char, fromRadix));
                }

                result += (ulong)baseChar.IndexOf(@char) * (ulong)Math.Pow(baseChar.Length, value.Length - i - 1);
            }

            return result;
        }

        public static string H2X(ulong value, int toRadix)
        {
            if (toRadix < 2 || toRadix > 62)
            {
                throw new ArgumentOutOfRangeException(nameof(toRadix));
            }

            if (value == 0)
            {
                return "0";
            }

            var baseChar = GetBaseChar(toRadix);

            string result = string.Empty;
            while (value > 0)
            {
                int index = (int)(value % (ulong)baseChar.Length);
                result = baseChar[index] + result;
                value = value / (ulong)baseChar.Length;
            }

            return result;
        }

        private static string GetBaseChar(int radix)
        {
            string result;
            switch (radix)
            {
                case 26:
                    result = "abcdefghijklmnopqrstuvwxyz";
                    break;

                case 32:
                    result = "0123456789ABCDEFGHJKMNPQRSTVWXYZabcdefghijklmnopqrstuvwxyz";
                    break;

                case 36:
                    result = "0123456789abcdefghijklmnopqrstuvwxyz";
                    break;

                case 52:
                    result = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
                    break;

                case 58:
                    result = "123456789abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNPQRSTUVWXYZ";
                    break;

                case 62:
                    result = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
                    break;

                default:
                    result = BaseChar;
                    break;
            }

            return result.Substring(0, radix);
        }
    }
}