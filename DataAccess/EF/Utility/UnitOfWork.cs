using Model.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EF
{
    public class UnitOfWork : IDisposable
    {
        OrderMealEntities context = new OrderMealEntities();
        
        public UnitOfWork(bool EnableTransaction = false)
        {
            if (EnableTransaction)
            {
                BeginTransaction += new Transaction(BT);
                CommitTransaction += new Transaction(CT);
                Rollback += new Transaction(RB);
            }
            else
            {
                BeginTransaction += new Transaction(ThrowTips);
                CommitTransaction += new Transaction(ThrowTips);
                Rollback += new Transaction(ThrowTips);
            }
        }

        #region 事务
        DbContextTransaction transaction;

        public delegate void Transaction();
        public Transaction BeginTransaction;
        public Transaction CommitTransaction;
        public Transaction Rollback;

        /// <summary>
        /// 开始事务
        /// </summary>
        public void BT()
        {
            transaction = context.Database.BeginTransaction();
        }
        /// <summary>
        /// 提交事务
        /// </summary>
        public void CT()
        {
            if (transaction != null)
                transaction.Commit();
        }
        /// <summary>
        /// 回滚事务
        /// </summary>
        public void RB()
        {
            if (transaction != null)
                transaction.Rollback();
        }
        public void ThrowTips()
        {
            throw new Exception("UnitOfWork在实例化时未启用事务。");
        }
        #endregion

        #region 申明私有属性是为了关闭公共属性的set方法
        #region CCR
        private GenericRepository<UserInfo> _UserInfo;
        private GenericRepository<sellers> _sellers;
        private GenericRepository<supports> _supports;

        private GenericRepository<goods> _goods;
        private GenericRepository<foods> _foods;
        private GenericRepository<ratings> _ratings;

        private GenericRepository<OrderInfo> _OrderInfo;
        private GenericRepository<OrderDetailsInfo> _OrderDetailsInfo;

        private GenericRepository<RatingsSellers> _ratingsseller;

        private GenericRepository<Token> _token;
        private GenericRepository<Token_seller> _Token_seller;
        #endregion
        #endregion

        #region 对外入口
        #region CCR
        public GenericRepository<UserInfo> UserInfo
        {
            get
            {
                if (_UserInfo == null)
                    _UserInfo = new GenericRepository<UserInfo>(context);
                return _UserInfo;
            }
        }
        public GenericRepository<sellers> Sellers
        {
            get
            {
                if (_sellers == null)
                    _sellers = new GenericRepository<sellers>(context);
                return _sellers;
            }
        }
        public GenericRepository<supports> Supports
        {
            get
            {
                if (_supports == null)
                    _supports = new GenericRepository<supports>(context);
                return _supports;
            }
        }
        public GenericRepository<goods> Goods
        {
            get
            {
                if (_goods == null)
                    _goods = new GenericRepository<goods>(context);
                return _goods;
            }
        }
        public GenericRepository<foods> Foods
        {
            get
            {
                if (_foods == null)
                    _foods = new GenericRepository<foods>(context);
                return _foods;
            }
        }
        public GenericRepository<ratings> Ratings
        {
            get
            {
                if (_ratings == null)
                    _ratings = new GenericRepository<ratings>(context);
                return _ratings;
            }
        }

        public GenericRepository<OrderInfo> OrderInfo
        {
            get
            {
                if (_OrderInfo == null)
                    _OrderInfo = new GenericRepository<OrderInfo>(context);
                return _OrderInfo;
            }
        }
        public GenericRepository<OrderDetailsInfo> OrderDetailsInfo
        {
            get
            {
                if (_OrderDetailsInfo == null)
                    _OrderDetailsInfo = new GenericRepository<OrderDetailsInfo>(context);
                return _OrderDetailsInfo;
            }
        }

        public GenericRepository<RatingsSellers> RatingsSeller
        {
            get
            {
                if (_ratingsseller == null)
                    _ratingsseller = new GenericRepository<RatingsSellers>(context);
                return _ratingsseller;
            }
        }
        public GenericRepository<Token> Token
        {
            get
            {
                if (_token == null)
                    _token = new GenericRepository<Token>(context);
                return _token;
            }
        }
        public GenericRepository<Token_seller> Token_seller
        {
            get
            {
                if (_Token_seller == null)
                    _Token_seller = new GenericRepository<Token_seller>(context);
                return _Token_seller;
            }
        }
        

        #endregion
        #endregion

        #region 通用泛型操作
        /// <summary>
        /// 执行查询sql语句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<T> ExcuteSql<T>(string sql, params object[] parameters)
        {
            List<T> result = context.Database.SqlQuery<T>(sql, parameters).ToList();
            return result;
        }

        /// <summary>
        /// 执行增删改sql语句
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int ExecuteSqlCommand(string sql, params object[] parameters)
        {
            int result = context.Database.ExecuteSqlCommand(sql, parameters);
            return result;
        }

        /// <summary>
        /// 执行context.SaveChanges()
        /// </summary>
        public int Save()
        {
            return context.SaveChanges();
        }
        #endregion

        #region 资源回收
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
                context.Dispose();
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
