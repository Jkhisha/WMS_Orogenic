using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WareHouseMVC.Models
{ 
    public class HandOverByRepository : IHandOverByRepository
    {
        WareHouseMVCContext context;

         public  HandOverByRepository()
            : this(new WareHouseMVCContext())
        {
            
        }
         public HandOverByRepository(WareHouseMVCContext context)
        {
            
            this.context = context;
        }



        public IQueryable<HandOverBy> All
        {
            get { return context.HandOverBies; }
        }

        public IQueryable<HandOverBy> AllIncluding(params Expression<Func<HandOverBy, object>>[] includeProperties)
        {
            IQueryable<HandOverBy> query = context.HandOverBies;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public HandOverBy Find(long id)
        {
            return context.HandOverBies.Find(id);
        }

        public void InsertOrUpdate(HandOverBy handoverby)
        {
            if (handoverby.HandOverById == default(long)) {
                // New entity
                context.HandOverBies.Add(handoverby);
            } else {
                // Existing entity
                context.Entry(handoverby).State = EntityState.Modified;
            }
        }

        public void Delete(long id)
        {
            var handoverby = context.HandOverBies.Find(id);
            context.HandOverBies.Remove(handoverby);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Dispose() 
        {
            context.Dispose();
        }

        public  HandOverBy GetByTrId(int trId)
        {
            //context.HandOverBies.
            return null;// context.HandOverBies.Where(h => h.TransmittalINs[0].TransmittalINId == trId).FirstOrDefault();
        }
    }

    public interface IHandOverByRepository : IDisposable
    {
        IQueryable<HandOverBy> All { get; }
        IQueryable<HandOverBy> AllIncluding(params Expression<Func<HandOverBy, object>>[] includeProperties);
        HandOverBy Find(long id);
        void InsertOrUpdate(HandOverBy handoverby);
        HandOverBy GetByTrId(int trId);
        void Delete(long id);
        void Save();
    }
}