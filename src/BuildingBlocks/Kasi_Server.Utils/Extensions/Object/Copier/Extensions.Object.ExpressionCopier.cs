using System.Linq.Expressions;
using System.Reflection;

namespace Kasi_Server.Utils.Extensions
{
    public static partial class ObjectExtensions
    {
        public static T ExpressionCopy<T>(this T source) => Copier<T>.Copy(source);

        private static class Copier<T>
        {
            private static readonly ParameterExpression _parameterExpression = Expression.Parameter(typeof(T), "p");

            private static readonly Dictionary<string, Expression> _check = new Dictionary<string, Expression>();

            private static Func<T, T> _func;

            public static T Copy(T source)
            {
                if (_func == null)
                {
                    var memberBindings = new List<MemberBinding>();
                    foreach (var item in GetAllPropertiesOrFields())
                    {
                        if (_check.ContainsKey(item.Name))
                        {
                            var memberBinding = Expression.Bind(item, _check[item.Name]);
                            memberBindings.Add(memberBinding);
                        }
                        else
                        {
                            if (typeof(T).GetProperty(item.Name) != null || typeof(T).GetField(item.Name) != null)
                            {
                                var memberBinding = Expression.Bind(item,
                                    Expression.PropertyOrField(_parameterExpression, item.Name));
                                memberBindings.Add(memberBinding);
                            }
                        }
                    }

                    var memberInitExpression =
                        Expression.MemberInit(Expression.New(typeof(T)), memberBindings.ToArray());
                    var lambda = Expression.Lambda<Func<T, T>>(memberInitExpression, _parameterExpression);
                    _func = lambda.Compile();
                }
                return _func.Invoke(source);
            }

            private static IEnumerable<MemberInfo> GetAllPropertiesOrFields()
            {
                foreach (var item in typeof(T).GetProperties())
                    yield return item;
                foreach (var item in typeof(T).GetFields())
                    yield return item;
            }
        }
    }
}