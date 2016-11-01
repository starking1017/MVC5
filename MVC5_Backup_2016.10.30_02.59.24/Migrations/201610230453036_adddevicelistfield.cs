namespace MVC5.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adddevicelistfield : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Devices", "DeviceList", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Devices", "DeviceList");
        }
    }
}
