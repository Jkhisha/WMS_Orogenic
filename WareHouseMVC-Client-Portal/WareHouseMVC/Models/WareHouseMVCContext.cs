using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration.Conventions;
using WareHouseMVC.Models;

namespace WareHouseMVC.Models
{
    public class WareHouseMVCContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, add the following
        // code to the Application_Start method in your Global.asax file.
        // Note: this will destroy and re-create your database with every model change.
        // 
        // System.Data.Entity.Database.SetInitializer(new System.Data.Entity.DropCreateDatabaseIfModelChanges<WareHouseMVC.Models.WareHouseMVCContext>());

        public DbSet<WareHouseMVC.Models.Country> Countries { get; set; }

        public DbSet<WareHouseMVC.Models.Test> Tests { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<WareHouseMVC.Models.Warehouse> Warehouses { get; set; }

        public DbSet<WareHouseMVC.Models.Floor> Floors { get; set; }

        public DbSet<WareHouseMVC.Models.Zone> Zones { get; set; }

        public DbSet<Vehicle> Vehicles { get; set; }

        public DbSet<TransportVendor> TransportVendors { get; set; }

        public DbSet<TransmittalOUT> TransmittalOUTs { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            //modelBuilder.Entity<Floor>()
            //   .HasRequired(f => f.Warehouse)
            //   .WithRequiredDependent()
            //   .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Zone>()
            //   .HasRequired(f => f.Floor)
            //   .WithRequiredDependent()
            //   .WillCascadeOnDelete(false);

        }

        public DbSet<WareHouseMVC.Models.Train> Trains { get; set; }

        public DbSet<WareHouseMVC.Models.Rack> Racks { get; set; }

        public DbSet<WareHouseMVC.Models.Level> Levels { get; set; }

        public DbSet<WareHouseMVC.Models.Height> Heights { get; set; }

        public DbSet<WareHouseMVC.Models.Column> Columns { get; set; }

        public DbSet<WareHouseMVC.Models.Row> Rows { get; set; }

        public DbSet<WareHouseMVC.Models.Client> Clients { get; set; }

        public DbSet<WareHouseMVC.Models.Department> Departments { get; set; }

        public DbSet<WareHouseMVC.Models.ContactPerson> ContactPersons { get; set; }

        public DbSet<WareHouseMVC.Models.ORBLDepartment> ORBLDepartments { get; set; }

        public DbSet<WareHouseMVC.Models.SupportStuff> SupportStuffs { get; set; }

        public DbSet<WareHouseMVC.Models.Driver> Drivers { get; set; }

        public DbSet<WareHouseMVC.Models.TransmittalIN> TransmittalINs { get; set; }

        public DbSet<WareHouseMVC.Models.Item> Items { get; set; }

        public DbSet<WareHouseMVC.Models.HandOverBy> HandOverBies { get; set; }

        public DbSet<WareHouseMVC.Models.ReceivedBy> ReceivedBies { get; set; }

        public DbSet<WareHouseMVC.Models.AssignBox> AssignBoxes { get; set; }
        public DbSet<WareHouseMVC.Models.AddBoxDOC> AddBoxDOC { get; set; }

        public DbSet<WareHouseMVC.Models.ChallanIN> ChallanINs { get; set; }

        public DbSet<WareHouseMVC.Models.Project> Projects { get; set; }

        public DbSet<WareHouseMVC.Models.TransmittalINStatus> TransmittalINStatus { get; set; }

        public DbSet<WareHouseMVC.Models.BoxLocation> BoxLocations { get; set; }

        public DbSet<WareHouseMVC.Models.BarCode> BarCodes { get; set; }

        public DbSet<WareHouseMVC.Models.TransmittalOUTStatus> TransmittalOUTStatus { get; set; }

        public DbSet<WareHouseMVC.Models.AssignBoxTrOUT> AssignBoxTrOUTs { get; set; }

        public DbSet<WareHouseMVC.Models.EmptyBox> EmptyBoxes { get; set; }
        public DbSet<WareHouseMVC.Models.EmptyBoxBarcode> EmptyBoxeBarcodes { get; set; }
        public DbSet<WareHouseMVC.Models.HostInformation> HostInformations { get; set; }

        public DbSet<WareHouseMVC.Models.ORBLOperator> ORBLOperators { get; set; }

        public DbSet<WareHouseMVC.Models.Pallet> Pallets { get; set; }

        public DbSet<WareHouseMVC.Models.AutoZoneSuggention> AutoZoneSuggentions { get; set; }

        public DbSet<WareHouseMVC.Models.ChangeLocation> ChangeLocations { get; set; }

        public DbSet<WareHouseMVC.Models.DelPendingBoxModel> DelPendingBoxModels { get; set; }

        public DbSet<WareHouseMVC.Models.LoginTrail> LoginTrails { get; set; }

        public DbSet<WareHouseMVC.Models.TransmittalINAuditTrail> TransmittalINAuditTrails { get; set; }

        public DbSet<WareHouseMVC.Models.TransmittalOUTAuditTrail> TransmittalOUTAuditTrails { get; set; }

        public DbSet<WareHouseMVC.Models.DelPendingBoxModelFile> DelPendingBoxModelFiles { get; set; }

        public DbSet<WareHouseMVC.Models.BarcodeMapping> BarcodeMappings { get; set; }

        public DbSet<WareHouseMVC.Models.ErrorLog> ErrorLogs { get; set; }

        public DbSet<WareHouseMVC.Models.Region> Regions { get; set; }

        public DbSet<WareHouseMVC.Models.ClientBillingInfo> ClientBillingInfoes { get; set; }

        public DbSet<WareHouseMVC.Models.InvoiceViewModel> InvoiceViewModels { get; set; }
        public DbSet<WareHouseMVC.Models.AuditTrailCP> AuditTrailCPs { get; set; }
        public DbSet<WareHouseMVC.Models.DestructionHistory> DestructionHistories { get; set; }
        public DbSet<WareHouseMVC.Models.ClientUser> ClientUsers { get; set; }
        public DbSet<WareHouseMVC.Models.BoxDestruction> BoxDestructions { get; set; }
        public DbSet<WareHouseMVC.Models.ClientBarCodeMap> ClientBarCodeMaps { get; set; }





    }
}