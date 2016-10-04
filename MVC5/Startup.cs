using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using MVC5.Models;
using Owin;

[assembly: OwinStartupAttribute(typeof(MVC5.Startup))]

namespace MVC5
{
  public partial class Startup
  {
    public void Configuration(IAppBuilder app)
    {
      ConfigureAuth(app);
      CreateRolesandUsers();
    }

    private void CreateRolesandUsers()
    {
      ApplicationDbContext context = new ApplicationDbContext();

      var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
      var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));


      // In Startup iam creating first Admin Role and creating a default Admin User 
      if (!roleManager.RoleExists("Admin"))
      {

        // first we create Admin rool
        var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
        role.Name = "Admin";
        roleManager.Create(role);

        //Here we create a Admin super user who will maintain the website				

        var user = new ApplicationUser();
        user.UserName = "admin@gmail.com";
        user.Email = "admin@gmail.com";
        user.IsEnabled = true;
        string userPWD = "wdy2fv";

        var chkUser = UserManager.Create(user, userPWD);

        //Add default User to Role Admin
        if (chkUser.Succeeded)
        {
          var result1 = UserManager.AddToRole(user.Id, "Admin");

        }
      }
    }
  }
}