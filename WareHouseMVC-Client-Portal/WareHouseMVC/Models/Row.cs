using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace WareHouseMVC.Models
{
    public class Row
    {
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long RowID { get; set; }

        [Required]
        [Display(Name = "Row name")]
        public string RowName { get; set; }

        [Display(Name = "Row code")]
        public string RowCode { get; set; }

         [DefaultValue(false)]
        public bool IsAssigned { get; set; }


        public virtual Column Column { get; set; }

        [Required]
        [Display(Name = "Column name")]
        public long ColumnID { get; set; }


        public virtual Height Height { get; set; }

        [Required]
        [Display(Name = "Height name")]
        public long HeightID { get; set; }



        public virtual Level Level { get; set; }

        [Required]
        [Display(Name = "Level name")]
        public long LevelID { get; set; }

        public virtual Rack Rack { get; set; }

        [Required]
        [Display(Name = "Rack name")]
        public long RackID { get; set; }


        public virtual Train Train { get; set; }

        [Required]
        [Display(Name = "Train name")]
        public long TrainID { get; set; }

        public virtual Zone Zone { get; set; }

        [Required]
        [Display(Name = "Zone name")]
        public long ZoneID { get; set; }


        public virtual Floor Floor { get; set; }

        [Required]
        [Display(Name = "Floor name")]
        public long FloorID { get; set; }

        public virtual Warehouse Warehouse { get; set; }

        [Required]
        [Display(Name = "Warehouse name")]
        public virtual long WarehouseID { get; set; }
    }
}