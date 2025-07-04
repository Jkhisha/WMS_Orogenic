using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WareHouseMVC.Models
{ 
    public class LoginTrailRepository : ILoginTrailRepository
    {
        WareHouseMVCContext context;

         public  LoginTrailRepository()
            : this(new WareHouseMVCContext())
        {
            
        }
         public LoginTrailRepository(WareHouseMVCContext context)
        {
            
            this.context = context;
        }



        public IQueryable<LoginTrail> All
        {
            get { return context.LoginTrails; }
        }

        public IQueryable<LoginTrail> AllIncluding(params Expression<Func<LoginTrail, object>>[] includeProperties)
        {
            IQueryable<LoginTrail> query = context.LoginTrails;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public LoginTrail Find(long id)
        {
            return context.LoginTrails.Find(id);
        }

        public void InsertOrUpdate(LoginTrail logintrail)
        {
            if (logintrail.LoginTrainId == default(long)) {
                // New entity
                context.LoginTrails.Add(logintrail);
            } else {
                // Existing entity
                context.Entry(logintrail).State = EntityState.Modified;
            }
        }

        public void Delete(long id)
        {
            var logintrail = context.LoginTrails.Find(id);
            context.LoginTrails.Remove(logintrail);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Dispose() 
        {
            context.Dispose();
        }



        public  List<LoginTrail> GetAll()
        {
            return context.LoginTrails.OrderByDescending(l => l.LoginDate).ToList();
        }
    }

    public interface ILoginTrailRepository : IDisposable
    {
        IQueryable<LoginTrail> All { get; }
        IQueryable<LoginTrail> AllIncluding(params Expression<Func<LoginTrail, object>>[] includeProperties);
        LoginTrail Find(long id);
        void InsertOrUpdate(LoginTrail logintrail);
        void Delete(long id);
        void Save();
    }
}