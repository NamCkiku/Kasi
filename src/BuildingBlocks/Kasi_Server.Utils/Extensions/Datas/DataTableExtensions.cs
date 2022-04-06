using Kasi_Server.Utils.Helpers;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Reflection;

namespace Kasi_Server.Utils.Extensions.Datas
{
    public static class DataTableExtensions
    {
        public static IList<T> ToRefList<T>(this DataTable dataTable) where T : new()
        {
            if (dataTable == null)
                throw new ArgumentNullException(nameof(dataTable), @"数据表不可为空！");

            var columnNames = dataTable.Columns.Cast<DataColumn>()
                .Select(c => c.ColumnName.ToLower())
                .ToList();

            var properties = typeof(T).GetProperties();

            return dataTable.AsEnumerable().Select(row =>
            {
                T objT = new T();

                foreach (var property in properties)
                {
                    if (columnNames.Contains(property.Name.ToLower()))
                    {
                        if (!property.CanWrite) continue;
                        var setter = property.GetSetMethod(true);
                        if (setter != null)
                        {
                            var value = row[property.Name] == DBNull.Value ? null : row[property.Name];
                            setter.Invoke(objT, new[] { value });
                        }
                    }
                }

                return objT;
            }).ToList();
        }

        public static List<T> ToList<T>(this DataTable dataTable)
        {
            return dataTable.ToList(typeof(List<T>)) as List<T>;
        }

        public static object ToList(this DataTable dataTable, Type returnType)
        {
            var isGenericType = returnType.IsGenericType;
            var underlyingType = isGenericType ? returnType.GenericTypeArguments.First() : returnType;

            var resultType = typeof(List<>).MakeGenericType(underlyingType);
            var list = Activator.CreateInstance(resultType);
            var addMethod = resultType.GetMethod("Add");

            var dataRows = dataTable.AsEnumerable();

            if (underlyingType.IsRichPrimitive())
            {
                foreach (var dataRow in dataRows)
                {
                    var firstColumnValue = dataRow[0];
                    var destValue = firstColumnValue?.ChangeType(underlyingType);
                    _ = addMethod.Invoke(list, new[] { destValue });
                }
            }
            else if (underlyingType == typeof(object))
            {
                var columns = dataTable.Columns;

                foreach (var dataRow in dataRows)
                {
                    var dic = new Dictionary<string, object>();
                    foreach (DataColumn column in columns)
                    {
                        dic.Add(column.ColumnName, dataRow[column]);
                    }
                    _ = addMethod.Invoke(list, new[] { dic });
                }
            }
            else
            {
                var dataColumns = dataTable.Columns;
                var properties = underlyingType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (var dataRow in dataRows)
                {
                    var model = Activator.CreateInstance(underlyingType);

                    foreach (var property in properties)
                    {
                        var columnName = property.Name;
                        if (property.IsDefined(typeof(ColumnAttribute), true))
                        {
                            var columnAttribute = property.GetCustomAttribute<ColumnAttribute>(true);
                            if (!string.IsNullOrWhiteSpace(columnAttribute.Name)) columnName = columnAttribute.Name;
                        }

                        if (!dataColumns.Contains(columnName)) continue;

                        var columnValue = dataRow[columnName];
                        if (columnValue == DBNull.Value) continue;

                        var destValue = columnValue?.ChangeType(property.PropertyType);
                        property.SetValue(model, destValue);
                    }

                    _ = addMethod.Invoke(list, new[] { model });
                }
            }

            return list;
        }

        internal static bool IsRichPrimitive(this Type type)
        {
            if (type.ToString().StartsWith(typeof(ValueTuple).FullName)) return false;

            if (type.IsArray) return type.GetElementType().IsRichPrimitive();

            if (type.IsPrimitive || type.IsValueType || type == typeof(string)) return true;

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>)) return type.GenericTypeArguments[0].IsRichPrimitive();

            return false;
        }
    }
}