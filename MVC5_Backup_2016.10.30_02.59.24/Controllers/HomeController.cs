using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MVC5.Models;

namespace MVC5.Controllers
{
  public class HomeController : Controller
  {
    ApplicationDbContext context = new ApplicationDbContext();

    [Authorize]
    public ActionResult Index()
    {
      ViewBag.displayMenu = "No";

      if (IsAdminUser())
      {
        ViewBag.displayMenu = "Yes";
      }

      return View();
    }

    [Authorize]
    public ActionResult About()
    {
      ViewBag.Message = "Your application description page.";

      return View();
    }
    
    [Authorize]
    public ActionResult Contact()
    {
      ViewBag.Message = "Your contact page.";

      return View();
    }

    public bool IsAdminUser()
    {
      if (User.Identity.IsAuthenticated)
      {
        var user = User.Identity;
        var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
        var s = UserManager.GetRoles(user.GetUserId());
        if (s.Count > 0)
        {
          if (s[0].ToString() == "Admin")
          {
            return true;
          }
          else
          {
            return false;
          }
        }
      }
      return false;
    }
  }
}