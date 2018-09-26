using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        DbSet<T> Entity { get; }

        void Add(T entity);

        void Commit();

        void CommitAutoSave();

        void Delete(T entity);

        void Delete(object[] keys);

        T Get(Expression<Func<T, bool>> filter, params Expression<Func<T, dynamic>>[] includes);

        List<object> Get<TOrder>(int start, int length, out int totalRecords, string[] columns, Expression<Func<T, bool>> filter = null, Expression<Func<T, TOrder>> orderSelector = null, bool ascendingOrder = true);

        T GetByKeys(object[] keyValues, params Expression<Func<T, dynamic>>[] includes);

        List<T> GetMany(Expression<Func<T, bool>> filter, params Expression<Func<T, dynamic>>[] includes);

        void SetAutoSave(bool value);

        void Update(T entity);
    }
}
