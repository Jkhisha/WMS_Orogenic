using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WareHouseMVC.Models
{ 
    public class TransmittalOUTStatusRepository : ITransmittalOUTStatusRepository
    {
        WareHouseMVCContext context;

         public  TransmittalOUTStatusRepository()
            : this(new WareHouseMVCContext())
        {
            
        }
         public TransmittalOUTStatusRepository(WareHouseMVCContext context)
        {
            
            this.context = context;
        }



        public IQueryable<TransmittalOUTStatus> All
        {
            get { return context.TransmittalOUTStatus; }
        }

        public IQueryable<TransmittalOUTStatus> AllIncluding(params Expression<Func<TransmittalOUTStatus, object>>[] includeProperties)
        {
            IQueryable<TransmittalOUTStatus> query = context.TransmittalOUTStatus;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public TransmittalOUTStatus Find(long id)
        {
            return context.TransmittalOUTStatus.Find(id);
        }

        public void InsertOrUpdate(TransmittalOUTStatus transmittaloutstatus)
        {
            if (transmittaloutstatus.TransmittalOUTStatusId == default(long)) {
                // New entity
                context.TransmittalOUTStatus.Add(transmittaloutstatus);
            } else {
                // Existing entity
                context.Entry(transmittaloutstatus).State = EntityState.Modified;
            }
        }

        public void Delete(long id)
        {
            var transmittaloutstatus = context.TransmittalOUTStatus.Find(id);
            context.TransmittalOUTStatus.Remove(transmittaloutstatus);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Dispose() 
        {
            context.Dispose();
        }

        public  TransmittalOUTStatus GetFirst()
        {
            return context.TransmittalOUTStatus.FirstOrDefault();
        }
    }

    public interface ITransmittalOUTStatusRepository : IDisposable
    {
        IQueryable<TransmittalOUTStatus> All { get; }
        IQueryable<TransmittalOUTStatus> AllIncluding(params Expression<Func<TransmittalOUTStatus, object>>[] includeProperties);
        TransmittalOUTStatus Find(long id);
        void InsertOrUpdate(TransmittalOUTStatus transmittaloutstatus);
        void Delete(long id);
        void Save();
    }
}