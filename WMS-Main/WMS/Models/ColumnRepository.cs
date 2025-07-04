using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WareHouseMVC.Models
{ 
    public class ColumnRepository : IColumnRepository
    {
        WareHouseMVCContext context;

         public  ColumnRepository()
            : this(new WareHouseMVCContext())
        {
            
        }
         public ColumnRepository(WareHouseMVCContext context)
        {
            
            this.context = context;
        }



        public IQueryable<Column> All
        {
            get { return context.Columns; }
        }

        public IQueryable<Column> AllIncluding(params Expression<Func<Column, object>>[] includeProperties)
        {
            IQueryable<Column> query = context.Columns;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public IQueryable<Column> allList(params Expression<Func<Column, object>>[] includeProperties)
        {
            IQueryable<Column> query = context.Columns;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty).Take(10);
            }
            return query;
        }

        public Column Find(long id)
        {
            return context.Columns.Find(id);
        }

        public void InsertOrUpdate(Column column)
        {
            if (column.ColumnID == default(long)) {
                // New entity
                context.Columns.Add(column);
            } else {
                // Existing entity
                context.Entry(column).State = EntityState.Modified;
            }
        }

        public void Delete(long id)
        {
            var column = context.Columns.Find(id);
            context.Columns.Remove(column);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Dispose() 
        {
            context.Dispose();
        }

     

        public List<Column> FindByHeightID(long HeightID)
        {
            return context.Columns.Where(z => z.HeightID == HeightID).ToList();
        }

        public  Column GetByHIDandColumnName(long _tempHeightID, string columnName)
        {
            return context.Columns.Where(c => c.HeightID == _tempHeightID && c.ColumnName == columnName).FirstOrDefault();
        }

        public List<Column> allList(long? HeightID)
        {
            return context.Columns.Where(z => z.HeightID == HeightID).ToList();
        }
    }

    public interface IColumnRepository : IDisposable
    {
        IQueryable<Column> All { get; }
        IQueryable<Column> AllIncluding(params Expression<Func<Column, object>>[] includeProperties);
        Column Find(long id);
        void InsertOrUpdate(Column column);
        void Delete(long id);
        void Save();
    }
}