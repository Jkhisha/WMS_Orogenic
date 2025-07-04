using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WareHouseMVC.Models
{ 
    public class SupportStuffRepository : ISupportStuffRepository
    {
        WareHouseMVCContext context;

         public  SupportStuffRepository()
            : this(new WareHouseMVCContext())
        {
            
        }
         public SupportStuffRepository(WareHouseMVCContext context)
        {
            
            this.context = context;
        }



        public IQueryable<SupportStuff> All
        {
            get { return context.SupportStuffs; }
        }

        public IQueryable<SupportStuff> AllIncluding(params Expression<Func<SupportStuff, object>>[] includeProperties)
        {
            IQueryable<SupportStuff> query = context.SupportStuffs;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public SupportStuff Find(long id)
        {
            return context.SupportStuffs.Find(id);
        }
         
        public void InsertOrUpdate(SupportStuff supportstuff)
        {
            if (supportstuff.SupportStuffID == default(long)) {
                // New entity
                context.SupportStuffs.Add(supportstuff);
            } else {
                // Existing entity
                context.Entry(supportstuff).State = EntityState.Modified;
            }
        }

        public void Delete(long id)
        {
            var supportstuff = context.SupportStuffs.Find(id);
            context.SupportStuffs.Remove(supportstuff);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Dispose() 
        {
            context.Dispose();
        }

      

        public SupportStuff GetByNameandDeptID(string tempSupportStuff, long _tempORBLDeptID)
        {
            return context.SupportStuffs.Where(s => s.SupportStuffName == tempSupportStuff && s.ORBLDepartmentID==_tempORBLDeptID).FirstOrDefault();
        }
    }

    public interface ISupportStuffRepository : IDisposable
    {
        IQueryable<SupportStuff> All { get; }
        IQueryable<SupportStuff> AllIncluding(params Expression<Func<SupportStuff, object>>[] includeProperties);
        SupportStuff Find(long id);
        void InsertOrUpdate(SupportStuff supportstuff);
        void Delete(long id);
        void Save();
    }
}