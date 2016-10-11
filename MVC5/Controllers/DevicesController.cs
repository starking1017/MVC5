using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MVC5.Models;

namespace MVC5.Controllers
{
  public class DevicesController : Controller
  {
    private ApplicationDbContext db = new ApplicationDbContext();

    // GET: Devices
    [Authorize]
    public async Task<ActionResult> Index()
    {
      string currentUserId = User.Identity.GetUserId();
      ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);

      var deviceLists = await db.Devices.ToListAsync();
      var list = deviceLists.Where(li => li.ApplicationUser == currentUser);
      return View(list);
    }

    // GET: Devices/Details/5
    [Authorize]
    public async Task<ActionResult> Details(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Device device = await db.Devices.FindAsync(id);
      if (device == null)
      {
        return HttpNotFound();
      }
      return View(device);
    }

    // GET: Devices/Create
    [Authorize]
    public ActionResult Create()
    {
      return View();
    }

    // POST: Devices/Create
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create([Bind(Include = "ID,DeviceId,Name,Type,Factory,Model,Amount,Description,MaintainFrequency,ReplaceFee,MaxUsedYear")]  Device device)
    {
      if (ModelState.IsValid)
      {
        string currentUserId = User.Identity.GetUserId();
        device.ApplicationUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
        db.Devices.Add(device);

        await db.SaveChangesAsync();
        return RedirectToAction("Index");
      }

      return View(device);
    }

    // GET: Devices/Edit/5
    [Authorize]
    public async Task<ActionResult> Edit(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Device device = await db.Devices.FindAsync(id);
      if (device == null)
      {
        return HttpNotFound();
      }
      return View(device);
    }

    // POST: Devices/Edit/5
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit([Bind(Include = "ID,DeviceId,Name,Type,Factory,Model,Amount,Description,MaintainFrequency,ReplaceFee,MaxUsedYear")] Device device)
    {
      if (ModelState.IsValid)
      {
        db.Entry(device).State = EntityState.Modified;
        await db.SaveChangesAsync();
      }
      return RedirectToAction("Index");
    }

    // GET: Devices/Delete/5
    [Authorize]
    public async Task<ActionResult> Delete(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Device device = await db.Devices.FindAsync(id);
      if (device == null)
      {
        return HttpNotFound();
      }
      return View(device);
    }

    // POST: Devices/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<ActionResult> DeleteConfirmed(int id)
    {
      Device device = await db.Devices.FindAsync(id);
      db.Devices.Remove(device);
      await db.SaveChangesAsync();
      return RedirectToAction("Index");
    }

    // GET: Devices/LifeManageMenu/5
    [Authorize]
    public async Task<ActionResult> LifeManageMenu(int? id)
    {
      Device device = await db.Devices.FindAsync(id);
      return View("LifeManageMenu",device);
    }

    public ActionResult Save(IEnumerable<HttpPostedFileBase> files)
    {
      // The Name of the Upload component is "files"
      if (files != null)
      {
        foreach (var file in files)
        {
          // Some browsers send file names with full path.
          // We are only interested in the file name.
          var fileName = Path.GetFileName(file.FileName);
          var physicalPath = Path.Combine(Server.MapPath("~/App_Data"), fileName);

          // The files are not actually saved in this demo
          file.SaveAs(physicalPath);
        }
      }

      // Return an empty string to signify success
      return Content("");
    }

    public ActionResult Remove(string[] fileNames)
    {
      // The parameter of the Remove action must be called "fileNames"

      if (fileNames != null)
      {
        foreach (var fullName in fileNames)
        {
          var fileName = Path.GetFileName(fullName);
          var physicalPath = Path.Combine(Server.MapPath("~/App_Data"), fileName);

          // TODO: Verify user permissions

          if (System.IO.File.Exists(physicalPath))
          {
            // The files are not actually removed in this demo
            System.IO.File.Delete(physicalPath);
          }
        }
      }

      // Return an empty string to signify success
      return Content("");
    }


    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        db.Dispose();
      }
      base.Dispose(disposing);
    }

    // GET: Devices/AgingCurve/5
    [Authorize]
    public async Task<ActionResult> AgingCurve(int? id)
    {
      Device device = await db.Devices.FindAsync(id);
      return View("AgingCurve", device);
    }

    public async Task<ActionResult> FaultPrediction(int id)
    {
      Device device = await db.Devices.FindAsync(id);
      return View("FaultPrediction", device);
    }

    public async Task<ActionResult> RiskMining(int id)
    {
      Device device = await db.Devices.FindAsync(id);
      return View("RiskMining", device);
    }

    public async Task<ActionResult> OptReplaceage(int id)
    {
      Device device = await db.Devices.FindAsync(id);
      return View("OptReplaceage", device);
    }

    public async Task<ActionResult> OptReplaceage2(int id)
    {
      Device device = await db.Devices.FindAsync(id);
      return View("OptReplaceage2", device);
    }

    public async Task<ActionResult> OptReplaceage3(int id)
    {
      Device device = await db.Devices.FindAsync(id);
      return View("OptReplaceage3", device);
    }

    public async Task<ActionResult> LongtermInvestmentOpt(int id)
    {
      Device device = await db.Devices.FindAsync(id);
      return View("LongtermInvestmentOpt", device);
    }

    public async Task<ActionResult> CloudComparison(int id)
    {
      Device device = await db.Devices.FindAsync(id);
      return View("CloudComparison", device);
    }
    public async Task<ActionResult> AlarmDecision(int id)
    {
      Device device = await db.Devices.FindAsync(id);
      return View("AlarmDecision", device);
    }
  }
}
