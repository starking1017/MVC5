namespace MVC5.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addrole : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUserRoles", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.AspNetUserRoles", "Role_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.AspNetUserRoles", "ApplicationUser_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.AspNetRoles", "Description", c => c.String());
            AddColumn("dbo.AspNetRoles", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.AspNetUserRoles", "Role_Id");
            CreateIndex("dbo.AspNetUserRoles", "ApplicationUser_Id");
            AddForeignKey("dbo.AspNetUserRoles", "Role_Id", "dbo.AspNetRoles", "Id");
            AddForeignKey("dbo.AspNetUserRoles", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "Role_Id", "dbo.AspNetRoles");
            DropIndex("dbo.AspNetUserRoles", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "Role_Id" });
            DropColumn("dbo.AspNetRoles", "Discriminator");
            DropColumn("dbo.AspNetRoles", "Description");
            DropColumn("dbo.AspNetUserRoles", "ApplicationUser_Id");
            DropColumn("dbo.AspNetUserRoles", "Role_Id");
            DropColumn("dbo.AspNetUserRoles", "Discriminator");
        }
    }
}
