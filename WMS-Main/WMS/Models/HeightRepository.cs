using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WareHouseMVC.Models
{ 
    public class HeightRepository : IHeightRepository
    {
        WareHouseMVCContext context;

         public  HeightRepository()
            : this(new WareHouseMVCContext())
        {
            
        }
         public HeightRepository(WareHouseMVCContext context)
        {
            
            this.context = context;
        }



        public IQueryable<Height> All
        {
            get { return context.Heights; }
        }

        public IQueryable<Height> AllIncluding(params Expression<Func<Height, object>>[] includeProperties)
        {
            IQueryable<Height> query = context.Heights;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }


        public IQueryable<Height> allList(params Expression<Func<Height, object>>[] includeProperties)
        {
            IQueryable<Height> query = context.Heights;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty).Take(10);
            }

            return query;
        }

        public Height Find(long id)
        {
            return context.Heights.Find(id);
        }

        public void InsertOrUpdate(Height height)
        {
            if (height.HeightID == default(long)) {
                // New entity
                context.Heights.Add(height);
            } else {
                // Existing entity
                context.Entry(height).State = EntityState.Modified;
            }
        }

        public void Delete(long id)
        {
            var height = context.Heights.Find(id);
            context.Heights.Remove(height);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Dispose() 
        {
            context.Dispose();
        }

        
        public List<Height> FindByLevelID(long LevelID)
        {
            return context.Heights.Where(z => z.LevelID == LevelID).ToList();
        }

        public  Height GetByLIDandHeightName(long _tempLevelID, string heightName)
        {
            return context.Heights.Where(h=>h.LevelID==_tempLevelID && h.HeightName==heightName).FirstOrDefault();
        }

        public List<Height> allList(long? LevelID)
        {
            return context.Heights.Where(z => z.LevelID == LevelID).ToList();
        }
    }

    public interface IHeightRepository : IDisposable
    {
        IQueryable<Height> All { get; }
        IQueryable<Height> AllIncluding(params Expression<Func<Height, object>>[] includeProperties);
        Height Find(long id);
        void InsertOrUpdate(Height height);
        void Delete(long id);
        void Save();
    }
}