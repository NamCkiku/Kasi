using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;

namespace Kasi_Server.Utils.Helpers
{
    public class ValidateHelper<T> where T : class
    {
        private Dictionary<string, List<ValidationAttribute>> dicValidations = new Dictionary<string, List<ValidationAttribute>>();

        private Dictionary<string, string> dErrors = new Dictionary<string, string>();

        private bool isIDataErrorInfo;

        private T validateObj;

        public ValidateHelper(T obj)
        {
            RegisterEvent(obj);
        }

        public ValidateHelper(T obj, Dictionary<string, string> errors)
        {
            RegisterEvent(obj);
            Register(errors);
        }

        public ValidateHelper<T> Register(Expression<Func<T, object>> expr, List<ValidationAttribute> metadatas)
        {
            var prpName = GetExprName(expr);
            if (string.IsNullOrEmpty(prpName)) return this;
            if (dicValidations.ContainsKey(prpName))
            {
                dicValidations[prpName].AddRange(metadatas);
            }
            else
            {
                dicValidations.Add(prpName, metadatas);
            }
            return this;
        }

        public ValidateHelper<T> Register(Expression<Func<T, object>> expr, ValidationAttribute metadata)
        {
            var prpName = GetExprName(expr);
            if (string.IsNullOrEmpty(prpName))
                return this;
            var key = prpName;
            if (dicValidations.ContainsKey(key))
            {
                var metadatas = dicValidations[key];
                metadatas.Add(metadata);
            }
            else
            {
                var list = new List<ValidationAttribute>();
                list.Add(metadata);
                dicValidations.Add(key, list);
            }
            return this;
        }

        public ValidateHelper<T> Register(Dictionary<string, string> dataErrors)
        {
            dErrors = dataErrors;
            return this;
        }

        public ValidateHelper<T> Register()
        {
            var properties = typeof(T).GetProperties();
            var baseType = typeof(ValidationAttribute);
            var metadatas = new List<ValidationAttribute>();
            foreach (var property in properties)
            {
                var atts = property.GetCustomAttributes(false);
                foreach (var att in atts)
                {
                    if (att.GetType().IsSubclassOf(baseType))
                    {
                        metadatas.Add((ValidationAttribute)att);
                    }
                }
                dicValidations.Add(property.Name, metadatas);
            }
            return this;
        }

        public void UnRegister()
        {
            if (validateObj != null)
            {
                var type = validateObj.GetType();
                if (typeof(INotifyPropertyChanged).IsAssignableFrom(type))
                {
                    ((INotifyPropertyChanged)validateObj).PropertyChanged -= PropertyChanged;
                }
            }
        }

        public bool Validate()
        {
            if (isIDataErrorInfo && dErrors != null)
            {
                if (dErrors.Count > 0)
                    return false;
            }

            var notify = GetPropertyChangedMethod(validateObj);

            var erroCount = 0;
            foreach (var key in dicValidations.Keys)
            {
                if (dErrors != null)
                    erroCount = dErrors.Count;
                if (notify != null)
                {
                    notify.Invoke(validateObj, new object[] { key });

                    if (dErrors != null && dErrors.Count != erroCount)
                    {
                        notify.Invoke(validateObj, new object[] { key });
                    }
                }
                else
                {
                    PropertyChanged(validateObj, new PropertyChangedEventArgs(key));
                }
            }

            if (isIDataErrorInfo && dErrors != null)
            {
                if (dErrors.Count > 0)
                    return false;
            }
            return true;
        }

        protected virtual void PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var value = GetPropertyValue(sender, e.PropertyName);
            if (dicValidations.ContainsKey(e.PropertyName))
            {
                var context = new ValidationContext(validateObj, null, null) { MemberName = e.PropertyName };
                var metadatas = dicValidations[e.PropertyName];
                foreach (var metadata in metadatas)
                {
                    try
                    {
                        metadata.Validate(value, context);
                    }
                    catch (Exception ex)
                    {
                        if (isIDataErrorInfo && dErrors != null)
                        {
                            if (dErrors.ContainsKey(e.PropertyName))
                            {
                                dErrors[e.PropertyName] = ex.Message;
                            }
                            else
                            {
                                dErrors.Add(e.PropertyName, ex.Message);
                            }
                            return;
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                if (isIDataErrorInfo && dErrors != null && dErrors.ContainsKey(e.PropertyName))
                {
                    dErrors.Remove(e.PropertyName);
                }
            }
        }

        private void RegisterEvent(T obj)
        {
            validateObj = obj;
            var type = obj.GetType();
            if (typeof(INotifyPropertyChanged).IsAssignableFrom(type))
            {
                ((INotifyPropertyChanged)obj).PropertyChanged += PropertyChanged;
            }
            isIDataErrorInfo = typeof(IDataErrorInfo).IsAssignableFrom(type);
        }

        private string GetExprName(Expression<Func<T, object>> expr)
        {
            string prpName;
            switch (expr.Body.NodeType)
            {
                case ExpressionType.MemberAccess:
                    prpName = ((MemberExpression)expr.Body).Member.Name;
                    break;

                case ExpressionType.Convert:
                    prpName = ((MemberExpression)((UnaryExpression)expr.Body).Operand).Member.Name;
                    break;

                default:
                    prpName = string.Empty;
                    break;
            }
            return prpName;
        }

        private object GetPropertyValue(object sender, string propertyName)
        {
            var property = sender.GetType().GetProperty(propertyName);
            if (property != null)
            {
                return property.GetValue(sender, null);
            }
            return null;
        }

        private MethodInfo GetPropertyChangedMethod(object sender)
        {
            var methods = sender.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance);
            foreach (var method in methods)
            {
                if (method.Name.Contains("PropertyChanged") && !method.Name.Contains("add_") && !method.Name.Contains("remove_"))
                {
                    var parameters = method.GetParameters();
                    if (parameters.Length == 1 && parameters[0].ParameterType == typeof(string))
                    {
                        return method;
                    }
                }
            }
            return null;
        }

        private object GetEventMember(object bindableObject)
        {
            var bindableObjectType = bindableObject.GetType();
            FieldInfo propChangedFieldInfo = null;
            while (bindableObjectType != null)
            {
                propChangedFieldInfo = bindableObjectType.GetField("PropertyChanged", BindingFlags.Instance | BindingFlags.NonPublic);
                if (propChangedFieldInfo != null)
                    break;
                bindableObjectType = bindableObjectType.BaseType;
            }

            if (propChangedFieldInfo == null) return null;
            var fieldValue = propChangedFieldInfo.GetValue(bindableObject);
            if (fieldValue == null)
                return null;

            return null;
        }
    }
}