using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WareHouseMVC.Models
{ 
    public class AutoZoneSuggentionRepository : IAutoZoneSuggentionRepository
    {
        WareHouseMVCContext context;

         public  AutoZoneSuggentionRepository()
            : this(new WareHouseMVCContext())
        {
            
        }
         public AutoZoneSuggentionRepository(WareHouseMVCContext context)
        {
            
            this.context = context;
        }



        public IQueryable<AutoZoneSuggention> All
        {
            get { return context.AutoZoneSuggentions; }
        }

        public IQueryable<AutoZoneSuggention> AllIncluding(params Expression<Func<AutoZoneSuggention, object>>[] includeProperties)
        {
            IQueryable<AutoZoneSuggention> query = context.AutoZoneSuggentions;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public AutoZoneSuggention Find(long id)
        {
            return context.AutoZoneSuggentions.Find(id);
        }

        public void InsertOrUpdate(AutoZoneSuggention autozonesuggention)
        {
            if (autozonesuggention.AutoZoneSuggentionId == default(long)) {
                // New entity
                context.AutoZoneSuggentions.Add(autozonesuggention);
            } else {
                // Existing entity
                context.Entry(autozonesuggention).State = EntityState.Modified;
            }
        }

        public void Delete(long id)
        {
            var autozonesuggention = context.AutoZoneSuggentions.Find(id);
            context.AutoZoneSuggentions.Remove(autozonesuggention);
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

    public interface IAutoZoneSuggentionRepository : IDisposable
    {
        IQueryable<AutoZoneSuggention> All { get; }
        IQueryable<AutoZoneSuggention> AllIncluding(params Expression<Func<AutoZoneSuggention, object>>[] includeProperties);
        AutoZoneSuggention Find(long id);
        void InsertOrUpdate(AutoZoneSuggention autozonesuggention);
        void Delete(long id);
        void Save();
    }
}