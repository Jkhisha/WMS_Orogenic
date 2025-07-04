using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WareHouseMVC.Models
{ 
    public class DriverRepository : IDriverRepository
    {
        WareHouseMVCContext context;

         public  DriverRepository()
            : this(new WareHouseMVCContext())
        {
            
        }
         public DriverRepository(WareHouseMVCContext context)
        {
            
            this.context = context;
        }



        public IQueryable<Driver> All
        {
            get { return context.Drivers; }
        }

        public IQueryable<Driver> AllIncluding(params Expression<Func<Driver, object>>[] includeProperties)
        {
            IQueryable<Driver> query = context.Drivers;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public Driver Find(long id)
        {
            return context.Drivers.Find(id);
        }

        public void InsertOrUpdate(Driver driver)
        {
            if (driver.DriverId == default(long)) {
                // New entity
                context.Drivers.Add(driver);
            } else {
                // Existing entity
                context.Entry(driver).State = EntityState.Modified;
            }
        }

        public void Delete(long id)
        {
            var driver = context.Drivers.Find(id);
            context.Drivers.Remove(driver);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Dispose() 
        {
            context.Dispose();
        }
    }

    public interface IDriverRepository : IDisposable
    {
        IQueryable<Driver> All { get; }
        IQueryable<Driver> AllIncluding(params Expression<Func<Driver, object>>[] includeProperties);
        Driver Find(long id);
        void InsertOrUpdate(Driver driver);
        void Delete(long id);
        void Save();
    }
}