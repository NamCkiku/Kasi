using System.Text.RegularExpressions;

namespace Kasi_Server.Utils.Judgments
{
    public static class GuidJudgment
    {
        public static bool IsNullOrEmpty(Guid guid) => guid == Guid.Empty;

        public static bool IsNullOrEmpty(Guid? guid) => guid is null || IsNullOrEmpty(guid.Value);

        private static readonly Regex GuidSchema = new Regex("^[A-Fa-f0-9]{32}$|" +
                                                             "^({|\\()?[A-Fa-f0-9]{8}-([A-Fa-f0-9]{4}-){3}[A-Fa-f0-9]{12}(}|\\))?$|" +
                                                             "^({)?[0xA-Fa-f0-9]{3,10}(, {0,1}[0xA-Fa-f0-9]{3,6}){2},{0,1}({)([0xA-Fa-f0-9]{3,4}, {0,1}){7}[0xA-Fa-f0-9]{3,4}(}})$");

        public static bool IsValid(string guidStr) => !string.IsNullOrWhiteSpace(guidStr) && GuidSchema.Match(guidStr).Success;
    }
}