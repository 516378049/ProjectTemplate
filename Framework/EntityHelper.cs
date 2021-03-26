using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    public static class EntityHelper
    {
        /// <summary>
        /// Convert Data Table to model list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static List<T> DataTableToList<T>(this DataTable source) where T : class, new()
        {
            List<T> itemlist = null;
            if (source == null || source.Rows.Count == 0)
            {
                return itemlist;
            }
            itemlist = new List<T>();
            T item = null;
            Type targetType = typeof(T);
            PropertyInfo[] props = targetType.GetProperties();

            DataRowCollection drs = source.Rows;
            int columnsCount = source.Columns.Count;
            foreach (DataRow dr in drs)
            {
                item = new T();
                for (int i = 0; i < columnsCount; i++)
                {
                    string colName = dr.Table.Columns[i].ColumnName;
                    PropertyInfo propertyInfo = SetPropertyInfo(colName, props, targetType);

                    if (propertyInfo != null && dr[i] != DBNull.Value)
                    {
                        TableColumnNameAttribute tba = (TableColumnNameAttribute)propertyInfo.GetCustomAttribute(typeof(TableColumnNameAttribute));

                        if (tba.ColType == ColumnType.Date)
                        {
                            propertyInfo.SetValue(item, Convert.ToDateTime(dr[i]).ToString("MM/dd/yyyy"), null);
                        }
                        else
                        {
                            propertyInfo.SetValue(item, dr[i].ToString(), null);
                        }
                    }
                }
                itemlist.Add(item);

            }
            return itemlist;
        }
        /// <summary>
        /// Set PropertyInfo
        /// </summary>
        /// <param name="colName"></param>
        /// <param name="props"></param>
        /// <param name="targetType"></param>
        /// <returns></returns>
        public static PropertyInfo SetPropertyInfo(string colName, PropertyInfo[] props, Type targetType)
        {
            PropertyInfo propertyInfo = null;
            foreach (var prop in props)
            {
                var customAttr = prop.GetCustomAttribute(typeof(TableColumnNameAttribute));
                if (customAttr != null)
                {
                    TableColumnNameAttribute tableCustomAttr = (TableColumnNameAttribute)customAttr;
                    if (tableCustomAttr.DBColName == colName)
                    {
                        propertyInfo = prop;
                        break;
                    }
                }
            }
            if (null == propertyInfo)
            {
                propertyInfo = targetType.GetProperty(colName);
            }
            return propertyInfo;
        }

        /// <summary>
        /// EntityCopy
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T EntityCopy<T, T1>(T1 source)
        {
            return JsonHelper.ToObject<T>(JsonHelper.ToJson(source));
        }

        /// <summary>
        /// 键值对集合转化为类
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TKeyVal"></typeparam>
        /// <param name="TKV"></param>
        /// <returns></returns>
        public static TEntity EntityCopyFromDic<TEntity, TKeyVal>(TKeyVal TKV, TEntity T = null) where TKeyVal : IEnumerable<KeyValuePair<string, object>> where TEntity : class, new()
        {
            if(T==null)
            {
                T = new TEntity();
            }
            foreach (KeyValuePair<string, object> KValue in TKV)
            {
                Type targetType = typeof(TEntity);

                PropertyInfo[] props = targetType.GetProperties();
                PropertyInfo propertyInfo = SetPropertyInfo(KValue.Key, props, targetType);

                if (propertyInfo != null && KValue.Value != DBNull.Value)
                {
                    TableColumnNameAttribute tba = (TableColumnNameAttribute)propertyInfo.GetCustomAttribute(typeof(TableColumnNameAttribute));

                    if (tba != null && tba.ColType == ColumnType.Date)
                    {
                        propertyInfo.SetValue(T, Convert.ToDateTime(KValue.Value).ToString("MM/dd/yyyy"), null);
                    }
                    else
                    {
                        if (propertyInfo.PropertyType.FullName.Contains("Int32"))
                        {
                            int _value = int.Parse(string.IsNullOrWhiteSpace(KValue.Value.ToString())?"0": KValue.Value.ToString());
                            propertyInfo.SetValue(T, _value, null);
                        }
                        else if (propertyInfo.PropertyType.FullName.Contains("DateTime"))
                        {
                            DateTime dt;
                            if (!string.IsNullOrWhiteSpace(KValue.Value.ToString()))
                            {
                                dt = Convert.ToDateTime(KValue.Value);
                                propertyInfo.SetValue(T, dt, null);
                            }
                        }
                        else
                        {
                            propertyInfo.SetValue(T, KValue.Value, null);
                        }
                    }
                    //if (KValue.Key == item.Name)
                    //{
                    //    item.SetValue(T, KValue.Value);
                    //    break;
                    //}
                }
            }
            return T;
        }
    }


    public class TableColumnNameAttribute : Attribute
    {
        public TableColumnNameAttribute() { }
        public TableColumnNameAttribute(string labelName)
        {
            this.labelName = labelName;
            this.dbColName = string.Empty;
            this.colType = ColumnType.String;
        }

        public TableColumnNameAttribute(string labelName, string dbColName)
        {
            this.labelName = labelName;
            this.dbColName = dbColName;
            this.colType = ColumnType.String;
        }


        public TableColumnNameAttribute(string labelName, ColumnType colType)
        {
            this.labelName = labelName;
            this.colType = colType;
            this.dbColName = string.Empty;
        }

        public TableColumnNameAttribute(string labelName, string dbColName, ColumnType colType)
        {
            this.labelName = labelName;
            this.dbColName = dbColName;
            this.colType = colType;
        }

        private string labelName;
        private string dbColName;
        private ColumnType colType;

        public string LabelName
        {
            get { return labelName; }
            set { value = labelName; }
        }
        public ColumnType ColType
        {
            get { return colType; }
            set { value = colType; }
        }
        public string DBColName
        {
            get { return dbColName; }
            set { value = dbColName; }
        }

    }
    public class NumberInfoAttribute : Attribute
    {
        public NumberInfoAttribute(int precision, float min, float max)
        {
            this.precision = precision;
            this.min = min;
            this.max = max;
        }

        private int precision;
        private float min;
        private float max;

        public int Precision
        {
            get { return precision; }
            set { value = precision; }
        }
        public float Min
        {
            get { return min; }
            set { value = min; }
        }
        public float Max
        {
            get { return max; }
            set { value = max; }
        }
    }
    public enum ColumnType
    {
        String,
        Number,
        NumRange,
        YN,
        Select,
        Date,
        MultiText
    }
}
