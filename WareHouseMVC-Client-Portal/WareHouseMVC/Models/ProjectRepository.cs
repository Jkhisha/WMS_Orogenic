using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WareHouseMVC.Models
{ 
    public class ProjectRepository : IProjectRepository
    {
        WareHouseMVCContext context;

         public  ProjectRepository()
            : this(new WareHouseMVCContext())
        {
            
        }
         public ProjectRepository(WareHouseMVCContext context)
        {
            
            this.context = context;
        }



        public IQueryable<Project> All
        {
            get { return context.Projects; }
        }

        public IQueryable<Project> AllIncluding(params Expression<Func<Project, object>>[] includeProperties)
        {
            IQueryable<Project> query = context.Projects;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public Project Find(long id)
        {
            return context.Projects.Find(id);
        }

        public void InsertOrUpdate(Project project)
        {
            if (project.ProjectId == default(long)) {
                // New entity
                context.Projects.Add(project);
            } else {
                // Existing entity
                context.Entry(project).State = EntityState.Modified;
            }
        }

        public void Delete(long id)
        {
            var project = context.Projects.Find(id);
            context.Projects.Remove(project);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Dispose() 
        {
            context.Dispose();
        }
      
        public List<Project> FindByDepartmentID(long DepartmentID)
        {
            return context.Projects.Where(p => p.DepartmentID == DepartmentID).ToList();
        }

        public  Project GetByCIDandDeptIDandProjectName(long _tempClientID, long _tempDepartmentID, string project)
        {
            return context.Projects.Where(p => p.ClientID == _tempClientID && p.DepartmentID == _tempDepartmentID && p.ProjectName == project).FirstOrDefault();
        }

        public List<Project> GetByClientId(long clientId)
        {
            return context.Projects.Where(p => p.ClientID == clientId).ToList();
        }
    }

    public interface IProjectRepository : IDisposable
    {
        IQueryable<Project> All { get; }
        IQueryable<Project> AllIncluding(params Expression<Func<Project, object>>[] includeProperties);
        Project Find(long id);
        void InsertOrUpdate(Project project);
        void Delete(long id);
        void Save();
    }
}