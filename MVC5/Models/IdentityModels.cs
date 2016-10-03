using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MVC5.Models
{
  // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
  public class ApplicationUser : IdentityUser
  {
    public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
    {
      // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
      var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
      // Add custom user claims here
      return userIdentity;
    }
    public bool IsEnabled { get; set; }
    public ICollection<ApplicationUserRole> UserRoles { get; set; }
    public virtual ICollection<Device> Devices { get; set; }
  }

  public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
  {
    public DbSet<ApplicationRole> Roles { get; set; }
    public DbSet<Device> Devices { get; set; }

    public ApplicationDbContext()
        : base("DefaultConnection", throwIfV1Schema: false)
    {
    }

    public static ApplicationDbContext Create()
    {
      return new ApplicationDbContext();
    }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
      if (modelBuilder == null)
      {
        throw new ArgumentNullException("ModelBuilder is NULL");
      }

      base.OnModelCreating(modelBuilder);

      //Defining the keys and relations
      modelBuilder.Entity<ApplicationUser>().ToTable("AspNetUsers");
      modelBuilder.Entity<ApplicationRole>().HasKey<string>(r => r.Id).ToTable("AspNetRoles");
      modelBuilder.Entity<ApplicationUser>().HasMany<ApplicationUserRole>((ApplicationUser u) => u.UserRoles);
      modelBuilder.Entity<ApplicationUserRole>().HasKey(r => new { UserId = r.UserId, RoleId = r.RoleId }).ToTable("AspNetUserRoles");
    }
  }
}