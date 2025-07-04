using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WareHouseMVC.Models
{ 
    public class ORBLOperatorRepository : IORBLOperatorRepository
    {
        WareHouseMVCContext context;

         public  ORBLOperatorRepository()
            : this(new WareHouseMVCContext())
        {
            
        }
         public ORBLOperatorRepository(WareHouseMVCContext context)
        {
            
            this.context = context;
        }



        public IQueryable<ORBLOperator> All
        {
            get { return context.ORBLOperators; }
        }

        public IQueryable<ORBLOperator> AllIncluding(params Expression<Func<ORBLOperator, object>>[] includeProperties)
        {
            IQueryable<ORBLOperator> query = context.ORBLOperators;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public ORBLOperator Find(long id)
        {
            return context.ORBLOperators.Find(id);
        }

        public void InsertOrUpdate(ORBLOperator orbloperator)
        {
            if (orbloperator.ORBLOperatorId == default(long)) {
                // New entity
                context.ORBLOperators.Add(orbloperator);
            } else {
                // Existing entity
                context.Entry(orbloperator).State = EntityState.Modified;
            }
        }

        public void Delete(long id)
        {
            var orbloperator = context.ORBLOperators.Find(id);
            context.ORBLOperators.Remove(orbloperator);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Dispose() 
        {
            context.Dispose();
        }

        public  ORBLOperator GetByName(string name)
        {
            return context.ORBLOperators.Where(o => o.Name == name).FirstOrDefault();
        }
    }

    public interface IORBLOperatorRepository : IDisposable
    {
        IQueryable<ORBLOperator> All { get; }
        IQueryable<ORBLOperator> AllIncluding(params Expression<Func<ORBLOperator, object>>[] includeProperties);
        ORBLOperator Find(long id);
        void InsertOrUpdate(ORBLOperator orbloperator);
        void Delete(long id);
        void Save();
    }
}