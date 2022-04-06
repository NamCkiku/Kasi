using System.Diagnostics;
using System.Reflection;

namespace Kasi_Server.Utils.Extensions
{
    public static class AssemblyExtensions
    {
        public static Version GetFileVersion(this Assembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            FileVersionInfo info = FileVersionInfo.GetVersionInfo(assembly.Location);
            return new Version(info.FileVersion);
        }

        public static Version GetProductVersion(this Assembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            FileVersionInfo info = FileVersionInfo.GetVersionInfo(assembly.Location);
            return new Version(info.ProductVersion);
        }

        public static IList<Type> GetTypes(this Assembly[] assemblys, Func<Type, bool> selector) =>
            (
                from assembly in assemblys
                from type in assembly.GetTypes()
                select type
            )
            .Where(selector)
            .ToList();

        public static IList<Type> GetTypes(this Assembly assembly, Func<Type, bool> selector) => assembly.GetTypes().Where(selector).ToList();

        public static bool AnyBaseType(this Type type, Type baseType) => type.GetBaseTypes().Any(c => c.IsParticularGeneric(baseType));

        public static bool IsParticularGeneric(this Type type, Type generic) => type.IsGenericType && type.GetGenericTypeDefinition() == generic;

        public static IEnumerable<Type> GetBaseTypes(this Type type)
        {
            Type t = type;
            while (true)
            {
                t = t.BaseType;
                if (t == null) break;
                yield return t;
            }
        }
    }
}