using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;

namespace PIS.AutoEnt
{
    public static class DataExtensions
    {
        #region 枚举转换

        public static EntityState ToEntityState(this EntityObjectState state)
        {
            EntityState entityState = EntityState.Unchanged;

            switch (state)
            {
                case EntityObjectState.Added:
                    entityState = EntityState.Added;
                    break;
                case EntityObjectState.Modified:
                    entityState = EntityState.Modified;
                    break;
                case EntityObjectState.Deleted:
                    entityState = EntityState.Deleted;
                    break;
            }

            return entityState;
        }

        #endregion

        #region DataSet, DataTable 转换

        /// <summary>
        /// DataSet ToList方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataset"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(this DataSet dataset)
            where T : new()
        {
            return ToList<T>(dataset, null);
        }

        /// <summary>
        /// DataSet ToList方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataset"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(this DataSet dataset, string tableName) where T : new()
        {
            var entityList = new List<T>();

            if (dataset == null || dataset.Tables.Count <= 0)
                return entityList;

            var dataTable = GetDataTableByName(dataset, tableName);

            return ToList<T>(dataTable);
        }

        /// <summary>
        /// DataTable ToList方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(this DataTable dataTable)
           where T : new()
        {
            var entityList = new List<T>();

            if (dataTable == null) return entityList;

            var type = typeof(T);
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            // key: property name 
            // value: column index
            var propNameMappingDic = GetPropertyIndexMapping(dataTable, properties);

            for (int rowIndex = 0; rowIndex < dataTable.Rows.Count; rowIndex++)
            {
                var target = new T();
                var dr = dataTable.Rows[rowIndex];
                if (dr.RowState == DataRowState.Deleted) continue;

                foreach (PropertyInfo pi in properties)
                {
                    if (propNameMappingDic.ContainsKey(pi.Name))
                    {
                        var columnIndex = propNameMappingDic[pi.Name];
                        var instance = dr[columnIndex];
                        if (instance != DBNull.Value)
                        {
                            SetProperty<T>(target, pi, instance);
                        }
                    }
                }

                entityList.Add(target);
            }

            return entityList;
        }

        /// <summary>
        /// 设置指定对象属性值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <param name="pi"></param>
        /// <param name="instance"></param>
        private static void SetProperty<T>(T target, PropertyInfo pi, object instance) where T : new()
        {
            var revisedValue = default(object);
            var targetType = pi.PropertyType;

            if (targetType != typeof(String))
            {
                if (targetType.IsGenericType && targetType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                {
                    NullableConverter nullableConverter = new NullableConverter(targetType);
                    targetType = nullableConverter.UnderlyingType;
                    if (!instance.Equals(string.Empty))
                    {
                        revisedValue = Convert.ChangeType(instance, targetType);
                    }
                }
                else if (targetType.IsEnum)
                {
                    if (instance is System.String || instance is System.Int32)
                        revisedValue = System.Enum.Parse(targetType, instance.ToString());
                }

                else if (targetType.IsValueType)
                {
                    if (instance != null && !string.IsNullOrWhiteSpace(instance.ToString())) revisedValue = Convert.ChangeType(instance, targetType);
                }
            }
            else
                revisedValue = instance;

            pi.SetValue(target, revisedValue, null);
        }

        /// <summary>
        /// 获取对象Properties与DataTable中字段对应关系
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="properties"></param>
        /// <returns></returns>
        private static Dictionary<string, int> GetPropertyIndexMapping(DataTable dataTable, PropertyInfo[] properties)
        {
            var mapping = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            var columns = dataTable.Columns;

            foreach (var prop in properties)
            {
                if (prop.CanWrite)
                {
                    for (int cIndex = 0; cIndex < columns.Count; cIndex++)
                    {
                        if (String.Equals(prop.Name, columns[cIndex].ColumnName, StringComparison.OrdinalIgnoreCase))
                        {
                            mapping[prop.Name] = cIndex;

                            break;
                        }
                    }
                }

            }

            return mapping;
        }

        /// <summary>
        /// 由表名从DataSet中获取DataTable
        /// </summary>
        /// <param name="dataset"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        internal static DataTable GetDataTableByName(DataSet dataset, string tableName)
        {
            int tableIndex = -1;

            if (!string.IsNullOrWhiteSpace(tableName))
            {
                for (int i = 0; i < dataset.Tables.Count; i++)
                {
                    if (String.Equals(tableName, dataset.Tables[i].TableName, StringComparison.OrdinalIgnoreCase))
                    {
                        tableIndex = i;
                        break;
                    }
                }
            }
            else
            {
                tableIndex = 0;
            }

            if (tableIndex == -1) return null;

            return dataset.Tables[tableIndex];

        }

        #endregion
    }
}
