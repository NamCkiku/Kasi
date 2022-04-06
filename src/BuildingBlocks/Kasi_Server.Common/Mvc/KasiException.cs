namespace Kasi_Server.Common.Types
{
    public class KasiException : Exception
    {
        public string Code { get; }

        public KasiException()
        {
        }

        public KasiException(string code)
        {
            Code = code;
        }

        public KasiException(string message, params object[] args)
            : this(string.Empty, message, args)
        {
        }

        public KasiException(string code, string message, params object[] args)
            : this(null, code, message, args)
        {
        }

        public KasiException(Exception innerException, string message, params object[] args)
            : this(innerException, string.Empty, message, args)
        {
        }

        public KasiException(Exception innerException, string code, string message, params object[] args)
            : base(string.Format(message, args), innerException)
        {
            Code = code;
        }
    }
}