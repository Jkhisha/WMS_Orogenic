using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WareHouseMVC.Models
{ 
    public class ORBLDepartmentRepository : IORBLDepartmentRepository
    {
        WareHouseMVCContext context;

         public  ORBLDepartmentRepository()
            : this(new WareHouseMVCContext())
        {
            
        }
         public ORBLDepartmentRepository(WareHouseMVCContext context)
        {
            
            this.context = context;
        }



        public IQueryable<ORBLDepartment> All
        {
            get { return context.ORBLDepartments; }
        }

        public IQueryable<ORBLDepartment> AllIncluding(params Expression<Func<ORBLDepartment, object>>[] includeProperties)
        {
            IQueryable<ORBLDepartment> query = context.ORBLDepartments;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public ORBLDepartment Find(long id)
        {
            return context.ORBLDepartments.Find(id);
        }

        public void InsertOrUpdate(ORBLDepartment orbldepartment)
        {
            if (orbldepartment.ORBLDepartmentID == default(long)) {
                // New entity
                context.ORBLDepartments.Add(orbldepartment);
            } else {
                // Existing entity
                context.Entry(orbldepartment).State = EntityState.Modified;
            }
        }

        public void Delete(long id)
        {
            var orbldepartment = context.ORBLDepartments.Find(id);
            context.ORBLDepartments.Remove(orbldepartment);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Dispose() 
        {
            context.Dispose();
        }

        public  ORBLDepartment GetByName(string tempOrblDeptName)
        {
            return context.ORBLDepartments.Where(d => d.ORBLDepartmentName == tempOrblDeptName).FirstOrDefault();
        }
    }

    public interface IORBLDepartmentRepository : IDisposable
    {
        IQueryable<ORBLDepartment> All { get; }
        IQueryable<ORBLDepartment> AllIncluding(params Expression<Func<ORBLDepartment, object>>[] includeProperties);
        ORBLDepartment Find(long id);
        void InsertOrUpdate(ORBLDepartment orbldepartment);
        void Delete(long id);
        void Save();
    }
}