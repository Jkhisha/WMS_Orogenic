using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WareHouseMVC.Models
{ 
    public class VehicleRepository : IVehicleRepository
    {
        WareHouseMVCContext context;

         public  VehicleRepository()
            : this(new WareHouseMVCContext())
        {
            
        }
         public VehicleRepository(WareHouseMVCContext context)
        {
            
            this.context = context;
        }



        public IQueryable<Vehicle> All
        {
            get { return context.Vehicles; }
        }

        public IQueryable<Vehicle> AllIncluding(params Expression<Func<Vehicle, object>>[] includeProperties)
        {
            IQueryable<Vehicle> query = context.Vehicles;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public Vehicle Find(long id)
        {
            return context.Vehicles.Find(id);
        }

        public void InsertOrUpdate(Vehicle vehicle)
        {
            if (vehicle.VehicleId == default(long)) {
                // New entity
                context.Vehicles.Add(vehicle);
            } else {
                // Existing entity
                context.Entry(vehicle).State = EntityState.Modified;
            }
        }

        public void Delete(long id)
        {
            var vehicle = context.Vehicles.Find(id);
            context.Vehicles.Remove(vehicle);
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

    public interface IVehicleRepository : IDisposable
    {
        IQueryable<Vehicle> All { get; }
        IQueryable<Vehicle> AllIncluding(params Expression<Func<Vehicle, object>>[] includeProperties);
        Vehicle Find(long id);
        void InsertOrUpdate(Vehicle vehicle);
        void Delete(long id);
        void Save();
    }
}