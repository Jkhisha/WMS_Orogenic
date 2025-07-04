using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WareHouseMVC.Models
{ 
    public class LevelRepository : ILevelRepository
    {
        WareHouseMVCContext context;

         public  LevelRepository()
            : this(new WareHouseMVCContext())
        {
            
        }
         public LevelRepository(WareHouseMVCContext context)
        {
            
            this.context = context;
        }



        public IQueryable<Level> All
        {
            get { return context.Levels; }
        }

        public IQueryable<Level> AllIncluding(params Expression<Func<Level, object>>[] includeProperties)
        {
            IQueryable<Level> query = context.Levels;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public IQueryable<Level> allList(params Expression<Func<Level, object>>[] includeProperties)
        {
            IQueryable<Level> query = context.Levels;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty).Take(10);
            }
            return query;
        }

        public Level Find(long id)
        {
            return context.Levels.Find(id);
        }

        public void InsertOrUpdate(Level level)
        {
            if (level.LevelID == default(long)) {
                // New entity
                context.Levels.Add(level);
            } else {
                // Existing entity
                context.Entry(level).State = EntityState.Modified;
            }
        }

        public void Delete(long id)
        {
            var level = context.Levels.Find(id);
            context.Levels.Remove(level);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Dispose() 
        {
            context.Dispose();
        }

        public List<Level> FindByRackID(long RackID)
        {
            return context.Levels.Where(z => z.RackID == RackID).ToList();
        }

        public  Level GetByWIDandFIDandZIDandTIDandRIDandLName(long _tempWarehouseID, long _tempFloorID, long _tempZoneID, long _tempTrainID, long _tempRackID, string levelName)
        {
            return context.Levels.Where(l => l.WarehouseID == _tempWarehouseID && l.FloorID == _tempFloorID && l.ZoneID == _tempZoneID && l.TrainID == _tempTrainID && l.RackID == _tempRackID && l.LevelName == levelName).FirstOrDefault();
        }

        public List<Level> allList(long? RackID)
        {
            return context.Levels.Where(z => z.RackID == RackID).ToList();
        }
    }

    public interface ILevelRepository : IDisposable
    {
        IQueryable<Level> All { get; }
        IQueryable<Level> AllIncluding(params Expression<Func<Level, object>>[] includeProperties);
        Level Find(long id);
        void InsertOrUpdate(Level level);
        void Delete(long id);
        void Save();
    }
}