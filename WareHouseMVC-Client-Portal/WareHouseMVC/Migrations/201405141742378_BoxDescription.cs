namespace WareHouseMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BoxDescription : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Items", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Items", "Description");
        }
    }
}
