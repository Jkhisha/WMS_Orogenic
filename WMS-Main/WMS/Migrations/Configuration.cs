namespace WareHouseMVC.Migrations
{
    using System.Data.Entity.Migrations;

    public sealed class Configuration : DbMigrationsConfiguration<WareHouseMVC.Models.WareHouseMVCContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(WareHouseMVC.Models.WareHouseMVCContext context)
        {
            // Seed your database here
            // Example:
            // context.Users.AddOrUpdate(u => u.Email, new User { Email = "admin@example.com" });
        }
    }
}