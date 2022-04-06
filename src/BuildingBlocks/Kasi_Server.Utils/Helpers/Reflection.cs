using Kasi_Server.Utils.Extensions;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Runtime.Loader;

namespace Kasi_Server.Utils.Helpers
{
    public static class Reflection
    {
        public static string GetDescription<T>() => GetDescription(Common.GetType<T>());

        public static string GetDescription<T>(string memberName) => GetDescription(Common.GetType<T>(), memberName);

        public static string GetDescription(Type type, string memberName)
        {
            if (type == null)
                return string.Empty;
            return memberName.IsEmpty()
                ? string.Empty
                : GetDescription(type.GetTypeInfo().GetMember(memberName).FirstOrDefault());
        }

        public static string GetDescription(MemberInfo member)
        {
            if (member == null)
                return string.Empty;
            return member.GetCustomAttribute<DescriptionAttribute>() is DescriptionAttribute attribute
                ? attribute.Description
                : member.Name;
        }

        public static string GetDisplayName<T>() => GetDisplayName(Common.GetType<T>());

        private static string GetDisplayName(MemberInfo member)
        {
            if (member == null)
                return string.Empty;
            if (member.GetCustomAttribute<DisplayAttribute>() is DisplayAttribute displayAttribute)
                return displayAttribute.Description; ;
            if (member.GetCustomAttribute<DisplayNameAttribute>() is DisplayNameAttribute displayNameAttribute)
                return displayNameAttribute.DisplayName;
            return string.Empty;
        }

        public static string GetDisplayNameOrDescription<T>()
        {
            return GetDisplayNameOrDescription(Common.GetType<T>());
        }

        public static string GetDisplayNameOrDescription(MemberInfo member)
        {
            var result = GetDisplayName(member);
            return string.IsNullOrWhiteSpace(result) ? GetDescription(member) : result;
        }

        public static List<Type> FindTypes<TFind>(params Assembly[] assemblies)
        {
            var findType = typeof(TFind);
            return FindTypes(findType, assemblies);
        }

        public static List<Type> FindTypes(Type findType, params Assembly[] assemblies)
        {
            var result = new List<Type>();
            foreach (var assembly in assemblies)
            {
                result.AddRange(GetTypes(findType, assembly));
            }

            return result.Distinct().ToList();
        }

        private static List<Type> GetTypes(Type findType, Assembly assembly)
        {
            var result = new List<Type>();
            if (assembly == null)
            {
                return result;
            }

            Type[] types;
            try
            {
                types = assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException)
            {
                return result;
            }

            foreach (var type in types)
            {
                AddType(result, findType, type);
            }

            return result;
        }

        private static void AddType(List<Type> result, Type findType, Type type)
        {
            if (type.IsInterface || type.IsAbstract)
            {
                return;
            }

            if (findType.IsAssignableFrom(type) == false && MatchGeneric(findType, type) == false)
            {
                return;
            }

            result.Add(type);
        }

        private static bool MatchGeneric(Type findType, Type type)
        {
            if (findType.IsGenericTypeDefinition == false)
            {
                return false;
            }

            var definition = findType.GetGenericTypeDefinition();
            foreach (var implementedInterface in type.FindInterfaces((filiter, criteria) => true, null))
            {
                if (implementedInterface.IsGenericType == false)
                {
                    continue;
                }

                return definition.IsAssignableFrom(implementedInterface.GetGenericTypeDefinition());
            }

            return false;
        }

        public static List<TInterface> GetInstancesByInterface<TInterface>(Assembly assembly)
        {
            var typeInterface = typeof(TInterface);
            return
                assembly.GetTypes()
                    .Where(
                        t =>
                            typeInterface.GetTypeInfo().IsAssignableFrom(t) && t != typeInterface &&
                            t.GetTypeInfo().IsAbstract == false)
                    .Select(t => CreateInstance<TInterface>(t))
                    .ToList();
        }

        public static T CreateInstance<T>(Type type, params object[] parameters) => Conv.To<T>(Activator.CreateInstance(type, parameters));

        public static T CreateInstance<T>(string className, params object[] parameters)
        {
            var type = Type.GetType(className) ?? Assembly.GetCallingAssembly().GetType(className);
            return CreateInstance<T>(type, parameters);
        }

        public static List<Assembly> GetAssemblies(string directoryPath)
        {
            return
                Directory.GetFiles(directoryPath, "*.*", SearchOption.AllDirectories)
                    .ToList()
                    .Where(t => t.EndsWith(".exe") || t.EndsWith(".dll"))
                    .Select(path => Assembly.Load(new AssemblyName(path)))
                    .ToList();
        }

        public static string GetCurrentAssemblyName() => Assembly.GetCallingAssembly().GetName().Name;

        public static TAttribute GetAttribute<TAttribute>(MemberInfo memberInfo) where TAttribute : Attribute
        {
            return (TAttribute)memberInfo.GetCustomAttributes(typeof(TAttribute), false).FirstOrDefault();
        }

        public static TAttribute[] GetAttributes<TAttribute>(MemberInfo memberInfo) where TAttribute : Attribute
        {
            return Array.ConvertAll(memberInfo.GetCustomAttributes(typeof(TAttribute), false), x => (TAttribute)x);
        }

        public static PropertyInfo GetPropertyInfo(Type type, string propertyName) => type.GetProperties().FirstOrDefault(p => p.Name.Equals(propertyName));

        public static bool IsBool(MemberInfo member)
        {
            if (member == null)
            {
                return false;
            }
            switch (member.MemberType)
            {
                case MemberTypes.TypeInfo:
                    return member.ToString() == "System.Boolean";

                case MemberTypes.Property:
                    return IsBool((PropertyInfo)member);
            }
            return false;
        }

        public static bool IsBool(PropertyInfo property)
        {
            return property.PropertyType == typeof(bool) || property.PropertyType == typeof(bool?);
        }

        public static bool IsEnum(MemberInfo member)
        {
            if (member == null)
            {
                return false;
            }
            switch (member.MemberType)
            {
                case MemberTypes.TypeInfo:
                    return ((TypeInfo)member).IsEnum;

                case MemberTypes.Property:
                    return IsEnum((PropertyInfo)member);
            }
            return false;
        }

        public static bool IsEnum(PropertyInfo property)
        {
            if (property.PropertyType.GetTypeInfo().IsEnum)
            {
                return true;
            }
            var value = Nullable.GetUnderlyingType(property.PropertyType);
            if (value == null)
            {
                return false;
            }
            return value.GetTypeInfo().IsEnum;
        }

        public static bool IsDate(MemberInfo member)
        {
            if (member == null)
            {
                return false;
            }
            switch (member.MemberType)
            {
                case MemberTypes.TypeInfo:
                    return member.ToString() == "System.DateTime";

                case MemberTypes.Property:
                    return IsDate((PropertyInfo)member);
            }
            return false;
        }

        public static bool IsDate(PropertyInfo property)
        {
            if (property.PropertyType == typeof(DateTime))
            {
                return true;
            }
            if (property.PropertyType == typeof(DateTime?))
            {
                return true;
            }
            return false;
        }

        public static bool IsInt(MemberInfo member)
        {
            if (member == null)
            {
                return false;
            }
            switch (member.MemberType)
            {
                case MemberTypes.TypeInfo:
                    return member.ToString() == "System.Int32" || member.ToString() == "System.Int16" ||
                           member.ToString() == "System.Int64";

                case MemberTypes.Property:
                    return IsInt((PropertyInfo)member);
            }
            return false;
        }

        public static bool IsInt(PropertyInfo property)
        {
            if (property.PropertyType == typeof(int))
            {
                return true;
            }
            if (property.PropertyType == typeof(int?))
            {
                return true;
            }
            if (property.PropertyType == typeof(short))
            {
                return true;
            }
            if (property.PropertyType == typeof(short?))
            {
                return true;
            }
            if (property.PropertyType == typeof(long))
            {
                return true;
            }
            if (property.PropertyType == typeof(long?))
            {
                return true;
            }
            return false;
        }

        public static bool IsNumber(MemberInfo member)
        {
            if (member == null)
            {
                return false;
            }

            if (IsInt(member))
            {
                return true;
            }
            switch (member.MemberType)
            {
                case MemberTypes.TypeInfo:
                    return member.ToString() == "System.Double" || member.ToString() == "System.Decimal" ||
                           member.ToString() == "System.Single";

                case MemberTypes.Property:
                    return IsNumber((PropertyInfo)member);
            }
            return false;
        }

        public static bool IsNumber(PropertyInfo property)
        {
            if (property.PropertyType == typeof(double))
            {
                return true;
            }
            if (property.PropertyType == typeof(double?))
            {
                return true;
            }
            if (property.PropertyType == typeof(decimal))
            {
                return true;
            }
            if (property.PropertyType == typeof(decimal?))
            {
                return true;
            }
            if (property.PropertyType == typeof(float))
            {
                return true;
            }
            if (property.PropertyType == typeof(float?))
            {
                return true;
            }
            return false;
        }

        public static bool IsCollection(Type type) => type.IsArray || IsGenericCollection(type);

        public static bool IsGenericCollection(Type type)
        {
            if (!type.IsGenericType)
                return false;
            var typeDefinition = type.GetGenericTypeDefinition();
            return typeDefinition == typeof(IEnumerable<>)
                   || typeDefinition == typeof(IReadOnlyCollection<>)
                   || typeDefinition == typeof(IReadOnlyList<>)
                   || typeDefinition == typeof(ICollection<>)
                   || typeDefinition == typeof(IList<>)
                   || typeDefinition == typeof(List<>);
        }

        public static List<Item> GetPublicProperties(object instance)
        {
            var properties = instance.GetType().GetProperties();
            return properties.ToList().Select(t => new Item(t.Name, t.GetValue(instance))).ToList();
        }

        public static Type GetTopBaseType<T>()
        {
            return GetTopBaseType(typeof(T));
        }

        public static Type GetTopBaseType(Type type)
        {
            if (type == null)
            {
                return null;
            }

            if (type.IsInterface)
            {
                return type;
            }

            if (type.BaseType == typeof(object))
            {
                return type;
            }

            return GetTopBaseType(type.BaseType);
        }

        public static bool IsDeriveClassFrom<TBaseType>(Type type, bool canAbstract = false) => IsDeriveClassFrom(type, typeof(TBaseType), canAbstract);

        public static bool IsDeriveClassFrom(Type type, Type baseType, bool canAbstract = false)
        {
            Check.NotNull(type, nameof(type));
            Check.NotNull(baseType, nameof(baseType));

            return type.IsClass && (!canAbstract && !type.IsAbstract) && type.IsBaseOn(baseType);
        }

        public static bool IsBaseOn<TBaseType>(Type type) => IsBaseOn(type, typeof(TBaseType));

        public static bool IsBaseOn(Type type, Type baseType) => baseType.IsGenericTypeDefinition
            ? baseType.IsGenericAssignableFrom(type)
            : baseType.IsAssignableFrom(type);

        public static bool IsGenericAssignableFrom(Type genericType, Type type)
        {
            Check.NotNull(genericType, nameof(genericType));
            Check.NotNull(type, nameof(type));

            if (!genericType.IsGenericType)
            {
                throw new ArgumentException("该功能只支持泛型类型的调用，非泛型类型可使用 IsAssignableFrom 方法。");
            }

            var allOthers = new List<Type>() { type };
            if (genericType.IsInterface)
            {
                allOthers.AddRange(type.GetInterfaces());
            }

            foreach (var other in allOthers)
            {
                var cur = other;
                while (cur != null)
                {
                    if (cur.IsGenericType)
                    {
                        cur = cur.GetGenericTypeDefinition();
                    }

                    if (cur.IsSubclassOf(genericType) || cur == genericType)
                    {
                        return true;
                    }

                    cur = cur.BaseType;
                }
            }

            return false;
        }

        public static IEnumerable<Assembly> GetAssemblies(Func<Assembly, bool> predicate)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                 .Where(assembly =>
                     !assembly.FullName.StartsWith("System") &&
                     !assembly.FullName.StartsWith("Microsoft") &&
                     !assembly.FullName.StartsWith("netstandard") &&
                     !assembly.FullName.StartsWith("Pomelo")
                 );

            if (predicate != null)
                assemblies = assemblies.Where(predicate);

            return assemblies;
        }

        public static Assembly GetEntryAssembly()
        {
            return Assembly.GetEntryAssembly();
        }

        public static Assembly GetAssembly(string assemblyName)
        {
            return AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(assemblyName));
        }

        public static Assembly LoadAssembly(string path)
        {
            if (!File.Exists(path)) return default;
            return AssemblyLoadContext.Default.LoadFromAssemblyPath(path);
        }

        public static Assembly LoadAssembly(MemoryStream assembly)
        {
            return Assembly.Load(assembly.ToArray());
        }

        public static Type GetType(string assemblyName, string typeFullName)
        {
            return GetAssembly(assemblyName).GetType(typeFullName);
        }

        public static Type GetType(Assembly assembly, string typeFullName)
        {
            return assembly.GetType(typeFullName);
        }

        public static Type GetType(MemoryStream assembly, string typeFullName)
        {
            return LoadAssembly(assembly).GetType(typeFullName);
        }

        public static string GetAssemblyName(Assembly assembly)
        {
            return assembly.GetName().Name;
        }

        public static string GetAssemblyName(Type type)
        {
            return GetAssemblyName(type.GetTypeInfo());
        }

        public static string GetAssemblyName(TypeInfo typeInfo)
        {
            return GetAssemblyName(typeInfo.Assembly);
        }
    }
}