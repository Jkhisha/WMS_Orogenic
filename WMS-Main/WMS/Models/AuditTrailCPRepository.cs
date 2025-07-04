using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WareHouseMVC.Models
{
    public class AuditTrailCPRepository : IAuditTrailCPRepository
    {
        WareHouseMVCContext context;

         public  AuditTrailCPRepository()
            : this(new WareHouseMVCContext())
        {
            
        }
         public AuditTrailCPRepository(WareHouseMVCContext context)
        {
            
            this.context = context;
        }



         public IQueryable<AuditTrailCP> All
        {
            get { return context.AuditTrailCPs; }
        }

         public IQueryable<AuditTrailCP> AllIncluding(params Expression<Func<AuditTrailCP, object>>[] includeProperties)
        {
            IQueryable<AuditTrailCP> query = context.AuditTrailCPs;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

         public AuditTrailCP Find(long id)
        {
            return context.AuditTrailCPs.Find(id);
        }

        public void InsertOrUpdate(AuditTrailCP test)
        {
            if (test.AuditTrailCPId == default(long)) {
                // New entity
                context.AuditTrailCPs.Add(test);
            } else {
                // Existing entity
                context.Entry(test).State = EntityState.Modified;
            }
        }

        public void Delete(long id)
        {
            var test = context.AuditTrailCPs.Find(id);
            context.AuditTrailCPs.Remove(test);
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

    public interface IAuditTrailCPRepository : IDisposable
    {
        IQueryable<AuditTrailCP> All { get; }
        IQueryable<AuditTrailCP> AllIncluding(params Expression<Func<AuditTrailCP, object>>[] includeProperties);
        AuditTrailCP Find(long id);
        void InsertOrUpdate(AuditTrailCP test);
        void Delete(long id);
        void Save();
    }
}