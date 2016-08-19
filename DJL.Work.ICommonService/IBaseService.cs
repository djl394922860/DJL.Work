using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DJL.Work.ICommonService
{
    public interface IBaseService<T> where T : class
    {
        T GetEntityByPrimaryKey(int Id);

        void Add(T entity);

        void Update(T entity);

        void Delete(T entity);

        int Delete(params int[] ids);

        IEnumerable<T> LoadEntities(Expression<Func<T, bool>> whereLambda);

        IEnumerable<T> LoadPageEntities<S>(int pageSize, int pageIndex, out int total, Expression<Func<T, bool>> whereLambda, Expression<Func<T, S>> orderbyLambda, bool isAsc);
    }
}
