using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WareHouseMVC.Models
{
    public class BoxDestructionRepository : IBoxDestructionRepository
    {
         WareHouseMVCContext context;

         public  BoxDestructionRepository()
            : this(new WareHouseMVCContext())
        {
            
        }
         public BoxDestructionRepository(WareHouseMVCContext context)
        {
            
            this.context = context;
        }



         public IQueryable<BoxDestruction> All
        {
            get { return context.BoxDestructions; }
        }

         public IQueryable<BoxDestruction> AllIncluding(params Expression<Func<BoxDestruction, object>>[] includeProperties)
        {
            IQueryable<BoxDestruction> query = context.BoxDestructions;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

         public BoxDestruction Find(long id)
        {
            return context.BoxDestructions.Find(id);
        }

         public void InsertOrUpdate(BoxDestruction test)
        {
            if (test.BoxDestructionId == default(long)) {
                // New entity
                context.BoxDestructions.Add(test);
            } else {
                // Existing entity
                context.Entry(test).State = EntityState.Modified;
            }
        }

        public void Delete(long id)
        {
            var test = context.BoxDestructions.Find(id);
            context.BoxDestructions.Remove(test);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Dispose() 
        {
            context.Dispose();
        }

        internal List<BoxDestruction> GetAll()
        {
            return context.BoxDestructions.ToList();
        }
    }

    public interface IBoxDestructionRepository : IDisposable
    {
        IQueryable<BoxDestruction> All { get; }
        IQueryable<BoxDestruction> AllIncluding(params Expression<Func<BoxDestruction, object>>[] includeProperties);
        BoxDestruction Find(long id);
        void InsertOrUpdate(BoxDestruction test);
        void Delete(long id);
        void Save();
    }
}