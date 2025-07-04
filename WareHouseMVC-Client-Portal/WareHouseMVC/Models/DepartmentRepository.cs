using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

using PagedList;
namespace WareHouseMVC.Models
{
    public class DepartmentRepository : IDepartmentRepository
    {
        WareHouseMVCContext context;

        public DepartmentRepository()
           : this(new WareHouseMVCContext())
        {

        }
        public DepartmentRepository(WareHouseMVCContext context)
        {

            this.context = context;
        }



        public IQueryable<Department> All
        {
            get { return context.Departments; }
        }

        public IQueryable<Department> AllIncluding(params Expression<Func<Department, object>>[] includeProperties)
        {
            IQueryable<Department> query = context.Departments;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query.OrderBy(o => o.DepartmentName);
        }

        public Department Find(long id)
        {
            return context.Departments.Find(id);
        }

        public void InsertOrUpdate(Department department)
        {
            if (department.DepartmentID == default(long))
            {
                // New entity
                context.Departments.Add(department);
            }
            else
            {
                // Existing entity
                context.Entry(department).State = EntityState.Modified;
            }
        }

        public void Delete(long id)
        {
            var department = context.Departments.Find(id);
            context.Departments.Remove(department);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Dispose()
        {
            context.Dispose();
        }



        public List<Department> FindByClientID(long ClientID)
        {
            return context.Departments.Where(d => d.ClientID == ClientID).OrderBy(o => o.DepartmentName).ToList();
        }

        public Department GetDepartmentByUser(Guid userId)
        {
            var department = (from d in context.Departments
                                  join c in context.ClientUsers on d.DepartmentID equals c.DepartmentID
                                  where c.UserId == userId
                                  select d).FirstOrDefault();

            return department;
        }

        public List<string> GetAllSubDepartments(long clientId, long deptId)
        {
            var subDepartments = context.ClientUsers
                                        .Where(u => u.ClientId == clientId && u.DepartmentID == deptId)
                                        .OrderBy(o => o.SubDepartment)
                                        .Select(s => s.SubDepartment)
                                        .Distinct()
                                        .ToList();
            return subDepartments;
        }

        public string GetSubDepartmentName(Guid userId)
        {
            var subDepartmentName = (from c in context.ClientUsers
                                  where c.UserId == userId
                                  select c.SubDepartment).FirstOrDefault();

            return subDepartmentName;
        }


        public Department GetByClientIDandDeptName(long _tempClientID, string department)
        {
            return context.Departments.Where(d => d.ClientID == _tempClientID && d.DepartmentName == department).FirstOrDefault();
        }

        public Department GetByDeptName(string departmentName)
        {
            return context.Departments.Where(d => d.DepartmentName == departmentName).FirstOrDefault();
        }

        public Department GetByClientIDandDepId(long clientId, long deptId)
        {
            return context.Departments.Where(d => d.ClientID == clientId && d.DepartmentID == deptId).FirstOrDefault();
        }

        public PagedList.IPagedList<Department> GetAllPagedList(int pageNumber, int pageSize)
        {
            return context.Departments.OrderByDescending(a => a.DepartmentID).ToPagedList(pageNumber, pageSize);
        }

        public List<Department> GetAllPagedList()
        {
            return context.Departments.ToList();
        }

        public PagedList.IPagedList<Department> GetAllPagedListByClientId(long clientId, int pageNumber, int pageSize)
        {
            return context.Departments.Where(c => c.ClientID == clientId).OrderByDescending(a => a.DepartmentID).ToPagedList(pageNumber, pageSize);
        }
    }

    public interface IDepartmentRepository : IDisposable
    {
        IQueryable<Department> All { get; }
        IQueryable<Department> AllIncluding(params Expression<Func<Department, object>>[] includeProperties);
        Department Find(long id);
        void InsertOrUpdate(Department department);
        void Delete(long id);
        void Save();
    }
}