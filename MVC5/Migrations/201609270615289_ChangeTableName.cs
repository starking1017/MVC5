namespace MVC5.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeTableName : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.DeviceModels", newName: "Devices");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.Devices", newName: "DeviceModels");
        }
    }
}
