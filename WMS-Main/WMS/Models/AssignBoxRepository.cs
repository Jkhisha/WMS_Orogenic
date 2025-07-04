using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using PagedList;
using System.Data.SqlClient;
using System.Data;

namespace WareHouseMVC.Models
{
    public class AssignBoxRepository : IAssignBoxRepository
    {
        WareHouseMVCContext context;
        public IPagedList<AssignBox> pagedListItem { get; set; }
        public AssignBoxRepository()
            : this(new WareHouseMVCContext())
        {

        }
        public AssignBoxRepository(WareHouseMVCContext context)
        {

            this.context = context;
        }



        public IQueryable<AssignBox> All
        {
            get { return context.AssignBoxes; }
        }

        public IQueryable<AssignBox> AllIncluding(params Expression<Func<AssignBox, object>>[] includeProperties)
        {
            IQueryable<AssignBox> query = context.AssignBoxes;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public AssignBox Find(long id)
        {
            return context.AssignBoxes.Find(id);
        }

        public void InsertOrUpdate(AssignBox assignbox)
        {
            if (assignbox.AssignBoxId == default(long))
            {
                // New entity
                context.AssignBoxes.Add(assignbox);
            }
            else
            {
                // Existing entity
                context.Entry(assignbox).State = EntityState.Modified;
            }
        }

        public void Delete(long id)
        {
            var assignbox = context.AssignBoxes.Find(id);
            context.AssignBoxes.Remove(assignbox);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public List<AssignBox> GetByTrInId(long trID)
        {
            return context.AssignBoxes.Where(a => a.TransmittalINId == trID).ToList();
        }



        public AssignBox GetByTrInIdWithStatus(int trID)
        {
            return context.AssignBoxes.Where(a => a.TransmittalINId == trID).FirstOrDefault();
        }

        public AssignBox GetByItemIdandTrINId(long itemID, int trID)
        {
            return context.AssignBoxes.Where(b => b.ItemId == itemID && b.TransmittalINId == trID).FirstOrDefault();
        }

        public List<AssignBox> GetByTrIDForChallan(long trID)
        {
            return context.AssignBoxes.Where(s => s.TransmittalINId == trID && s.TransmittalINStatusId == 2).ToList();
        }

        public List<AssignBox> GetNotAssignedBoxList(int trID)
        {
            return context.AssignBoxes.Where(s => s.TransmittalINId == trID && s.TransmittalINStatusId == 1).ToList();
        }

        //jewel & Anik 23092020
        public List<long> GetCustomTINIDSS(long wID, int count)
        {
            return context.AssignBoxes.Where(a => a.WarehouseID == wID).OrderByDescending(d => d.AssignDate).Select(x => x.TransmittalINId).Distinct().Take(count).ToList();
        }

        public List<long> GetCustomTINID(long wID, int count, string trInNo)
        {
            List<long> resultList = new List<long>();


            var connection = (System.Data.SqlClient.SqlConnection)context.Database.Connection;

            if (connection != null && connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }

            if (string.IsNullOrEmpty(trInNo))
            {
                var dt = new DataTable();
                var distinctDt = new DataTable();

                string sql = string.Format("select TransmittalINId,AssignDate from AssignBoxes where WarehouseID = {0} order by AssignDate desc", wID);
                using (var com = new System.Data.SqlClient.SqlDataAdapter(sql, connection))
                {
                    com.Fill(dt);
                }
                if (dt != null && dt.Rows.Count != 0)
                {
                    DataView view = new DataView(dt);
                    distinctDt = view.ToTable(true, "TransmittalINId");
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
                string sql = string.Format("SELECT TransmittalINId FROM TransmittalINs WHERE TransmittalNo = '{0}'", trInNo.Trim());
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

        public List<long> GetByByWidandStatus(long wID)
        {
            return context.AssignBoxes.Where(a => a.WarehouseID == wID).OrderByDescending(d => d.AssignDate).Select(x => x.TransmittalINId).Distinct().ToList();
        }

        public AssignBox GetStatusByTrID(long trID, long Wid, long trStatusId)
        {
            return context.AssignBoxes.Where(a => a.WarehouseID == Wid && a.TransmittalINId == trID && a.TransmittalINStatusId == trStatusId).FirstOrDefault();
        }

        public List<AssignBox> GetByTrInIdAndWid(long TrID, long wid)
        {
            return context.AssignBoxes.Where(a => a.TransmittalINId == TrID && a.WarehouseID == wid).ToList();
        }

        public List<AssignBox> GetByTrInIdAndWidandStatus(long TrID, long wID, long statusID)
        {
            return context.AssignBoxes.Where(a => a.TransmittalINId == TrID && a.WarehouseID == wID && a.TransmittalINStatusId == statusID).ToList();
        }

        public List<AssignBox> GetByTrInIdAndWidandStatusAll(long TrID, long wID, long statusID)
        {
            return context.AssignBoxes.Where(a => a.TransmittalINId == TrID && a.WarehouseID == wID && a.TransmittalINStatusId >= statusID).ToList();
        }

        public List<AssignBox> GetByByWidandTrStatus(long wID, long status)
        {
            return context.AssignBoxes.Where(a => a.WarehouseID == wID && a.TransmittalINStatusId >= status).ToList();
        }

        public AssignBox GetByItemIdandStatus(long _itemId, long _statusId)
        {
            return context.AssignBoxes.Where(a => a.ItemId == _itemId && a.TransmittalINStatusId >= _statusId && a.CurrentStatus == 1).FirstOrDefault();
        }

        public List<AssignBox> GetByByTrStatus(long status)
        {
            return context.AssignBoxes.Where(a => a.TransmittalINStatusId == status).ToList();
        }

        internal string GetByTrID(long trID)
        {
            throw new NotImplementedException();
        }

        public AssignBox GetByItemIDandCurrentStatus(long _itemID)
        {
            List<AssignBox> allBoxes = new List<AssignBox>();
            allBoxes = context.AssignBoxes.Where(a => a.ItemId == _itemID).GroupBy(item => item.ItemId).Select(grp => grp.OrderByDescending(item => item.AssignDate).FirstOrDefault()).ToList();

            //            .GroupBy(item => item.ItemId).Select(grp => grp.OrderByDescending(item => item.AssignDate).FirstOrDefault())

            if (allBoxes.Count > 0)
            {
                if (allBoxes[0].CurrentStatus == 0)
                {
                    return null;
                }
                else
                {
                    return allBoxes[0];
                }
            }

            return null;
            // return context.AssignBoxes.Where(a => a.ItemId == _itemID && a.CurrentStatus == 1).FirstOrDefault();
        }

        public AssignBox GetByTrIDandItemIDwithCurrentStatus(long trID, long id)
        {
            return context.AssignBoxes.Where(a => a.ItemId == id && a.TransmittalINId == trID && a.CurrentStatus == 1).FirstOrDefault();
        }

        public AssignBox GetByItemID(long itemID)
        {
            return context.AssignBoxes.Where(a => a.ItemId == itemID).OrderByDescending(s => s.AssignDate).FirstOrDefault();
        }



        public AssignBox GetByItemIDandMonthWithStatus(long itemId, DateTime month, int statusId)
        {
            month = month.AddMonths(1).AddDays(-1);

            return context.AssignBoxes.Where(a => a.ItemId == itemId && a.TransmittalINStatusId >= statusId && a.AssignDate <= month).FirstOrDefault();
        }

        public List<AssignBox> GetByTransmittalINID(long trID)
        {
            return context.AssignBoxes.Where(a => a.TransmittalINId == trID).ToList();
        }

        public List<AssignBox> GetAll()
        {
            return context.AssignBoxes.ToList();
        }

        public List<AssignBox> GetByTrInIdandWID(long trID, long _wID)
        {
            return context.AssignBoxes.Where(a => a.TransmittalINId == trID && a.WarehouseID == _wID).ToList();
        }



        public List<AssignBox> GetByByTrStatusAndClientId(long status, long _clientID)
        {
            return context.AssignBoxes.Where(a => a.TransmittalINStatusId == status && a.Item.ClientID == _clientID).ToList();
        }

        public List<AssignBox> GetByClientIdandDeptID(long deptId, long clientId)
        {
            return context.AssignBoxes.Where(a => a.Item.ClientID == clientId && a.Item.DepartmentID == deptId).ToList();
        }

        public AssignBox GetByClientIdandDeptID(long deptId, long clientId, string boxName, string boxNo, string year)
        {
            return context.AssignBoxes.Where(a => a.Item.ClientID == clientId && a.Item.DepartmentID == deptId && a.BoxName == boxName && a.BoxNo == boxNo && a.Year == year).FirstOrDefault();
        }

        public List<AssignBox> GetByClientIdAndDepartmentId(long _clientID, long _deptID)
        {
            return context.AssignBoxes.Where(a => a.Item.ClientID == _clientID && a.Item.DepartmentID == _deptID).ToList();
        }



        public AssignBox GetByClientIdandDeptIDwithoutYear(long _deptID, long _clientID, string boxName, string boxNo)
        {
            return context.AssignBoxes.Where(a => a.Item.ClientID == _clientID && a.Item.DepartmentID == _deptID && a.BoxName == boxName && a.BoxNo == boxNo).FirstOrDefault();
        }


        public AssignBox GetByClientIdandDeptIDAndStatus(long deptId, long clientId, string boxName, string boxNo, string year, long statusId, long ItemNo)
        {
            return context.AssignBoxes.Where(a => a.Item.ClientID == clientId && a.Item.DepartmentID == deptId && a.BoxName == boxName && a.BoxNo == boxNo && a.Year == year && a.TransmittalINStatusId >= statusId && a.CurrentStatus == 1 && a.ItemId == ItemNo).FirstOrDefault();
        }

        public AssignBox GetByClientIdandDeptIDwithoutYearAndStatus(long _deptID, long _clientID, string boxName, string boxNo, long statusId, long ItemNo)
        {
            return context.AssignBoxes.Where(a => a.Item.ClientID == _clientID && a.Item.DepartmentID == _deptID && a.BoxName == boxName && a.BoxNo == boxNo && a.TransmittalINStatusId >= statusId && a.CurrentStatus == 1 && a.ItemId == ItemNo).FirstOrDefault();
        }

        public List<AssignBox> GetByClientId(long _clientID)
        {
            return context.AssignBoxes.Where(a => a.Item.ClientID == _clientID).ToList();
        }

        public List<AssignBox> GetByByTrStatusAndClientIdFile(long status, long _clientID)
        {
            return context.AssignBoxes.Where(a => a.TransmittalINStatusId == status && a.Item.ClientID == _clientID && a.TransmittalIN.Type == "File").ToList();
        }

        public List<AssignBox> GetByByTrStatusFile(long status)
        {
            return context.AssignBoxes.Where(a => a.TransmittalINStatusId == status && a.TransmittalIN.Type == "File").ToList();
        }

        public List<AssignBox> GetAllFile()
        {
            return context.AssignBoxes.Where(a => a.TransmittalIN.Type == "File").ToList();
        }
        public List<AssignBox> GetCustomeFileForPageLoad(int count)
        {
            return context.AssignBoxes.Where(a => a.TransmittalIN.Type == "File").Take(count).ToList();
        }


        public List<AssignBox> GetByClientIdAndDepartmentIdFile(long _clientID, long _deptID)
        {
            return context.AssignBoxes.Where(a => a.Item.ClientID == _clientID && a.Item.DepartmentID == _deptID && a.TransmittalIN.Type == "File").ToList();
        }
        //Write by Jewel&Anik 23SEP2020
        public List<AssignBox> GetByClientIdAndFileNo(long _clientID, long deptID, string fileNo)
        {
            if (deptID != 0)
                return context.AssignBoxes.Where(a => a.Item.ClientID == _clientID && a.Item.DepartmentID == deptID && a.Item.FileNumber == fileNo && a.TransmittalIN.Type == "File").ToList();
            else
                return context.AssignBoxes.Where(a => a.Item.ClientID == _clientID && a.Item.FileNumber == fileNo && a.TransmittalIN.Type == "File").ToList();
        }

        public AssignBox GetByClientIdandDeptIDwithoutYearAndStatusFile(long _deptId, long _clientId, string _boxNo, string _bookNo, string _fileNumber, string _refNumber, string _ringNumber, string _accountNumber, long _statusId)
        {
            return context.AssignBoxes.Where(a => a.Item.ClientID == _clientId && a.Item.DepartmentID == _deptId && a.BoxNo == _boxNo && a.BoxNameFile == _bookNo && a.FileNumber == _fileNumber && a.ReferrenceNo == _refNumber && a.RingNo == _ringNumber && a.AccountNo == _accountNumber && a.TransmittalINStatusId >= _statusId && a.CurrentStatus == 1).FirstOrDefault();
        }

        public AssignBox GetByClientIdandDeptIDAndStatusFile(long _deptId, long _clientId, string _boxNo, string _bookNo, string _fileNumber, string _refNumber, string _ringNumber, string _accountNumber, string _year, long _statusId)
        {
            return context.AssignBoxes.Where(a => a.Item.ClientID == _clientId && a.Item.DepartmentID == _deptId && a.BoxNo == _boxNo && a.BoxNameFile == _bookNo && a.FileNumber == _fileNumber && a.ReferrenceNo == _refNumber && a.RingNo == _ringNumber && a.AccountNo == _accountNumber && a.Year == _year && a.TransmittalINStatusId >= _statusId && a.CurrentStatus == 1).FirstOrDefault();
        }

        public List<AssignBox> GetAllBox()
        {
            return context.AssignBoxes.Where(a => a.Item.Unit == "Box").Take(10).ToList();
        }

        public AssignBox GetByClientIdandDeptIDwithoutYearFile(long DepartmentID, long ClientID, string boxNo, string bookNo, string fileNo, string refNo, string ringNo, string accNo)
        {
            return context.AssignBoxes.Where(a => a.Item.ClientID == ClientID && a.Item.DepartmentID == DepartmentID && a.BoxNo == boxNo && a.BoxNameFile == bookNo && a.FileNumber == fileNo && a.ReferrenceNo == refNo && a.RingNo == ringNo && a.AccountNo == accNo && a.Item.Unit == "File").FirstOrDefault();
        }

        public AssignBox GetByClientIdandDeptIDFile(long DepartmentID, long ClientID, string boxNo, string bookNo, string fileNo, string refNo, string ringNo, string accNo, string year)
        {

            return context.AssignBoxes.Where(a => a.Item.ClientID == ClientID && a.Item.DepartmentID == DepartmentID && a.BoxNo == boxNo && a.BoxNameFile == bookNo && a.FileNumber == fileNo && a.ReferrenceNo == refNo && a.RingNo == ringNo && a.AccountNo == accNo && a.Year == year && a.Item.Unit == "File").FirstOrDefault();
        }

        public List<AssignBox> GetMyFilteredData(int TrID, long wID, long statusID, int assignBoxId)
        {
            return context.AssignBoxes.Where(a => a.TransmittalINId == TrID && a.WarehouseID == wID && a.TransmittalINStatusId >= statusID && a.AssignBoxId > assignBoxId).ToList();
        }

        public List<AssignBox> GetByWareHouseId(int p)
        {
            return context.AssignBoxes.Where(a => a.WarehouseID == p).ToList();
        }

        public PagedList.IPagedList<AssignBox> GetByByTrStatusAndClientIdPagedList(long status, long _clientID, int pageNumber, int pageSize)
        {
            return context.AssignBoxes.Where(a => a.TransmittalINStatusId == status && a.Item.ClientID == _clientID && a.CurrentStatus != 0).OrderBy(o => o.AssignDate).ToPagedList(pageNumber, pageSize);
        }

        public IPagedList<AssignBox> GetByByTrStatusPList(long status, int pageNumber, int pageSize)
        {
            return context.AssignBoxes.Where(a => a.TransmittalINStatusId == status && a.CurrentStatus != 0).OrderByDescending(o => o.AssignBoxId).ToPagedList(pageNumber, pageSize);

        }

        public IPagedList<AssignBox> GetByByTrStatusAndClientIdFilePList(long status, long _clientID, int pageNumber, int pageSize)
        {
            return context.AssignBoxes.Where(a => a.TransmittalINStatusId == status && a.Item.ClientID == _clientID && a.TransmittalIN.Type == "File").OrderBy(o => o.AssignDate).ToPagedList(pageNumber, pageSize);
        }

        public IPagedList<AssignBox> GetByByTrStatusFilePList(long status, int pageNumber, int pageSize)
        {
            return context.AssignBoxes.Where(a => a.TransmittalINStatusId == status && a.TransmittalIN.Type == "File").OrderBy(o => o.AssignDate).ToPagedList(pageNumber, pageSize);
        }

        public IPagedList<AssignBox> GetAllPlist(int pageNumber, int pageSize)
        {
            return context.AssignBoxes.OrderBy(o => o.AssignDate).ToPagedList(pageNumber, pageSize);
        }

        public IPagedList<AssignBox> GetByClientIdAndDepartmentIdPList(long _clientID, long _deptID, DateTime startDate, DateTime endDate, string _boxName, string _boxNo, int pageNumber, int pageSize)
        {
            IPagedList<AssignBox> retnrPageList;
            retnrPageList = context.AssignBoxes.OrderBy(o => o.AssignDate).ToPagedList(pageNumber, pageSize);// .Select(o=>o.BoxLocation .ToList();

            if ((!string.IsNullOrEmpty(_boxName) && !string.IsNullOrEmpty(_boxNo)) || (!string.IsNullOrEmpty(_boxName) && string.IsNullOrEmpty(_boxNo)) || (string.IsNullOrEmpty(_boxName) && !string.IsNullOrEmpty(_boxNo)))
            {
                if (!string.IsNullOrEmpty(_boxName) && !string.IsNullOrEmpty(_boxNo))
                {

                    retnrPageList = context.AssignBoxes.Where(a => a.Item.ClientID == _clientID && a.BoxName.Contains(_boxName.Trim()) && a.BoxNo.Contains(_boxNo.Trim())).GroupBy(item => item.ItemId).Select(grp => grp.OrderByDescending(item => item.AssignDate).FirstOrDefault()).OrderBy(o => o.AssignDate).ToPagedList(pageNumber, pageSize);// .Select(o=>o.BoxLocation .ToList();
                }

                else if (!string.IsNullOrEmpty(_boxName) && string.IsNullOrEmpty(_boxNo))
                {

                    retnrPageList = context.AssignBoxes.Where(a => a.Item.ClientID == _clientID && a.BoxName.Contains(_boxName.Trim())).GroupBy(item => item.ItemId).Select(grp => grp.OrderByDescending(item => item.AssignDate).FirstOrDefault()).OrderBy(o => o.AssignDate).ToPagedList(pageNumber, pageSize);// .Select(o=>o.BoxLocation .ToList();
                }

                else if (string.IsNullOrEmpty(_boxName) && !string.IsNullOrEmpty(_boxNo))
                {
                    //List<AssignBox> list = context.AssignBoxes.Where(s => s.BoxNo.Contains(_boxNo)).ToList();

                    retnrPageList = context.AssignBoxes.Where(a => a.Item.ClientID == _clientID && a.BoxNo.Contains(_boxNo.Trim())).GroupBy(item => item.ItemId).Select(grp => grp.OrderByDescending(item => item.AssignDate).FirstOrDefault()).OrderBy(o => o.AssignDate).ToPagedList(pageNumber, pageSize);// .Select(o=>o.BoxLocation .ToList();
                }
            }

            else
            {

                if (startDate != DateTime.MinValue && endDate != DateTime.MinValue && _clientID != 0 && _deptID != 0)
                {
                    retnrPageList = context.AssignBoxes.Where(a => a.Item.ClientID == _clientID && a.Item.DepartmentID == _deptID && a.AssignDate >= startDate && a.AssignDate <= endDate).GroupBy(item => item.ItemId).Select(grp => grp.OrderByDescending(item => item.AssignDate).FirstOrDefault()).OrderBy(o => o.AssignDate).ToPagedList(pageNumber, pageSize);// .Select(o=>o.BoxLocation .ToList();
                }

                else if (startDate != DateTime.MinValue && endDate != DateTime.MinValue && _clientID != 0 && _deptID == 0)
                {
                    retnrPageList = context.AssignBoxes.Where(a => a.Item.ClientID == _clientID && a.AssignDate >= startDate && a.AssignDate <= endDate).GroupBy(item => item.ItemId).Select(grp => grp.OrderByDescending(item => item.AssignDate).FirstOrDefault()).OrderBy(o => o.AssignDate).ToPagedList(pageNumber, pageSize);// .Select(o=>o.BoxLocation .ToList();
                }

                else if (startDate == DateTime.MinValue && endDate == DateTime.MinValue && _clientID != 0 && _deptID != 0)
                {
                    retnrPageList = context.AssignBoxes.Where(a => a.Item.ClientID == _clientID && a.Item.DepartmentID == _deptID).GroupBy(item => item.ItemId).Select(grp => grp.OrderByDescending(item => item.AssignDate).FirstOrDefault()).OrderBy(o => o.AssignDate).ToPagedList(pageNumber, pageSize);// .Select(o=>o.BoxLocation .ToList();
                }

                else if (startDate == DateTime.MinValue && endDate == DateTime.MinValue && _clientID != 0 && _deptID == 0)
                {
                    retnrPageList = context.AssignBoxes.Where(a => a.Item.ClientID == _clientID).GroupBy(item => item.ItemId).Select(grp => grp.OrderByDescending(item => item.AssignDate).FirstOrDefault()).OrderBy(o => o.AssignDate).ToPagedList(pageNumber, pageSize);// .Select(o=>o.BoxLocation .ToList();
                }


                else
                {
                    retnrPageList = context.AssignBoxes.GroupBy(item => item.ItemId).Select(grp => grp.OrderByDescending(item => item.AssignDate).FirstOrDefault()).OrderBy(o => o.AssignDate).ToPagedList(pageNumber, pageSize);// .Select(o=>o.BoxLocation .ToList();
                }
            }
            return retnrPageList;
        }

        public IPagedList<AssignBox> GetByByTrStatusAndClientIdPagedListFilter(long status, string txtBoxNo, string txtBoxNname, long _clientID, int pageNumber, int pageSize)
        {
            return context.AssignBoxes.Where(a => a.TransmittalINStatusId == status && a.Item.ClientID == _clientID && a.Item.ItemName == txtBoxNname && a.Item.BoxNo == txtBoxNo && a.CurrentStatus != 0).OrderBy(o => o.AssignDate).ToPagedList(pageNumber, pageSize);
        }

        public IPagedList<AssignBox> GetByByTrStatusPListFilter(long status, string txtBoxNo, string txtBoxNname, int pageNumber, int pageSize)
        {
            return context.AssignBoxes.Where(a => a.TransmittalINStatusId == status && a.Item.BoxNo == txtBoxNo && a.Item.ItemName == txtBoxNname && a.CurrentStatus != 0).OrderByDescending(o => o.AssignBoxId).ToPagedList(pageNumber, pageSize); ;
        }

        public IPagedList<AssignBox> GetByByTrStatusAndClientIdPagedListFilterOnlyName(long status, string txtBoxNname, long _clientID, int pageNumber, int pageSize)
        {
            return context.AssignBoxes.Where(a => a.TransmittalINStatusId == status && a.Item.ClientID == _clientID && a.Item.ItemName == txtBoxNname && a.CurrentStatus != 0).OrderBy(o => o.AssignDate).ToPagedList(pageNumber, pageSize);
        }

        public IPagedList<AssignBox> GetByByTrStatusPListFilterOnlyName(long status, string txtBoxNname, int pageNumber, int pageSize)
        {
            return context.AssignBoxes.Where(a => a.TransmittalINStatusId == status && a.Item.ItemName == txtBoxNname && a.CurrentStatus != 0).OrderByDescending(o => o.AssignBoxId).ToPagedList(pageNumber, pageSize); ;
        }

        public IPagedList<AssignBox> GetByByTrStatusAndClientIdPagedListFilterOnlyNo(long status, string txtBoxNo, long _clientID, int pageNumber, int pageSize)
        {
            return context.AssignBoxes.Where(a => a.TransmittalINStatusId == status && a.Item.ClientID == _clientID && a.Item.BoxNo == txtBoxNo && a.CurrentStatus != 0).OrderBy(o => o.AssignDate).ToPagedList(pageNumber, pageSize);
        }

        public IPagedList<AssignBox> GetByByTrStatusPListFilterOnlyNo(long status, string txtBoxNo, int pageNumber, int pageSize)
        {
            return context.AssignBoxes.Where(a => a.TransmittalINStatusId == status && a.Item.BoxNo == txtBoxNo && a.CurrentStatus != 0).OrderByDescending(o => o.AssignBoxId).ToPagedList(pageNumber, pageSize);
        }

        public AssignBox FindAssignedBoxBarcodeByBarcodeText(string barcodeText)
        {
            return context.AssignBoxes.Where(e => e.BarCodeText == barcodeText).FirstOrDefault();
        }
        public IPagedList<AssignBox> GetByCliendIDandDeptIdandProjectIdPagedList(long _clientId, long _deptId, long _projectId, int pageNumber, int pageSize)
        {
            List<AssignBox> items = new List<AssignBox>();
            pagedListItem = new PagedList<AssignBox>(items, pageNumber, pageSize);
            if (_projectId == -1)
            {
                pagedListItem = context.AssignBoxes.Where(i => i.Item.ClientID == _clientId && i.Item.Unit == "Box").GroupBy(item => item.ItemId).Select(grp => grp.OrderByDescending(item => item.AssignDate).FirstOrDefault()).OrderBy(o => o.BoxName).ToPagedList(pageNumber, pageSize);//&& i.DepartmentID == _deptId
            }
            else
            {
                pagedListItem = context.AssignBoxes.Where(i => i.Item.ClientID == _clientId && i.Item.ProjectId == _projectId && i.Item.Unit == "Box").GroupBy(item => item.ItemId).Select(grp => grp.OrderByDescending(item => item.AssignDate).FirstOrDefault()).OrderBy(o => o.BoxName).ToPagedList(pageNumber, pageSize);//&& i.DepartmentID == _deptId
            }
            return pagedListItem;
        }

        public IPagedList<AssignBox> GetByCliendIDandDeptIdPagedList(long _clientId, long _deptId, int pageNumber, int pageSize)
        {
            return context.AssignBoxes.Where(i => i.Item.ClientID == _clientId && i.Item.Unit == "Box").GroupBy(item => item.ItemId).Select(grp => grp.OrderByDescending(item => item.AssignDate).FirstOrDefault()).OrderBy(o => o.BoxName).ToPagedList(pageNumber, pageSize);//&& i.DepartmentID == _deptId
        }

        public IPagedList<AssignBox> GetByCliendIDandDeptIdandProjectIdFilePagedList(long _clientId, long _deptId, long _projectId, int pageNumber, int pageSize)
        {
            List<AssignBox> itemList = new List<AssignBox>();
            pagedListItem = new PagedList<AssignBox>(itemList, pageNumber, pageSize);
            if (_projectId == -1)
            {
                pagedListItem = context.AssignBoxes.Where(i => i.Item.ClientID == _clientId && i.Item.Unit == "File").OrderBy(o => o.BoxName).ToPagedList(pageNumber, pageSize);//.ToList();//&& i.DepartmentID == _deptId
            }
            else
            {
                pagedListItem = context.AssignBoxes.Where(i => i.Item.ClientID == _clientId && i.Item.ProjectId == _projectId && i.Item.Unit == "File").OrderBy(o => o.BoxName).ToPagedList(pageNumber, pageSize);//&& i.DepartmentID == _deptId
            }
            return pagedListItem;
        }

        public IPagedList<AssignBox> GetByCliendIDandDeptIdFilePagedList(long _clientId, long _deptId, int pageNumber, int pageSize)
        {
            return context.AssignBoxes.Where(i => i.Item.ClientID == _clientId && i.Item.Unit == "File").OrderBy(o => o.BoxName).ToPagedList(pageNumber, pageSize);//&& i.DepartmentID == _deptId// && i.DepartmentID == _deptIdc
        }

        public IPagedList<AssignBox> GetByByWidandTrStatusPagedList(long _wID, long status, int pageNumber, int pageSize)
        {
            return context.AssignBoxes.Where(a => a.WarehouseID == _wID && a.TransmittalINStatusId >= status).OrderBy(o => o.AssignDate).ToPagedList(pageNumber, pageSize);//.ToList();
        }

        public IPagedList<AssignBox> GetByClientIdPagedList(long _clientID, int pageNumber, int pageSize)
        {
            return context.AssignBoxes.Where(a => a.Item.ClientID == _clientID).OrderBy(o => o.AssignDate).ToPagedList(pageNumber, pageSize);
        }

        public IPagedList<AssignBox> GetByClientIdPagedListwithBoxNoandName(long _clientID, string txtBoxNo, string txtBoxNname, int pageNumber, int pageSize)
        {
            return context.AssignBoxes.Where(a => a.Item.ClientID == _clientID && a.BoxName == txtBoxNname && a.BoxNo == txtBoxNo).OrderBy(o => o.AssignDate).ToPagedList(pageNumber, pageSize);
        }

        public IPagedList<AssignBox> GetByClientIdPagedListwithBoxName(long _clientID, string txtBoxNname, int pageNumber, int pageSize)
        {
            return context.AssignBoxes.Where(a => a.Item.ClientID == _clientID && a.BoxName == txtBoxNname).OrderBy(o => o.AssignDate).ToPagedList(pageNumber, pageSize);
        }

        public IPagedList<AssignBox> GetByClientIdPagedListwithBoxNo(long _clientID, string txtBoxNo, int pageNumber, int pageSize)
        {
            return context.AssignBoxes.Where(a => a.Item.ClientID == _clientID && a.BoxNo == txtBoxNo).OrderBy(o => o.AssignDate).ToPagedList(pageNumber, pageSize);
        }

        public string GetByTrInIdSTatus(long trID)
        {
            try
            {
                return context.AssignBoxes.Where(a => a.TransmittalINId == trID).OrderBy(o => o.TransmittalINStatusId).FirstOrDefault().TransmittalINStatus.StatusName;
            }
            catch
            {
                return "N/A";
            }
        }

        public int GetByTrInIdCount(long trID)
        {
            return context.AssignBoxes.Where(a => a.TransmittalINId == trID).Count();
        }

        public IPagedList<AssignBox> GetByTrInIdandWIDPagedList(long trID, long _wID, int pageSize, int pageNumber)
        {
            return context.AssignBoxes.Where(a => a.TransmittalINId == trID && a.WarehouseID == _wID).OrderBy(o => o.BoxName).ToPagedList(pageNumber, pageSize);
        }

        public IPagedList<AssignBox> GetByTrIdPlist(long trID, int pageNumber, int pageSize)
        {
            return context.AssignBoxes.Where(a => a.TransmittalINId == trID).OrderBy(o => o.BoxName).ToPagedList(pageNumber, pageSize);
        }

        public List<AssignBox> GetByItemId(long itemId)
        {
            return context.AssignBoxes.Where(i => i.ItemId == itemId).ToList();
        }

        public AssignBox GetByWidandTrStatus(long AssignBoxId, long _wID, long status)
        {
            return context.AssignBoxes.Where(a => a.AssignBoxId == AssignBoxId && a.WarehouseID == _wID && a.TransmittalINStatusId >= status).FirstOrDefault();
        }

        public List<AssignBox> GetBytrStatusForBarcode(long status)
        {
            return context.AssignBoxes.Where(a => a.TransmittalINStatusId >= status).ToList();
        }

        public List<AssignBox> GetByTrIdandWidandTrSts(long TrID, long wID, long statusID)
        {
            return context.AssignBoxes.Where(a => a.TransmittalINId == TrID && a.WarehouseID == wID && a.TransmittalINStatusId >= statusID).ToList();
        }

        public IPagedList<AssignBox> GetByTrIdandWidandTrStsPagedList(long TrID, long wID, long statusID, int pageSize, int pageNumber)
        {
            return context.AssignBoxes.Where(a => a.TransmittalINId == TrID && a.WarehouseID == wID && a.TransmittalINStatusId >= statusID).OrderBy(o => o.BoxName).ToPagedList(pageNumber, pageSize);
        }

        public AssignBox GetByitemIDandTrIdandTrStatus(long itemId, long trId, long status)
        {
            return context.AssignBoxes.Where(a => a.ItemId == itemId && a.TransmittalINId == trId && a.TransmittalINStatusId >= status).FirstOrDefault();
        }

        public List<InventoryViewModel> GetInventoryListByClientIDandWareHouseID(long clientID, long warehouseID)
        {
            List<InventoryViewModel> reurnLst = new List<InventoryViewModel>();

            List<long> itemTbl = new List<long>();
            itemTbl = context.Items.Where(i => i.ClientID == clientID).Select(i => i.ItemId).ToList();

            List<long> assignBoxtbl = new List<long>();
            assignBoxtbl = context.AssignBoxes.Where(a => a.WarehouseID == warehouseID).Select(i => i.ItemId).ToList();

            List<long> filteredLst = new List<long>();
            filteredLst = itemTbl.Intersect(assignBoxtbl).ToList();

            List<Item> itemList = new List<Item>();
            //foreach (long item in filteredLst)
            //{
            //    Item _item = new Item();
            //    _item = context.Items.Where(i => i.ItemId == item).FirstOrDefault();
            //    itemList.Add(_item);

            //}

            itemList = context.Items.Where(e => filteredLst.Contains(e.ItemId)).ToList();

            List<long> deptTotal = itemList.Select(s => s.DepartmentID).Distinct().ToList();

            foreach (long item in deptTotal)
            {
                InventoryViewModel model = new InventoryViewModel();
                model.ClientID = clientID;
                model.ClientName = context.Clients.Where(c => c.ClientID == clientID).FirstOrDefault().ClientName;
                model.WareHouseName = context.Warehouses.Where(w => w.WarehouseID == warehouseID).FirstOrDefault().WarehouseName;
                model.WareHouseID = warehouseID;
                model.DeptName = context.Departments.Where(d => d.DepartmentID == item && d.ClientID == clientID).FirstOrDefault().DepartmentName;
                model.BoxTotal = itemList.Where(s => s.DepartmentID == item).Count();
                reurnLst.Add(model);

            }



            return reurnLst;

        }


        public List<InventoryViewModel> GetInventoryReport()
        {
            return null;
        }


        public ReportViewModelForInventory InventoryReportMethod(List<InventoryViewModel> finalList, string ClientName, string WareHouseName)
        {


            var itemList = new List<object>();
            long totalBox = 0;

            foreach (var item in finalList)
            {
                totalBox = totalBox + item.BoxTotal;
                itemList.Add(item);
            }
            //Assuming the person printing the report is me
            var UserPrinting = HttpContext.Current.User.Identity.Name;




            string clientName = ClientName;
            string warehouse = WareHouseName;

            var reportViewModel = new ReportViewModelForInventory()
            {
                FileName = "~/ReportsHolder/InventoryReport.rdlc",
                Name = "Inventory Report",
                ReportDate = DateTime.Now,
                ReportTitle = "Inventory Report",
                ClientName = clientName,
                WareHouseName = warehouse,
                TotalBoxCount = totalBox,

                ReportLanguage = "en-US",

                Format = ReportViewModelForInventory.ReportFormat.PDF,
                ViewAsAttachment = false,

            };
            reportViewModel.ReportDataSets.Add(new ReportViewModelForInventory.ReportDataSet() { DataSetData = itemList.ToList(), DatasetName = "DataSet1" });


            return reportViewModel;
        }

        public List<AssignBox> GetByClientMonthStatu(long clientID, DateTime month, int statusId)
        {

            month = month.AddMonths(1).AddDays(-1);

            return context.AssignBoxes.Where(a => a.Item.ClientID == clientID && a.TransmittalINStatusId >= statusId && a.AssignDate <= month).ToList();
        }

        public List<AssignBox> GetByClientIDandDateRange(long clientID, DateTime monthStart, DateTime monthEnd)
        {
            return context.AssignBoxes.Where(t => t.Item.ClientID == clientID && t.AssignDate >= monthStart && t.AssignDate <= monthEnd).ToList();
        }

        public List<AssignBox> GetByClientIDandDtIDanMmonthStartandMonthEnd(long clientID, long? deptID, DateTime monthStart, DateTime monthEnd)
        {

            List<AssignBox> list = new List<AssignBox>();

            if (deptID != -1)
            {
                list = context.AssignBoxes.Where(t => t.Item.ClientID == clientID && t.Item.DepartmentID == deptID.Value && t.AssignDate >= monthStart && t.AssignDate <= monthEnd).ToList();
            }
            else
            {
                list = context.AssignBoxes.Where(t => t.Item.ClientID == clientID && t.AssignDate >= monthStart && t.AssignDate <= monthEnd).ToList();
            }
            return list;
        }

        public List<AssignBox> GetListOfDuplicateByClientIdandDeptId(long clientId, long deptId)
        {
            List<AssignBox> list = new List<AssignBox>();

            list = context.AssignBoxes.Where(s => s.Item.ClientID == clientId && s.Item.DepartmentID == deptId).ToList();

            //var lst= context.AssignBoxes.Where(s => s.Item.ClientID == clientId && s.Item.DepartmentID == deptId).GroupBy(x => new { x.BoxNo, x.BoxName,x.Year }).ToList();
            //foreach (var item in lst)
            //{


            //}



            return list;

        }

        public List<AssignBox> GetListByBoxNameBoxNoYear(string name, string no, string year)
        {
            return context.AssignBoxes.Where(a => a.BoxName == name && a.BoxNo == no && a.Year == year).ToList();
        }

        public List<AssignBox> GetListByBoxNameBoxNo(string name, string no)
        {
            return context.AssignBoxes.Where(a => a.BoxName == name && a.BoxNo == no).ToList();
        }

        public AssignBox GetByItemIdAndStatus(long itemId, long status)
        {
            return context.AssignBoxes.Where(a => a.ItemId == itemId && a.TransmittalINStatusId >= status && a.CurrentStatus == 1).FirstOrDefault();
        }

        public List<AssignBox> CheckForBarcode(long _itemId, long _statusId)
        {
            return context.AssignBoxes.Where(a => a.ItemId == _itemId && a.TransmittalINStatusId >= _statusId && a.CurrentStatus == 1).GroupBy(item => item.ItemId).Select(grp => grp.OrderByDescending(item => item.AssignDate).FirstOrDefault()).ToList();
        }

        public List<AssignBox> CheckForBarcodeSingle(long itemId)
        {
            return context.AssignBoxes.Where(a => a.ItemId == itemId && a.CurrentStatus == 1).GroupBy(item => item.ItemId).Select(grp => grp.OrderByDescending(item => item.AssignDate).FirstOrDefault()).ToList();
        }

        internal List<AssignBox> GetListByTrId(long trId)
        {
            return context.AssignBoxes.Where(s => s.TransmittalINId == trId).ToList();
        }

        //newly modify by kalam 15/06/2021
        public IPagedList<AssignBox> GetByCliendIDandDeptIdandProjectIdPagedList2(long _clientId, long _deptId, long _projectId, string searchString, int pageNumber, int pageSize)
        {
            List<AssignBox> items = new List<AssignBox>();
            pagedListItem = new PagedList<AssignBox>(items, pageNumber, pageSize);

            if (!String.IsNullOrEmpty(searchString))
            {
                if (_projectId == -1)
                {
                    pagedListItem = context.AssignBoxes.Where(i => i.Item.ClientID == _clientId && i.Item.Unit == "Box").GroupBy(item => item.ItemId).Select(grp => grp.OrderByDescending(item => item.AssignDate).FirstOrDefault()).OrderBy(o => o.BoxName).Where(d => d.Item.Department.DepartmentName.StartsWith(searchString)).ToPagedList(pageNumber, pageSize);//&& i.DepartmentID == _deptId
                }
                else
                {
                    pagedListItem = context.AssignBoxes.Where(i => i.Item.ClientID == _clientId && i.Item.ProjectId == _projectId && i.Item.Unit == "Box").GroupBy(item => item.ItemId).Select(grp => grp.OrderByDescending(item => item.AssignDate).FirstOrDefault()).OrderBy(o => o.BoxName).Where(d => d.Item.Department.DepartmentName.StartsWith(searchString)).ToPagedList(pageNumber, pageSize);//&& i.DepartmentID == _deptId
                }
            }
            else
            {
                if (_projectId == -1)
                {
                    pagedListItem = context.AssignBoxes.Where(i => i.Item.ClientID == _clientId && i.Item.Unit == "Box").GroupBy(item => item.ItemId).Select(grp => grp.OrderByDescending(item => item.AssignDate).FirstOrDefault()).OrderBy(o => o.BoxName).ToPagedList(pageNumber, pageSize);//&& i.DepartmentID == _deptId
                }
                else
                {
                    pagedListItem = context.AssignBoxes.Where(i => i.Item.ClientID == _clientId && i.Item.ProjectId == _projectId && i.Item.Unit == "Box").GroupBy(item => item.ItemId).Select(grp => grp.OrderByDescending(item => item.AssignDate).FirstOrDefault()).OrderBy(o => o.BoxName).Where(d => d.Item.Department.DepartmentName.Contains(searchString)).ToPagedList(pageNumber, pageSize);//&& i.DepartmentID == _deptId
                }
            }

            return pagedListItem;
        }


        public IPagedList<AssignBox> GetByCliendIDandDeptIdPagedList2(long _clientId, long _deptId, string searchString, int pageNumber, int pageSize)
        {
            if (!String.IsNullOrEmpty(searchString))
            {
                return context.AssignBoxes.Where(i => i.Item.ClientID == _clientId && i.Item.Unit == "Box").GroupBy(item => item.ItemId).Select(grp => grp.OrderByDescending(item => item.AssignDate).FirstOrDefault()).OrderBy(o => o.BoxName).Where(d => d.Item.Department.DepartmentName.StartsWith(searchString)).ToPagedList(pageNumber, pageSize);//&& i.DepartmentID == _deptId
            }
            else
            {
                return context.AssignBoxes.Where(i => i.Item.ClientID == _clientId && i.Item.Unit == "Box").GroupBy(item => item.ItemId).Select(grp => grp.OrderByDescending(item => item.AssignDate).FirstOrDefault()).OrderBy(o => o.BoxName).ToPagedList(pageNumber, pageSize);//&& i.DepartmentID == _deptId
            }

        }

        // End 15/06/2021
    }
    public interface IAssignBoxRepository : IDisposable
    {
        IQueryable<AssignBox> All { get; }
        IQueryable<AssignBox> AllIncluding(params Expression<Func<AssignBox, object>>[] includeProperties);
        AssignBox Find(long id);
        void InsertOrUpdate(AssignBox assignbox);
        void Delete(long id);
        void Save();
    }
}