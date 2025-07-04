using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WareHouseMVC.Models
{ 
    public class FloorRepository : IFloorRepository
    {
        
        WareHouseMVCContext context;
         public  FloorRepository()
            : this(new WareHouseMVCContext())
        {
            
        }
         public FloorRepository(WareHouseMVCContext context)
        {
            
            this.context = context;
        }





        public IQueryable<Floor> All
        {
            get { return context.Floors; }
        }

        public IQueryable<Floor> AllIncluding(params Expression<Func<Floor, object>>[] includeProperties)
        {
            IQueryable<Floor> query = context.Floors;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }
        public List<Floor> allList()
        {

            return context.Floors.Take(2).ToList();
        }


        public Floor Find(long id)
        {
            return context.Floors.Find(id);
        }

        public List<Floor> FindByWarehouseID(long WarehouseID)
        {
            return context.Floors.Where(f => f.WarehouseID == WarehouseID).ToList();
        }

        public void InsertOrUpdate(Floor floor)
        {
            if (floor.FloorID == default(long)) {
                // New entity
                context.Floors.Add(floor);
            } else {
                // Existing entity
                context.Entry(floor).State = EntityState.Modified;
            }
        }

        public void Delete(long id)
        {
            var floor = context.Floors.Find(id);
            context.Floors.Remove(floor);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Dispose() 
        {
            context.Dispose();
        }

        public Floor GetByWidandFName(long _tempWarehouseID, string floorName)
        {
            return context.Floors.Where(f => f.WarehouseID == _tempWarehouseID && f.FloorName == floorName).FirstOrDefault();
        }

        public List<Floor> allList(long wId)
        {
            return context.Floors.Where(w => w.WarehouseID == wId).ToList();
        }
    }

    public interface IFloorRepository : IDisposable
    {
        IQueryable<Floor> All { get; }
        IQueryable<Floor> AllIncluding(params Expression<Func<Floor, object>>[] includeProperties);
        Floor Find(long id);
        void InsertOrUpdate(Floor floor);
        void Delete(long id);
        void Save();
    }
}