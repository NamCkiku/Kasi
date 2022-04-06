using System.Text;

namespace Kasi_Server.Utils.Helpers
{
    public partial class Str
    {
        private StringBuilder Builder { get; set; }

        public int Length => Builder.Length;

        public Str()
        {
            Builder = new StringBuilder();
        }

        public Str Append<T>(T value)
        {
            Builder.Append(value);
            return this;
        }

        public Str Append(string value, params object[] args)
        {
            if (args == null)
            {
                args = new object[] { string.Empty };
            }

            if (args.Length == 0)
            {
                Builder.Append(value);
            }
            else
            {
                Builder.AppendFormat(value, args);
            }

            return this;
        }

        public Str AppendLine()
        {
            Builder.AppendLine();
            return this;
        }

        public Str AppendLine<T>(T value)
        {
            Append(value);
            AppendLine();
            return this;
        }

        public Str AppendLine(string value, params object[] args)
        {
            Append(value, args);
            Builder.AppendLine();
            return this;
        }

        public Str Replace(string value)
        {
            Builder.Clear();
            Builder.Append(value);
            return this;
        }

        public Str RemoveEnd(string end)
        {
            string result = Builder.ToString();
            if (!result.EndsWith(end))
            {
                return this;
            }

            Builder = new StringBuilder(result.TrimEnd(end.ToCharArray()));
            return this;
        }

        public Str Clear()
        {
            Builder = Builder.Clear();
            return this;
        }

        public override string ToString()
        {
            return Builder.ToString();
        }
    }
}