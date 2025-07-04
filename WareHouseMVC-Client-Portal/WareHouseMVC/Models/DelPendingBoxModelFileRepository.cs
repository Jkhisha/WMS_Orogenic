using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WareHouseMVC.Models
{ 
    public class DelPendingBoxModelFileRepository : IDelPendingBoxModelFileRepository
    {
        WareHouseMVCContext context;

         public  DelPendingBoxModelFileRepository()
            : this(new WareHouseMVCContext())
        {
            
        }
         public DelPendingBoxModelFileRepository(WareHouseMVCContext context)
        {
            
            this.context = context;
        }



        public IQueryable<DelPendingBoxModelFile> All
        {
            get { return context.DelPendingBoxModelFiles; }
        }

        public IQueryable<DelPendingBoxModelFile> AllIncluding(params Expression<Func<DelPendingBoxModelFile, object>>[] includeProperties)
        {
            IQueryable<DelPendingBoxModelFile> query = context.DelPendingBoxModelFiles;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public DelPendingBoxModelFile Find(long id)
        {
            return context.DelPendingBoxModelFiles.Find(id);
        }

        public void InsertOrUpdate(DelPendingBoxModelFile delpendingboxmodelfile)
        {
            if (delpendingboxmodelfile.DelPendingBoxModelFileId == default(long)) {
                // New entity
                context.DelPendingBoxModelFiles.Add(delpendingboxmodelfile);
            } else {
                // Existing entity
                context.Entry(delpendingboxmodelfile).State = EntityState.Modified;
            }
        }

        public void Delete(long id)
        {
            var delpendingboxmodelfile = context.DelPendingBoxModelFiles.Find(id);
            context.DelPendingBoxModelFiles.Remove(delpendingboxmodelfile);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Dispose() 
        {
            context.Dispose();
        }

        public  List<DelPendingBoxModelFile> GetAllDeliveryPendingList()
        {
            return context.DelPendingBoxModelFiles.ToList();
        }
    }

    public interface IDelPendingBoxModelFileRepository : IDisposable
    {
        IQueryable<DelPendingBoxModelFile> All { get; }
        IQueryable<DelPendingBoxModelFile> AllIncluding(params Expression<Func<DelPendingBoxModelFile, object>>[] includeProperties);
        DelPendingBoxModelFile Find(long id);
        void InsertOrUpdate(DelPendingBoxModelFile delpendingboxmodelfile);
        void Delete(long id);
        void Save();
    }
}