using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WareHouseMVC.Models
{ 
    public class ReceivedByRepository : IReceivedByRepository
    {
        WareHouseMVCContext context;

         public  ReceivedByRepository()
            : this(new WareHouseMVCContext())
        {
            
        }
         public ReceivedByRepository(WareHouseMVCContext context)
        {
            
            this.context = context;
        }



        public IQueryable<ReceivedBy> All
        {
            get { return context.ReceivedBies; }
        }

        public IQueryable<ReceivedBy> AllIncluding(params Expression<Func<ReceivedBy, object>>[] includeProperties)
        {
            IQueryable<ReceivedBy> query = context.ReceivedBies;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public ReceivedBy Find(long id)
        {
            return context.ReceivedBies.Find(id);
        }

        public void InsertOrUpdate(ReceivedBy receivedby)
        {
            if (receivedby.ReceivedById == default(long)) {
                // New entity
                context.ReceivedBies.Add(receivedby);
            } else {
                // Existing entity
                context.Entry(receivedby).State = EntityState.Modified;
            }
        }

        public void Delete(long id)
        {
            var receivedby = context.ReceivedBies.Find(id);
            context.ReceivedBies.Remove(receivedby);
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

    public interface IReceivedByRepository : IDisposable
    {
        IQueryable<ReceivedBy> All { get; }
        IQueryable<ReceivedBy> AllIncluding(params Expression<Func<ReceivedBy, object>>[] includeProperties);
        ReceivedBy Find(long id);
        void InsertOrUpdate(ReceivedBy receivedby);
        void Delete(long id);
        void Save();
    }
}