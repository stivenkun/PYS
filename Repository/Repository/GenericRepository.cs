using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Text;

using LinqKit;
using System.Data.Entity.Validation;
using System.Diagnostics;
using Repository.Interfaces;
using PYS.VirtualClinic.Infrastructure;

namespace Repository.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        public GenericRepository(DbContext dbContext)
        {
            if (dbContext == null)
                throw new ArgumentNullException("Null DbContext");

            _dbContext = dbContext;
            Entity = _dbContext.Set<T>();
            _isAutoSave = true;
        }

        protected DbContext _dbContext { get; set; }

        private bool _isAutoSave;

        public DbSet<T> Entity { get; private set; }

        private Expression<Func<T, dynamic>> CreateSelectLambdaExpression(string[] columns)
        {
            var typeOfT = typeof(T);
            var unknownType = ClassBuilder.CompileResultType<T>(columns, typeOfT.Name + "Mock");
            var xParameter = Expression.Parameter(typeOfT, "o");
            var xNew = Expression.New(unknownType);
            var bindings = columns.Select(c => {

                // property "Field1"
                var mi = unknownType.GetProperty(c);

                // original value "o.Field1"
                var xOriginal = Expression.Property(xParameter, typeOfT.GetProperty(c));

                // set value "Field1 = o.Field1"
                return Expression.Bind(mi, xOriginal);
            });

            var xInit = Expression.MemberInit(xNew, bindings);

            return Expression.Lambda<Func<T, dynamic>>(xInit, xParameter);
        }

        private IQueryable<T> GetQueryable(Expression<Func<T, bool>> filter, params Expression<Func<T, dynamic>>[] includes)
        {
            var set = Entity.AsExpandable();

            if (filter != null)
                set = set.Where(filter);

            if (includes != null)
            {
                foreach (var inc in includes)
                {
                    set = set.Include(inc);
                }
            }

            return set.AsNoTracking();
        }

        public virtual void Add(T entity)
        {
            DbEntityEntry dbEntityEntry = _dbContext.Entry(entity);
            if (dbEntityEntry.State != System.Data.Entity.EntityState.Detached)
            {
                dbEntityEntry.State = System.Data.Entity.EntityState.Added;
            }
            else
            {
                Entity.Add(entity);
            }

            CommitAutoSave();
        }

        public virtual void Commit()
        {
            _dbContext.SaveChanges();
        }

        public void CommitAutoSave()
        {
            if (_isAutoSave)
            {
                _dbContext.SaveChanges();
            }
        }

        public virtual void Delete(T entity)
        {
            DbEntityEntry dbEntityEntry = _dbContext.Entry(entity);
            if (dbEntityEntry.State != System.Data.Entity.EntityState.Deleted)
            {
                dbEntityEntry.State = System.Data.Entity.EntityState.Deleted;
            }
            else
            {
                Entity.Attach(entity);
                Entity.Remove(entity);
            }

            CommitAutoSave();
        }

        public virtual void Delete(object[] keyValues)
        {
            var entity = GetByKeys(keyValues);
            if (entity == null)
                return; // not found; assume already deleted.
            Delete(entity);
            CommitAutoSave();
        }

        public virtual T Get(Expression<Func<T, bool>> filter, params Expression<Func<T, dynamic>>[] includes)
        {
            try
            {
                if (filter == null) throw new ArgumentNullException("filter");

                var query = GetQueryable(filter, includes);

                return query.SingleOrDefault();
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var entityValidationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in entityValidationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                    }
                }
                return null;
            }

        }

        public List<object> Get<TOrder>(int start, int length, out int totalRecords, string[] columns, Expression<Func<T, bool>> filter = null, Expression<Func<T, TOrder>> orderSelector = null, bool ascendingOrder = true)
        {
            var set = Entity.AsExpandable();

            if (filter != null)
                set = set.Where(filter);

            totalRecords = set.Count();

            if (orderSelector != null)
            {
                if (ascendingOrder)
                    set = set.OrderBy(orderSelector);
                else
                    set = set.OrderByDescending(orderSelector);
            }

            var lambda = CreateSelectLambdaExpression(columns);

            return set.Skip(start).Take(length).AsNoTracking().Select(lambda.Compile()).ToList();
        }

        public virtual T GetByKeys(object[] keyValues, params Expression<Func<T, dynamic>>[] includes)
        {
            if (keyValues == null || !keyValues.Any())
                throw new ArgumentNullException("keyValues");

            var setObject = ((IObjectContextAdapter)_dbContext).ObjectContext.CreateObjectSet<T>();
            //
            // get the key properties for entity
            var keyProperties = setObject.EntitySet.ElementType.KeyMembers.Select(k => k.Name).ToArray();
            //
            // throws exception if there are not keys
            if (!keyProperties.Any())
                throw new InvalidOperationException(string.Format("The {0} type has no key properties.", typeof(T).Name));

            var entity = Entity.AsQueryable();

            if (includes != null)
            {
                foreach (var inc in includes)
                {
                    entity = entity.Include(inc);
                }
            }

            var sqlWhere = new StringBuilder();
            var i = 0;
            foreach (var key in keyProperties)
            {
                sqlWhere.AppendFormat("AND {0} = @{1}", key, i++);
            }
            //
            // apply filter
            entity = entity.Where(sqlWhere.ToString().Substring(4), keyValues);
            //
            // return the selected object
            return entity.AsNoTracking().SingleOrDefault();
        }

        public List<T> GetMany(Expression<Func<T, bool>> filter, params Expression<Func<T, dynamic>>[] includes)
        {
            if (filter == null) throw new ArgumentNullException("filter");

            var query = GetQueryable(filter, includes);

            return query.ToList();
        }

        public void SetAutoSave(bool value)
        {
            _isAutoSave = value;
        }

        public virtual void Update(T entity)
        {
            DbEntityEntry dbEntityEntry = _dbContext.Entry(entity);
            if (dbEntityEntry.State == System.Data.Entity.EntityState.Detached)
            {
                Entity.Attach(entity);
            }
            dbEntityEntry.State = System.Data.Entity.EntityState.Modified;
            CommitAutoSave();
        }
    }
}
