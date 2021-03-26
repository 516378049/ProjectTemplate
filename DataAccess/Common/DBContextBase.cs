/******************************************************************
 * 功能描述：数据层基类，所有DA对象均需继承该基类
 * 创建时间：2015-03-04
 * 作者：Ray
 * 版本：1.0
 * 修订描述：
 * 最后修订日期：
 ******************************************************************/
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FluentData;
using ThinkDev.Data;
using ThinkDev.Data.MSSql;
using ThinkDev.FrameWork.Result;
using ThinkDev.Logging;
using IDataReader = System.Data.IDataReader;
using ParameterDirection = System.Data.ParameterDirection;
using Model.Common;
using Model;

namespace JR.NewTenancy.DataAccess
{
    public class DBContextBase : DBContext
    {
       //Logger logger = LoggerFactory.GetLogger("StorageLogger");

        static DBContextBase()
        {
            ModelTableMapping.InitMapping();
        }
        public DBContextBase()
        {
            base.OnExecuteDataSet = Inline_ExecuteDataSet;
            base.OnExecuteDataTable = Inline_ExecuteDataTable;
            base.OnExecuteNonQuery = Inline_ExecuteNonQuery;
            base.OnExecuteScalar = Inline_ExecuteScalar;
            base.OnExecuteDataReader = Inline_ExecuteDataReader;
            //base.OnDebug = logger.Debug;
            //base.OnInfo = logger.Info;
            //base.OnWarn = logger.Warn;
            //base.OnError = logger.Error;
            base.CurrentConnection = ConnectionManager.NEW_TENANCY;
            CurrentDatabase = CreateDatabase(CurrentConnection);
        }

        #region get the Context of Fluent
        /// <summary>
        /// 获得Fluent的Context
        /// </summary>
        /// <returns></returns>
        protected IDbContext Context()
        {
            var context = new DbContext().ConnectionString(CurrentConnection, new SqlServerProvider());
            return context.IgnoreIfAutoMapFails(true);
        }
        #endregion

        #region get Command of sqlClient 
        /// <summary>
        /// 生成Sql语句的Command
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        protected DbCommand CreateSqlStringCommand(string commandText)
        {
            return new SqlCommand(commandText);
        }
        #endregion

        #region get the Database of EnterpriseLibrary
        //当前访问实例数据库操作对象(EnterpriseLibrary)
        protected Database CurrentDatabase { get; set; }

        /// <summary>
        /// 统一的Database入口
        /// </summary>
        /// <param name="connString"></param>
        /// <returns></returns>
        protected Database CreateDatabase(string connString)
        {
            return new SqlDatabase(connString);
        }

        /// <summary>
        /// get DataBase of EnterpriseLibrary (变更当前(Fluent)对象连接字符串)
        /// </summary>
        /// <param name="conn"></param>
        protected void ChangeConnection(string conn)
        {
            CurrentDatabase = CreateDatabase(conn);
        }
        #endregion

        #region 数据库操作方法具体实现(EnterpriseLibrary)

        private int Inline_ExecuteNonQuery(string conn, DbCommand cmd)
        {
            if (conn != CurrentConnection)
            {
                CurrentDatabase = CreateDatabase(conn);
            }
            if (CurrentDatabase == null)
            {
                CurrentDatabase = CreateDatabase(CurrentConnection);
            }
            return CurrentDatabase.ExecuteNonQuery(cmd);
        }

        private DataTable Inline_ExecuteDataTable(string conn, DbCommand cmd, int tableIndex = 0)
        {
            if (conn != CurrentConnection)
            {
                CurrentDatabase = CreateDatabase(conn);
            }
            if (CurrentDatabase == null)
            {
                CurrentDatabase = CreateDatabase(CurrentConnection);
            }
            DataSet ds = CurrentDatabase.ExecuteDataSet(cmd);
            if (ds.Tables.Count > tableIndex)
            {
                return ds.Tables[tableIndex];
            }
            return null;
        }

        private DataSet Inline_ExecuteDataSet(string conn, DbCommand cmd)
        {
            if (conn != CurrentConnection)
            {
                CurrentDatabase = CreateDatabase(conn);
            }
            if (CurrentDatabase == null)
            {
                CurrentDatabase = CreateDatabase(CurrentConnection);
            }
            return CurrentDatabase.ExecuteDataSet(cmd);
        }

        private IDataReader Inline_ExecuteDataReader(string conn, DbCommand cmd)
        {
            if (conn != CurrentConnection)
            {
                CurrentDatabase = CreateDatabase(conn);
            }
            if (CurrentDatabase == null)
            {
                CurrentDatabase = CreateDatabase(CurrentConnection);
            }
            return CurrentDatabase.ExecuteReader(cmd);
        }

        private object Inline_ExecuteScalar(string conn, DbCommand cmd)
        {
            if (conn != CurrentConnection)
            {
                CurrentDatabase = CreateDatabase(conn);
            }
            if (CurrentDatabase == null)
            {
                CurrentDatabase = CreateDatabase(CurrentConnection);
            }
            return CurrentDatabase.ExecuteScalar(cmd);
        }

        #endregion


        //thinkDev transfer to EnterpriseLibrary
        #region 获取当前环境的SqlBuilder (ThinkDev)
        /// <summary>
        /// 获取当前环境的SqlBuilder
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        protected ISqlBuilder CreateSqlBuilder(string tableName)
        {
            return SqlBuilder.From(tableName);
        }
        /// <summary>
        /// 获取当前环境的SqlBuilder
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"></param>
        /// <returns></returns>
        protected ISqlBuilder<T> CreateSqlBuilder<T>(string tableName)
        {
            return SqlBuilder<T>.From(tableName);
        }
        /// <summary>
        /// 获取当前环境的SqlBuilder
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        protected ISqlBuilder<T> CreateSqlBuilder<T>(T item, string tableName)
        {
            return SqlBuilder<T>.From(item, tableName);
        }

        /// <summary>
        /// 提供Linq查询功能入口
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected IObjectQuery<T> From<T>() where T : class, new()
        {
            return new ThinkDev.Data.MSSql.ObjectQuery<T>(this);
        }

        /// <summary>
        /// 提供Linq查询功能入口
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected IObjectQuery<T> From<T>(T item) where T : class, new()
        {
            return new ThinkDev.Data.MSSql.ObjectQuery<T>(this, item);
        }
        #endregion
        #region 针对Model的通用操作方法  
        /// <summary>
        /// 用于组装条件参数的方法，返回数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expressions"></param>
        /// <returns></returns>
        public Expression<Func<T, bool>>[] CreateBoolExpressions<T>(params Expression<Func<T, bool>>[] expressions)
        {
            return expressions;
        }

        /// <summary>
        /// 针对符合要求的Model对象进行Insert操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="info"></param>
        /// <param name="expressions"></param>
        /// <returns></returns>
        public ResultInfo<int> Insert<T>(T info, params Expression<Func<T, object>>[] expressions) where T : ModelBase, new()
        {
            string sql = CreateSqlBuilder<T>(info, TableModelMapper.GetTableName(typeof(T)))
                .Insert()
                .AddInsert(expressions)
                .WithIdentity()
                .GetSql();
            object ret = ExecuteScalar(sql);
            if (ret != null)
            {
                return new ResultInfo<int>(false, "", "0", int.Parse(ret.ToString()));
            }
            else
            {
                return new ResultInfo<int>(true, "新增记录失败", GetErrCode("100001"), -1);
            }
        }

        /// <summary>
        /// 针对符合要求的Model对象进行Update操作（条件部分只能进行And合并）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="info"></param>
        /// <param name="exp_Where"></param>
        /// <param name="exp_Sel"></param>
        /// <returns></returns>
        public ResultInfo<int> Update<T>(T info, Expression<Func<T, bool>>[] exp_Where, params Expression<Func<T, object>>[] exp_Sel) where T : ModelBase, new()
        {
            ISqlBuilder<T> sqlBuilder = CreateSqlBuilder<T>(info, TableModelMapper.GetTableName(typeof(T)))
                .Update()
                .Set(exp_Sel);
            if (exp_Where != null && exp_Where.Count() > 0)
            {
                sqlBuilder = sqlBuilder.Where(exp_Where[0]);
                if (exp_Where.Count() > 1)
                {
                    for (int i = 1; i < exp_Where.Count(); i++)
                    {
                        sqlBuilder = sqlBuilder.And(exp_Where[i]);
                    }
                }
            }
            string sql = sqlBuilder.GetSql();

            int ret = ExecuteNonQuery(sql);
            if (ret > 0)
            {
                return new ResultInfo<int>(false, "", "0", ret);
            }
            else
            {
                return new ResultInfo<int>(true, "更新记录失败", GetErrCode("100001"), -1);
            }
        }

        /// <summary>
        /// 针对符合要求的Model对象进行Update操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="info"></param>
        /// <param name="exp_Where"></param>
        /// <param name="exp_Sel"></param>
        /// <returns></returns>
        public ResultInfo<int> Update<T>(T info, Expression<Func<T, bool>> exp_Where, params Expression<Func<T, object>>[] exp_Sel) where T : ModelBase, new()
        {
            var exps_Where = CreateBoolExpressions<T>(exp_Where);
            return Update<T>(info, exps_Where, exp_Sel);
        }

        /// <summary>
        /// 针对符合要求的Model对象进行单字段条件删除操作（条件部分只能进行And合并）
        /// </summary>
        /// <param name="info"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public ResultInfo<int> Delete<T>(params Expression<Func<T, bool>>[] exp_Where) where T : ModelBase, new()
        {
            ISqlBuilder<T> sqlBuilder = CreateSqlBuilder<T>(TableModelMapper.GetTableName(typeof(T)))
                .Delete();
            if (exp_Where != null && exp_Where.Count() > 0)
            {
                sqlBuilder = sqlBuilder.Where(exp_Where[0]);
                if (exp_Where.Count() > 1)
                {
                    for (int i = 1; i < exp_Where.Count(); i++)
                    {
                        sqlBuilder = sqlBuilder.And(exp_Where[i]);
                    }
                }
            }
            string sql = sqlBuilder.GetSql();
            int ret = ExecuteNonQuery(sql);
            if (ret > 0)
            {
                return new ResultInfo<int>(false, "", "0", int.Parse(ret.ToString()));
            }
            else
            {
                return new ResultInfo<int>(true, "删除记录失败，删除记录数为0", GetErrCode("100001"), -1);
            }
        }

        /// <summary>
        /// 针对符合要求的Model对象进行单字段条件删除操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="info"></param>
        /// <param name="exp_Where"></param>
        /// <returns></returns>
        public ResultInfo<int> Delete<T>(Expression<Func<T, bool>> exp_Where) where T : ModelBase, new()
        {
            var exps_Where = CreateBoolExpressions<T>(exp_Where);
            return Delete<T>(exps_Where);
        }

        /// <summary>
        /// 针对符合要求的Model对象进行单字段条件检查是否存在（条件部分只能进行And合并）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="info"></param>
        /// <param name="exp_Where"></param>
        /// <returns></returns>
        public bool Exists<T>(params Expression<Func<T, bool>>[] exp_Where) where T : ModelBase, new()
        {
            ISqlBuilder<T> sqlBuilder = CreateSqlBuilder<T>(TableModelMapper.GetTableName(typeof(T)))
                .Select("1");
            if (exp_Where != null && exp_Where.Count() > 0)
            {
                sqlBuilder = sqlBuilder.Where(exp_Where[0]);
                if (exp_Where.Count() > 1)
                {
                    for (int i = 1; i < exp_Where.Count(); i++)
                    {
                        sqlBuilder = sqlBuilder.And(exp_Where[i]);
                    }
                }
            }
            string sql = sqlBuilder.GetSql();
            object ret = ExecuteScalar(sql);
            if (ret != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 针对符合要求的Model对象进行单字段条件检查是否存在
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="info"></param>
        /// <param name="exp_Where"></param>
        /// <returns></returns>
        public bool Exists<T>(Expression<Func<T, bool>> exp_Where) where T : ModelBase, new()
        {
            var exps_Where = CreateBoolExpressions<T>(exp_Where);
            return Exists<T>(exps_Where);
        }

        /// <summary>
        /// 针对符合要求的Model对象进行单字段条件的查询操作，返回单对象（条件部分只能进行And合并）
        /// </summary>
        /// <param name="info"></param>
        /// <param name="expressions"></param>
        /// <returns></returns>
        public T Select<T>(Expression<Func<T, bool>>[] exp_Where, params Expression<Func<T, object>>[] exps_Sel) where T : ModelBase, new()
        {
            ISqlBuilder<T> sqlBuilder = CreateSqlBuilder<T>(TableModelMapper.GetTableName(typeof(T)));
            if (exps_Sel == null || exps_Sel.Count() <= 0)
            {
                sqlBuilder = sqlBuilder.Select();
            }
            else
            {
                sqlBuilder = sqlBuilder.Select(exps_Sel);
            }
            if (exp_Where != null && exp_Where.Count() > 0)
            {
                sqlBuilder = sqlBuilder.Where(exp_Where[0]);
                if (exp_Where.Count() > 1)
                {
                    for (int i = 1; i < exp_Where.Count(); i++)
                    {
                        sqlBuilder = sqlBuilder.And(exp_Where[i]);
                    }
                }
            }

            string sql = sqlBuilder.GetSql();
            using (IDataReader dr = ExecuteDataReader(sql))
            {
                return GetObject<T>(dr);
            }
        }

        /// <summary>
        /// 以指定正序排序字段查询数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exp_Order"></param>
        /// <param name="exp_Where"></param>
        /// <returns></returns>
        public List<T> SelectOrderBy<T>(Expression<Func<T, object>> exp_Order, params Expression<Func<T, bool>>[] exp_Where) where T : ModelBase, new()
        {
            ISqlBuilder<T> sqlBuilder = CreateSqlBuilder<T>(TableModelMapper.GetTableName(typeof(T)));
            sqlBuilder = sqlBuilder.Select();
            if (exp_Where != null && exp_Where.Count() > 0)
            {
                sqlBuilder = sqlBuilder.Where(exp_Where[0]);
                if (exp_Where.Count() > 1)
                {
                    for (int i = 1; i < exp_Where.Count(); i++)
                    {
                        sqlBuilder = sqlBuilder.And(exp_Where[i]);
                    }
                }
            }
            if (exp_Order != null)
            {
                sqlBuilder = sqlBuilder.OrderBy(exp_Order);
            }

            string sql = sqlBuilder.GetSql();
            using (IDataReader dr = ExecuteDataReader(sql))
            {
                return GetObjectList<T>(dr);
            }
        }

        /// <summary>
        /// 以指定倒序排序字段查询数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exp_Order"></param>
        /// <param name="exp_Where"></param>
        /// <returns></returns>
        public List<T> SelectOrderByDescending<T>(Expression<Func<T, object>> exp_Order, params Expression<Func<T, bool>>[] exp_Where) where T : ModelBase, new()
        {
            ISqlBuilder<T> sqlBuilder = CreateSqlBuilder<T>(TableModelMapper.GetTableName(typeof(T)));
            sqlBuilder = sqlBuilder.Select();
            if (exp_Where != null && exp_Where.Count() > 0)
            {
                sqlBuilder = sqlBuilder.Where(exp_Where[0]);
                if (exp_Where.Count() > 1)
                {
                    for (int i = 1; i < exp_Where.Count(); i++)
                    {
                        sqlBuilder = sqlBuilder.And(exp_Where[i]);
                    }
                }
            }
            if (exp_Order != null)
            {
                sqlBuilder = sqlBuilder.OrderBy(exp_Order, SortType.Desc);
            }

            string sql = sqlBuilder.GetSql();
            using (IDataReader dr = ExecuteDataReader(sql))
            {
                return GetObjectList<T>(dr);
            }
        }


        /// <summary>
        /// 针对符合要求的Model对象进行多字段条件的查询操作,条件为And，返回单对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exp_Where"></param>
        /// <returns></returns>
        public T Select<T>(params Expression<Func<T, bool>>[] exps_Where) where T : ModelBase, new()
        {
            if (exps_Where == null || exps_Where.Count() <= 0)
            {
                exps_Where = null;
            }
            return Select<T>(exps_Where, null);
        }

        /// <summary>
        /// 针对符合要求的Model对象进行多字段条件的查询操作,条件为And，返回对象List（条件部分只能进行And合并）
        /// </summary>
        /// <param name="info"></param>
        /// <param name="exp_Where"></param>
        /// <param name="exps_Sel"></param>
        /// <returns></returns>
        public List<T> SelectList<T>(Expression<Func<T, bool>>[] exp_Where, params Expression<Func<T, object>>[] exps_Sel) where T : ModelBase, new()
        {
            ISqlBuilder<T> sqlBuilder = CreateSqlBuilder<T>(TableModelMapper.GetTableName(typeof(T)));
            if (exps_Sel == null || exps_Sel.Count() <= 0)
            {
                sqlBuilder = sqlBuilder.Select();
            }
            else
            {
                sqlBuilder = sqlBuilder.Select(exps_Sel);
            }
            if (exp_Where != null && exp_Where.Count() > 0)
            {
                sqlBuilder = sqlBuilder.Where(exp_Where[0]);
                if (exp_Where.Count() > 1)
                {
                    for (int i = 1; i < exp_Where.Count(); i++)
                    {
                        sqlBuilder = sqlBuilder.And(exp_Where[i]);
                    }
                }
            }
            string sql = sqlBuilder.GetSql();
            using (IDataReader dr = ExecuteDataReader(sql))
            {
                return GetObjectList<T>(dr);
            }
        }

        /// <summary>
        /// 针对符合要求的Model对象进行多字段条件的查询操作，返回对象List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exp_Where"></param>
        /// <returns></returns>
        public List<T> SelectList<T>(params Expression<Func<T, bool>>[] exps_Where) where T : ModelBase, new()
        {
            if (exps_Where == null || exps_Where.Count() <= 0)
            {
                exps_Where = null;
            }
            return SelectList<T>(exps_Where, null);
        }

        /// <summary>
        /// 针对符合要求的Model对象进行单字段条件的查询操作，返回对象List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="info"></param>
        /// <param name="exps_Sel"></param>
        /// <returns></returns>
        public List<T> SelectList<T>() where T : ModelBase, new()
        {
            Expression<Func<T, bool>> exp_Where = null;
            return SelectList<T>(exp_Where);
        }

        /// <summary>
        /// 针对符合要求的Model对象进行单字段条件的查询操作，返回对象DataTable（条件部分只能进行And合并）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="info"></param>
        /// <param name="exp_Where"></param>
        /// <param name="exps_Sel"></param>
        /// <returns></returns>
        public DataTable SelectDataTable<T>(Expression<Func<T, bool>>[] exp_Where, params Expression<Func<T, object>>[] exps_Sel) where T : ModelBase, new()
        {
            ISqlBuilder<T> sqlBuilder = CreateSqlBuilder<T>(TableModelMapper.GetTableName(typeof(T)));
            if (exps_Sel == null || exps_Sel.Count() <= 0)
            {
                sqlBuilder = sqlBuilder.Select();
            }
            else
            {
                sqlBuilder = sqlBuilder.Select(exps_Sel);
            }
            if (exp_Where != null && exp_Where.Count() > 0)
            {
                sqlBuilder = sqlBuilder.Where(exp_Where[0]);
                if (exp_Where.Count() > 1)
                {
                    for (int i = 1; i < exp_Where.Count(); i++)
                    {
                        sqlBuilder = sqlBuilder.And(exp_Where[i]);
                    }
                }
            }
            string sql = sqlBuilder.GetSql();
            return ExecuteDataTable(sql);
        }

        /// <summary>
        /// 针对符合要求的Model对象进行单字段条件的查询操作，返回对象DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="info"></param>
        /// <param name="exp_Where"></param>
        /// <param name="exps_Sel"></param>
        /// <returns></returns>
        public DataTable SelectDataTable<T>(Expression<Func<T, bool>> exp_Where, params Expression<Func<T, object>>[] exps_Sel) where T : ModelBase, new()
        {
            Expression<Func<T, bool>>[] exps_Where = null;
            if (exp_Where != null)
            {
                exps_Where = CreateBoolExpressions<T>(exp_Where);
            }
            return SelectDataTable<T>(exps_Where, exps_Sel);
        }

        /// <summary>
        /// 针对符合要求的Model对象进行单字段条件的查询操作，返回对象DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="info"></param>
        /// <param name="exps_Sel"></param>
        /// <returns></returns>
        public DataTable SelectDataTable<T>(params Expression<Func<T, object>>[] exps_Sel) where T : ModelBase, new()
        {
            Expression<Func<T, bool>> exps_Where = null;
            return SelectDataTable<T>(exps_Where, exps_Sel);
        }


        /// <summary>
        /// 针对符合要求的Model对象进行单字段条件的查询操作，返回INT（条件部分只能进行And合并）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="info"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public int Count<T>(params Expression<Func<T, bool>>[] exp_Where) where T : ModelBase, new()
        {
            ISqlBuilder<T> sqlBuilder = CreateSqlBuilder<T>(TableModelMapper.GetTableName(typeof(T)))
                .SelectCount();
            if (exp_Where != null && exp_Where.Count() > 0)
            {
                sqlBuilder = sqlBuilder.Where(exp_Where[0]);
                if (exp_Where.Count() > 1)
                {
                    for (int i = 1; i < exp_Where.Count(); i++)
                    {
                        sqlBuilder = sqlBuilder.And(exp_Where[i]);
                    }
                }
            }
            string sql = sqlBuilder.GetSql();
            return int.Parse(ExecuteScalar(sql).ToString());
        }

        /// <summary>
        /// 针对符合要求的Model对象进行单字段条件的查询操作，返回INT
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="info"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public int Count<T>(Expression<Func<T, bool>> exp_Where = null) where T : ModelBase, new()
        {
            Expression<Func<T, bool>>[] exps_Where = null;
            if (exp_Where != null)
            {
                exps_Where = CreateBoolExpressions<T>(exp_Where);
            }
            return Count<T>(exps_Where);
        }

        /// <summary>
        /// 根据类型获取对应的表名
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        protected string GetTableName(Type t)
        {
            return TableModelMapper.GetTableName(t);
        }

        protected string GetErrCode(string code)
        {
            return Util.ErrCode_Pre_DataAccess + code;
        }

        #endregion
        #region 应用自定义封装数据库操作方法 below function event of ThinkDev are transfer to the functon of EnterpriseLibrary

        /// <summary>
        /// 仅用于Sql语句执行的方法
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        protected int ExecuteNonQuery(string commandText, string conn = "")
        {
            if (string.IsNullOrEmpty(conn)) conn = CurrentConnection;
            DbCommand cmd = CreateSqlStringCommand(commandText);
            return ExecuteNonQuery(conn, cmd);
        }

        /// <summary>
        /// 根据传入的Command和ConnectString执行ExecuteNonQuery方法
        /// </summary>
        /// <param name="command"></param>
        /// <param name="conn"></param>
        /// <returns></returns>
        protected int ExecuteNonQuery(DbCommand command, string conn = "")
        {
            if (string.IsNullOrEmpty(conn)) conn = CurrentConnection;
            return ExecuteNonQuery(conn, command);
        }

        /// <summary>
        /// 仅用于Sql语句执行的方法
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        protected object ExecuteScalar(string commandText, string conn = "")
        {
            if (string.IsNullOrEmpty(conn)) conn = CurrentConnection;
            DbCommand cmd = CreateSqlStringCommand(commandText);
            return ExecuteScalar(conn, cmd);
        }

        /// <summary>
        /// 根据传入的Command和ConnectString执行ExecuteScalar方法
        /// </summary>
        /// <param name="command"></param>
        /// <param name="conn"></param>
        /// <returns></returns>
        protected object ExecuteScalar(DbCommand command, string conn = "")
        {
            if (string.IsNullOrEmpty(conn)) conn = CurrentConnection;
            return ExecuteScalar(conn, command);
        }

        /// <summary>
        /// 仅用于Sql语句执行的方法
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        protected DataSet ExecuteDataSet(string commandText, string conn = "")
        {
            if (string.IsNullOrEmpty(conn)) conn = CurrentConnection;
            DbCommand cmd = CreateSqlStringCommand(commandText);
            return ExecuteDataSet(conn, cmd);
        }

        /// <summary>
        /// 根据传入的Command和ConnectString执行ExecuteDataSet方法
        /// </summary>
        /// <param name="command"></param>
        /// <param name="conn"></param>
        /// <returns></returns>
        protected DataSet ExecuteDataSet(DbCommand command, string conn = "")
        {
            if (string.IsNullOrEmpty(conn)) conn = CurrentConnection;
            return ExecuteDataSet(conn, command);
        }

        /// <summary>
        /// 仅用于Sql语句执行的方法
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        protected IDataReader ExecuteDataReader(string commandText, string conn = "")
        {
            if (string.IsNullOrEmpty(conn)) conn = CurrentConnection;
            DbCommand cmd = CreateSqlStringCommand(commandText);
            return ExecuteDataReader(conn, cmd);
        }

        /// <summary>
        /// 根据传入的Command和ConnectString执行ExecuteDataReader方法
        /// </summary>
        /// <param name="command"></param>
        /// <param name="conn"></param>
        /// <returns></returns>
        protected IDataReader ExecuteDataReader(DbCommand command, string conn = "")
        {
            if (string.IsNullOrEmpty(conn)) conn = CurrentConnection;
            return ExecuteDataReader(conn, command);
        }

        /// <summary>
        /// 仅用于Sql语句执行的方法
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        protected DataTable ExecuteDataTable(string commandText, int tableIndex = 0, string conn = "")
        {
            if (string.IsNullOrEmpty(conn)) conn = CurrentConnection;
            DbCommand cmd = CreateSqlStringCommand(commandText);
            return ExecuteDataTable(conn, cmd, tableIndex);
        }

        /// <summary>
        /// 根据传入的Command和ConnectString执行ExecuteDataTable方法
        /// </summary>
        /// <param name="command"></param>
        /// <param name="tableIndex"></param>
        /// <param name="conn"></param>
        /// <returns></returns>
        protected DataTable ExecuteDataTable(DbCommand command, int tableIndex = 0, string conn = "")
        {
            if (string.IsNullOrEmpty(conn)) conn = CurrentConnection;
            return ExecuteDataTable(conn, command, tableIndex);
        }

        #endregion


        #region 逻辑异常处理
        /// <summary>
        /// 设置逻辑异常封装对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commandParameters"></param>
        /// <param name="obj"></param>
        /// <param name="correctReturnValue">存储过程执行正确时的返回值</param>
        /// <returns></returns>
        protected ResultInfo<T> SetRullErr<T>(DbParameterCollection commandParameters, T obj, int correctReturnValue)
        {
            if (commandParameters == null || commandParameters.Count <= 0)
            {
                return SetRullErr<T>(false, string.Empty, string.Empty, obj);
            }
            try
            {
                string ReturnVlaue = string.Empty;
                foreach (DbParameter para in commandParameters)
                {
                    if (para.Direction == ParameterDirection.ReturnValue)
                    {
                        ReturnVlaue = para.Value == null ? string.Empty : para.Value.ToString();
                    }
                }
                int ReturnCode;
                if (!int.TryParse(ReturnVlaue, out ReturnCode))
                {
                    ReturnCode = int.MinValue;
                }
                if (ReturnCode != correctReturnValue)
                {
                    return SetRullErr<T>(true, ReturnVlaue, ReturnVlaue, obj);
                }
                return SetRullErr<T>(false, ReturnVlaue, ReturnVlaue, obj);
            }
            catch (Exception ex)
            {
                return SetRullErr<T>(true, "处理存储过程逻辑异常判断失败，请检查逻辑异常处理代码", ex.Message, obj);
            }
        }

        protected ResultInfo<T> SetRullErr<T>(bool HasError, string Message, string HRESULT, T obj)
        {
            return new ResultInfo<T>(HasError, Message, HRESULT, obj);
        }

        /// <summary>
        /// 设置逻辑异常封装对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commandParameters"></param>
        /// <param name="obj"></param>
        /// <param name="correctReturnValue">存储过程执行正确时的返回值</param>
        /// <returns></returns>
        protected ResultInfo<T> SetRullErr<T>(string ReturnVlaue, T obj, int correctReturnValue)
        {
            try
            {
                int ReturnCode;
                if (!int.TryParse(ReturnVlaue, out ReturnCode))
                {
                    ReturnCode = int.MinValue;
                }
                if (ReturnCode != correctReturnValue)
                {
                    return SetRullErr<T>(true, ReturnVlaue, ReturnVlaue, obj);
                }
                return SetRullErr<T>(false, ReturnVlaue, ReturnVlaue, obj);
            }
            catch (Exception ex)
            {
                return SetRullErr<T>(true, "处理存储过程逻辑异常判断失败，请检查逻辑异常处理代码", ex.Message, obj);
            }
        }
        #endregion

    }
}
