using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WareHouseMVC.Models
{ 
    public class TrainRepository : ITrainRepository
    {
         WareHouseMVCContext context;
         public  TrainRepository()
            : this(new WareHouseMVCContext())
        {
            
        }
         public TrainRepository(WareHouseMVCContext context)
        {
            
            this.context = context;
        }


        public IQueryable<Train> All
        {
            get { return context.Trains; }
        }

        public IQueryable<Train> AllIncluding(params Expression<Func<Train, object>>[] includeProperties)
        {
            IQueryable<Train> query = context.Trains;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public IQueryable<Train> allList(params Expression<Func<Train, object>>[] includeProperties)
        {
            IQueryable<Train> query = context.Trains;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty).Take(10);
            }
            return query;
        }

        public Train Find(long id)
        {
            return context.Trains.Find(id);
        }

        public void InsertOrUpdate(Train train)
        {
            if (train.TrainID == default(long)) {
                // New entity
                context.Trains.Add(train);
            } else {
                // Existing entity
                context.Entry(train).State = EntityState.Modified;
            }
        }

        public void Delete(long id)
        {
            var train = context.Trains.Find(id);
            context.Trains.Remove(train);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Dispose() 
        {
            context.Dispose();
        }

       

        public List<Train> FindByZoneID(long ZoneID)
        {
            return context.Trains.Where(z => z.ZoneID == ZoneID).ToList();
        }

        public  Train GetByWIDandFIDandZIDandTrainName(long _tempWarehouseID, long _tempFloorID, long _tempZoneID, string trainName)
        {
            return context.Trains.Where(t => t.WarehouseID == _tempWarehouseID && t.FloorID == _tempFloorID && t.ZoneID == _tempZoneID && t.TrainName == trainName).FirstOrDefault();
        }

        public List<Train> allList(long zoneId)
        {
            return context.Trains.Where(z => z.ZoneID == zoneId).ToList();
        }
    }

    public interface ITrainRepository : IDisposable
    {
        IQueryable<Train> All { get; }
        IQueryable<Train> AllIncluding(params Expression<Func<Train, object>>[] includeProperties);
        Train Find(long id);
        void InsertOrUpdate(Train train);
        void Delete(long id);
        void Save();
    }
}