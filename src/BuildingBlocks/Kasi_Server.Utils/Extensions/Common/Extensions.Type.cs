using Kasi_Server.Utils.Helpers;
using System.Collections;
using System.Reflection;

namespace Kasi_Server.Utils.Extensions
{
    public static partial class Extensions
    {
        public static bool IsNullableType(this Type type) => type != null && type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);

        public static bool IsNullableType(this Type type, Type genericParameterType)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            return genericParameterType == Nullable.GetUnderlyingType(type);
        }

        public static bool IsNullableEnum(this Type type) => Nullable.GetUnderlyingType(type)?.GetTypeInfo().IsEnum ?? false;

        public static bool HasAttribute<T>(this Type type, bool inherit = false) where T : Attribute => type.GetTypeInfo().IsDefined(typeof(T), inherit);

        public static IEnumerable<T> GetAttributes<T>(this Type type, bool inherit = false) where T : Attribute => type.GetTypeInfo().GetCustomAttributes<T>(inherit);

        public static T GetAttribute<T>(this Type type, bool inherit = false) where T : Attribute => type.GetTypeInfo().GetCustomAttributes<T>(inherit).FirstOrDefault();

        public static bool IsCustomType(this Type type)
        {
            if (type.IsPrimitive)
                return false;
            if (type.IsArray && type.HasElementType && type.GetElementType().IsPrimitive)
                return false;
            return type != typeof(object) && type != typeof(Guid) &&
                   Type.GetTypeCode(type) == TypeCode.Object && !type.IsGenericType;
        }

        public static bool IsAnonymousType(this Type type)
        {
            const string csharpAnonPrefix = "<>f__AnonymousType";
            const string vbAnonPrefix = "VB$Anonymous";
            var typeName = type.Name;
            return typeName.StartsWith(csharpAnonPrefix) || typeName.StartsWith(vbAnonPrefix);
        }

        public static bool IsBaseType(this Type type, Type checkingType)
        {
            while (type != typeof(object))
            {
                if (type == null)
                    continue;
                if (type == checkingType)
                    return true;
                type = type.BaseType;
            }
            return false;
        }

        public static bool CanUseForDb(this Type type) =>
            type == typeof(string)
            || type == typeof(int)
            || type == typeof(long)
            || type == typeof(uint)
            || type == typeof(ulong)
            || type == typeof(float)
            || type == typeof(double)
            || type == typeof(Guid)
            || type == typeof(byte[])
            || type == typeof(decimal)
            || type == typeof(char)
            || type == typeof(bool)
            || type == typeof(DateTime)
            || type == typeof(TimeSpan)
            || type == typeof(DateTimeOffset)
            || type.GetTypeInfo().IsEnum
            || Nullable.GetUnderlyingType(type) != null && CanUseForDb(Nullable.GetUnderlyingType(type));

        public static bool IsDeriveClassFrom<TBaseType>(this Type type, bool canAbstract = false) => Reflection.IsDeriveClassFrom<TBaseType>(type, canAbstract);

        public static bool IsDeriveClassFrom(this Type type, Type baseType, bool canAbstract = false) => Reflection.IsDeriveClassFrom(type, baseType, canAbstract);

        public static bool IsBaseOn<TBaseType>(this Type type) => Reflection.IsBaseOn<TBaseType>(type);

        public static bool IsBaseOn(this Type type, Type baseType) => Reflection.IsBaseOn(type, baseType);

        public static bool IsGenericAssignableFrom(this Type genericType, Type type) => Reflection.IsGenericAssignableFrom(genericType, type);

        public static bool IsIntegerType(this Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                    return true;

                default:
                    return false;
            }
        }

        public static bool IsCollectionType(this Type type) => type.GetInterfaces().Any(n => n.Name == nameof(IEnumerable));

        public static bool IsValueType(this Type type)
        {
            var result = IsIntegerType(type);
            if (!result)
            {
                switch (Type.GetTypeCode(type))
                {
                    case TypeCode.String:
                    case TypeCode.Boolean:
                    case TypeCode.Char:
                    case TypeCode.DateTime:
                    case TypeCode.Decimal:
                    case TypeCode.Double:
                    case TypeCode.Empty:
                    case TypeCode.Single:
                        result = true;
                        break;

                    default:
                        result = false;
                        break;
                }
            }
            return result;
        }
    }
}