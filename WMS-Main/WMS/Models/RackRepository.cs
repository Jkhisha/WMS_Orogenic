using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WareHouseMVC.Models
{ 
    public class RackRepository : IRackRepository
    {
        WareHouseMVCContext context;

         public  RackRepository()
            : this(new WareHouseMVCContext())
        {
            
        }
         public RackRepository(WareHouseMVCContext context)
        {
            
            this.context = context;
        }



        public IQueryable<Rack> All
        {
            get { return context.Racks; }
        }

        public IQueryable<Rack> AllIncluding(params Expression<Func<Rack, object>>[] includeProperties)
        {
            IQueryable<Rack> query = context.Racks;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public IQueryable<Rack> allList(params Expression<Func<Rack, object>>[] includeProperties)
        {
            IQueryable<Rack> query = context.Racks;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty).Take(10);
            }
            return query;
        }

        public Rack Find(long id)
        {
            return context.Racks.Find(id);
        }

        public void InsertOrUpdate(Rack rack)
        {
            if (rack.RackID == default(long)) {
                // New entity
                context.Racks.Add(rack);
            } else {
                // Existing entity
                context.Entry(rack).State = EntityState.Modified;
            }
        }

        public void Delete(long id)
        {
            var rack = context.Racks.Find(id);
            context.Racks.Remove(rack);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Dispose() 
        {
            context.Dispose();
        }

      
        public List<Rack> FindByTrainID(long TrainID)
        {
            return context.Racks.Where(z => z.TrainID == TrainID).ToList();
        }

        public  Rack GetByWIDandFIDandZIDandTIDandRackName(long _tempWarehouseID, long _tempFloorID, long _tempZoneID, long _tempTrainID, string rackName)
        {
            return context.Racks.Where(r => r.WarehouseID == _tempWarehouseID && r.FloorID == _tempFloorID && r.ZoneID == _tempZoneID && r.TrainID == _tempTrainID && r.RackName == rackName).FirstOrDefault();

        }

        public List<Rack> allList(long? TrainID)
        {
            return context.Racks.Where(z => z.TrainID == TrainID).ToList();
        }
    }

    public interface IRackRepository : IDisposable
    {
        IQueryable<Rack> All { get; }
        IQueryable<Rack> AllIncluding(params Expression<Func<Rack, object>>[] includeProperties);
        Rack Find(long id);
        void InsertOrUpdate(Rack rack);
        void Delete(long id);
        void Save();
    }
}