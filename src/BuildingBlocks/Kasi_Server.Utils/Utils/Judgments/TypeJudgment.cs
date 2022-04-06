using System.Reflection;

namespace Kasi_Server.Utils.Judgments
{
    public static class TypeJudgment
    {
        public static bool IsNumericType(Type type) =>
            type == typeof(byte)
            || type == typeof(short)
            || type == typeof(int)
            || type == typeof(long)
            || type == typeof(sbyte)
            || type == typeof(ushort)
            || type == typeof(uint)
            || type == typeof(ulong)
            || type == typeof(decimal)
            || type == typeof(double)
            || type == typeof(float);

        public static bool IsNumericType(TypeInfo typeInfo) => IsNumericType(typeInfo.AsType());

        public static bool IsNullableType(Type type) =>
            type != null
            && type.GetTypeInfo().IsGenericType
            && type.GetGenericTypeDefinition() == typeof(Nullable<>);

        public static bool IsNullableType(TypeInfo typeInfo) => IsNullableType(typeInfo.AsType());
    }
}