using DJL.Work.DataModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DJL.Work.CommonService
{
    public class BaseService<T> where T : class
    {
        public virtual T GetEntityByPrimaryKey(int Id)
        {
            using (var db = new MusicOnlineDBContext())
            {
                return db.Set<T>().Find(Id);
            }
        }

        public virtual void Add(T entity)
        {
            using (var db = new MusicOnlineDBContext())
            {
                db.Set<T>().Add(entity);
                db.SaveChanges();
            }
        }

        public virtual void Update(T entity)
        {
            using (var db = new MusicOnlineDBContext())
            {
                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public virtual void Delete(T entity)
        {
            using (var db = new MusicOnlineDBContext())
            {
                db.Entry(entity).State = EntityState.Deleted;
                db.SaveChanges();
            }
        }

        public virtual int Delete(params int[] ids)
        {
            int count;
            using (var db = new MusicOnlineDBContext())
            {
                foreach (var item in ids)
                {
                    var entity = db.Set<T>().Find(item);
                    if (entity != null)
                        db.Set<T>().Remove(entity);
                }
                count = db.SaveChanges();
            }
            return count;
        }

        public IEnumerable<T> LoadEntities(Expression<Func<T, bool>> whereLambda)
        {
            using (var db = new MusicOnlineDBContext())
            {
                return db.Set<T>().Where(whereLambda).ToArray();
            }
        }


        public IEnumerable<T> LoadPageEntities<S>(int pageSize, int pageIndex, out int total, Expression<Func<T, bool>> whereLambda, Expression<Func<T, S>> orderbyLambda, bool isAsc)
        {
            using (var db = new MusicOnlineDBContext())
            {
                total = db.Set<T>().Where(whereLambda).Count();

                if (isAsc)
                {
                    return
                    db.Set<T>()
                      .Where(whereLambda)
                      .OrderBy(orderbyLambda)
                      .Skip(pageSize * (pageIndex - 1))
                      .Take(pageSize)
                      .ToArray();
                }
                else
                {
                    return
                   db.Set<T>()
                     .Where(whereLambda)
                     .OrderByDescending(orderbyLambda)
                     .Skip(pageSize * (pageIndex - 1))
                     .Take(pageSize)
                     .ToArray();
                }
            }
        }
    }
}
