using Kasi_Server.Utils.Expressions;
using Kasi_Server.Utils.Helpers;
using System.Linq.Expressions;
using System.Reflection;

namespace Kasi_Server.Utils.Extensions
{
    public static partial class Extensions
    {
        public static Expression Property(this Expression expression, string propertyName)
        {
            if (propertyName.All(t => t != '.'))
                return Expression.Property(expression, propertyName);
            var propertyNameList = propertyName.Split('.');
            Expression result = null;
            for (int i = 0; i < propertyName.Length; i++)
            {
                if (i == 0)
                {
                    result = Expression.Property(expression, propertyNameList[0]);
                    continue;
                }
                result = result.Property(propertyNameList[i]);
            }
            return result;
        }

        public static Expression Property(this Expression expression, MemberInfo member) => Expression.MakeMemberAccess(expression, member);

        public static Expression And(this Expression left, Expression right, bool condition = true)
        {
            if (!condition) return left;

            if (left == null)
                return right;
            if (right == null)
                return left;
            return Expression.AndAlso(left, right);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> left,
            Expression<Func<T, bool>> right, bool condition = true)
        {
            if (!condition) return left;

            if (left == null)
                return right;
            if (right == null)
                return left;
            return left.Compose(right, Expression.AndAlso);
        }

        public static Expression Or(this Expression left, Expression right, bool condition = true)
        {
            if (!condition) return left;

            if (left == null)
                return right;
            if (right == null)
                return left;
            return Expression.OrElse(left, right);
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> left,
            Expression<Func<T, bool>> right, bool condition = true)
        {
            if (!condition) return left;

            if (left == null)
                return right;
            if (right == null)
                return left;
            return left.Compose(right, Expression.OrElse);
        }

        public static object Value<T>(this Expression<Func<T, bool>> expression) => Lambda.GetValue(expression);

        public static Expression Equal(this Expression left, Expression right) => Expression.Equal(left, right);

        public static Expression Equal(this Expression left, object value) => left.Equal(Lambda.Constant(value, left));

        public static Expression NotEqual(this Expression left, Expression right) => Expression.NotEqual(left, right);

        public static Expression NotEqual(this Expression left, object value) => left.NotEqual(Lambda.Constant(value, left));

        public static Expression Greater(this Expression left, Expression right) => Expression.GreaterThan(left, right);

        public static Expression Greater(this Expression left, object value) => left.Greater(Lambda.Constant(value, left));

        public static Expression GreaterEqual(this Expression left, Expression right) => Expression.GreaterThanOrEqual(left, right);

        public static Expression GreaterEqual(this Expression left, object value) => left.GreaterEqual(Lambda.Constant(value, left));

        public static Expression Less(this Expression left, Expression right) => Expression.LessThan(left, right);

        public static Expression Less(this Expression left, object value) => left.Less(Lambda.Constant(value, left));

        public static Expression LessEqual(this Expression left, Expression right) => Expression.LessThanOrEqual(left, right);

        public static Expression LessEqual(this Expression left, object value) => left.LessEqual(Lambda.Constant(value, left));

        public static Expression StartsWith(this Expression left, object value) => left.Call("StartsWith", new[] { typeof(string) }, value);

        public static Expression EndsWith(this Expression left, object value) => left.Call("EndsWith", new[] { typeof(string) }, value);

        public static Expression Contains(this Expression left, object value) => left.Call("Contains", new[] { typeof(string) }, value);

        public static Expression Operation(this Expression left, Operator @operator, object value)
        {
            switch (@operator)
            {
                case Operator.Equal:
                    return left.Equal(value);

                case Operator.NotEqual:
                    return left.NotEqual(value);

                case Operator.Greater:
                    return left.Greater(value);

                case Operator.GreaterEqual:
                    return left.GreaterEqual(value);

                case Operator.Less:
                    return left.Less(value);

                case Operator.LessEqual:
                    return left.LessEqual(value);

                case Operator.Starts:
                    return left.StartsWith(value);

                case Operator.Ends:
                    return left.EndsWith(value);

                case Operator.Contains:
                    return left.Contains(value);
            }
            throw new NotImplementedException();
        }

        public static Expression Operation(this Expression left, Operator @operator, Expression value)
        {
            switch (@operator)
            {
                case Operator.Equal:
                    return left.Equal(value);

                case Operator.NotEqual:
                    return left.NotEqual(value);

                case Operator.Greater:
                    return left.Greater(value);

                case Operator.GreaterEqual:
                    return left.GreaterEqual(value);

                case Operator.Less:
                    return left.Less(value);

                case Operator.LessEqual:
                    return left.LessEqual(value);
            }
            throw new NotImplementedException();
        }

        public static Expression Call(this Expression instance, string methodName, params Expression[] values)
        {
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));
            var types = values?.Select(x => x.Type).ToArray();
            var methodInfo = types == null
                ? instance.Type.GetMethod(methodName)
                : instance.Type.GetMethod(methodName, types);
            if (methodInfo == null)
                return null;
            return Expression.Call(instance, methodInfo, values);
        }

        public static Expression Call(this Expression instance, string methodName, params object[] values)
        {
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));
            var methodInfo = instance.Type.GetMethod(methodName);
            if (methodInfo == null)
                return null;
            if (values == null || values.Length == 0)
                return Expression.Call(instance, methodInfo);
            return Expression.Call(instance, methodInfo, values.Select(Expression.Constant));
        }

        public static Expression Call(this Expression instance, string methodName, Type[] paramTypes,
            params object[] values)
        {
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));
            var methodInfo = instance.Type.GetMethod(methodName, paramTypes);
            if (methodInfo == null)
                return null;
            if (values == null || values.Length == 0)
                return Expression.Call(instance, methodInfo);
            return Expression.Call(instance, methodInfo, values.Select(Expression.Constant));
        }

        internal static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second,
            Func<Expression, Expression, Expression> merge)
        {
            var map = first.Parameters.Select((f, i) => new { f, s = second.Parameters[i] })
                .ToDictionary(p => p.s, p => p.f);
            var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);
            return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
        }

        public static Expression<TDelegate> ToLambda<TDelegate>(this Expression body,
            params ParameterExpression[] parameters)
        {
            if (body == null)
                return null;
            return Expression.Lambda<TDelegate>(body, parameters);
        }

        public static Expression<Func<T, bool>> ToPredicate<T>(this Expression body,
            params ParameterExpression[] parameters) =>
            ToLambda<Func<T, bool>>(body, parameters);
    }
}