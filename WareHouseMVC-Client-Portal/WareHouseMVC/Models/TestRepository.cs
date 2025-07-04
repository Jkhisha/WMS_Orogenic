using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WareHouseMVC.Models
{ 
    public class TestRepository : ITestRepository
    {
        WareHouseMVCContext context;

         public  TestRepository()
            : this(new WareHouseMVCContext())
        {
            
        }
         public TestRepository(WareHouseMVCContext context)
        {
            
            this.context = context;
        }



        public IQueryable<Test> All
        {
            get { return context.Tests; }
        }

        public IQueryable<Test> AllIncluding(params Expression<Func<Test, object>>[] includeProperties)
        {
            IQueryable<Test> query = context.Tests;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public Test Find(long id)
        {
            return context.Tests.Find(id);
        }

        public void InsertOrUpdate(Test test)
        {
            if (test.TestID == default(long)) {
                // New entity
                context.Tests.Add(test);
            } else {
                // Existing entity
                context.Entry(test).State = EntityState.Modified;
            }
        }

        public void Delete(long id)
        {
            var test = context.Tests.Find(id);
            context.Tests.Remove(test);
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

    public interface ITestRepository : IDisposable
    {
        IQueryable<Test> All { get; }
        IQueryable<Test> AllIncluding(params Expression<Func<Test, object>>[] includeProperties);
        Test Find(long id);
        void InsertOrUpdate(Test test);
        void Delete(long id);
        void Save();
    }
}