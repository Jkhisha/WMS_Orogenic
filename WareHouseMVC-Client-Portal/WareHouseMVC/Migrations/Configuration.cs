namespace WareHouseMVC.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using WareHouseMVC.Models;
    using System.Web.Security;
    using System.Collections.Generic;
    using System.Web;

    public sealed class Configuration : DbMigrationsConfiguration<WareHouseMVC.Models.WareHouseMVCContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            AutomaticMigrationDataLossAllowed = false;
        }

        protected override void Seed(WareHouseMVC.Models.WareHouseMVCContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //


            //WebSecurity.Register("Demo", "123456", "demo@demo.com", true, "Demo", "Demo");
            //Roles.CreateRole("Admin");
            //Roles.CreateRole("Transmittal IN Manager");
            //Roles.CreateRole("Transmittal OUT Manager");
            //Roles.CreateRole("Empty Box Requisition Manager");
            //Roles.CreateRole("Billing Manager");
            //Roles.CreateRole("Destruction Process Manager");
            //Roles.CreateRole("Warehouse Manager");
            //Roles.CreateRole("Reports Manager");


            //Roles.AddUserToRole("Demo", "Admin");

            //List<Country> countries = new List<Country>
            //{
            //    new Country {CountryName ="Canada", IsRemoved=false},
            //     new Country {CountryName ="USA", IsRemoved=false},
            //      new Country {CountryName ="UK", IsRemoved=false},

            //       new Country {CountryName ="UAE", IsRemoved=false},
            //};
            //countries.ForEach(s => context.Countries.AddOrUpdate(s));


            //context.SaveChanges();

            //List<Test> tests = new List<Test>
            //{
            //    new Test { TestName ="Badhon", Amount=500, IsRemoved=false},
            //     new Test {TestName ="Najib", Amount=600, IsRemoved=false},
            //      new Test {TestName ="Rubel", Amount=700, IsRemoved=false},

            //       new Test {TestName ="Koushik", Amount=800, IsRemoved=false},
            //};
            //tests.ForEach(s => context.Tests.AddOrUpdate(s));


            //context.SaveChanges();



            //List<TransmittalINStatus> status = new List<TransmittalINStatus>
            //{
            //    //new TransmittalINStatus {  },
            //     new WareHouseMVC.Models.TransmittalINStatus {StatusName="Pending"},
            //      new WareHouseMVC.Models.TransmittalINStatus {StatusName="WareHouse Assigned For Boxes"},
            //      new WareHouseMVC.Models.TransmittalINStatus {StatusName="Received Boxes at WareHouse"},
            //      new WareHouseMVC.Models.TransmittalINStatus {StatusName="Barcode Assigned at Warehouse"},
            //      new WareHouseMVC.Models.TransmittalINStatus {StatusName="Location Assigned at WareHouse"},
            //      new WareHouseMVC.Models.TransmittalINStatus {StatusName="Location Varified at WareHouse"},
                 
            //};
            //status.ForEach(s => context.TransmittalINStatus.AddOrUpdate(s));


            //context.SaveChanges();

            //List<TransmittalOUTStatus> outStatus = new List<TransmittalOUTStatus>
            //{
            //     new WareHouseMVC.Models.TransmittalOUTStatus {StatusName="Pending"},
            //      new WareHouseMVC.Models.TransmittalOUTStatus {StatusName="TrOUT Generated"},
            //      new WareHouseMVC.Models.TransmittalOUTStatus {StatusName="Approved at WareHouse"},
            //      new WareHouseMVC.Models.TransmittalOUTStatus {StatusName="Gate Pass Generated"},
            //      new WareHouseMVC.Models.TransmittalOUTStatus {StatusName="Box Out from WareHouse"},
            //      new WareHouseMVC.Models.TransmittalOUTStatus {StatusName="Box Not Found"},
                 
            //};
            //outStatus.ForEach(s => context.TransmittalOUTStatus.AddOrUpdate(s));


            //context.SaveChanges();



            //List<Warehouse> wareHouse = new List<Warehouse>
            //{
            //    new Warehouse { WarehouseName ="WareHouise 1", WarehouseID=1, WarehouseCode="WH1"},
            //     new Warehouse {WarehouseName ="WareHouise 2", WarehouseID=2, WarehouseCode="WH2"},
            //      new Warehouse {WarehouseName ="WareHouise 3", WarehouseID=3, WarehouseCode="WH3"},
            //       new Warehouse {WarehouseName ="WareHouise 4", WarehouseID=4, WarehouseCode="WH4"},
            //        new Warehouse { WarehouseName ="WareHouise 5", WarehouseID=5, WarehouseCode="WH5"},
            //     new Warehouse {WarehouseName ="WareHouise 6", WarehouseID=6, WarehouseCode="WH6"},
            //      new Warehouse {WarehouseName ="WareHouise 7", WarehouseID=7, WarehouseCode="WH7"},
            //       new Warehouse {WarehouseName ="WareHouise 8", WarehouseID=8, WarehouseCode="WH8"},
            //        new Warehouse { WarehouseName ="WareHouise 9", WarehouseID=9, WarehouseCode="WH9"},
            //     new Warehouse {WarehouseName ="WareHouise 10", WarehouseID=10, WarehouseCode="WH10"},
            //      new Warehouse {WarehouseName ="WareHouise 11", WarehouseID=11, WarehouseCode="WH11"},
            //       new Warehouse {WarehouseName ="WareHouise 12", WarehouseID=12, WarehouseCode="WH12"},
            //};
            //wareHouse.ForEach(s => context.Warehouses.AddOrUpdate(s));


            //context.SaveChanges();



           

        }
    }
}
