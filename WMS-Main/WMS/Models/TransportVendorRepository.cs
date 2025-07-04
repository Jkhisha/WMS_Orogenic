using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WareHouseMVC.Models
{ 
    public class TransportVendorRepository : ITransportVendorRepository
    {
        WareHouseMVCContext context;

         public  TransportVendorRepository()
            : this(new WareHouseMVCContext())
        {
            
        }
         public TransportVendorRepository(WareHouseMVCContext context)
        {
            
            this.context = context;
        }



        public IQueryable<TransportVendor> All
        {
            get { return context.TransportVendors; }
        }

        public IQueryable<TransportVendor> AllIncluding(params Expression<Func<TransportVendor, object>>[] includeProperties)
        {
            IQueryable<TransportVendor> query = context.TransportVendors;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public TransportVendor Find(long id)
        {
            return context.TransportVendors.Find(id);
        }

        public void InsertOrUpdate(TransportVendor transportvendor)
        {
            if (transportvendor.TransportVendorId == default(long)) {
                // New entity
                context.TransportVendors.Add(transportvendor);
            } else {
                // Existing entity
                context.Entry(transportvendor).State = EntityState.Modified;
            }
        }

        public void Delete(long id)
        {
            var transportvendor = context.TransportVendors.Find(id);
            context.TransportVendors.Remove(transportvendor);
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

    public interface ITransportVendorRepository : IDisposable
    {
        IQueryable<TransportVendor> All { get; }
        IQueryable<TransportVendor> AllIncluding(params Expression<Func<TransportVendor, object>>[] includeProperties);
        TransportVendor Find(long id);
        void InsertOrUpdate(TransportVendor transportvendor);
        void Delete(long id);
        void Save();
    }
}