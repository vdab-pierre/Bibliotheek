using Bibliotheek.Domain.Abstract;
using Bibliotheek.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheek.Domain.Concrete
{
    public class EFBibliotheekRepository:DbContext,IBibliotheekRepository
    {
        public EFBibliotheekRepository()
            :base("name=LocalDb_Bib30")
        {

        }

        public DbSet<Boek> Boeken { get; set; }
        public DbSet<Auteur> Auteurs { get; set; }
        public DbSet<Uitgever> Uitgevers { get; set; }
        public DbSet<Etiket> Etiketten { get; set; }
        public DbSet<Exemplaar> Exemplaren { get; set; }
        public DbSet<Uitlening> Uitleningen { get; set; }

        public IQueryable<T> Query<T>() where T : class
        {
            return Set<T>();
        }

        public void Add<T>(T entity) where T : class
        {
            Set<T>().Add(entity);
        }

        public void Update<T>(T entity) where T : class
        {
            Entry(entity).State = System.Data.EntityState.Modified;
        }

        public void Remove<T>(T entity) where T : class
        {
            Set<T>().Remove(entity);
        }

        public new void SaveChanges()
        {
            base.SaveChanges();
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
