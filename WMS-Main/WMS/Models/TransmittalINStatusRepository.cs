using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WareHouseMVC.Models
{ 
    public class TransmittalINStatusRepository : ITransmittalINStatusRepository
    {
        WareHouseMVCContext context;

         public  TransmittalINStatusRepository()
            : this(new WareHouseMVCContext())
        {
            
        }
         public TransmittalINStatusRepository(WareHouseMVCContext context)
        {
            
            this.context = context;
        }



        public IQueryable<TransmittalINStatus> All
        {
            get { return context.TransmittalINStatus; }
        }

        public IQueryable<TransmittalINStatus> AllIncluding(params Expression<Func<TransmittalINStatus, object>>[] includeProperties)
        {
            IQueryable<TransmittalINStatus> query = context.TransmittalINStatus;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public TransmittalINStatus Find(long id)
        {
            return context.TransmittalINStatus.Find(id);
        }

        public void InsertOrUpdate(TransmittalINStatus transmittalinstatus)
        {
            if (transmittalinstatus.TransmittalINStatusId == default(long)) {
                // New entity
                context.TransmittalINStatus.Add(transmittalinstatus);
            } else {
                // Existing entity
                context.Entry(transmittalinstatus).State = EntityState.Modified;
            }
        }

        public void Delete(long id)
        {
            var transmittalinstatus = context.TransmittalINStatus.Find(id);
            context.TransmittalINStatus.Remove(transmittalinstatus);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Dispose() 
        {
            context.Dispose();
        }

      
    
public  TransmittalINStatus GetFirst()
{
    return context.TransmittalINStatus.FirstOrDefault();
}
    
    }

    public interface ITransmittalINStatusRepository : IDisposable
    {
        IQueryable<TransmittalINStatus> All { get; }
        IQueryable<TransmittalINStatus> AllIncluding(params Expression<Func<TransmittalINStatus, object>>[] includeProperties);
        TransmittalINStatus Find(long id);
        void InsertOrUpdate(TransmittalINStatus transmittalinstatus);
        void Delete(long id);
        void Save();
    }
}