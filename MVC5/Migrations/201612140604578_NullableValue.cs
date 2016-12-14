namespace MVC5.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NullableValue : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Devices", "AvgChangeYear", c => c.Double());
            AlterColumn("dbo.Devices", "OptChangeYear", c => c.Double());
            AlterColumn("dbo.Devices", "ScrapCost", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Devices", "ScrapCost", c => c.Double(nullable: false));
            AlterColumn("dbo.Devices", "OptChangeYear", c => c.Double(nullable: false));
            AlterColumn("dbo.Devices", "AvgChangeYear", c => c.Double(nullable: false));
        }
    }
}
