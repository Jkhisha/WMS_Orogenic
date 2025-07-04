using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using WareHouseMVC.Models;

namespace WareHouseMVC.Models
{
    public class BoxLocation
    {
        public long BoxLocationId { get; set; }

        [DefaultValue(false)]
        public bool IsLeatest { get; set; }

        public string CurrentStatus { get; set; }
        public virtual List<AssignBox> AssignBox { get; set; }
        public long AssignBoxId { get; set; }

        public virtual Warehouse Warehouse { get; set; }
        public long WareHouseID { get; set; }

        public virtual Floor Floor { get; set; }
        public long FloorID { get; set; }

        public virtual Zone Zone { get; set; }
        public long ZoneID { get; set; }

        public virtual Train Train { get; set; }
        public long? TrainID { get; set; }

        public virtual Rack Rack { get; set; }
        public long? RackID { get; set; }

        public virtual Level Level { get; set; }
        public long? LevelID { get; set; }

        public virtual Height Height { get; set; }
        public long? HeightID { get; set; }

        public virtual Column Column { get; set; }
        public long? ColumnID { get; set; }

        public virtual Row Row { get; set; }
        public long? RowID { get; set; }

        public virtual Pallet Pallet { get; set; }
        public long? PalletId { get; set; }

        public bool  IsPallet { get; set; }




    }

    public class ItemDTO
    {
        public long ItemId { get; set; }
        public string BoxNo { get; set; }
        public string ItemName { get; set; }
       // public DateTime? BoxDestruction { get; set; }
        public string Description { get; set; }
        
        // string Legalhold { get; set; }
        // public string DepartmentName {get ; set;}
        public long DepartmentId { get; set; }
        public string SubDeptName { get; set; }
    }

    public class AssignBoxesDTO
    {
        public long ItemId { get; set; }
        public long AssignBoxId { get; set; }
        public string BoxNo { get; set; }
        public string BoxName { get; set; }
        public string Year { get; set; }
        public DateTime? DestructionPeriod { get; set; }
        public string Legalhold { get; set; }
        public DateTime AssignDate { get; set; }
        public string DepartmentName { get; set; }
        public string ClientName { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        public string SubDeptName { get; set; }
    }

    public class DepartmentDTO
    {
        public long DepartmentId { get; set; }
        public string DepartmentName { get; set; }
    }

    public class ClientsDTO
    {
        public long ClientId { get; set; }
        public string ClientName { get; set; }
    }

    public class TransmittalINDTO
    {
        public string TransmittalNo { get; set; }
        public DateTime TransmittalDate { get; set; }
        public string TransmittalStatus { get; set; }
        public List<ItemDTO> Items { get; set; }
    }
}

   
