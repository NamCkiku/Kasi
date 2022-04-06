using System.Reflection;

namespace Kasi_Server.Utils.Extensions
{
    public static class PropertyInfoExtensions
    {
        public static bool IsStatic(this PropertyInfo property) => (property.GetMethod ?? property.SetMethod).IsStatic;
    }
}