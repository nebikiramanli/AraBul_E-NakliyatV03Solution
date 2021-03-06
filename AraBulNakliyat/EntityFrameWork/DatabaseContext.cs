using AraBulNakliyat.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AraBulNakliyat.DataAccessLayer.EntityFrameWork
{
    public class DatabaseContext :DbContext
    {
        public DbSet<AraBulUser> AraBulUsers { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Liked>  Likes { get; set; }
        public DbSet<Notice> Notices { get; set; }

        public DatabaseContext()
        {
            Database.SetInitializer(new MyInitializer() );
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Fluent Apı


            // Kategori Silinmesi İçin ilişkili like ve yorumlarında silinmesi İçin
            // Database olusurken Cascade yaparak ilişkilerinde Sİlinmesini Sagliyoruz

            //Yorumların Silinmesi
            modelBuilder.Entity<Notice>()
                .HasMany(n => n.Comments)
                .WithRequired(c => c.Notice)
                .WillCascadeOnDelete(true);

            // Like Silinmesi
            modelBuilder.Entity<Notice>()
                .HasMany(n => n.Likes)
                .WithRequired(c => c.Notice)
                .WillCascadeOnDelete(true);
        }
    }
}
