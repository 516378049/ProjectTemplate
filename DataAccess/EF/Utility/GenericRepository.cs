using Model.EF;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;

namespace DataAccess.EF
{
    public class GenericRepository<TEntity> where TEntity : class
    {
        internal OrderMealEntities context;
        internal DbSet<TEntity> dbSet;
        string conStr = ConfigurationManager.ConnectionStrings["OrderMeal"].ConnectionString;
        /// <summary>
        /// 构造函数统一管理context
        /// </summary>
        /// <param name="context"></param>
        public GenericRepository(OrderMealEntities context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

        #region select
        /// <summary>
        /// 通用根据id查询实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TEntity GetById(int id)
        {
            return dbSet.Find(id);
        }
        /// <summary>
        /// 通用查询
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <param name="orderBy">排序</param>
        /// <param name="includeProperties">贪婪加载</param>
        /// <param name="pageIndex">页码，小于1时不做分页</param>
        /// <param name="pageSize"></param>
        /// <param name="AsNoTracking">为true时不在上下文中跟踪</param>
        /// <returns></returns>
        public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "", int pageIndex = -1, int pageSize = 10, bool AsNoTracking = true)
        {
            IQueryable<TEntity> query = dbSet;
            if (filter != null)
                query = query.Where(filter);
            if (orderBy != null)
                query = orderBy(query);
            if (pageIndex > 0)
                query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            foreach (string includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                query = query.Include(includeProperty);
            if (AsNoTracking)
                query = query.AsNoTracking();
            return query;
        }
        /// <summary>
        /// 通用查询
        /// </summary>
        /// <param name="filters">筛选条件</param>
        /// <param name="orderBy">排序</param>
        /// <param name="includeProperties">贪婪加载</param>
        /// <param name="pageIndex">页码，小于1时不做分页</param>
        /// <param name="pageSize"></param>
        /// <param name="AsNoTracking">为true时不在上下文中跟踪</param>
        /// <returns></returns>
        public IEnumerable<TEntity> Get(List<Expression<Func<TEntity, bool>>> filters = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "", int pageIndex = -1, int pageSize = 10, bool AsNoTracking = true)
        {
            IQueryable<TEntity> query = dbSet;
            if (filters != null && filters.Count > 0)
                foreach (var filter in filters)
                    query = query.Where(filter);
            if (orderBy != null)
                query = orderBy(query);
            if (pageIndex > 0)
                query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            foreach (string includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                query = query.Include(includeProperty);
            if (AsNoTracking)
                query = query.AsNoTracking();
            return query;
        }
        /// <summary>
        /// 通用查询
        /// </summary>
        /// <param name="count">总数量</param>
        /// <param name="filters">筛选条件</param>
        /// <param name="orderBy">排序</param>
        /// <param name="includeProperties">贪婪加载</param>
        /// <param name="pageIndex">页码，小于1时不做分页</param>
        /// <param name="pageSize"></param>
        /// <param name="AsNoTracking">为true时不在上下文中跟踪</param>
        /// <returns></returns>
        public IEnumerable<TEntity> Get(out int count, List<Expression<Func<TEntity, bool>>> filters = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "", int pageIndex = -1, int pageSize = 10, bool AsNoTracking = true)
        {
            IQueryable<TEntity> query = dbSet;
            if (filters != null && filters.Count > 0)
                foreach (var filter in filters)
                    query = query.Where(filter);
            foreach (string includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                query = query.Include(includeProperty);
            count = query.Count();
            if (orderBy != null)
                query = orderBy(query);
            if (pageIndex > 0)
                query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            if (AsNoTracking)
                query = query.AsNoTracking();
            return query;
        }
        public IEnumerable<TEntity> Get(out int count, Expression<Func<TEntity, bool>> filters = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "", int pageIndex = -1, int pageSize = 10, bool AsNoTracking = true)
        {
            IQueryable<TEntity> query = dbSet;
            if (filters != null)
                query = query.Where(filters);
            foreach (string includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                query = query.Include(includeProperty);
            count = query.Count();
            if (orderBy != null)
                query = orderBy(query);
            if (pageIndex > 0)
                query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            if (AsNoTracking)
                query = query.AsNoTracking();
            return query;
        }

        public IEnumerable<TEntity> Get11(out int count,
            List<Expression<Func<TEntity, bool>>> filters = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IQueryable<IGrouping<object, TEntity>>> groupBy = null,
            Expression<Func<TEntity, object>> include = null,
            int pageIndex = -1, int pageSize = 10, bool AsNoTracking = true)
        {
            IQueryable<TEntity> query = dbSet;
            filters.ForEach(f => query = query.Where(f));
            //query = groupBy?.Invoke(query);
            query = orderBy?.Invoke(query);
            query = query.Include(include);
            count = query.Count();
            if (pageIndex > 0)
                query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            if (AsNoTracking)
                query = query.AsNoTracking();
            return query;
        }

        /// <summary>
        /// 通用查询
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <param name="orderBy">排序</param>
        /// <param name="includeProperties">贪婪加载</param>
        /// <param name="pageIndex">页码，小于1时不做分页</param>
        /// <param name="pageSize"></param>
        /// <param name="AsNoTracking">为true时不在上下文中跟踪</param>
        /// <returns></returns>
        public IQueryable<TEntity> GetIQ(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "", int pageIndex = -1, int pageSize = 10, bool AsNoTracking = true)
        {
            IQueryable<TEntity> query = dbSet;
            if (filter != null)
                query = query.Where(filter);
            if (orderBy != null)
                query = orderBy(query);
            if (pageIndex > 0)
                query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            foreach (string includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                query = query.Include(includeProperty);
            if (AsNoTracking)
                query = query.AsNoTracking();
            return query;
        }
        /// <summary>
        /// 通用查询，查询单个或者多个字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filter"></param>
        /// <param name="scalar"></param>
        /// <param name="orderBy"></param>
        /// <param name="includeProperties"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="AsNoTracking"></param>
        /// <returns></returns>
        public IEnumerable<T> GetField<T>(Expression<Func<TEntity, T>> scalar, Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "", int pageIndex = -1, int pageSize = 10, bool AsNoTracking = true, bool distinct = false)
        {
            IQueryable<TEntity> query = dbSet;
            if (filter != null)
                query = query.Where(filter);
            if (orderBy != null)
                query = orderBy(query);
            if (pageIndex > 0)
                query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            foreach (string includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                query = query.Include(includeProperty);
            if (AsNoTracking)
                query = query.AsNoTracking();
            if (distinct)
                return query.Select(scalar).Distinct();
            else
                return query.Select(scalar);
        }
        #endregion

        #region insert
        /// <summary>
        /// 通用新增
        /// </summary>
        /// <param name="entity">要被新增的对象</param>
        /// <param name="SaveChanges">为true时直接提交修改</param>
        /// <returns></returns>
        public TEntity Insert(TEntity entity, bool saveChanges = true)
        {
            try
            {
                if (typeof(TEntity).GetProperties().Where(w => w.Name == "DelFlag").Count() > 0)
                    entity.GetType().GetProperty("DelFlag").SetValue(entity, 0);
                if (typeof(TEntity).GetProperties().Where(w => w.Name == "CreateTime").Count() > 0)
                    entity.GetType().GetProperty("CreateTime").SetValue(entity, DateTime.Now);
                if (typeof(TEntity).GetProperties().Where(w => w.Name == "WriteTime").Count() > 0)
                    entity.GetType().GetProperty("WriteTime").SetValue(entity, DateTime.Now);
                if (typeof(TEntity).GetProperties().Where(w => w.Name == "UpdateTime").Count() > 0)
                    entity.GetType().GetProperty("UpdateTime").SetValue(entity, DateTime.Now);
                TEntity result = dbSet.Add(entity);
                if (saveChanges)
                    context.SaveChanges();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 通用新增
        /// </summary>
        /// <param name="entity">要被新增的对象</param>
        /// <param name="SaveChanges">为true时直接提交修改</param>
        /// <returns></returns>
        public int Insert(IEnumerable<TEntity> entity, bool saveChanges = true)
        {
            try
            {
                foreach (var item in entity)
                {
                    if (typeof(TEntity).GetProperties().Where(w => w.Name == "DelFlag").Count() > 0)
                        item.GetType().GetProperty("DelFlag").SetValue(item,0);
                    if (typeof(TEntity).GetProperties().Where(w => w.Name == "CreateTime").Count() > 0)
                        item.GetType().GetProperty("CreateTime").SetValue(item, DateTime.Now);
                    if (typeof(TEntity).GetProperties().Where(w => w.Name == "UpdateTime").Count() > 0)
                        item.GetType().GetProperty("UpdateTime").SetValue(item, DateTime.Now);
                    if (typeof(TEntity).GetProperties().Where(w => w.Name == "WriteTime").Count() > 0)
                        item.GetType().GetProperty("WriteTime").SetValue(item, DateTime.Now);
                }
                dbSet.AddRange(entity);
                int result = 0;
                if (saveChanges)
                    result = context.SaveChanges();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IEnumerable<TEntity> InsertRange(IEnumerable<TEntity> entities, bool saveChanges = true)
        {
            try
            {
                //给实体集合新增时间和修改时间赋初始值
                foreach (var item in entities)
                {
                    if (typeof(TEntity).GetProperties().Where(w => w.Name == "DelFlag").Count() > 0)
                        item.GetType().GetProperty("DelFlag").SetValue(item, 0);
                    if (typeof(TEntity).GetProperties().Where(w => w.Name == "CreateTime").Count() > 0)
                        item.GetType().GetProperty("CreateTime").SetValue(item, DateTime.Now);
                    if (typeof(TEntity).GetProperties().Where(w => w.Name == "UpdateTime").Count() > 0)
                        item.GetType().GetProperty("UpdateTime").SetValue(item, DateTime.Now);
                    if (typeof(TEntity).GetProperties().Where(w => w.Name == "WriteTime").Count() > 0)
                        item.GetType().GetProperty("WriteTime").SetValue(item, DateTime.Now);
                }
                IEnumerable<TEntity> result = dbSet.AddRange(entities);
                if (saveChanges)
                    context.SaveChanges();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        public int BulkInsertAll(IEnumerable<TEntity> entities)
        {
            try
            {
                entities = entities.ToArray();
                string cs = conStr;
                var conn = new SqlConnection(cs);
                conn.Open();

                Type t = typeof(TEntity);
                var bulkCopy = new SqlBulkCopy(conn)
                {
                    DestinationTableName = (t.GetCustomAttributes(typeof(TableAttribute), true).FirstOrDefault() as TableAttribute).Name
                };
                List<System.Reflection.PropertyInfo> properties = new List<System.Reflection.PropertyInfo>();
                foreach (var item in t.GetProperties())
                {
                    if (item.GetCustomAttributes(typeof(NotMappedAttribute), true).Length <= 0
                    && item.GetCustomAttributes(typeof(ForeignKeyAttribute), true).Length <= 0)
                        properties.Add(item);
                }
                DataTable table = new DataTable();
                foreach (var item in properties)
                    table.Columns.Add(new DataColumn(item.Name, item.PropertyType.IsGenericType && item.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(item.PropertyType) : item.PropertyType));
                foreach (var entity in entities)
                    table.Rows.Add(properties.Select(property => property.GetValue(entity, null).GetPropertyValue()).ToArray());
                bulkCopy.WriteToServer(table);
                conn.Close();
                return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int BulkInsertAll_Mine(IEnumerable<TEntity> entities)
        {
            try
            {
                entities = entities.ToArray();
                string cs = conStr;
                var conn = new SqlConnection(cs);
                conn.Open();

                Type t = typeof(TEntity);
                var bulkCopy = new SqlBulkCopy(conn)
                {
                    DestinationTableName = (t.GetCustomAttributes(typeof(TableAttribute), true).FirstOrDefault() as TableAttribute).Name
                };
                List<System.Reflection.PropertyInfo> properties = new List<System.Reflection.PropertyInfo>();
                foreach (var item in t.GetProperties())
                {
                    if (item.GetCustomAttributes(typeof(NotMappedAttribute), true).Length <= 0
                    && item.GetCustomAttributes(typeof(ForeignKeyAttribute), true).Length <= 0)
                        properties.Add(item);
                }
                DataTable table = new DataTable();
                foreach (var item in properties)
                    table.Columns.Add(new DataColumn(item.Name, item.PropertyType.IsGenericType && item.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(item.PropertyType) : item.PropertyType));
                foreach (var entity in entities)
                    table.Rows.Add(properties.Select(property => GetPropertyValue(property.GetValue(entity, null))).ToArray());
                bulkCopy.WriteToServer(table);
                conn.Close();
                return 0;
            }
            catch (Exception ex)
            {
                return -9999;
                throw ex;
            }
        }
        #endregion

        #region update
        /// <summary>
        /// 更新整个实体(CreateTime除外)
        /// </summary>
        /// <param name="entity">将被更新的实体</param>
        /// <param name="saveChanges">为true将直接提交修改</param>
        /// <returns>返回1说明更新成功,返回0说明saveChanges为false或者更新失败</returns>
        public int Update(TEntity entity, bool saveChanges = true)
        {
            try
            {
                if (typeof(TEntity).GetProperties().Where(w => w.Name == "UpdateTime").Count() > 0)
                    entity.GetType().GetProperty("UpdateTime").SetValue(entity, DateTime.Now);
                int result = 0;
                var dbee = context.Entry<TEntity>(entity);
                dbee.State = EntityState.Modified;
                if (typeof(TEntity).GetProperties().Where(w => w.Name == "CreateTime").Count() > 0)
                    dbee.Property("CreateTime").IsModified = false;
                if (typeof(TEntity).GetProperties().Where(w => w.Name == "WriteTime").Count() > 0)
                    dbee.Property("WriteTime").IsModified = false;
                if (saveChanges)
                    result = context.SaveChanges();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }/// <summary>
        public int Update(TEntity entity, bool saveChange = true, bool inFileds = true, params string[] fileds)
        {
            if (typeof(TEntity).GetProperties().Where(w => w.Name == "UpdateTime").Count() > 0)
                entity.GetType().GetProperty("UpdateTime").SetValue(entity, DateTime.Now);
            List<string> fs = fileds.ToList();
            if (inFileds)
                fs.Add("UpdateTime");
            else
                fs.Add("CreateTime");
            var entityEntry = context.Entry(entity);
            entityEntry.State = inFileds ? EntityState.Unchanged : EntityState.Modified;
            foreach (var propertyName in fs)
            {
                entityEntry.Property(propertyName).IsModified = inFileds ? true : false;
            }
            return context.SaveChanges();
        }

        /// <summary>  
        /// 更新指定字段,若实体包含UpdateTime字段，则此字段必定被更新
        /// </summary>  
        /// <param name="entity">实体</param>  
        /// <param name="fileds">更新字段数组</param>  
        /// <param name="saveChanges">为true将直接提交修改</param>
        /// <returns>返回1说明修改成功，返回-1说明未指定要修改的字段或者被修改的对象为空,返回0说明saveChanges为false或者更新失败</returns>
        public int UpdateEntityFields(TEntity entity, List<string> fileds, bool saveChanges = true)
        {
            try
            {
                int result = 0;
                if (entity != null && fileds != null)
                {
                    if (typeof(TEntity).GetProperties().Where(w => w.Name == "UpdateTime").Count() > 0)
                    {
                        entity.GetType().GetProperty("UpdateTime").SetValue(entity, DateTime.Now);
                        fileds.Add("UpdateTime");
                    }
                    context.Set<TEntity>().Attach(entity);
                    var SetEntry = ((IObjectContextAdapter)context).ObjectContext.
                        ObjectStateManager.GetObjectStateEntry(entity);
                    foreach (var t in fileds)
                    {
                        SetEntry.SetModifiedProperty(t);
                    }
                    if (saveChanges)
                        result = context.SaveChanges();
                }
                else
                    result = -1;
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string GetFieldNameByLambda(Expression exprBody)
        {
            var property = "";
            if (exprBody is UnaryExpression)
            {
                property = ((MemberExpression)((UnaryExpression)exprBody).Operand).Member.Name;
            }
            else if (exprBody is MemberExpression)
            {
                property = ((MemberExpression)exprBody).Member.Name;
            }
            else if (exprBody is ParameterExpression)
            {
                property = ((ParameterExpression)exprBody).Type.Name;
            }
            return property;
        }
        #endregion

        #region Delete
        /// <summary>
        /// 根据编号删除实体
        /// </summary>
        /// <param name="id">实体编号</param>
        /// <param name="saveChanges">为true将直接提交修改</param>
        /// <returns>返回1说明删除成功,返回0说明saveChanges为false或者删除失败</returns>
        public int Delete(int id, bool saveChanges = true)
        {
            try
            {
                int result = 0;
                TEntity TEntity = dbSet.Find(id);
                dbSet.Remove(TEntity);
                if (saveChanges)
                    result = context.SaveChanges();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Delete(TEntity entityToDelete, bool saveChanges = true)
        {
            try
            {
                if (context.Entry(entityToDelete).State == EntityState.Detached)
                    dbSet.Attach(entityToDelete);
                dbSet.Remove(entityToDelete);
                if (saveChanges)
                    context.SaveChanges();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int DeleteRange(IEnumerable<TEntity> entities, bool saveChanges = true)
        {
            try
            {
                foreach (var item in entities)
                {
                    if (context.Entry(item).State == EntityState.Detached)
                        dbSet.Attach(item);
                }
                int result = 0;
                dbSet.RemoveRange(entities);
                if (saveChanges)
                    result = context.SaveChanges();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion

        private object GetPropertyValue( object o)
        {
            if (o == null)
                return DBNull.Value;
            return o;
        }
    }
    public static class CommonHelper
    {
        public static object GetPropertyValue(this object o)
        {
            if (o == null)
                return DBNull.Value;
            return o;
        }
    }
    }
