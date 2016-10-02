namespace MVC5.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDevice : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DeviceModels",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        DeviceId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        Type = c.String(maxLength: 100),
                        Factory = c.String(maxLength: 100),
                        Model = c.String(maxLength: 100),
                        Amount = c.Int(nullable: false),
                        Description = c.String(maxLength: 10),
                        MaintainFrequency = c.Double(nullable: false),
                        ReplaceFee = c.Int(nullable: false),
                        MaxUsedYear = c.Int(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DeviceModels", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.DeviceModels", new[] { "ApplicationUser_Id" });
            DropTable("dbo.DeviceModels");
        }
    }
}
