using Microsoft.Practices.EnterpriseLibrary.Data;
using WebAPI.Core.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using ThinkDev.FrameWork.Result;

namespace WebAPI.Core.DataAccess
{
    public class ApiAutomaticConfigDA : DBContextBase
    {

        public ApiAutomaticConfigDA()
        {
            ChangeConnection(ConnectionManager.Default);
        }

        /// <summary>
        /// 获取API配置
        /// </summary>
        /// <param name="action"></param>
        /// <param name="module"></param>
        /// <returns></returns>
        public ApiAutomaticConfig ConfigQuery(string action, string module)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT * FROM ApiAutomaticConfig WITH(NOLOCK) WHERE Action = @Action AND Module = @Module AND DelFlag=0");
            DbCommand cmd = CurrentDatabase.GetSqlStringCommand(sql.ToString());
            CurrentDatabase.AddInParameter(cmd, "@Action", DbType.String, action);
            CurrentDatabase.AddInParameter(cmd, "@Module", DbType.String, module);
            ApiAutomaticConfig info;
            using (System.Data.IDataReader dr = ExecuteDataReader(cmd))
            {
                info = GetObject<ApiAutomaticConfig>(dr);
            }
            return info;
        }

        public DataSet DGetDataSet(string connStr, string code, List<DynamicParam> paramList, out int output)
        {
            using (DbCommand cmd = CurrentDatabase.GetSqlStringCommand(code))
            {
                cmd.CommandTimeout = 120;
                foreach (DynamicParam p in paramList)
                {
                    if (p.ParamType == ParamType.OUT)
                    {
                        CurrentDatabase.AddOutParameter(cmd, "@" + p.Name, p.DataType2DbType, 11);
                    }
                    else
                    {
                        CurrentDatabase.AddInParameter(cmd, "@" + p.Name, p.DataType2DbType, p.Value);
                    }
                }
                CurrentDatabase.AddParameter(cmd, "RETURN_VALUE", System.Data.DbType.Int32, System.Data.ParameterDirection.ReturnValue, "", System.Data.DataRowVersion.Current, 0);
                output = 0;

                DataSet ds = ExecuteDataSet(cmd, connStr);
                DynamicParam outP = paramList.Find(f => f.ParamType == ParamType.OUT);
                if (outP != null)
                {
                    output = int.Parse(cmd.Parameters["@Count"].Value.ToString());
                }
                return ds;
            }
        }

        public DataTable DGetTable(string connStr, string code, List<DynamicParam> paramList, out int output)
        {
            using (DbCommand cmd = CurrentDatabase.GetSqlStringCommand(code))
            {
                cmd.CommandTimeout = 120;
                foreach (DynamicParam p in paramList)
                {
                    if (p.ParamType == ParamType.OUT)
                    {
                        CurrentDatabase.AddOutParameter(cmd, "@" + p.Name, p.DataType2DbType, 0);
                    }
                    else
                    {
                        CurrentDatabase.AddInParameter(cmd, "@" + p.Name, p.DataType2DbType, p.Value);
                    }
                }
                CurrentDatabase.AddParameter(cmd, "RETURN_VALUE", System.Data.DbType.Int32, System.Data.ParameterDirection.ReturnValue, "", System.Data.DataRowVersion.Current, 0);
                output = 0;

                DataTable dt = ExecuteDataTable(cmd, 0, connStr);
                DynamicParam outP = paramList.Find(f => f.ParamType == ParamType.OUT);
                if (outP != null)
                {
                    output = int.Parse(cmd.Parameters["@" + outP.Name].Value.ToString());
                }
                return dt;
            }
        }

        public dynamic DGet(string connStr, string code, List<DynamicParam> paramList, string returnType, out int output)
        {
            using (var context = Context(connStr))
            {
                //var cmd = context.StoredProcedure(code);
                var cmd = context.Sql(code);
                foreach (DynamicParam p in paramList)
                {
                    if (p.ParamType == ParamType.OUT)
                    {
                        cmd = cmd.ParameterOut(p.Name, FluentData.DataTypes.Int32);
                    }
                    else
                    {
                        cmd = cmd.Parameter(p.Name, p.Value);
                    }
                }
                dynamic info = null;
                if (returnType == "Object")
                {
                    info = cmd.QuerySingle<dynamic>();
                    if (info != null)
                    {
                        output = 1;
                    }
                    else
                    {
                        output = 0;
                    }
                }
                else
                {
                    info = cmd.QueryMany<dynamic>();
                    output = info.Count;
                }
                DynamicParam outP = paramList.Find(f => f.ParamType == ParamType.OUT);

                if (outP != null)
                {
                    output = cmd.ParameterValue<int>(outP.Name);
                }
                return info;
            }
        }

        public ResultInfo<string> DPost(string connStr, string code, List<DynamicParam> paramList)
        {
            using (DbCommand cmd = CurrentDatabase.GetSqlStringCommand(code))
            {
                string output = string.Empty;
                foreach (DynamicParam p in paramList)
                {
                    if (p.ParamType == ParamType.OUT)
                    {
                        CurrentDatabase.AddOutParameter(cmd, "@" + p.Name, p.DataType2DbType, 4000);
                    }
                    else
                    {
                        CurrentDatabase.AddInParameter(cmd, "@" + p.Name, p.DataType2DbType, p.Value);
                    }
                }
                CurrentDatabase.AddParameter(cmd, "RETURN_VALUE", System.Data.DbType.Int32, System.Data.ParameterDirection.ReturnValue, "", System.Data.DataRowVersion.Current, 0);

                int ret = ExecuteNonQuery(cmd, connStr);

                DynamicParam outP = paramList.Find(f => f.ParamType == ParamType.OUT);
                if (outP != null)
                {
                    output = cmd.Parameters["@" + outP.Name].Value.ToString();
                }
                ResultInfo<string> result = SetRullErr<string>(cmd.Parameters, output, 0);
                return result;
            }
        }

        //public ResultInfo<string> DPost(string connStr, string code, List<DynamicParam> paramList)
        //{
        //    using (var context = Context(connStr))
        //    {
        //        string output = string.Empty;
        //        var cmd = context.StoredProcedure(code);
        //        foreach (DynamicParam p in paramList)
        //        {
        //            if (p.ParamType == ParamType.OUT)
        //            {
        //                cmd = cmd.ParameterOut(p.Name, p.DataType2FluentDbType, 64);
        //            }
        //            else
        //            {
        //                cmd = cmd.Parameter(p.Name, p.Value);
        //            }
        //        }
        //        cmd.Data.Command.Parameter("ReturnValue", 0, FluentData.DataTypes.Int32, FluentData.ParameterDirection.ReturnValue, 11);
        //        int ret = cmd.Execute();
        //        int returnValue = cmd.Data.Command.ParameterValue<int>("ReturnValue");
        //        DynamicParam outP = paramList.Find(f => f.ParamType == ParamType.OUT);
        //        if (outP != null)
        //        {
        //            output = cmd.ParameterValue<string>(outP.Name);
        //        }
        //        if (returnValue == 0)
        //        {
        //            return new ResultInfo<string>(false, returnValue.ToString(), returnValue.ToString(), output);
        //        }
        //        else
        //        {
        //            return new ResultInfo<string>(true, returnValue.ToString(), returnValue.ToString(), "");
        //        }
        //    }
        //}
    }
}
