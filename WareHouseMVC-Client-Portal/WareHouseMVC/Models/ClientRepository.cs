using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WareHouseMVC.Models
{ 
    public class ClientRepository : IClientRepository
    {
        WareHouseMVCContext context;

         public  ClientRepository()
            : this(new WareHouseMVCContext())
        {
            
        }
         public ClientRepository(WareHouseMVCContext context)
        {
            
            this.context = context;
        }



        public IQueryable<Client> All
        {
            get { return context.Clients; }
        }

        public IQueryable<Client> AllIncluding(params Expression<Func<Client, object>>[] includeProperties)
        {
            IQueryable<Client> query = context.Clients;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public Client Find(long id)
        {
            return context.Clients.Find(id);
        }

        public void InsertOrUpdate(Client client)
        {
            if (client.ClientID == default(long)) {
                // New entity
                context.Clients.Add(client);
            } else {
                // Existing entity
                context.Entry(client).State = EntityState.Modified;
            }
        }

        public void InsertOrUpdateClientUser(ClientUser client)
        {
            if (client.ClientId == default(long))
            {
                // New entity
                context.ClientUsers.Add(client);
            }
            else
            {
                // Existing entity
                context.Entry(client).State = EntityState.Modified;
            }
        }

        public void Delete(long id)
        {
            var client = context.Clients.Find(id);
            context.Clients.Remove(client);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Dispose() 
        {
            context.Dispose();
        }

        public  Client GetByClientName(string clientName)
        {
            Client cl = new Client();
            cl = context.Clients.Where(c => c.ClientName == clientName).FirstOrDefault();
            return cl;
        }



        public  List<Client> GetAll()
        {
            return context.Clients.ToList();
        }

        public List<Client> GetAllUserClient()
        {
            return context.Clients.Where(c => c.UserName != null).ToList();
        }

        public  Client GetByUserID(Guid userId)
        {
            return context.Clients.Where(c => c.UserId == userId).FirstOrDefault();
        }

        public  Client GetByUserName(string userName)
        {
            return context.Clients.Where(c => c.UserName == userName).FirstOrDefault();
        }

        public ClientUser GetClientUserByUserName(string userName)
        {
            return context.ClientUsers.Where(c => c.UserName == userName).FirstOrDefault();
        }

        internal ClientUser GetcUserByUserId(Guid guid)
        {
            return context.ClientUsers.Where(s => s.UserId == guid).FirstOrDefault();
        }
    }

    public interface IClientRepository : IDisposable
    {
        IQueryable<Client> All { get; }
        IQueryable<Client> AllIncluding(params Expression<Func<Client, object>>[] includeProperties);
        Client Find(long id);
        void InsertOrUpdate(Client client);
        void Delete(long id);
        void Save();
    }
}