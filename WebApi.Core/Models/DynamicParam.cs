using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace WebAPI.Core.Models
{
    public class DynamicParam : ModelBase
    {
        public string Name { get; set; }
        public string DataType { get; set; }

        public DbType DataType2DbType
        {
            get
            {
                switch (DataType.ToUpper())
                {
                    case "VARCHAR":
                        return DbType.String;
                    case "NVARCHAR":
                        return DbType.String;
                    case "INT":
                        return DbType.Int32;
                    case "BIGINT":
                        return DbType.Int64;
                    case "DOUBLE":
                        return DbType.Double;
                    case "DECIMAL":
                        return DbType.Decimal;
                    case "DATETIME":
                        return DbType.DateTime;

                    default:
                        return DbType.Object;
                }
            }
        }

        public FluentData.DataTypes DataType2FluentDbType
        {
            get
            {
                switch (DataType.ToUpper())
                {
                    case "VARCHAR":
                        return FluentData.DataTypes.String;
                    case "NVARCHAR":
                        return FluentData.DataTypes.String;
                    case "INT":
                        return FluentData.DataTypes.Int32;
                    case "BIGINT":
                        return FluentData.DataTypes.Int64;
                    case "DOUBLE":
                        return FluentData.DataTypes.Double;
                    case "DECIMAL":
                        return FluentData.DataTypes.Decimal;
                    case "DATETIME":
                        return FluentData.DataTypes.DateTime;

                    default:
                        return FluentData.DataTypes.Object;
                }
            }
        }

        public ParamType ParamType { get; set; }

        /// <summary>
        /// 是否必填
        /// True|False
        /// </summary>
        public bool Required { get; set; }

        public object Value { get; set; }
    }

    public enum ParamType
    {
        IN = 0,
        OUT = 1
    }
}
