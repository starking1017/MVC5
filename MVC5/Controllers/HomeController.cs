using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC5.Controllers
{
  public class HomeController : Controller
  {
    [Authorize]
    public ActionResult Index()
    {
      return View();
    }

    [Authorize]
    public ActionResult About()
    {
      ViewBag.Message = "Your application description page.";

      return View();
    }

    [Authorize]
    public ActionResult Matlab(string p1, string p2, string name)
    {
      ViewBag.name = name + ".jpg";

      System.Diagnostics.Process pExecuteEXE = new System.Diagnostics.Process();
      pExecuteEXE.StartInfo.FileName = HttpRuntime.AppDomainAppPath + "temp\\lifecurve1.exe";
      pExecuteEXE.StartInfo.WorkingDirectory = HttpRuntime.AppDomainAppPath + "temp\\";
      pExecuteEXE.StartInfo.Arguments = "80 30 20 50 80 " + name;
      pExecuteEXE.StartInfo.UseShellExecute = false;
      pExecuteEXE.Start();

      pExecuteEXE.WaitForExit();
      return View();
    }

    [Authorize]
    public ActionResult Contact()
    {
      ViewBag.Message = "Your contact page.";

      return View();
    }


  }
}