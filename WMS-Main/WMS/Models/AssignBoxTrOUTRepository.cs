using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using PagedList;
using System.Data;

namespace WareHouseMVC.Models
{ 
    public class AssignBoxTrOUTRepository : IAssignBoxTrOUTRepository
    {
        WareHouseMVCContext context;

         public  AssignBoxTrOUTRepository()
            : this(new WareHouseMVCContext())
        {
            
        }
         public AssignBoxTrOUTRepository(WareHouseMVCContext context)
        {
            
            this.context = context;
        }



        public IQueryable<AssignBoxTrOUT> All
        {
            get { return context.AssignBoxTrOUTs; }
        }

        public IQueryable<AssignBoxTrOUT> AllIncluding(params Expression<Func<AssignBoxTrOUT, object>>[] includeProperties)
        {
            IQueryable<AssignBoxTrOUT> query = context.AssignBoxTrOUTs;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public AssignBoxTrOUT Find(long id)
        {
            return context.AssignBoxTrOUTs.Find(id);
        }

        public void InsertOrUpdate(AssignBoxTrOUT assignboxtrout)
        {
            if (assignboxtrout.AssignBoxTrOUTId == default(long)) {
                // New entity
                context.AssignBoxTrOUTs.Add(assignboxtrout);
            } else {
                // Existing entity
                context.Entry(assignboxtrout).State = EntityState.Modified;
            }
        }

        public void Delete(long id)
        {
            var assignboxtrout = context.AssignBoxTrOUTs.Find(id);
            context.AssignBoxTrOUTs.Remove(assignboxtrout);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Dispose() 
        {
            context.Dispose();
        }

        public  AssignBoxTrOUT GetByTrIDandItemId(long trID, long itemId)
        {
            return context.AssignBoxTrOUTs.Where(a => a.TransmittalOUTId == trID && a.ItemId == itemId).FirstOrDefault();
        }

        public  List<long> GetByByWidandStatus(long _wID)
        {
            return context.AssignBoxTrOUTs.Where(a => a.WarehouseID == _wID).OrderByDescending(d=>d.AssignDate).Select(x => x.TransmittalOUTId).Distinct().ToList();
        }

        //jewel & anik 05102020
        public List<long> GetCustomTOUTID(long wID, int count, string trOutNo)
        {
            List<long> resultList = new List<long>();


            var connection = (System.Data.SqlClient.SqlConnection)context.Database.Connection;

            if (connection != null && connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }

            if (string.IsNullOrEmpty(trOutNo))
            {
                var dt = new DataTable();
                var distinctDt = new DataTable();

                string sql = string.Format("select TransmittalOUTId,AssignDate from AssignBoxTrOUTs where WarehouseID = {0} order by AssignDate desc", wID);
                using (var com = new System.Data.SqlClient.SqlDataAdapter(sql, connection))
                {
                    com.Fill(dt);
                }
                if (dt != null && dt.Rows.Count != 0)
                {
                    DataView view = new DataView(dt);
                    distinctDt = view.ToTable(true, "TransmittalOUTId");
                    if (distinctDt != null && distinctDt.Rows.Count != 0)
                    {
                        int distCount = distinctDt.Rows.Count;
                        if (distCount > count)
                            distCount = count;

                        for (int i = 0; i < distCount; i++)
                        {
                            resultList.Add(Convert.ToInt64(distinctDt.Rows[i][0]));

                        }
                    }
                }

            }
            else
            {
                var dt = new DataTable();
                string sql = string.Format("SELECT TransmittalOUTId FROM TransmittalOUTs WHERE TransmittalNo = '{0}'", trOutNo.Trim());
                using (var com = new System.Data.SqlClient.SqlDataAdapter(sql, connection))
                {
                    com.Fill(dt);
                }
                if (dt != null && dt.Rows.Count != 0)
                {
                    resultList.Add(Convert.ToInt64(dt.Rows[0][0]));
                }
            }
            if (connection != null && connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
            return resultList;
        }


        public List<long> GetByByWidandStatus(long _wID, int Count)
        {
            return context.AssignBoxTrOUTs.Where(a => a.WarehouseID == _wID).OrderByDescending(d => d.AssignDate).Select(x => x.TransmittalOUTId).Distinct().Take(Count).ToList();
        }

        public List<AssignBoxTrOUT> GetByTrInId(long trID)
        {
            return context.AssignBoxTrOUTs.Where(a => a.TransmittalOUTId == trID).ToList();
        }

        public  List<AssignBoxTrOUT> GetByTrInIdAndStatus(long trID, long trOUTStatusID)
        {
            return context.AssignBoxTrOUTs.Where(a => a.TransmittalOUTId == trID && a.TransmittalOUTStatusId == trOUTStatusID).ToList();
        }

        public  AssignBoxTrOUT GetByItemId(long itemId)
        {
            return context.AssignBoxTrOUTs.Where(a => a.ItemId == itemId).FirstOrDefault();
        }

        public List<AssignBoxTrOUT> GetByTrInIdandWID(long trID, long _wID)
        {
            return context.AssignBoxTrOUTs.Where(a => a.TransmittalOUTId == trID && a.WarehouseID==_wID).ToList();
        }



        public  string GetLatestByItemId(long itemID)
        {
            string returnDate = string.Empty;
            AssignBoxTrOUT _assignTrOut = new AssignBoxTrOUT();
            _assignTrOut = context.AssignBoxTrOUTs.Where(a => a.ItemId == itemID).OrderByDescending(s=>s.AssignDate).FirstOrDefault();

            if (_assignTrOut != null)
                returnDate = _assignTrOut.AssignDate.ToShortDateString();

            return returnDate;
        }

        public  int GetByTrInIdCount(long trID)
        {
            return context.AssignBoxTrOUTs.Where(a => a.TransmittalOUTId == trID).Count();
        }

        public  string GetByTrInIdStatus(long trID)
        {
            try
            {
                return context.AssignBoxTrOUTs.Where(a => a.TransmittalOUTId == trID).OrderBy(o => o.TransmittalOUTStatusId).FirstOrDefault().TransmittalOUTStatus.StatusName;
            }
            catch
            {
                return "N/A";
            }
        }

        public  IPagedList<AssignBoxTrOUT> GetByTrInIdandWIDPagedList(long trID, long _wID, int pageNumber, int pageSize)
        {
            return context.AssignBoxTrOUTs.Where(a => a.TransmittalOUTId == trID && a.WarehouseID == _wID).OrderBy(o => o.BoxNo).ToPagedList(pageNumber, pageSize);
        }

        public  IPagedList<AssignBoxTrOUT> GetByTrInIdPagedList(long trID, int pageNumber, int pageSize)
        {
            return context.AssignBoxTrOUTs.Where(a => a.TransmittalOUTId == trID ).OrderBy(o => o.BoxNo).ToPagedList(pageNumber, pageSize);
        }

        public  List<AssignBoxTrOUT> GetListByItemId(long itemId)
        {
            return context.AssignBoxTrOUTs.Where(i => i.ItemId == itemId).ToList();
        }

        public List<AssignBoxTrOUT> GetByClientIDandDateRange(long clientID, DateTime monthStart, DateTime monthEnd)
        {
            return context.AssignBoxTrOUTs.Where(t => t.Item.ClientID == clientID && t.AssignDate >= monthStart && t.AssignDate <= monthEnd).ToList();
        }

        public List<AssignBoxTrOUT> GetByClientIDandDtIDanMmonthStartandMonthEnd(long clientID, long? deptID, DateTime monthStart, DateTime monthEnd)
        {

            List<AssignBoxTrOUT> list = new List<AssignBoxTrOUT>();

            if (deptID!=-1)
            {
                list = context.AssignBoxTrOUTs.Where(t => t.Item.ClientID == clientID && t.Item.DepartmentID == deptID.Value && t.AssignDate >= monthStart && t.AssignDate <= monthEnd).ToList();
            }
            else
            {
                list = context.AssignBoxTrOUTs.Where(t => t.Item.ClientID == clientID && t.AssignDate >= monthStart && t.AssignDate <= monthEnd).ToList();
            }
            return list;
        }
    }

    public interface IAssignBoxTrOUTRepository : IDisposable
    {
        IQueryable<AssignBoxTrOUT> All { get; }
        IQueryable<AssignBoxTrOUT> AllIncluding(params Expression<Func<AssignBoxTrOUT, object>>[] includeProperties);
        AssignBoxTrOUT Find(long id);
        void InsertOrUpdate(AssignBoxTrOUT assignboxtrout);
        void Delete(long id);
        void Save();
    }
}