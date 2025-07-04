using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WareHouseMVC.Models
{ 
    public class ZoneRepository : IZoneRepository
    {
        WareHouseMVCContext context;



         public  ZoneRepository()
            : this(new WareHouseMVCContext())
        {
            
        }
         public ZoneRepository(WareHouseMVCContext context)
        {
            
            this.context = context;
        }

        public IQueryable<Zone> All
        {
            get { return context.Zones; }
        }

        public IQueryable<Zone> AllIncluding(params Expression<Func<Zone, object>>[] includeProperties)
        {
            IQueryable<Zone> query = context.Zones;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public IQueryable<Zone> allList(params Expression<Func<Zone, object>>[] includeProperties)
        {
            IQueryable<Zone> query = context.Zones;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty).Take(10);
            }
            return query;
        }

        public Zone Find(long id)
        {
            return context.Zones.Find(id);
        }

        public void InsertOrUpdate(Zone zone)
        {
            if (zone.ZoneID == default(long)) {
                // New entity
                context.Zones.Add(zone);
            } else {
                // Existing entity
                context.Entry(zone).State = EntityState.Modified;
            }
        }

        public void Delete(long id)
        {
            var zone = context.Zones.Find(id);
            context.Zones.Remove(zone);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Dispose() 
        {
            context.Dispose();
        }

        public List<Zone> FindByFloorID(long FloorID)
        {
            return context.Zones.Where(z => z.FloorID == FloorID).ToList();
        }

        public  Zone GetBywIDandFIDandzoneName(long _tempWarehouseID, long _tempFloorID, string zoneName)
        {
            return context.Zones.Where(z => z.WarehouseID == _tempWarehouseID && z.FloorID == _tempFloorID && z.ZoneName == zoneName).FirstOrDefault();
        }

        public  List<Zone> allList(long floorId)
        {
            return context.Zones.Where(z => z.FloorID == floorId).ToList();
        }
    }

    public interface IZoneRepository : IDisposable
    {
        IQueryable<Zone> All { get; }
        IQueryable<Zone> AllIncluding(params Expression<Func<Zone, object>>[] includeProperties);
        Zone Find(long id);
        void InsertOrUpdate(Zone zone);
        void Delete(long id);
        void Save();
    }
}