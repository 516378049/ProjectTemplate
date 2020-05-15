
using FluentData;
using System.Collections.Generic;

using ThinkDev.FrameWork.Result;

using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using Models;
using System.Text;
using Framework;
using System;
using System.Reflection;

namespace JR.NewTenancy.DataAccess.Common
{
    public class Utility : DBContextBase
    {
        //usage scenario:

        //for below function , we suggest fllow's scenario
        //1、ExcuteProEntpriseLib ： excute the sp  with returnValue
        //2、ExcuteSqlEntpriseLib ： excute the sql  with returnValue
        //3、getDataTableEntpriseLib ： excute the sp  with DataTable and  ReturnValue and outputs
        //4、getListEntpriseLibsql ： excute the sql  with List<> 

        #region FluentData
        /// <summary>
        /// excute SP by the framwork ThinkDev  just for the sp has no table to return 
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="ProcedureName"></param>
        /// <returns></returns>
        public ResultInfo<int> ExcutePro(string ProcedureName, Dictionary<string, object> dic)
        {
            using (IDbContext context = Context())
            {
                var cmd = context.StoredProcedure(ProcedureName);
                foreach (string str in dic.Keys)
                {
                    cmd.Parameter(str, dic[str]);
                }
                cmd.Data.Command.Parameter("ReturnValue", 0, DataTypes.Int32, FluentData.ParameterDirection.ReturnValue, 11);
                cmd.ParameterOut("Count", DataTypes.Int32, 100);
                cmd.Execute(); 
                int ReturnValue = cmd.Data.Command.ParameterValue<int>("ReturnValue");
                int Count = cmd.Data.Command.ParameterValue<int>("Count");
                ResultInfo<int> result = SetRullErr<int>(ReturnValue.ToString(), ReturnValue, 0);
                return result;
            }
        }
        /// <summary>
        /// get entity list 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dic"></param>
        /// <param name="ProcedureName"></param>
        /// <returns></returns>
        public ResultInfo<List<T>> getList<T>( string ProcedureName, Dictionary<string, object> dic)
        {
            List<T> list = new List<T>();
            using (var context = Context())
            {
                var cmd = context.StoredProcedure(ProcedureName)
                .ParameterOut("Count", DataTypes.Int32, 100);
                cmd.Data.Command.Parameter("ReturnValue", 0, DataTypes.Int32, FluentData.ParameterDirection.ReturnValue, 11);
                foreach (string str in dic.Keys)
                {
                    cmd.Parameter(str, dic[str]);
                }
                list = cmd.QueryMany<T>();
                int count = cmd.ParameterValue<int>("Count");
                int ReturnValue = cmd.Data.Command.ParameterValue<int>("ReturnValue");
                ResultInfo<List<T>> result = new ResultInfo<List<T>>(false, "", count.ToString(), list);
                return result;
            }
        }
        /// <summary>
        /// get dataTable  return the default dataTable, if want to get MultiTable please use the function MultiResultSql 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dic"></param>
        /// <param name="ProcedureName"></param>
        /// <returns></returns>
        public ResultInfo<List<DataTable>> getList(string ProcedureName, Dictionary<string, object> dic)
        {
            List<DataTable> list = new List<DataTable>();
            using (var context = Context())
            {
                var cmd = context.StoredProcedure(ProcedureName)
                .ParameterOut("Count", DataTypes.Int32, 100);
                cmd.Data.Command.Parameter("ReturnValue", 0, DataTypes.Int32, FluentData.ParameterDirection.ReturnValue, 11);
                foreach (string str in dic.Keys)
                {
                    cmd.Parameter(str, dic[str]);
                }
                list = cmd.QueryMany<DataTable>();
                int count = cmd.ParameterValue<int>("Count");
                int ReturnValue = cmd.Data.Command.ParameterValue<int>("ReturnValue");
                ResultInfo<List<DataTable>> result = new ResultInfo<List<DataTable>>(false, "", count.ToString(), list);
                return result;
            }
        }
        #endregion

        #region SQL.data.sqlclient
        /// <summary>
        /// get dataSet by exc sp
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="spName"></param>
        /// <returns></returns>
        public ResultInfo<DataSet> getDataTableMSsql(string spName, Dictionary<string, object> dic)
        {
            DataSet st = new DataSet();
            using (var sqnCon = new SqlConnection(ConnectionManager.NEW_TENANCY))
            {
                SqlCommand cmd = new SqlCommand();
                foreach (string str in dic.Keys)
                {
                    cmd.Parameters.Add(new SqlParameter(str, dic[str]));
                }
                cmd.Parameters.Add(new SqlParameter("ReturnValue", SqlDbType.Int, 10, System.Data.ParameterDirection.ReturnValue, true, 10, 2, null, DataRowVersion.Default, null));
                cmd.Parameters.Add(new SqlParameter("@Count", SqlDbType.Int, 10, System.Data.ParameterDirection.Output, true, 10, 2, null, DataRowVersion.Default, null));
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = sqnCon;
                cmd.CommandText = spName;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(st);
                int count = int.Parse(cmd.Parameters["@Count"].Value.ToString());
                int ReturnValue = int.Parse(cmd.Parameters["ReturnValue"].Value.ToString());
            }
            return new ResultInfo<DataSet>(false, "", "0", st);
        }
        /// <summary>
        /// exc sp and get the return value
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="spName"></param>
        /// <returns></returns>
        public ResultInfo<int> ExcuteProMSsql( string spName,Dictionary<string, object> dic)
        {
            using (var sqnCon = new SqlConnection(ConnectionManager.NEW_TENANCY))
            {
                sqnCon.Open();
                SqlCommand cmd = new SqlCommand();
                foreach (string str in dic.Keys)
                {
                    cmd.Parameters.Add(new SqlParameter(str, dic[str]));
                }
                cmd.Parameters.Add(new SqlParameter("ReturnValue", SqlDbType.Int, 10, System.Data.ParameterDirection.ReturnValue, true,10,2, null, DataRowVersion.Default,null));
                cmd.Parameters.Add(new SqlParameter("@Count", SqlDbType.Int, 10, System.Data.ParameterDirection.Output, true, 10, 2, null, DataRowVersion.Default, null));
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = sqnCon;
                cmd.CommandText = spName;
                cmd.ExecuteNonQuery();
                int count = int.Parse(cmd.Parameters["@Count"].Value.ToString());
                int ReturnValue = int.Parse(cmd.Parameters["ReturnValue"].Value.ToString());
                ResultInfo<int> result = SetRullErr(ReturnValue.ToString(), ReturnValue, 0);
                return result;
            }
        }
        #endregion

        #region EnterpriseLibrary
        /// <summary>
        /// exect sp
        /// </summary>
        /// <param name="sProcName"></param>
        /// <param name="dicParams"></param>
        /// <returns></returns>
        public ResultInfo<int> ExcuteProEntpriseLib(string sProcName,Dictionary<string, object> dicParams)
        {
            using (DbCommand cmd = CurrentDatabase.GetStoredProcCommand(sProcName))
            {
                foreach (string str in dicParams.Keys)
                {
                    CurrentDatabase.AddInParameter(cmd, str,DbType.Object, dicParams[str]);
                }
                CurrentDatabase.AddParameter(cmd, "ReturnValue", System.Data.DbType.Int32, System.Data.ParameterDirection.ReturnValue, "", System.Data.DataRowVersion.Current, 0);
                CurrentDatabase.AddOutParameter(cmd,"@Count", DbType.Int32, 10);

                
                base.ExecuteNonQuery(cmd);//use thinkDev.data.dll to excute sp  
                //CurrentDatabase.ExecuteNonQuery(cmd);

                int count = int.Parse(cmd.Parameters["@Count"].Value.ToString());
                int ReturnValue = int.Parse(cmd.Parameters["@ReturnValue"].Value.ToString());

                ResultInfo<int> result = SetRullErr(ReturnValue.ToString(), ReturnValue, 0);
                return result;
            }
        }

        /// <summary>
        /// exect sql
        /// </summary>
        /// <param name="sProcName"></param>
        /// <param name="dicParams"></param>
        /// <returns></returns>
        public ResultInfo<int> ExcuteSqlEntpriseLib(string sql, Dictionary<string, object> dicParams)
        {
            using (DbCommand cmd = CurrentDatabase.GetSqlStringCommand(sql))
            {
                foreach (string str in dicParams.Keys)
                {
                    CurrentDatabase.AddInParameter(cmd, str, DbType.Object, dicParams[str]);
                }
                CurrentDatabase.AddParameter(cmd, "ReturnValue", System.Data.DbType.Int32, System.Data.ParameterDirection.ReturnValue, "", System.Data.DataRowVersion.Current, 0);
                base.ExecuteNonQuery(cmd);//use thinkDev.data.dll to excute sp  
                //CurrentDatabase.ExecuteNonQuery(cmd);
                int ReturnValue = int.Parse(cmd.Parameters["@ReturnValue"].Value.ToString());

                ResultInfo<int> result = SetRullErr(ReturnValue.ToString(), ReturnValue, 0);
                return result;
            }
        }

        /// <summary>
        /// get dataSet
        /// </summary>
        /// <param name="sProcName"></param>
        /// <param name="dicParams"></param>
        /// <returns></returns>
        public ResultInfo<DataTable> getDataTableEntpriseLib(string sProcName, Dictionary<string, object> dicParams)
        {
            using (DbCommand cmd = CurrentDatabase.GetStoredProcCommand(sProcName))
            {
                foreach (string str in dicParams.Keys)
                {
                    CurrentDatabase.AddInParameter(cmd, str, DbType.Object, dicParams[str]);
                }
                CurrentDatabase.AddParameter(cmd, "ReturnValue", System.Data.DbType.Int32, System.Data.ParameterDirection.ReturnValue, "", System.Data.DataRowVersion.Current, 0);
                CurrentDatabase.AddOutParameter(cmd, "@Count", DbType.Int32, 10);

                DataTable st = base.ExecuteDataTable(cmd);//use thinkDev.data.dll to excute sp  
                //CurrentDatabase.ExecuteDataSet(cmd);
                int count = int.Parse(cmd.Parameters["@Count"].Value.ToString());
                int ReturnValue = int.Parse(cmd.Parameters["@ReturnValue"].Value.ToString());
                ResultInfo<DataTable> result = new ResultInfo<DataTable>(false, "", "0", st);
                return result;
            }
        }

        /// <summary>
        /// get dataSet
        /// </summary>
        /// <param name="sProcName"></param>
        /// <param name="dicParams"></param>
        /// <returns></returns>
        public ResultInfo<DataSet> getDataSetEntpriseLib(string sProcName, Dictionary<string, object> dicParams)
        {
            using (DbCommand cmd = CurrentDatabase.GetStoredProcCommand(sProcName))
            {
                foreach (string str in dicParams.Keys)
                {
                    CurrentDatabase.AddInParameter(cmd, str, DbType.Object, dicParams[str]);
                }
                CurrentDatabase.AddParameter(cmd, "ReturnValue", System.Data.DbType.Int32, System.Data.ParameterDirection.ReturnValue, "", System.Data.DataRowVersion.Current, 0);
                CurrentDatabase.AddOutParameter(cmd, "@Count", DbType.Int32, 10);

                DataSet st = base.ExecuteDataSet(cmd);//use thinkDev.data.dll to excute sp  
                //CurrentDatabase.ExecuteDataSet(cmd);
                int count = int.Parse(cmd.Parameters["@Count"].Value.ToString());
                int ReturnValue = int.Parse(cmd.Parameters["@ReturnValue"].Value.ToString());
                ResultInfo<DataSet> result = new ResultInfo<DataSet>(false, "", "0", st);
                return result;
            }
        }


        /// <summary>
        /// get dataSet by sql
        /// </summary>
        /// <param name="sProcName"></param>
        /// <param name="dicParams"></param>
        /// <returns></returns>
        public ResultInfo<DataSet> getDataTableEntpriseLibSql(string sql, Dictionary<string, object> dicParams)
        {
            using (DbCommand cmd = CurrentDatabase.GetSqlStringCommand(sql))
            {
                foreach (string str in dicParams.Keys)
                {
                    CurrentDatabase.AddInParameter(cmd, str, DbType.Object, dicParams[str]);
                }

                DataSet st = base.ExecuteDataSet(cmd);//use thinkDev.data.dll to excute sp  
                //CurrentDatabase.ExecuteDataSet(cmd);
                ResultInfo<DataSet> result = new ResultInfo<DataSet>(false, "", "0", st);
                return result;
            }
        }

        /// <summary>
        /// getList
        /// </summary>
        /// <param name="dicParams"></param>
        /// <param name="sProcName"></param>
        /// <returns></returns>
        public ResultInfo<List<SysUserInfo>> getListEntpriseLib( string sProcName, Dictionary<string, object> dicParams)
        {
            List<SysUserInfo> list = new List<SysUserInfo>();

            using (DbCommand cmd = CurrentDatabase.GetStoredProcCommand(sProcName))
            {
                foreach (string str in dicParams.Keys)
                {
                    CurrentDatabase.AddInParameter(cmd, str, DbType.Object, dicParams[str]);
                }
                //CurrentDatabase.AddParameter(cmd, "ReturnValue", System.Data.DbType.Int32, System.Data.ParameterDirection.ReturnValue, "", System.Data.DataRowVersion.Current, 0);
                CurrentDatabase.AddOutParameter(cmd, "@Count", DbType.Int32, 10);
               

                System.Data.IDataReader dr = ExecuteDataReader(cmd);//use thinkDev.data.dll to excute sp  
                //System.Data.IDataReader dr = CurrentDatabase.ExecuteReader(cmd);
                //int count = int.Parse(cmd.Parameters["@Count"].Value.ToString());
                //int ReturnValue = int.Parse(cmd.Parameters["ReturnValue"].Value.ToString());
                list = GetObjectList<SysUserInfo>(dr);
                
                ResultInfo<List<SysUserInfo>> result = new ResultInfo<List<SysUserInfo>>(false, "", "0", list);
                return result;
            }
        }
        /// <summary>
        /// getList by sql
        /// </summary>
        /// <param name="dicParams"></param>
        /// <param name="sProcName"></param>
        /// <returns></returns>
        public ResultInfo<List<SysUserInfo>> getListEntpriseLibsql(string sql, Dictionary<string, object> dicParams)
        {
            List<SysUserInfo> list = new List<SysUserInfo>();

            using (DbCommand cmd = CurrentDatabase.GetSqlStringCommand(sql))
            {
                foreach (string str in dicParams.Keys)
                {
                    CurrentDatabase.AddInParameter(cmd, str, DbType.Object, dicParams[str]);
                }
                CurrentDatabase.AddParameter(cmd, "ReturnValue", System.Data.DbType.Int32, System.Data.ParameterDirection.ReturnValue, "", System.Data.DataRowVersion.Current, 0);
                CurrentDatabase.AddOutParameter(cmd, "@Count", DbType.Int32, 10);
               

                System.Data.IDataReader dr = ExecuteDataReader(cmd);//use thinkDev.data.dll to excute sp  
                //System.Data.IDataReader dr = CurrentDatabase.ExecuteReader(cmd);
                //int ReturnValue = int.Parse(cmd.Parameters["@ReturnValue"].Value.ToString());
                list = GetObjectList<SysUserInfo>(dr);

                ResultInfo<List<SysUserInfo>> result = new ResultInfo<List<SysUserInfo>>(false, "", "0", list);
                return result;
            }
        }
        #endregion

        #region 返回邮件Body
        /// <summary>
        /// get content of MailBody 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public static string getMailBody( System.Data.DataTable data,string title)
        {
            string MailBody = "<p style=\"font-size: 10pt\">"+ title + "</p><table cellspacing=\"1\" cellpadding=\"3\" border=\"0\" style=\"font-size: 10pt;line-height: 15px;\">";
            MailBody += "<div align=\"center\">";
            MailBody += "<tr bgcolor=\"#d9edf7\">";
            for (int hcol = 0; hcol < data.Columns.Count; hcol++)
            {
                MailBody += "<th>&nbsp;&nbsp;&nbsp;";
                MailBody += data.Columns[hcol].ColumnName;
                MailBody += "&nbsp;&nbsp;&nbsp;</th>";
            }
            MailBody += "</tr>";

            for (int row = 0; row < data.Rows.Count; row++)
            {
                if(row % 2==0)
                {
                    MailBody += "<tr style=\"background-color:aliceblue\">"; 
                }
                else
                {
                    MailBody += "<tr>";
                }
                
                for (int col = 0; col < data.Columns.Count; col++)
                {
                    MailBody += "<td >&nbsp;&nbsp;&nbsp;";
                    MailBody += data.Rows[row][col].ToString();
                    MailBody += "&nbsp;&nbsp;&nbsp;</td>";
                }
                MailBody += "</tr>";
            }
            MailBody += "</table>";
            MailBody += "<p style=\"font-size: 10pt\">此邮件为系统自动发送，请勿直接回复，谢谢。</p>";
            MailBody += "</div>";
            return MailBody;
        }
        #endregion 
    }
}