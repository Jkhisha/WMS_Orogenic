using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WareHouseMVC.Models
{ 
    public class RegionRepository : IRegionRepository
    {
        WareHouseMVCContext context;

         public  RegionRepository()
            : this(new WareHouseMVCContext())
        {
            
        }
         public RegionRepository(WareHouseMVCContext context)
        {
            
            this.context = context;
        }



        public IQueryable<Region> All
        {
            get { return context.Regions; }
        }

        public IQueryable<Region> AllIncluding(params Expression<Func<Region, object>>[] includeProperties)
        {
            IQueryable<Region> query = context.Regions;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public Region Find(long id)
        {
            return context.Regions.Find(id);
        }

        public void InsertOrUpdate(Region region)
        {
            if (region.RegionId == default(long)) {
                // New entity
                context.Regions.Add(region);
            } else {
                // Existing entity
                context.Entry(region).State = EntityState.Modified;
            }
        }

        public void Delete(long id)
        {
            var region = context.Regions.Find(id);
            context.Regions.Remove(region);
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

    public interface IRegionRepository : IDisposable
    {
        IQueryable<Region> All { get; }
        IQueryable<Region> AllIncluding(params Expression<Func<Region, object>>[] includeProperties);
        Region Find(long id);
        void InsertOrUpdate(Region region);
        void Delete(long id);
        void Save();
    }
}