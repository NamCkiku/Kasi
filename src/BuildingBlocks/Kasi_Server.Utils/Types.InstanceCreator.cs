using AspectCore.Extensions.Reflection;
using System.Reflection;

namespace Kasi_Server.Utils
{
    public static partial class Types
    {
        public static TInstance CreateInstance<TInstance>(params object[] args) =>
            args is null || args.Length == 0
                ? CreateInstanceCore<TInstance>()
                : CreateInstanceCore<TInstance>(args);

        public static TInstance CreateInstance<TInstance>(string className, params object[] args)
        {
            var type = Type.GetType(className) ?? Assembly.GetCallingAssembly().GetType(className);
            return CreateInstance<TInstance>(type, args);
        }

        public static TInstance CreateInstance<TInstance>(Type type, params object[] args) =>
            CreateInstance(type, args) is TInstance ret ? ret : default;

        public static object CreateInstance(Type type, params object[] args) => args is null || args.Length == 0
            ? CreateInstanceCore(type)
            : CreateInstanceCore(type, args);

        private static TInstance CreateInstanceCore<TInstance>() =>
            CreateInstanceCore(typeof(TInstance)) is TInstance ret ? ret : default;

        private static TInstance CreateInstanceCore<TInstance>(object[] args) =>
            CreateInstanceCore(typeof(TInstance), args) is TInstance ret ? ret : default;

        private static object CreateInstanceCore(Type type) => type.GetConstructors()
            .FirstOrDefault(x => !x.GetParameters().Any())?.GetReflector().Invoke();

        private static object CreateInstanceCore(Type type, object[] args) => type.GetConstructor(Of(args))?.GetReflector().Invoke(args);
    }
}