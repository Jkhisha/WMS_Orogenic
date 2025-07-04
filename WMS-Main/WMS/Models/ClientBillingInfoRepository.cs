using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WareHouseMVC.Models
{ 
    public class ClientBillingInfoRepository : IClientBillingInfoRepository
    {
        WareHouseMVCContext context;

         public  ClientBillingInfoRepository()
            : this(new WareHouseMVCContext())
        {
            
        }
         public ClientBillingInfoRepository(WareHouseMVCContext context)
        {
            
            this.context = context;
        }



        public IQueryable<ClientBillingInfo> All
        {
            get { return context.ClientBillingInfoes; }
        }

        public IQueryable<ClientBillingInfo> AllIncluding(params Expression<Func<ClientBillingInfo, object>>[] includeProperties)
        {
            IQueryable<ClientBillingInfo> query = context.ClientBillingInfoes;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public ClientBillingInfo Find(long id)
        {
            return context.ClientBillingInfoes.Find(id);
        }

        public void InsertOrUpdate(ClientBillingInfo clientbillinginfo)
        {
            if (clientbillinginfo.ClientBillingInfoId == default(long)) {
                // New entity
                context.ClientBillingInfoes.Add(clientbillinginfo);
            } else {
                // Existing entity
                context.Entry(clientbillinginfo).State = EntityState.Modified;
            }
        }

        public void Delete(long id)
        {
            var clientbillinginfo = context.ClientBillingInfoes.Find(id);
            context.ClientBillingInfoes.Remove(clientbillinginfo);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Dispose() 
        {
            context.Dispose();
        }

        public  List<ClientBillingInfo> GetAll()
        {
            return context.ClientBillingInfoes.ToList();
        }

        public  ClientBillingInfo GetByClientId(long clientId)
        {
            return context.ClientBillingInfoes.Where(c => c.ClientID == clientId).FirstOrDefault();
        }
    }

    public interface IClientBillingInfoRepository : IDisposable
    {
        IQueryable<ClientBillingInfo> All { get; }
        IQueryable<ClientBillingInfo> AllIncluding(params Expression<Func<ClientBillingInfo, object>>[] includeProperties);
        ClientBillingInfo Find(long id);
        void InsertOrUpdate(ClientBillingInfo clientbillinginfo);
        void Delete(long id);
        void Save();
    }
}