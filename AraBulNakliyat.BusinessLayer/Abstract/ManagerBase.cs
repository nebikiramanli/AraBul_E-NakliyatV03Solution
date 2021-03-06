using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AraBulNakliyat.Core.DataAccess;
using AraBulNakliyat.DataAccessLayer.EntityFrameWork;

namespace AraBulNakliyat.BusinessLayer.Abstract
{
    public abstract class ManagerBase<T> : IDataAccess<T> where T : class
    {
        private Repository<T> repository = new Repository<T>();
        // Metodların virtual olmasının sebebi Ezilebllir metod istiyor olmammız
        // Metodlara virtual özelliği ekleyerek ezebiliriz
        public virtual List<T> List()
        {
            return repository.List();
        }

        public virtual IQueryable<T> ListQueryable()
        {
            return repository.ListQueryable();
        }

        public virtual List<T> List(Expression<Func<T, bool>> where)
        {
            return repository.List(where);
        }

        public virtual int Insert(T obj)
        {
            
            return repository.Insert(obj);
        }

        public virtual int Update(T obj)
        {
            return repository.Update(obj);
        }

        public virtual int Delete(T obj)
        {
            return repository.Delete(obj);
        }

        public virtual int Save()
        {
            return repository.Save();
        }

        public virtual T Find(Expression<Func<T, bool>> where)
        {
            return repository.Find(where);
        }
    }
}
