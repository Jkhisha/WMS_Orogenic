using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WareHouseMVC.Models
{ 
    public class HostInformationRepository : IHostInformationRepository
    {
        WareHouseMVCContext context;

         public  HostInformationRepository()
            : this(new WareHouseMVCContext())
        {
            
        }
         public HostInformationRepository(WareHouseMVCContext context)
        {
            
            this.context = context;
        }



        public IQueryable<HostInformation> All
        {
            get { return context.HostInformations; }
        }

        public IQueryable<HostInformation> AllIncluding(params Expression<Func<HostInformation, object>>[] includeProperties)
        {
            IQueryable<HostInformation> query = context.HostInformations;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public HostInformation Find(long id)
        {
            return context.HostInformations.Find(id);
        }

        public void InsertOrUpdate(HostInformation hostinformation)
        {
            if (hostinformation.HostInformationId == default(long)) {
                // New entity
                context.HostInformations.Add(hostinformation);
            } else {
                // Existing entity
                context.Entry(hostinformation).State = EntityState.Modified;
            }
        }

        public void Delete(long id)
        {
            var hostinformation = context.HostInformations.Find(id);
            context.HostInformations.Remove(hostinformation);
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

    public interface IHostInformationRepository : IDisposable
    {
        IQueryable<HostInformation> All { get; }
        IQueryable<HostInformation> AllIncluding(params Expression<Func<HostInformation, object>>[] includeProperties);
        HostInformation Find(long id);
        void InsertOrUpdate(HostInformation hostinformation);
        void Delete(long id);
        void Save();
    }
}