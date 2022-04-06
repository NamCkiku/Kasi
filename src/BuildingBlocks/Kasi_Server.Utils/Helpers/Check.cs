namespace Kasi_Server.Utils.Helpers
{
    public static class Check
    {
        private static void Require<TException>(bool assertion, string message) where TException : Exception
        {
            if (assertion)
                return;
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentNullException(nameof(message));
            var exception = (TException)Activator.CreateInstance(typeof(TException), message);
            throw exception;
        }

        public static void Required<T>(T value, Func<T, bool> assertionFunc, string message)
        {
            if (assertionFunc == null)
                throw new ArgumentNullException(nameof(assertionFunc));
            Require<Exception>(assertionFunc(value), message);
        }

        public static void Required<T, TException>(T value, Func<T, bool> assertionFunc, string message)
            where TException : Exception
        {
            if (assertionFunc == null)
                throw new ArgumentNullException(nameof(assertionFunc));
            Require<TException>(assertionFunc(value), message);
        }

        public static void NotNull<T>(T value, string paramName) => Require<ArgumentNullException>(value != null, string.Format("Tham số {0} không được là tham chiếu rỗng.", paramName));

        public static void NotNullOrEmpty(string value, string paramName)
        {
            NotNull(value, paramName);
            Require<ArgumentException>(!string.IsNullOrEmpty(value), string.Format("Tham số {0} không được là một tham chiếu rỗng hoặc một chuỗi trống.", paramName));
        }

        public static void NotEmpty(Guid value, string paramName) => Require<ArgumentException>(value != Guid.Empty, string.Format("Giá trị của tham số {0} không được là Guid.Empty", paramName));

        public static void NotNullOrEmpty<T>(IEnumerable<T> collection, string paramName)
        {
            NotNull(collection, paramName);
            Require<ArgumentException>(collection.Any(), string.Format("Tham số {0} không được là một tham chiếu rỗng hoặc một tập hợp rỗng.", paramName));
        }

        public static void NotNullOrEmpty<T>(IDictionary<string, T> dictionary, string paramName)
        {
            NotNull(dictionary, paramName);
            Require<ArgumentException>(dictionary.Any(), string.Format("Tham số {0} không được là một tham chiếu rỗng hoặc một tập hợp rỗng."));
        }

        public static void LessThan<T>(T value, string paramName, T target, bool canEqual = false)
            where T : IComparable<T>
        {
            bool flag = canEqual ? value.CompareTo(target) <= 0 : value.CompareTo(target) < 0;
            string format = canEqual ? "Giá trị của tham số {0} phải nhỏ hơn hoặc bằng {1}." : "Giá trị của tham số {0} phải nhỏ hơn {1}.";
            Require<ArgumentOutOfRangeException>(flag, string.Format(format, paramName, target));
        }

        public static void GreaterThan<T>(T value, string paramName, T target, bool canEqual = false)
            where T : IComparable<T>
        {
            bool flag = canEqual ? value.CompareTo(target) >= 0 : value.CompareTo(target) > 0;
            string format = canEqual ? "Giá trị của tham số {0} phải lớn hơn hoặc bằng {1}." : "Giá trị của tham số {0} phải lớn hơn {1}.";
            Require<ArgumentOutOfRangeException>(flag, string.Format(format, paramName, target));
        }

        public static void Between<T>(T value, string paramName, T start, T end, bool startEqual = false,
            bool endEqual = false) where T : IComparable<T>
        {
            bool flag = startEqual ? value.CompareTo(start) >= 0 : value.CompareTo(start) > 0;
            string message = startEqual
                ? string.Format("Giá trị của tham số {0} phải từ {1} đến {2}.", paramName, start, end)
                : string.Format("Giá trị của tham số {0} phải nằm trong khoảng từ {1} đến {2} và không được bằng {3}.", paramName, start, end, start);
            Require<ArgumentOutOfRangeException>(flag, message);

            flag = endEqual ? value.CompareTo(end) <= 0 : value.CompareTo(end) < 0;
            message = endEqual
                ? string.Format("Giá trị của tham số {0} phải từ {1} đến {2}.", paramName, start, end)
                : string.Format("Giá trị của tham số {0} phải nằm trong khoảng từ {1} đến {2} và không được bằng {3}.", paramName, start, end, end);
            Require<ArgumentOutOfRangeException>(flag, message);
        }

        public static void NotNegativeOrZero(TimeSpan timeSpan, string paramaName) => Require<ArgumentOutOfRangeException>(timeSpan > TimeSpan.Zero, paramaName);

        public static void DirectoryExists(string directory, string paramName = null)
        {
            NotNull(directory, paramName);
            Require<DirectoryNotFoundException>(Directory.Exists(directory), string.Format("Đường dẫn thư mục được chỉ định {0} không tồn tại.", directory));
        }

        public static void FileExists(string fileName, string paramName = null)
        {
            NotNull(fileName, paramName);
            Require<FileNotFoundException>(File.Exists(fileName), string.Format("Đường dẫn tệp được chỉ định {0} không tồn tại.", fileName));
        }
    }
}