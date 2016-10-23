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
using Microsoft.AspNet.Identity.EntityFramework;
using MVC5.Models;

namespace MVC5.Controllers
{
  public class DevicesController : Controller
  {
    private readonly ApplicationDbContext _dbContext;

    public DevicesController()
    {
      _dbContext = new ApplicationDbContext();
    }

    #region BasicCRUD
    // GET: Devices
    [Authorize]
    public async Task<ActionResult> Index()
    {
      if (User.Identity.IsAuthenticated)
      {
        ViewBag.displayMenu = "No";

        if (IsAdminUser())
        {
          ViewBag.displayMenu = "Yes";
        }
      }
      string currentUserId = User.Identity.GetUserId();
      ApplicationUser currentUser = _dbContext.Users.FirstOrDefault(x => x.Id == currentUserId);

      var deviceLists = await _dbContext.Devices.ToListAsync();
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
      Device device = await _dbContext.Devices.FindAsync(id);
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

    //// POST: Devices/Create
    //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create([Bind(Include = "ID,DeviceId,Name,Type,Factory,Model,Amount,Description,MaintainFrequency,ReplaceFee,MaxUsedYear")]  Device device)
    {
      if (ModelState.IsValid)
      {
        string currentUserId = User.Identity.GetUserId();
        device.ApplicationUser = _dbContext.Users.FirstOrDefault(x => x.Id == currentUserId);
        _dbContext.Devices.Add(device);

        await _dbContext.SaveChangesAsync();
        return RedirectToAction("Index");
      }

      return View(device);
    }
    // POST: Devices/Create
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(
      [Bind(Include = "ID,DeviceId,Name,Type,Factory,Model,Amount,Description,MaintainFrequency,ReplaceFee,MaxUsedYear")]
      Device device, IEnumerable<HttpPostedFileBase> attachments)
    {
      if (ModelState.IsValid)
      {
        string currentUserId = User.Identity.GetUserId();
        device.ApplicationUser = _dbContext.Users.FirstOrDefault(x => x.Id == currentUserId);
        if (attachments != null)
          device.DeviceList = true;
        else
          device.DeviceList = false;
        _dbContext.Devices.Add(device);

        await _dbContext.SaveChangesAsync();
      }

      if (attachments != null)
      {
        int id = _dbContext.Devices.Select(o => o.ID).Max();
        foreach (var file in attachments)
        {
          var fileName = Path.GetFileName(file.FileName);
          if (fileName != null)
          {
            // Create userid/deviceid folder
            var destinationPath = Path.Combine(Server.MapPath("~/App_Data"),
              User.Identity.GetUserName(), id.ToString());
            if (!Directory.Exists(destinationPath))
              Directory.CreateDirectory(destinationPath);

            // Set fix filename for all upload file
            var tempFileNames = "DeviceIdList" +
                                fileName.Substring(fileName.IndexOf(".", StringComparison.Ordinal));
            var destinationfilePath = Path.Combine(destinationPath, tempFileNames);

            file.SaveAs(destinationfilePath);
          }
        }
      }

      return RedirectToAction("Index");
    }

    // GET: Devices/Edit/5
    [Authorize]
    public async Task<ActionResult> Edit(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Device device = await _dbContext.Devices.FindAsync(id);
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
    public async Task<ActionResult> Edit([Bind(Include = "ID,DeviceId,Name,Type,Factory,Model,Amount,Description,MaintainFrequency,ReplaceFee,MaxUsedYear,DeviceList")] Device device
      , IEnumerable<HttpPostedFileBase> attachments)
    {
      if (ModelState.IsValid)
      {
        _dbContext.Entry(device).State = EntityState.Modified;

        // get upload file status and save to db
        if (attachments != null)
          device.DeviceList = true;
        else
          device.DeviceList = false;
        await _dbContext.SaveChangesAsync();

        // upload file to server
        if (attachments != null)
        {
          int id = device.ID;
          foreach (var file in attachments)
          {
            var fileName = Path.GetFileName(file.FileName);
            if (fileName != null)
            {
              // Create userid/deviceid folder
              var destinationPath = Path.Combine(Server.MapPath("~/App_Data"),
                User.Identity.GetUserName(), id.ToString());
              if (!Directory.Exists(destinationPath))
                Directory.CreateDirectory(destinationPath);

              // Set fix filename for all upload file
              var tempFileNames = "DeviceIdList" +
                                  fileName.Substring(fileName.IndexOf(".", StringComparison.Ordinal));
              var destinationfilePath = Path.Combine(destinationPath, tempFileNames);

              file.SaveAs(destinationfilePath);
            }
          }
        }
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
      Device device = await _dbContext.Devices.FindAsync(id);
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
      // Delete db device info
      Device device = await _dbContext.Devices.FindAsync(id);
      _dbContext.Devices.Remove(device);
      await _dbContext.SaveChangesAsync();

      // Delete device related files
      var destinationfilePath = Path.Combine(Server.MapPath("~/App_Data"),
            User.Identity.GetUserName(), id.ToString());

      if (Directory.Exists(destinationfilePath))
        Directory.Delete(destinationfilePath, true);

      return RedirectToAction("Index");
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        _dbContext.Dispose();
      }
      base.Dispose(disposing);
    }

    #endregion

    #region AsyncFilesUpload
    public async Task<ActionResult> Save(IEnumerable<HttpPostedFileBase> files,
      int id)
    {
      // The Name of the Upload component is "files"
      if (files != null)
      {
        // Save upload status to db
        Device device = await _dbContext.Devices.FindAsync(id);
        device.DeviceList = true;
        await _dbContext.SaveChangesAsync();

        foreach (var file in files)
        {
          // Some browsers send file names with full path.
          // We are only interested in the file name.
          var fileName = Path.GetFileName(file.FileName);
          if (fileName != null)
          {
            // Create userid/deviceid folder
            var destinationPath = Path.Combine(Server.MapPath("~/App_Data"),
              User.Identity.GetUserName(), id.ToString());
            if (!Directory.Exists(destinationPath))
              Directory.CreateDirectory(destinationPath);

            // Set fix filename for all upload file
            var tempFileNames = "DeviceIdList" +
                                fileName.Substring(fileName.IndexOf(".", StringComparison.Ordinal));
            var destinationfilePath = Path.Combine(destinationPath, tempFileNames);

            file.SaveAs(destinationfilePath);
          }
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
          if (fileName != null)
          {
            var physicalPath = Path.Combine(Server.MapPath("~/App_Data"), fileName);

            // TODO: Verify user permissions

            if (System.IO.File.Exists(physicalPath))
            {
              // The files are not actually removed in this demo
              System.IO.File.Delete(physicalPath);
            }
          }
        }
      }

      // Return an empty string to signify success
      return Content("");
    }

    #endregion

    #region LifeCycleManage
    // GET: Devices/LifeManageMenu/5
    [Authorize]
    public async Task<ActionResult> LifeManageMenu(int? id)
    {
      Device device = await _dbContext.Devices.FindAsync(id);
      return View("LifeManageMenu", device);
    }

    // GET: Devices/AgingCurve/5
    [Authorize]
    public async Task<ActionResult> AgingCurve(int? id)
    {
      Device device = await _dbContext.Devices.FindAsync(id);
      return View("AgingCurve", device);
    }

    [Authorize]
    public async Task<ActionResult> FaultPrediction(int id)
    {
      Device device = await _dbContext.Devices.FindAsync(id);
      return View("FaultPrediction", device);
    }

    [Authorize]
    public async Task<ActionResult> RiskMining(int id)
    {
      Device device = await _dbContext.Devices.FindAsync(id);
      return View("RiskMining", device);
    }

    [Authorize]
    public async Task<ActionResult> OptReplaceage(int id)
    {
      Device device = await _dbContext.Devices.FindAsync(id);
      return View("OptReplaceage", device);
    }

    [Authorize]
    public async Task<ActionResult> OptReplaceage2(int id)
    {
      Device device = await _dbContext.Devices.FindAsync(id);
      return View("OptReplaceage2", device);
    }

    [Authorize]
    public async Task<ActionResult> OptReplaceage3(int id)
    {
      Device device = await _dbContext.Devices.FindAsync(id);
      return View("OptReplaceage3", device);
    }

    [Authorize]
    public async Task<ActionResult> LongtermInvestmentOpt(int id)
    {
      Device device = await _dbContext.Devices.FindAsync(id);
      return View("LongtermInvestmentOpt", device);
    }

    [Authorize]
    public async Task<ActionResult> CloudComparison(int id)
    {
      Device device = await _dbContext.Devices.FindAsync(id);
      return View("CloudComparison", device);
    }

    [Authorize]
    public async Task<ActionResult> AlarmDecision(int id)
    {
      Device device = await _dbContext.Devices.FindAsync(id);
      return View("AlarmDecision", device);
    }

    #endregion

    public bool IsAdminUser()
    {
      if (User.Identity.IsAuthenticated)
      {
        var user = User.Identity;
        var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_dbContext));
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
