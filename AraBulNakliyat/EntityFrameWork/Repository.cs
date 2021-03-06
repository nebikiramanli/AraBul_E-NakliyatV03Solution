using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AraBulNakliyat.Common;
using AraBulNakliyat.Core.DataAccess;
using AraBulNakliyat.Entities;

namespace AraBulNakliyat.DataAccessLayer.EntityFrameWork
{
    public class Repository <T> :IDataAccess<T> where T : class 
    {
        private DatabaseContext dbContext;
        private DbSet<T> _objectSet;

        public Repository()
        {
           dbContext = RepositoryBase.CreateContext();
            _objectSet = dbContext.Set<T>();
        }

        public List<T> List()
        {
            return _objectSet.ToList();
        }

        public IQueryable<T> ListQueryable()
        {
            return _objectSet.AsQueryable<T>();
        }
        public List<T> List(Expression<Func<T, bool>> where)
        {
          return  _objectSet.Where(where).ToList();
        }

        // Orderby , ilk 10 Kayıdı atla son uc kayıdı ver turunden sorgularla veri 
        //veri cekmemizi saglayan metod 

        //public IQueryable List(Expression<Func<T, bool>> where)
        //{
        //    return _objectSet.Where(where);
        //}

        public int Insert(T obj)
        {
            _objectSet.Add(obj);
            if (obj is MyEntityBase)
            {
                MyEntityBase o= obj as MyEntityBase;

                DateTime now=DateTime.Now;
                o.CreatedOn = now;
                o.ModifiedOn = now;
                o.ModifiedUserName = App.Common.GetCurrentUserName();
            }

            
            return Save();
        }

        public int Update(T obj)
        {
            if (obj is MyEntityBase)
            {
                MyEntityBase o = obj as MyEntityBase;
                o.ModifiedOn = DateTime.Now;
                o.ModifiedUserName = App.Common.GetCurrentUserName();
            }

            return Save();
        }

        public int Delete(T obj)
        {
            //if (obj is MyEntityBase)
            //{
            //    MyEntityBase o = obj as MyEntityBase;
            //    o.ModifiedOn = DateTime.Now;
            //    o.ModifiedUserName =App.Common.GetCurrentUserName() ;// İşlem yapan kullanıcı adı yazılmalı
            //}
            _objectSet.Remove(obj);
            return Save();
        }
         

        public int Save()
        {
            // Hata Yakalamak İçin Yazıldı
            //try
            //{
            //    dbContext.SaveChanges();
            //}
            //catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            //{
            //    Exception raise = dbEx;
            //    foreach (var validationErrors in dbEx.EntityValidationErrors)
            //    {
            //        foreach (var validationError in validationErrors.ValidationErrors)
            //        {
            //            string message = string.Format("{0}:{1}",
            //                validationErrors.Entry.Entity.ToString(),
            //                validationError.ErrorMessage);
            //            // raise a new exception nesting  
            //            // the current instance as InnerException  
            //            raise = new InvalidOperationException(message, raise);
            //        }
            //    }
            //    throw raise;
            //}

            return dbContext.SaveChanges();
        }

        public T Find(Expression<Func<T, bool>> where)
        {
            return _objectSet.FirstOrDefault(where);
        }

         
    }
}
