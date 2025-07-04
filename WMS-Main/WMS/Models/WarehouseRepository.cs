using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WareHouseMVC.Models
{ 
    public class WarehouseRepository : IWarehouseRepository
    {
        WareHouseMVCContext context;
         public  WarehouseRepository()
            : this(new WareHouseMVCContext())
        {
            
        }
         public WarehouseRepository(WareHouseMVCContext context)
        {
            
            this.context = context;
        }

        public IQueryable<Warehouse> All
        {
            get { return context.Warehouses; }
        }

        public IQueryable<Warehouse> AllIncluding(params Expression<Func<Warehouse, object>>[] includeProperties)
        {
            IQueryable<Warehouse> query = context.Warehouses;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public Warehouse Find(long id)
        {
            return context.Warehouses.Find(id);
        }

        public void InsertOrUpdate(Warehouse warehouse)
        {
            if (warehouse.WarehouseID == default(long)) {
                // New entity
                context.Warehouses.Add(warehouse);
            } else {
                // Existing entity
                context.Entry(warehouse).State = EntityState.Modified;
            }
        }

        public void Delete(long id)
        {
            var warehouse = context.Warehouses.Find(id);
            context.Warehouses.Remove(warehouse);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Dispose() 
        {
            context.Dispose();
        }

        public  Warehouse GetFirst()
        {
            return context.Warehouses.FirstOrDefault();
        }

        public  List<Warehouse> GetListByID(long wID)
        {
            return context.Warehouses.Where(w => w.WarehouseID == wID).ToList();
        }

        public List<Warehouse> GetAllList()
        {
            return context.Warehouses.ToList();
        }

        public  Warehouse GetByName(string wareHouseName)
        {
            return context.Warehouses.Where(w => w.WarehouseName == wareHouseName).FirstOrDefault();
        }

       
    }

    public interface IWarehouseRepository : IDisposable
    {
        IQueryable<Warehouse> All { get; }
        IQueryable<Warehouse> AllIncluding(params Expression<Func<Warehouse, object>>[] includeProperties);
        Warehouse Find(long id);
        void InsertOrUpdate(Warehouse warehouse);
        void Delete(long id);
        void Save();
    }
}