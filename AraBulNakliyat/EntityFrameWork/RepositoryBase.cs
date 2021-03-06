using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AraBulNakliyat.DataAccessLayer;

namespace AraBulNakliyat.DataAccessLayer.EntityFrameWork
{

    //               ÖNEMLİ
    // Bı sınııfın amacı bırden cok new leme oldugu ıcın hafıza da tek new ı tutabılmek ıcın yapılır 


    // Bu olay Singleton Pattern Olarak Adlandırrılır 


   public class RepositoryBase
   {
       private static DatabaseContext _db;
       private  static  object _lockSync = new object();

       protected RepositoryBase()
       {
           
       }
       public static DatabaseContext CreateContext()
       {
          
           if (_db == null)
           {
               lock (_lockSync)
               {
                   if (_db == null)
                   {
                       _db = new DatabaseContext();
                   }
                   
               }
               
           }

           return _db;
       }
   }
}
