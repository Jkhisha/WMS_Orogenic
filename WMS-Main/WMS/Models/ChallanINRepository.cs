using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WareHouseMVC.Models
{ 
    public class ChallanINRepository : IChallanINRepository
    {
        WareHouseMVCContext context;

         public  ChallanINRepository()
            : this(new WareHouseMVCContext())
        {
            
        }
         public ChallanINRepository(WareHouseMVCContext context)
        {
            
            this.context = context;
        }

        public IQueryable<ChallanIN> All
        {
            get { return context.ChallanINs; }
        }

        public IQueryable<ChallanIN> AllIncluding(params Expression<Func<ChallanIN, object>>[] includeProperties)
        {
            IQueryable<ChallanIN> query = context.ChallanINs;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public ChallanIN Find(long id)
        {
            return context.ChallanINs.Find(id);
        }

        public void InsertOrUpdate(ChallanIN challanin)
        {
            if (challanin.ChallanINId == default(long)) {
                // New entity
                context.ChallanINs.Add(challanin);
            } else {
                // Existing entity
                context.Entry(challanin).State = EntityState.Modified;
            }
        }

        public void Delete(long id)
        {
            var challanin = context.ChallanINs.Find(id);
            context.ChallanINs.Remove(challanin);
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

    public interface IChallanINRepository : IDisposable
    {
        IQueryable<ChallanIN> All { get; }
        IQueryable<ChallanIN> AllIncluding(params Expression<Func<ChallanIN, object>>[] includeProperties);
        ChallanIN Find(long id);
        void InsertOrUpdate(ChallanIN challanin);
        void Delete(long id);
        void Save();
    }
}