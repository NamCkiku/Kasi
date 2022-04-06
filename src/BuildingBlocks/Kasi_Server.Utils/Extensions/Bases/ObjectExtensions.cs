using System.Runtime.Serialization.Formatters.Binary;

namespace Kasi_Server.Utils.Extensions
{
    public static partial class ObjectExtensions
    {
        public static T DeepClone<T>(this T obj) where T : class
        {
            if (obj == null)
            {
                return default(T);
            }

            if (!typeof(T).HasAttribute<SerializableAttribute>(true))
            {
                throw new NotSupportedException($"当前对象未标记特性“{typeof(SerializableAttribute)}”，无法进行DeepClone操作");
            }
            BinaryFormatter formatter = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                formatter.Serialize(ms, obj);
                ms.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(ms);
            }
        }

        public static int ClonePropertyFrom(this object destination, object source)
        {
            return destination.ClonePropertyFrom(source, null);
        }

        public static int ClonePropertyFrom(this object destination, object source, IEnumerable<string> excludeName)
        {
            if (source == null)
            {
                return 0;
            }
            return destination.ClonePropertyFrom(source, source.GetType(), excludeName);
        }

        public static int ClonePropertyFrom(this object @this, object source, Type type, IEnumerable<string> excludeName)
        {
            if (@this == null || source == null)
            {
                return 0;
            }

            if (@this == source)
            {
                return 0;
            }

            if (excludeName == null)
            {
                excludeName = new List<string>();
            }

            int i = 0;
            var desType = @this.GetType();
            foreach (var mi in type.GetFields())
            {
                if (excludeName.Contains(mi.Name))
                {
                    continue;
                }
                try
                {
                    var des = desType.GetField(mi.Name);
                    if (des != null && des.FieldType == mi.FieldType)
                    {
                        des.SetValue(@this, mi.GetValue(source));
                        i++;
                    }
                }
                catch
                {
                }
            }

            foreach (var pi in type.GetProperties())
            {
                if (excludeName.Contains(pi.Name))
                {
                    continue;
                }
                try
                {
                    var des = desType.GetProperty(pi.Name);
                    if (des != null && des.PropertyType == pi.PropertyType && des.CanWrite && pi.CanRead)
                    {
                        des.SetValue(@this, pi.GetValue(source, null), null);
                        i++;
                    }
                }
                catch
                {
                }
            }
            return i;
        }

        public static int ClonePropertyTo(this object source, object destination)
        {
            return source.ClonePropertyTo(destination, null);
        }

        public static int ClonePropertyTo(this object source, object destination, IEnumerable<string> excludeName)
        {
            if (destination == null)
            {
                return 0;
            }
            return destination.ClonePropertyFrom(source, source.GetType(), excludeName);
        }

        public static T? ToNullable<T>(this T value) where T : struct
        {
            return value.IsNull() ? null : (T?)value;
        }

        public static void Locking(this object source, Action action)
        {
            lock (source)
                action?.Invoke();
        }

        public static void Locking<T>(this T source, Action<T> action) where T : class
        {
            lock (source)
                action?.Invoke(source);
        }

        public static TResult Locking<TResult>(this object source, Func<TResult> func)
        {
            lock (source)
                return func();
        }

        public static TResult Locking<T, TResult>(this T source, Func<T, TResult> func) where T : class
        {
            lock (source)
                return func(source);
        }
    }
}