using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Security;
using WareHouseMVC.Models;
using WareHouseMVC.Migrations;


public class DataContextInitializer : MigrateDatabaseToLatestVersion<WareHouseMVCContext, Configuration>
{
    //protected void Seed(WareHouseMVCContext context)
    //{
    //    WebSecurity.Register("Demo", "123456", "demo@demo.com", true, "Demo", "Demo");
    //    Roles.CreateRole("Admin");
    //    Roles.CreateRole("Transmittal IN Manager");
    //    Roles.CreateRole("Transmittal OUT Manager");
    //    Roles.CreateRole("Empty Box Requisition Manager");
    //    Roles.CreateRole("Billing Manager");
    //    Roles.CreateRole("Destruction Process Manager");
    //    Roles.CreateRole("Warehouse Manager");
    //    Roles.CreateRole("Reports Manager");


    //    Roles.AddUserToRole("Demo", "Admin");

    //    List<Country> countries = new List<Country>
    //        {
    //            new Country {CountryName ="Canada", IsRemoved=false},
    //             new Country {CountryName ="USA", IsRemoved=false},
    //              new Country {CountryName ="UK", IsRemoved=false},

    //               new Country {CountryName ="UAE", IsRemoved=false},
    //        };
    //    countries.ForEach(s => context.Countries.Add(s));


    //    context.SaveChanges();

    //    List<Test> tests = new List<Test>
    //        {
    //            new Test { TestName ="Badhon", Amount=500, IsRemoved=false},
    //             new Test {TestName ="Najib", Amount=600, IsRemoved=false},
    //              new Test {TestName ="Rubel", Amount=700, IsRemoved=false},

    //               new Test {TestName ="Koushik", Amount=800, IsRemoved=false},
    //        };
    //    tests.ForEach(s => context.Tests.Add(s));


    //    context.SaveChanges();


    //    List<Warehouse> wareHouse = new List<Warehouse>
    //        {
    //            new Warehouse { WarehouseName ="WareHouise 1", WarehouseID=1, WarehouseCode="WH1"},
    //             new Warehouse {WarehouseName ="WareHouise 2", WarehouseID=2, WarehouseCode="WH2"},
    //              new Warehouse {WarehouseName ="WareHouise 3", WarehouseID=3, WarehouseCode="WH3"},
    //               new Warehouse {WarehouseName ="WareHouise 4", WarehouseID=4, WarehouseCode="WH4"},
    //                new Warehouse { WarehouseName ="WareHouise 5", WarehouseID=5, WarehouseCode="WH5"},
    //             new Warehouse {WarehouseName ="WareHouise 6", WarehouseID=6, WarehouseCode="WH6"},
    //              new Warehouse {WarehouseName ="WareHouise 7", WarehouseID=7, WarehouseCode="WH7"},
    //               new Warehouse {WarehouseName ="WareHouise 8", WarehouseID=8, WarehouseCode="WH8"},
    //                new Warehouse { WarehouseName ="WareHouise 9", WarehouseID=9, WarehouseCode="WH9"},
    //             new Warehouse {WarehouseName ="WareHouise 10", WarehouseID=10, WarehouseCode="WH10"},
    //              new Warehouse {WarehouseName ="WareHouise 11", WarehouseID=11, WarehouseCode="WH11"},
    //               new Warehouse {WarehouseName ="WareHouise 12", WarehouseID=12, WarehouseCode="WH12"},
    //        };
    //    wareHouse.ForEach(s => context.Warehouses.Add(s));


    //    context.SaveChanges();


    //}
}