using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WareHouseMVC.Models
{ 
    public class TransmittalOUTAuditTrailRepository : ITransmittalOUTAuditTrailRepository
    {
        WareHouseMVCContext context;

         public  TransmittalOUTAuditTrailRepository()
            : this(new WareHouseMVCContext())
        {
            
        }
         public TransmittalOUTAuditTrailRepository(WareHouseMVCContext context)
        {
            
            this.context = context;
        }



        public IQueryable<TransmittalOUTAuditTrail> All
        {
            get { return context.TransmittalOUTAuditTrails; }
        }

        public IQueryable<TransmittalOUTAuditTrail> AllIncluding(params Expression<Func<TransmittalOUTAuditTrail, object>>[] includeProperties)
        {
            IQueryable<TransmittalOUTAuditTrail> query = context.TransmittalOUTAuditTrails;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public TransmittalOUTAuditTrail Find(long id)
        {
            return context.TransmittalOUTAuditTrails.Find(id);
        }

        public void InsertOrUpdate(TransmittalOUTAuditTrail transmittaloutaudittrail)
        {
            if (transmittaloutaudittrail.TransmittalOUTAuditTrailId == default(long)) {
                // New entity
                context.TransmittalOUTAuditTrails.Add(transmittaloutaudittrail);
            } else {
                // Existing entity
                context.Entry(transmittaloutaudittrail).State = EntityState.Modified;
            }
        }

        public void Delete(long id)
        {
            var transmittaloutaudittrail = context.TransmittalOUTAuditTrails.Find(id);
            context.TransmittalOUTAuditTrails.Remove(transmittaloutaudittrail);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Dispose() 
        {
            context.Dispose();
        }

        public  TransmittalOUTAuditTrail GetByTransmittalNo(string transmittalNo)
        {
            return context.TransmittalOUTAuditTrails.Where(a => a.TransmittalOUTNo == transmittalNo).FirstOrDefault();
        }
    }

    public interface ITransmittalOUTAuditTrailRepository : IDisposable
    {
        IQueryable<TransmittalOUTAuditTrail> All { get; }
        IQueryable<TransmittalOUTAuditTrail> AllIncluding(params Expression<Func<TransmittalOUTAuditTrail, object>>[] includeProperties);
        TransmittalOUTAuditTrail Find(long id);
        void InsertOrUpdate(TransmittalOUTAuditTrail transmittaloutaudittrail);
        void Delete(long id);
        void Save();
    }
}