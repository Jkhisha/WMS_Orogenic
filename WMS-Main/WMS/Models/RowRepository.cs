using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WareHouseMVC.Models
{ 
    public class RowRepository : IRowRepository
    {
        WareHouseMVCContext context;

         public  RowRepository()
            : this(new WareHouseMVCContext())
        {
            
        }
         public RowRepository(WareHouseMVCContext context)
        {
            
            this.context = context;
        }



        public IQueryable<Row> All
        {
            get { return context.Rows; }
        }

        public IQueryable<Row> AllIncluding(params Expression<Func<Row, object>>[] includeProperties)
        {
            IQueryable<Row> query = context.Rows;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public IQueryable<Row> allList(params Expression<Func<Row, object>>[] includeProperties)
        {
            IQueryable<Row> query = context.Rows;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty).Take(10);
            }
            return query;
        }

        public Row Find(long id)
        {
            return context.Rows.Find(id);
        }

        public void InsertOrUpdate(Row row)
        {
            if (row.RowID == default(long)) {
                // New entity
                context.Rows.Add(row);
            } else {
                // Existing entity
                context.Entry(row).State = EntityState.Modified;
            }
        }

        public void Delete(long id)
        {
            var row = context.Rows.Find(id);
            context.Rows.Remove(row);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Dispose() 
        {
            context.Dispose();
        }


        public  List<Row> GetByEmptyStatusAndCount(bool status,long _wID, int count,long ClientId)
        {
            List<Row> finalList = new List<Row>();


            #region Check Zone Assigned or not

            List<AutoZoneSuggention> _autoZoneList = context.AutoZoneSuggentions.Where(a => a.ClientID == ClientId && a.Zone.WarehouseID == _wID).ToList();

            long tempZoneID=0;

            if (_autoZoneList.Count > 0)
            {

                foreach (var item in _autoZoneList)
                {
                    tempZoneID=item.ZoneID;
                    List<Row> rowlist = new List<Row>();
                    rowlist = context.Rows.Where(r => r.IsAssigned == status && r.ZoneID == item.ZoneID).Take(count).ToList();

                    if (rowlist.Count == count)
                    {
                        foreach (var item2 in rowlist)
                        {
                            finalList.Add(item2);

                        }
                        break;
                    }
                    else
                    {
                        foreach (var item2 in rowlist)
                        {
                            finalList.Add(item2);

                        }


                        List<Row> newrow = context.Rows.Where(r => r.IsAssigned == status && r.WarehouseID == _wID && r.ZoneID != tempZoneID).Take(count).ToList();
                        for (int i = 0; i < rowlist.Count; i++)
                        {
                            finalList.Add(newrow[i]);
                        }
                    }

                }

                


            }
            else
            {
                if (tempZoneID != 0)
                {
                    finalList = context.Rows.Where(r => r.IsAssigned == status && r.WarehouseID == _wID && r.ZoneID != tempZoneID).Take(count).ToList();
                }
                else
                {
                    finalList = context.Rows.Where(r => r.IsAssigned == status && r.WarehouseID == _wID).Take(count).ToList();
                }


                
            }


            #endregion


            //return context.Rows.Where(r => r.IsAssigned == status && r.WarehouseID==_wID).Take(count).ToList();

            return finalList;
        }

        public  Row GetByCIDandRowName(long _tempColumnID, string rowName)
        {
            return context.Rows.Where(r => r.ColumnID == _tempColumnID && r.RowName == rowName).FirstOrDefault();
        }

      

        public string GetAllEmptySpace(long wID)
        {
            return context.Rows.Where(r => r.WarehouseID == wID && r.IsAssigned == false).Count().ToString();//.FirstOrDefault();
        }


        public List<Row> FindByColumnID(long ColumnID)
        {
            return context.Rows.Where(r => r.ColumnID == ColumnID && r.IsAssigned == false).ToList();
        }

        public  List<Row> GetByStatusandWID(long wID)
        {
            return context.Rows.Where(r => r.WarehouseID == wID && r.IsAssigned == true).ToList();
        }

        public int GetByStatusandWIDCount(long wID)
        {
            return context.Rows.Where(r => r.WarehouseID == wID && r.IsAssigned == true).Count();
        }

        public List<Row> allList(long? ColumnID)
        {
            return context.Rows.Where(r => r.ColumnID == ColumnID).ToList();
        }

        public  long GetFreeCapacity(long _wID)
        {
            return context.Rows.Where(r => r.WarehouseID == _wID && r.IsAssigned == false).Count();
        }
    }

    public interface IRowRepository : IDisposable
    {
        IQueryable<Row> All { get; }
        IQueryable<Row> AllIncluding(params Expression<Func<Row, object>>[] includeProperties);
        Row Find(long id);
        void InsertOrUpdate(Row row);
        void Delete(long id);
        void Save();
    }
}