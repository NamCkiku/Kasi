using Kasi_Server.Utils.Extensions;
using System.Linq.Expressions;
using System.Reflection;

namespace Kasi_Server.Utils.Helpers
{
    public static class Lambda
    {
        public static Type GetType(Expression expression)
        {
            var memberExpression = GetMemberExpression(expression);
            return memberExpression?.Type;
        }

        public static MemberInfo GetMember(Expression expression)
        {
            var memberExpression = GetMemberExpression(expression);
            return memberExpression?.Member;
        }

        public static MemberExpression GetMemberExpression(Expression expression, bool right = false)
        {
            if (expression == null)
            {
                return null;
            }
            switch (expression.NodeType)
            {
                case ExpressionType.Lambda:
                    return GetMemberExpression(((LambdaExpression)expression).Body, right);

                case ExpressionType.Convert:
                case ExpressionType.Not:
                    return GetMemberExpression(((UnaryExpression)expression).Operand, right);

                case ExpressionType.MemberAccess:
                    return (MemberExpression)expression;

                case ExpressionType.Equal:
                case ExpressionType.NotEqual:
                case ExpressionType.GreaterThan:
                case ExpressionType.LessThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.LessThanOrEqual:
                    return GetMemberExpression(right
                        ? ((BinaryExpression)expression).Right
                        : ((BinaryExpression)expression).Left);

                case ExpressionType.Call:
                    return GetMethodCallExpressionName(expression);
            }
            return null;
        }

        private static MemberExpression GetMethodCallExpressionName(Expression expression)
        {
            var methodCallExpression = (MethodCallExpression)expression;
            var left = (MemberExpression)methodCallExpression.Object;
            if (Reflection.IsGenericCollection(left?.Type))
            {
                var argumentExpression = methodCallExpression.Arguments.FirstOrDefault();
                if (argumentExpression != null && argumentExpression.NodeType == ExpressionType.MemberAccess)
                {
                    return (MemberExpression)argumentExpression;
                }
            }
            return left;
        }

        public static string GetName(Expression expression)
        {
            var memberExpression = GetMemberExpression(expression);
            return GetMemberName(memberExpression);
        }

        public static string GetMemberName(MemberExpression memberExpression)
        {
            if (memberExpression == null)
            {
                return string.Empty;
            }
            string result = memberExpression.ToString();
            return result.Substring(result.IndexOf(".", StringComparison.Ordinal) + 1);
        }

        public static List<string> GetNames<T>(Expression<Func<T, object[]>> expression)
        {
            var result = new List<string>();
            if (expression == null)
            {
                return result;
            }

            if (!(expression.Body is NewArrayExpression arrayExpression))
            {
                return result;
            }
            foreach (var each in arrayExpression.Expressions)
            {
                AddName(result, each);
            }
            return result;
        }

        private static void AddName(List<string> result, Expression expression)
        {
            var name = GetName(expression);
            if (name.IsEmpty())
            {
                return;
            }
            result.Add(name);
        }

        public static string GetLastName(Expression expression, bool right = false)
        {
            var memberExpression = GetMemberExpression(expression, right);
            if (memberExpression == null)
            {
                return string.Empty;
            }

            if (IsValueExpression(memberExpression))
            {
                return string.Empty;
            }
            string result = memberExpression.ToString();
            return result.Substring(result.LastIndexOf(".", StringComparison.Ordinal) + 1);
        }

        private static bool IsValueExpression(Expression expression)
        {
            if (expression == null)
            {
                return false;
            }

            switch (expression.NodeType)
            {
                case ExpressionType.MemberAccess:
                    return IsValueExpression(((MemberExpression)expression).Expression);

                case ExpressionType.Constant:
                    return true;
            }

            return false;
        }

        public static List<string> GetLastNames<T>(Expression<Func<T, object[]>> expression)
        {
            var result = new List<string>();
            if (expression == null)
            {
                return result;
            }
            if (!(expression.Body is NewArrayExpression arrayExpression))
            {
                return result;
            }
            foreach (var each in arrayExpression.Expressions)
            {
                var name = GetLastName(each);
                if (string.IsNullOrWhiteSpace(name) == false)
                {
                    result.Add(name);
                }
            }
            return result;
        }

        public static object GetValue(Expression expression)
        {
            if (expression == null)
                return null;
            switch (expression.NodeType)
            {
                case ExpressionType.Lambda:
                    return GetValue(((LambdaExpression)expression).Body);

                case ExpressionType.Convert:
                    return GetValue(((UnaryExpression)expression).Operand);

                case ExpressionType.Equal:
                case ExpressionType.NotEqual:
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                    var hasParameter = HasParameter(((BinaryExpression)expression).Left);
                    if (hasParameter)
                        return GetValue(((BinaryExpression)expression).Right);
                    return GetValue(((BinaryExpression)expression).Left);

                case ExpressionType.Call:
                    return GetMethodCallExpressionValue(expression);

                case ExpressionType.MemberAccess:
                    return GetMemberValue((MemberExpression)expression);

                case ExpressionType.Constant:
                    return GetConstantExpressionValue(expression);

                case ExpressionType.Not:
                    if (expression.Type == typeof(bool))
                    {
                        return false;
                    }
                    return null;
            }
            return null;
        }

        private static bool HasParameter(Expression expression)
        {
            if (expression == null)
                return false;
            switch (expression.NodeType)
            {
                case ExpressionType.Convert:
                    return HasParameter(((UnaryExpression)expression).Operand);

                case ExpressionType.MemberAccess:
                    return HasParameter(((MemberExpression)expression).Expression);

                case ExpressionType.Parameter:
                    return true;
            }
            return false;
        }

        private static object GetMethodCallExpressionValue(Expression expression)
        {
            var methodCallExpression = (MethodCallExpression)expression;
            var value = GetValue(methodCallExpression.Arguments.FirstOrDefault());
            if (value != null)
            {
                return value;
            }
            if (methodCallExpression.Object == null)
            {
                return methodCallExpression.Type.InvokeMember(methodCallExpression.Method.Name,
                    BindingFlags.InvokeMethod, null, null, null);
            }
            return GetValue(methodCallExpression.Object);
        }

        private static object GetMemberValue(MemberExpression expression)
        {
            if (expression == null)
            {
                return null;
            }
            var field = expression.Member as FieldInfo;
            if (field != null)
            {
                var constValue = GetConstantExpressionValue(expression.Expression);
                return field.GetValue(constValue);
            }
            var property = expression.Member as PropertyInfo;
            if (property == null)
            {
                return null;
            }
            if (expression.Expression == null)
            {
                return property.GetValue(null);
            }
            var value = GetMemberValue(expression.Expression as MemberExpression);
            if (value == null)
            {
                if (property.PropertyType == typeof(bool))
                {
                    return true;
                }
                return null;
            }
            return property.GetValue(value);
        }

        private static object GetConstantExpressionValue(Expression expression)
        {
            var constantExpression = (ConstantExpression)expression;
            return constantExpression.Value;
        }

        public static Operator? GetOperator(Expression expression)
        {
            if (expression == null)
            {
                return null;
            }
            switch (expression.NodeType)
            {
                case ExpressionType.Lambda:
                    return GetOperator(((LambdaExpression)expression).Body);

                case ExpressionType.Convert:
                    return GetOperator(((UnaryExpression)expression).Operand);

                case ExpressionType.Equal:
                    return Operator.Equal;

                case ExpressionType.NotEqual:
                    return Operator.NotEqual;

                case ExpressionType.GreaterThan:
                    return Operator.Greater;

                case ExpressionType.LessThan:
                    return Operator.Less;

                case ExpressionType.GreaterThanOrEqual:
                    return Operator.GreaterEqual;

                case ExpressionType.LessThanOrEqual:
                    return Operator.LessEqual;

                case ExpressionType.Call:
                    return GetMethodCallExpressionOperator(expression);
            }
            return null;
        }

        private static Operator? GetMethodCallExpressionOperator(Expression expression)
        {
            var methodCallExpression = (MethodCallExpression)expression;
            switch (methodCallExpression?.Method?.Name?.ToLower())
            {
                case "contains":
                    return Operator.Contains;

                case "endswith":
                    return Operator.Ends;

                case "startswith":
                    return Operator.Starts;
            }
            return null;
        }

        public static ParameterExpression GetParameter(Expression expression)
        {
            if (expression == null)
            {
                return null;
            }
            switch (expression.NodeType)
            {
                case ExpressionType.Lambda:
                    return GetParameter(((LambdaExpression)expression).Body);

                case ExpressionType.Convert:
                    return GetParameter(((UnaryExpression)expression).Operand);

                case ExpressionType.Equal:
                case ExpressionType.NotEqual:
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                    return GetParameter(((BinaryExpression)expression).Left);

                case ExpressionType.MemberAccess:
                    return GetParameter(((MemberExpression)expression).Expression);

                case ExpressionType.Call:
                    return GetParameter(((MethodCallExpression)expression).Object);

                case ExpressionType.Parameter:
                    return (ParameterExpression)expression;
            }
            return null;
        }

        public static List<List<Expression>> GetGroupPredicates(Expression expression)
        {
            var result = new List<List<Expression>>();
            if (expression == null)
            {
                return result;
            }
            AddPredicates(expression, result, CreateGroup(result));
            return result;
        }

        private static List<Expression> CreateGroup(List<List<Expression>> result)
        {
            var group = new List<Expression>();
            result.Add(group);
            return group;
        }

        private static void AddPredicates(Expression expression, List<List<Expression>> result, List<Expression> group)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.Lambda:
                    AddPredicates(((LambdaExpression)expression).Body, result, group);
                    break;

                case ExpressionType.OrElse:
                    AddPredicates(((BinaryExpression)expression).Left, result, group);
                    AddPredicates(((BinaryExpression)expression).Right, result, CreateGroup(result));
                    break;

                case ExpressionType.AndAlso:
                    AddPredicates(((BinaryExpression)expression).Left, result, group);
                    AddPredicates(((BinaryExpression)expression).Right, result, group);
                    break;

                default:
                    group.Add(expression);
                    break;
            }
        }

        public static int GetConditionCount(LambdaExpression expression)
        {
            if (expression == null)
            {
                return 0;
            }
            var result = expression.ToString().Replace("AndAlso", "|").Replace("OrElse", "|");
            return result.Split('|').Length;
        }

        public static TAttribute GetAttribute<TAttribute>(Expression expression) where TAttribute : Attribute
        {
            var memberInfo = GetMember(expression);
            return memberInfo.GetCustomAttribute<TAttribute>();
        }

        public static TAttribute GetAttribute<TEntity, TProperty, TAttribute>(
            Expression<Func<TEntity, TProperty>> propertyExpression) where TAttribute : Attribute
        {
            return GetAttribute<TAttribute>(propertyExpression);
        }

        public static TAttribute GetAttribute<TProperty, TAttribute>(Expression<Func<TProperty>> propertyExpression)
            where TAttribute : Attribute
        {
            return GetAttribute<TAttribute>(propertyExpression);
        }

        public static IEnumerable<TAttribute> GetAttributes<TEntity, TProperty, TAttribute>(
            Expression<Func<TEntity, TProperty>> propertyExpression) where TAttribute : Attribute
        {
            var memberInfo = GetMember(propertyExpression);
            return memberInfo.GetCustomAttributes<TAttribute>();
        }

        public static ConstantExpression Constant(object value, Expression expression = null)
        {
            var type = GetType(expression);
            if (type == null)
            {
                return Expression.Constant(value);
            }

            return Expression.Constant(value, type);
        }

        public static ParameterExpression CreateParameter<T>()
        {
            return Expression.Parameter(typeof(T), "t");
        }

        public static Expression<Func<T, bool>> Equal<T>(string propertyName, object value)
        {
            var parameter = CreateParameter<T>();
            return parameter
                .Property(propertyName)
                .Equal(value)
                .ToPredicate<T>(parameter);
        }

        public static Expression<Func<T, bool>> NotEqual<T>(string propertyName, object value)
        {
            var parameter = CreateParameter<T>();
            return parameter
                .Property(propertyName)
                .NotEqual(value)
                .ToPredicate<T>(parameter);
        }

        public static Expression<Func<T, bool>> Greater<T>(string propertyName, object value)
        {
            var parameter = CreateParameter<T>();
            return parameter
                .Property(propertyName)
                .Greater(value)
                .ToPredicate<T>(parameter);
        }

        public static Expression<Func<T, bool>> GreaterEqual<T>(string propertyName, object value)
        {
            var parameter = CreateParameter<T>();
            return parameter
                .Property(propertyName)
                .GreaterEqual(value)
                .ToPredicate<T>(parameter);
        }

        public static Expression<Func<T, bool>> Less<T>(string propertyName, object value)
        {
            var parameter = CreateParameter<T>();
            return parameter
                .Property(propertyName)
                .Less(value)
                .ToPredicate<T>(parameter);
        }

        public static Expression<Func<T, bool>> LessEqual<T>(string propertyName, object value)
        {
            var parameter = CreateParameter<T>();
            return parameter
                .Property(propertyName)
                .LessEqual(value)
                .ToPredicate<T>(parameter);
        }

        public static Expression<Func<T, bool>> Starts<T>(string propertyName, object value)
        {
            var parameter = CreateParameter<T>();
            return parameter
                .Property(propertyName)
                .StartsWith(value)
                .ToPredicate<T>(parameter);
        }

        public static Expression<Func<T, bool>> Ends<T>(string propertyName, object value)
        {
            var parameter = CreateParameter<T>();
            return parameter
                .Property(propertyName)
                .EndsWith(value)
                .ToPredicate<T>(parameter);
        }

        public static Expression<Func<T, bool>> Contains<T>(string propertyName, object value)
        {
            var parameter = CreateParameter<T>();
            return parameter
                .Property(propertyName)
                .Contains(value)
                .ToPredicate<T>(parameter);
        }

        public static Expression<Func<T, bool>> ParsePredicate<T>(string propertyName, object value, Operator @operator)
        {
            var parameter = Expression.Parameter(typeof(T), "t");
            return parameter
                .Property(propertyName)
                .Operation(@operator, value)
                .ToPredicate<T>(parameter);
        }

        public static Dictionary<string, object> GetKeyValue<TModel>(this Expression<Func<TModel, TModel>> model)
        {
            var fieldList = new Dictionary<string, object>();

            var param = model.Body as MemberInitExpression;
            foreach (var item in param.Bindings)
            {
                object propertyValue;
                var memberAssignment = item as MemberAssignment;
                if (memberAssignment.Expression.NodeType == ExpressionType.Constant)
                {
                    propertyValue = (memberAssignment.Expression as ConstantExpression).Value;
                }
                else
                {
                    propertyValue = Expression.Lambda(memberAssignment.Expression, null).Compile().DynamicInvoke();
                }

                fieldList.Add(item.Member.Name, propertyValue);
            }

            return fieldList;
        }
    }
}