using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WareHouseMVC.Models
{ 
    public class ErrorLogRepository : IErrorLogRepository
    {
        WareHouseMVCContext context;

         public  ErrorLogRepository()
            : this(new WareHouseMVCContext())
        {
            
        }
         public ErrorLogRepository(WareHouseMVCContext context)
        {
            
            this.context = context;
        }



        public IQueryable<ErrorLog> All
        {
            get { return context.ErrorLogs; }
        }

        public IQueryable<ErrorLog> AllIncluding(params Expression<Func<ErrorLog, object>>[] includeProperties)
        {
            IQueryable<ErrorLog> query = context.ErrorLogs;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public ErrorLog Find(long id)
        {
            return context.ErrorLogs.Find(id);
        }

        public void InsertOrUpdate(ErrorLog errorlog)
        {
            if (errorlog.ErrorLogId == default(long)) {
                // New entity
                context.ErrorLogs.Add(errorlog);
            } else {
                // Existing entity
                context.Entry(errorlog).State = EntityState.Modified;
            }
        }

        public void Delete(long id)
        {
            var errorlog = context.ErrorLogs.Find(id);
            context.ErrorLogs.Remove(errorlog);
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

    public interface IErrorLogRepository : IDisposable
    {
        IQueryable<ErrorLog> All { get; }
        IQueryable<ErrorLog> AllIncluding(params Expression<Func<ErrorLog, object>>[] includeProperties);
        ErrorLog Find(long id);
        void InsertOrUpdate(ErrorLog errorlog);
        void Delete(long id);
        void Save();
    }
}