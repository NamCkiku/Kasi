using Kasi_Server.Utils.Extensions;
using Kasi_Server.Utils.Helpers;
using System.Linq.Expressions;

namespace Kasi_Server.Utils.Expressions
{
    public class PredicateExpressionBuilder<TEntity>
    {
        private readonly ParameterExpression _parameter;

        private Expression _result;

        public PredicateExpressionBuilder()
        {
            _parameter = Expression.Parameter(typeof(TEntity), "t");
        }

        public ParameterExpression GetParameter()
        {
            return _parameter;
        }

        public void Append<TProperty>(Expression<Func<TEntity, TProperty>> propertyExpression, Operator @operator,
            object value)
        {
            _result = _result.And(_parameter.Property(Lambda.GetMember(propertyExpression))
                .Operation(@operator, value));
        }

        public void Append<TProperty>(Expression<Func<TEntity, TProperty>> propertyExpression, Operator @operator,
            Expression value)
        {
            _result = _result.And(_parameter.Property(Lambda.GetMember(propertyExpression))
                .Operation(@operator, value));
        }

        public void Append(string property, Operator @operator, object value)
        {
            _result = _result.And(_parameter.Property(property).Operation(@operator, value));
        }

        public void Append(string property, Operator @operator, Expression value)
        {
            _result = _result.And(_parameter.Property(property).Operation(@operator, value));
        }

        public void Clear()
        {
            _result = null;
        }

        public Expression<Func<TEntity, bool>> ToLambda()
        {
            return _result.ToLambda<Func<TEntity, bool>>(_parameter);
        }
    }
}