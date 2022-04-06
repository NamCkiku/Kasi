using System.Reflection;

namespace Kasi_Server.Utils.Extensions
{
    public static partial class Extensions
    {
        public static object GetPropertyValue(this MemberInfo member, object instance)
        {
            if (member == null)
            {
                throw new ArgumentNullException(nameof(member));
            }

            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }
            return instance.GetType().GetProperty(member.Name)?.GetValue(instance);
        }

        public static T DeepCopyByReflect<T>(this T obj) where T : class
        {
            if (obj == null || obj.GetType() == typeof(string))
                return obj;
            object newObj = null;
            try
            {
                newObj = Activator.CreateInstance(obj.GetType());
            }
            catch
            {
                foreach (ConstructorInfo ci in obj.GetType().GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static))
                {
                    try
                    {
                        ParameterInfo[] pis = ci.GetParameters();
                        object[] objs = new object[pis.Length];
                        for (int i = 0; i < pis.Length; i++)
                        {
                            if (pis[i].ParameterType.IsValueType)
                                objs[i] = Activator.CreateInstance(pis[i].ParameterType);
                            else
                                objs[i] = null;
                        }
                        newObj = ci.Invoke(objs);
                        break;
                    }
                    catch
                    {
                    }
                }
            }
            foreach (FieldInfo fi in obj.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static))
            {
                if (fi.FieldType.IsValueType || fi.FieldType == typeof(string))
                    fi.SetValue(newObj, fi.GetValue(obj));
                else
                    fi.SetValue(newObj, DeepCopyByReflect(fi.GetValue(obj)));
            }
            Deep(newObj, obj);
            return (T)newObj;
        }

        private static void Deep(object newObj, object obj)
        {
            for (Type father = newObj.GetType().BaseType; father != typeof(object); father = father.BaseType)
            {
                foreach (FieldInfo fi in father.GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
                {
                    if (fi.IsPrivate)
                    {
                        if (fi.FieldType.IsValueType || fi.FieldType == typeof(string))
                        {
                            fi.SetValue(newObj, fi.GetValue(obj));
                        }
                        else
                        {
                            fi.SetValue(newObj, DeepCopyByReflect(fi.GetValue(obj)));
                        }
                    }
                }
            }
        }

        public static IList<Target> CopyByReflect<TSource, Target>(this IList<TSource> sources)
        {
            IList<Target> targetList = new List<Target>();

            if (sources == null || sources.Count == 0)
                return targetList;

            PropertyInfo[] _targetProperties = typeof(Target).GetProperties();
            PropertyInfo[] _sourceProperties = typeof(TSource).GetProperties();

            foreach (var source in sources)
            {
                Target model = Activator.CreateInstance<Target>();

                foreach (var _target in _targetProperties)
                {
                    foreach (var _source in _sourceProperties)
                    {
                        if (_target.Name == _source.Name && _target.PropertyType == _source.PropertyType)
                        {
                            _target.SetValue(model, _source.GetValue(source, null), null);
                            break;
                        }
                    }
                }

                targetList.Add(model);
            }

            return targetList;
        }

        public static Target CopyByReflect<TSource, Target>(this TSource source)
        {
            Target model = default(Target);
            if (source == null) return model;

            PropertyInfo[] _targetProperties = typeof(Target).GetProperties();
            PropertyInfo[] _sourceProperties = typeof(TSource).GetProperties();

            model = Activator.CreateInstance<Target>();

            foreach (var _target in _targetProperties)
            {
                foreach (var _source in _sourceProperties)
                {
                    if (_target.Name == _source.Name && _target.PropertyType == _source.PropertyType)
                    {
                        _target.SetValue(model, _source.GetValue(source, null), null);
                        break;
                    }
                }
            }
            return model;
        }
    }
}