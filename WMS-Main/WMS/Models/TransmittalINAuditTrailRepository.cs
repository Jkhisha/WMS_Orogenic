using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WareHouseMVC.Models
{ 
    public class TransmittalINAuditTrailRepository : ITransmittalINAuditTrailRepository
    {
        WareHouseMVCContext context;

         public  TransmittalINAuditTrailRepository()
            : this(new WareHouseMVCContext())
        {
            
        }
         public TransmittalINAuditTrailRepository(WareHouseMVCContext context)
        {
            
            this.context = context;
        }



        public IQueryable<TransmittalINAuditTrail> All
        {
            get { return context.TransmittalINAuditTrails; }
        }

        public IQueryable<TransmittalINAuditTrail> AllIncluding(params Expression<Func<TransmittalINAuditTrail, object>>[] includeProperties)
        {
            IQueryable<TransmittalINAuditTrail> query = context.TransmittalINAuditTrails;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public TransmittalINAuditTrail Find(long id)
        {
            return context.TransmittalINAuditTrails.Find(id);
        }

        public void InsertOrUpdate(TransmittalINAuditTrail transmittalinaudittrail)
        {
            if (transmittalinaudittrail.TransmittalINAuditTrailId == default(long)) {
                // New entity
                context.TransmittalINAuditTrails.Add(transmittalinaudittrail);
            } else {
                // Existing entity
                context.Entry(transmittalinaudittrail).State = EntityState.Modified;
            }
        }

        public void Delete(long id)
        {
            var transmittalinaudittrail = context.TransmittalINAuditTrails.Find(id);
            context.TransmittalINAuditTrails.Remove(transmittalinaudittrail);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Dispose() 
        {
            context.Dispose();
        }

        public  TransmittalINAuditTrail GetByTransmittalNo(string transmittalINNo)
        {
            return context.TransmittalINAuditTrails.Where(a => a.TransmittalINNo == transmittalINNo).FirstOrDefault();
        }
    }

    public interface ITransmittalINAuditTrailRepository : IDisposable
    {
        IQueryable<TransmittalINAuditTrail> All { get; }
        IQueryable<TransmittalINAuditTrail> AllIncluding(params Expression<Func<TransmittalINAuditTrail, object>>[] includeProperties);
        TransmittalINAuditTrail Find(long id);
        void InsertOrUpdate(TransmittalINAuditTrail transmittalinaudittrail);
        void Delete(long id);
        void Save();
    }
}