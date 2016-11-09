﻿using System;
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

        // Create userid/deviceid folder
        var destinationPath = Path.Combine(Server.MapPath("~/users"),
          User.Identity.GetUserName(), device.ID.ToString());
        if (!Directory.Exists(destinationPath))
        {
          Directory.CreateDirectory(destinationPath);
          string sourceFile = HttpRuntime.AppDomainAppPath + "exes\\results.csv";
          System.IO.File.Copy(sourceFile, destinationPath);
        }

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

      // update attachment for device
      if (attachments != null)
      {
        int id = _dbContext.Devices.Select(o => o.ID).Max();
        foreach (var file in attachments)
        {
          var fileName = Path.GetFileName(file.FileName);
          if (fileName != null)
          {
            // Create userid/deviceid folder
            var destinationPath = Path.Combine(Server.MapPath("~/users"),
              User.Identity.GetUserName(), id.ToString());
            if (!Directory.Exists(destinationPath))
            {
              // create folder and copy results.csv empty template
              Directory.CreateDirectory(destinationPath);
              string sourceFile = HttpRuntime.AppDomainAppPath + "exes\\results.csv";
              System.IO.File.Copy(sourceFile, destinationPath);
            }

            // Set fix filename for all upload file
            var tempFileNames = "deviceidyr" +
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
              var destinationPath = Path.Combine(Server.MapPath("~/users"),
                User.Identity.GetUserName(), id.ToString());
              if (!Directory.Exists(destinationPath))
                Directory.CreateDirectory(destinationPath);

              // Set fix filename for all upload file
              var tempFileNames = "deviceidyr" +
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
      var destinationfilePath = Path.Combine(Server.MapPath("~/users"),
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
            var destinationPath = Path.Combine(Server.MapPath("~/users"),
              User.Identity.GetUserName(), id.ToString());
            if (!Directory.Exists(destinationPath))
              Directory.CreateDirectory(destinationPath);

            // Set fix filename for all upload file
            var tempFileNames = "deviceidyr" +
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
            var physicalPath = Path.Combine(Server.MapPath("~/users"), fileName);

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

    #region AgingCurve
    // GET: Devices/AgingCurve/5
    [Authorize]
    public async Task<ActionResult> AgingCurve(int? id)
    {
      Device device = await _dbContext.Devices.FindAsync(id);
      return View("AgingCurve", device);
    }

    [Authorize]
    public async Task<ActionResult> GenerateAgingCurve(int? id)
    {
      Device device = await _dbContext.Devices.FindAsync(id);

      // Create userid/deviceid folder
      var destinationPath = Path.Combine(Server.MapPath("~/users"),
        User.Identity.GetUserName(), id.ToString());
      if (!Directory.Exists(destinationPath))
        Directory.CreateDirectory(destinationPath);

      System.Diagnostics.Process pExecuteEXE = new System.Diagnostics.Process();
      pExecuteEXE.StartInfo.FileName = HttpRuntime.AppDomainAppPath + "exes\\Degradation1.exe";
      pExecuteEXE.StartInfo.WorkingDirectory = HttpRuntime.AppDomainAppPath + "exes\\";
      pExecuteEXE.StartInfo.Arguments = User.Identity.GetUserName() + " " + id.ToString() + " " +
                                        device.MaxUsedYear;
      pExecuteEXE.StartInfo.UseShellExecute = false;
      pExecuteEXE.Start();
      pExecuteEXE.WaitForExit();


      // check failureforecast.csv
      string failureforecastFileName = HttpRuntime.AppDomainAppPath + "users\\" +
                        User.Identity.GetUserName() + "\\" +
                        device.ID.ToString() + "\\" + "failureforecast.csv";
      var failureforecast = new Dictionary<string, string>();
      if (System.IO.File.Exists(failureforecastFileName))
      {
        using (StreamReader SR = new StreamReader(failureforecastFileName))
        {
          string Line;
          while ((Line = SR.ReadLine()) != null)
          {
            var ReadLine_Array = Line.Split(',');
            if (ReadLine_Array.Length == 2)
              failureforecast.Add(ReadLine_Array[0], ReadLine_Array[1]);
          }
        }
      }

      ViewBag.name = "degradation.jpg";
      ViewBag.failureforecast = failureforecast;

      return View("FaultPrediction", device);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult> GenerateAgingCurveByInput(FormCollection form)
    {
      var id = int.Parse(form["id"].ToString());
      Device device = await _dbContext.Devices.FindAsync(id);

      var input1 = form["input1"] == "" ? 0 : int.Parse(form["input1"].ToString());
      var input2 = form["input2"] == "" ? 0 : int.Parse(form["input2"].ToString());
      var input3 = form["input3"] == "" ? 0 : int.Parse(form["input3"].ToString());
      var input4 = form["input4"] == "" ? 0 : int.Parse(form["input4"].ToString());

      // Create userid/deviceid folder
      var destinationPath = Path.Combine(Server.MapPath("~/users"),
        User.Identity.GetUserName(), id.ToString());
      if (!Directory.Exists(destinationPath))
        Directory.CreateDirectory(destinationPath);

      System.Diagnostics.Process pExecuteEXE = new System.Diagnostics.Process();
      pExecuteEXE.StartInfo.FileName = HttpRuntime.AppDomainAppPath + "exes\\Degradation2.exe";
      pExecuteEXE.StartInfo.WorkingDirectory = HttpRuntime.AppDomainAppPath + "exes\\";
      pExecuteEXE.StartInfo.Arguments = User.Identity.GetUserName() + " " +
                                        id.ToString() + " " +
                                        device.MaxUsedYear + " " +
                                        input1 + " " +
                                        input2 + " " +
                                        input3 + " " +
                                        input4 + " ";
      pExecuteEXE.StartInfo.UseShellExecute = false;
      pExecuteEXE.Start();
      pExecuteEXE.WaitForExit();

      // check failureforecast.csv
      string failureforecastFileName = HttpRuntime.AppDomainAppPath + "users\\" +
                        User.Identity.GetUserName() + "\\" +
                        device.ID.ToString() + "\\" + "failureforecast.csv";
      var failureforecast = new Dictionary<string, string>();
      if (System.IO.File.Exists(failureforecastFileName))
      {
        using (StreamReader SR = new StreamReader(failureforecastFileName))
        {
          string Line;
          while ((Line = SR.ReadLine()) != null)
          {
            var ReadLine_Array = Line.Split(',');
            if (ReadLine_Array.Length == 2)
              failureforecast.Add(ReadLine_Array[0], ReadLine_Array[1]);
          }
        }
      }

      ViewBag.name = "degradation.jpg";
      ViewBag.failureforecast = failureforecast;
      return View("FaultPrediction", device);

    }
    [Authorize]
    public async Task<ActionResult> DownloadFailureForecast(int id)
    {
      Device device = await _dbContext.Devices.FindAsync(id);

      string FileName = HttpRuntime.AppDomainAppPath + "users\\" +
                        User.Identity.GetUserName() + "\\" +
                        id.ToString() + "\\" + "failureforecast.csv";
      if (System.IO.File.Exists(FileName))
      {
        Stream iStream = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
        return File(iStream, "application/unknown", "failureforecast.csv");
      }
      else
      {
        ViewBag.result = "风险报告不存在，請先上传设备ID-年份列表，并运行衰老曲线";
        return View("FaultPrediction", device);
      }
    }
    #endregion

    #region FaultPrediction
    [Authorize]
    public async Task<ActionResult> FaultPrediction(int? id)
    {
      Device device = await _dbContext.Devices.FindAsync(id);

      // check failureforecast.csv
      string failureforecastFileName = HttpRuntime.AppDomainAppPath + "users\\" +
                        User.Identity.GetUserName() + "\\" +
                        device.ID.ToString() + "\\" + "failureforecast.csv";
      var failureforecast = new Dictionary<string, string>();

      // read value
      if (System.IO.File.Exists(failureforecastFileName))
      {
        using (StreamReader SR = new StreamReader(failureforecastFileName))
        {
          string Line;
          while ((Line = SR.ReadLine()) != null)
          {
            var ReadLine_Array = Line.Split(',');
            if (ReadLine_Array.Length == 2)
              failureforecast.Add(ReadLine_Array[0], ReadLine_Array[1]);
          }
        }
      }

      // check failureforecast.csv
      string degradationFileName = HttpRuntime.AppDomainAppPath + "users\\" +
                        User.Identity.GetUserName() + "\\" +
                        device.ID.ToString() + "\\" + "degradation.csv";
      if (System.IO.File.Exists(failureforecastFileName))
      {
        ViewBag.name = "degradation.jpg";
      }
      else
      {
        ViewBag.result = "请先运行衰老曲线，以生成故障预测图形";
      }

      ViewBag.failureforecast = failureforecast;
      return View("FaultPrediction", device);
    }

    #endregion

    #region RiskCurve
    [Authorize]
    public async Task<ActionResult> RiskMining(int id)
    {
      Device device = await _dbContext.Devices.FindAsync(id);

      // check failureforecast.csv
      string fcostcurvePic = HttpRuntime.AppDomainAppPath + "users\\" +
                        User.Identity.GetUserName() + "\\" +
                        device.ID.ToString() + "\\" + "fcostcurve.jpg";

      if (System.IO.File.Exists(fcostcurvePic))
      {
        ViewBag.name = "fcostcurve.jpg";
      }

      // check fcostforecast.csv
      string fcostforecastFileName = HttpRuntime.AppDomainAppPath + "users\\" +
                        User.Identity.GetUserName() + "\\" +
                        device.ID.ToString() + "\\" + "fcostforecast.csv";
      var fcostforecast = new Dictionary<string, string>();
      if (System.IO.File.Exists(fcostforecastFileName))
      {
        using (StreamReader SR = new StreamReader(fcostforecastFileName))
        {
          string Line;
          while ((Line = SR.ReadLine()) != null)
          {
            var ReadLine_Array = Line.Split(',');
            if (ReadLine_Array.Length == 2)
              fcostforecast.Add(ReadLine_Array[0], ReadLine_Array[1]);
          }
        }
      }

      ViewBag.fcostforecast = fcostforecast;
      return View("RiskMining", device);
    }

    [Authorize]
    public async Task<ActionResult> GenerateRiskCurve(int? id)
    {
      Device device = await _dbContext.Devices.FindAsync(id);

      // check failureforecast.csv
      string failureforecastFileName = HttpRuntime.AppDomainAppPath + "users\\" +
                        User.Identity.GetUserName() + "\\" +
                        device.ID.ToString() + "\\" + "failureforecast.csv";

      if (!System.IO.File.Exists(failureforecastFileName))
      {
        // 建议用户”先上传设备ID-年份列表并运行衰老曲线”
        ViewBag.result = "請先上传设备ID-年份列表并运行衰老曲线";
        return View("RiskMining", device);
      }

      // run matlab exe
      System.Diagnostics.Process pExecuteEXE = new System.Diagnostics.Process();
      pExecuteEXE.StartInfo.FileName = HttpRuntime.AppDomainAppPath + "exes\\risk2.exe";
      pExecuteEXE.StartInfo.WorkingDirectory = HttpRuntime.AppDomainAppPath + "exes\\";
      pExecuteEXE.StartInfo.Arguments = User.Identity.GetUserName() + " " +
                                        id.ToString() + " " +
                                        device.ReplaceFee + " " +
                                        device.MaxUsedYear;
      pExecuteEXE.StartInfo.UseShellExecute = false;
      pExecuteEXE.Start();
      pExecuteEXE.WaitForExit();

      // check fcostforecast.csv result
      string fcostforecastFileName = HttpRuntime.AppDomainAppPath + "users\\" +
                        User.Identity.GetUserName() + "\\" +
                        device.ID.ToString() + "\\" + "fcostforecast.csv";
      var fcostforecast = new Dictionary<string, string>();
      if (System.IO.File.Exists(fcostforecastFileName))
      {
        using (StreamReader SR = new StreamReader(fcostforecastFileName))
        {
          string Line;
          while ((Line = SR.ReadLine()) != null)
          {
            var ReadLine_Array = Line.Split(',');
            if (ReadLine_Array.Length == 2)
              fcostforecast.Add(ReadLine_Array[0], ReadLine_Array[1]);
          }
        }
      }

      // check reslut.csv and update value back to database.
      string resultFileName = HttpRuntime.AppDomainAppPath + "users\\" +
                        User.Identity.GetUserName() + "\\" +
                        device.ID.ToString() + "\\" + "results.csv";

      if (System.IO.File.Exists(resultFileName))
      {
        int optimizeChangeYear = 0;

        using (StreamReader SR = new StreamReader(resultFileName))
        {
          string Line;
          while ((Line = SR.ReadLine()) != null)
          {
            optimizeChangeYear = int.Parse(Line);
          }
        }
        // update value to database TODO

      }

      ViewBag.fcostforecast = fcostforecast;
      ViewBag.name = "fcostcurve.jpg";
      return View("RiskMining", device);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult> GenerateRiskCurveByInput(FormCollection form)
    {
      int id = 0;
      if (form["id"] != "")
      {
        int.TryParse(form["id"], out id);
      }
      Device device = await _dbContext.Devices.FindAsync(id);

      // check failureforecast.csv
      string failureforecastFileName = HttpRuntime.AppDomainAppPath + "users\\" +
                        User.Identity.GetUserName() + "\\" +
                        device.ID.ToString() + "\\" + "failureforecast.csv";

      if (!System.IO.File.Exists(failureforecastFileName))
      {
        ViewBag.result = "請先上传设备ID-年份列表并运行衰老曲线";
        return View("RiskMining", device);
      }

      var riskAverage = form["average"] == "" ? 0 : double.Parse(form["average"].ToString());

      System.Diagnostics.Process pExecuteEXE = new System.Diagnostics.Process();
      pExecuteEXE.StartInfo.FileName = HttpRuntime.AppDomainAppPath + "exes\\risk1.exe";
      pExecuteEXE.StartInfo.WorkingDirectory = HttpRuntime.AppDomainAppPath + "exes\\";
      pExecuteEXE.StartInfo.Arguments = User.Identity.GetUserName() + " " +
                                        id + " " +
                                        riskAverage + " " +
                                        device.ReplaceFee + " " +
                                        device.MaxUsedYear + " ";
      pExecuteEXE.StartInfo.UseShellExecute = false;
      pExecuteEXE.Start();
      pExecuteEXE.WaitForExit();

      // check fcostforecast.csv
      string fcostforecastFileName = HttpRuntime.AppDomainAppPath + "users\\" +
                        User.Identity.GetUserName() + "\\" +
                        device.ID.ToString() + "\\" + "fcostforecast.csv";
      var fcostforecast = new Dictionary<string, string>();
      if (System.IO.File.Exists(fcostforecastFileName))
      {
        using (StreamReader SR = new StreamReader(fcostforecastFileName))
        {
          string Line;
          while ((Line = SR.ReadLine()) != null)
          {
            var ReadLine_Array = Line.Split(',');
            if (ReadLine_Array.Length == 2)
              fcostforecast.Add(ReadLine_Array[0], ReadLine_Array[1]);
          }
        }
      }

      //// check reslut.csv and update value back to database.
      //string resultFileName = HttpRuntime.AppDomainAppPath + "users\\" +
      //                  User.Identity.GetUserName() + "\\" +
      //                  device.ID.ToString() + "\\" + "results.csv";

      //if (System.IO.File.Exists(resultFileName))
      //{
      //  var resultsList = new List<double>();

      //  using (StreamReader SR = new StreamReader(resultFileName))
      //  {
      //    string Line;
      //    while ((Line = SR.ReadLine()) != null)
      //    {
      //      var results = Line.Split(',');
      //      resultsList.Add(double.Parse(results[0]));
      //    }
      //  }
      //}

      ViewBag.name = "fcostcurve.jpg";
      ViewBag.fcostforecast = fcostforecast;
      return View("RiskMining", device);
    }


    [Authorize]
    public async Task<ActionResult> DownloadDeviceRisk(int id)
    {
      Device device = await _dbContext.Devices.FindAsync(id);

      string FileName = HttpRuntime.AppDomainAppPath + "users\\" +
                        User.Identity.GetUserName() + "\\" +
                        id.ToString() + "\\" + "fcostforecast.csv";

      if (System.IO.File.Exists(FileName))
      {
        Stream iStream = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
        return File(iStream, "application/unknown", "fcostforecast.csv");
      }
      else
      {
        ViewBag.result = "請先生成風險曲線，再下載風險報告";
        return View("RiskMining", device);
      }
    }

    #endregion

    #region OptReplace
    [Authorize]
    public async Task<ActionResult> OptReplaceage(int id)
    {
      Device device = await _dbContext.Devices.FindAsync(id);

      // check optimalage.jpg
      string optImalageFileName = HttpRuntime.AppDomainAppPath + "users\\" +
                        User.Identity.GetUserName() + "\\" +
                        device.ID + "\\" + "optimalage.jpg";

      if (!System.IO.File.Exists(optImalageFileName))
      {
        ViewBag.result = "图形不存在，请先进行风险挖掘";
        return View("OptReplaceage", device);
      }

      // check reslut.csv and update value back to database.
      string resultFileName = HttpRuntime.AppDomainAppPath + "users\\" +
                        User.Identity.GetUserName() + "\\" +
                        device.ID + "\\" + "results.csv";

      List<int> listResult = new List<int>();
      if (System.IO.File.Exists(resultFileName))
      {
        using (StreamReader SR = new StreamReader(resultFileName))
        {
          string Line;
          while ((Line = SR.ReadLine()) != null)
          {
            var results = Line.Split(',');
            listResult.Add(int.Parse(results[0]));
          }
        }
      }

      ViewBag.cost = listResult[1];
      ViewBag.name = "optimalage.jpg";
      return View("OptReplaceage", device);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult> OptReplaceage2(FormCollection form)
    {
      var id = int.Parse(form["id"].ToString());
      Device device = await _dbContext.Devices.FindAsync(id);

      // check fcostforecast.csv
      string fcostforecastFileName = HttpRuntime.AppDomainAppPath + "users\\" +
                        User.Identity.GetUserName() + "\\" +
                        device.ID.ToString() + "\\" + "fcostforecast.csv";

      if (!System.IO.File.Exists(fcostforecastFileName))
      {
        // check reslut.csv and update value back to database.
        string resultFileName = HttpRuntime.AppDomainAppPath + "users\\" +
                          User.Identity.GetUserName() + "\\" +
                          device.ID + "\\" + "results.csv";

        List<int> listResult = new List<int>();
        if (System.IO.File.Exists(resultFileName))
        {
          using (StreamReader SR = new StreamReader(resultFileName))
          {
            string Line;
            while ((Line = SR.ReadLine()) != null)
            {
              var results = Line.Split(',');
              listResult.Add(int.Parse(results[0]));
            }
          }
        }

        ViewBag.result = "风险预测档案不存在，请先进行风险挖掘";
        ViewBag.cost = listResult[1];
        ViewBag.name = "optimalage.jpg";
        return View("OptReplaceage", device);
      }

      var year = form["year"] == "" ? 0 : int.Parse(form["year"]);

      System.Diagnostics.Process pExecuteEXE = new System.Diagnostics.Process();
      pExecuteEXE.StartInfo.FileName = HttpRuntime.AppDomainAppPath + "exes\\defereffect.exe";
      pExecuteEXE.StartInfo.WorkingDirectory = HttpRuntime.AppDomainAppPath + "exes\\";
      pExecuteEXE.StartInfo.Arguments = User.Identity.GetUserName() + " " +
                                        id.ToString() + " " +
                                        year;
      pExecuteEXE.StartInfo.UseShellExecute = false;
      pExecuteEXE.Start();
      pExecuteEXE.WaitForExit();

      // check defereffect.jpg
      string defereffectFileName = HttpRuntime.AppDomainAppPath + "users\\" +
                        User.Identity.GetUserName() + "\\" +
                        device.ID.ToString() + "\\" + "defereffect.jpg";

      if (!System.IO.File.Exists(defereffectFileName))
      {
        ViewBag.result = "图形不存在，请重新执行提前/延迟成本变化分析";
        // check reslut.csv and update value back to database.
        string resultFileName = HttpRuntime.AppDomainAppPath + "users\\" +
                          User.Identity.GetUserName() + "\\" +
                          device.ID + "\\" + "results.csv";

        List<int> listResult = new List<int>();
        if (System.IO.File.Exists(resultFileName))
        {
          using (StreamReader SR = new StreamReader(resultFileName))
          {
            string Line;
            while ((Line = SR.ReadLine()) != null)
            {
              var results = Line.Split(',');
              listResult.Add(int.Parse(results[0]));
            }
          }
        }

        ViewBag.result = "风险预测档案不存在，请先进行风险挖掘";
        ViewBag.cost = listResult[1];
        ViewBag.name = "optimalage.jpg";
        return View("OptReplaceage", device);
      }

      ViewBag.name = "montecarloage.jpg";
      return View("OptReplaceage2", device);
    }
    #endregion

    #region LongTermInvestmentOpt

    [Authorize]
    public async Task<ActionResult> LongtermInvestmentOpt(int id)
    {
      Device device = await _dbContext.Devices.FindAsync(id);

      // check optimalage.jpg
      string montecarloinvestFileName = HttpRuntime.AppDomainAppPath + "users\\" +
                        User.Identity.GetUserName() + "\\" +
                        device.ID + "\\" + "montecarloinvest.jpg";

      if (System.IO.File.Exists(montecarloinvestFileName))
      {
        ViewBag.name = "montecarloinvest.jpg";
      }

      // check reslut.csv and update value back to database.
      string resultFile = HttpRuntime.AppDomainAppPath + "users\\" +
                        User.Identity.GetUserName() + "\\" +
                        device.ID + "\\" + "results.csv";

      List<int> listResults = new List<int>();
      if (System.IO.File.Exists(resultFile))
      {
        using (StreamReader SR = new StreamReader(resultFile))
        {
          string Line;
          while ((Line = SR.ReadLine()) != null)
          {
            var results = Line.Split(',');
            listResults.Add(int.Parse(results[0]));
          }
        }
        ViewBag.result1 = listResults[2];
        ViewBag.result2 = listResults[3];
        ViewBag.result3 = listResults[4];
      }

      return View("LongtermInvestmentOpt", device);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult> LongtermInvestmentOpt(FormCollection form)
    {
      var id = int.Parse(form["id"].ToString());
      Device device = await _dbContext.Devices.FindAsync(id);

      var maxAmount = form["maxamount"] == "" ? 0 : double.Parse(form["maxamount"].ToString());
      var fixPeriod = form["fixperiod"] == "" ? 0 : int.Parse(form["fixperiod"].ToString());

      System.Diagnostics.Process pExecuteEXE = new System.Diagnostics.Process();
      pExecuteEXE.StartInfo.FileName = HttpRuntime.AppDomainAppPath + "exes\\invest.exe";
      pExecuteEXE.StartInfo.WorkingDirectory = HttpRuntime.AppDomainAppPath + "exes\\";
      pExecuteEXE.StartInfo.Arguments = User.Identity.GetUserName() + " " +
                                        id.ToString() + " " +
                                        maxAmount + " " +
                                        fixPeriod;
      pExecuteEXE.StartInfo.UseShellExecute = false;
      pExecuteEXE.Start();
      pExecuteEXE.WaitForExit();

      // check optimalage.jpg
      string montecarloinvestFileName = HttpRuntime.AppDomainAppPath + "users\\" +
                        User.Identity.GetUserName() + "\\" +
                        device.ID + "\\" + "montecarloinvest.jpg";

      if (System.IO.File.Exists(montecarloinvestFileName))
      {
        ViewBag.name = "montecarloinvest.jpg";
      }

      // check reslut.csv and update value back to database.
      string resultFile = HttpRuntime.AppDomainAppPath + "users\\" +
                        User.Identity.GetUserName() + "\\" +
                        device.ID + "\\" + "results.csv";

      List<int> listResults = new List<int>();
      if (System.IO.File.Exists(resultFile))
      {
        using (StreamReader SR = new StreamReader(resultFile))
        {
          string Line;
          while ((Line = SR.ReadLine()) != null)
          {
            var results = Line.Split(',');
            listResults.Add(int.Parse(results[0]));
          }
        }
        ViewBag.result1 = listResults[2];
        ViewBag.result2 = listResults[3];
        ViewBag.result3 = listResults[4];
      }

      return View("LongtermInvestmentOpt", device);
    }

    #endregion

    [Authorize]
    public async Task<ActionResult> AlarmDecision(int id)
    {
      Device device = await _dbContext.Devices.FindAsync(id);
      return View("AlarmDecision", device);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult> AlarmDecision(FormCollection form)
    {
      var id = int.Parse(form["id"].ToString());
      Device device = await _dbContext.Devices.FindAsync(id);

      var sensor1 = form["sensor1"] == "" ? -1000 : double.Parse(form["sensor1"].ToString());
      var sensor2 = form["sensor2"] == "" ? -1000 : double.Parse(form["sensor2"].ToString());
      var sensor3 = form["sensor3"] == "" ? -1000 : double.Parse(form["sensor3"].ToString());
      var sensor4 = form["sensor4"] == "" ? -1000 : double.Parse(form["sensor4"].ToString());
      var sensor5 = form["sensor5"] == "" ? -1000 : double.Parse(form["sensor5"].ToString());
      var sensor6 = form["sensor6"] == "" ? -1000 : double.Parse(form["sensor6"].ToString());

      //      System.Diagnostics.Process pExecuteEXE = new System.Diagnostics.Process();
      //      pExecuteEXE.StartInfo.FileName = HttpRuntime.AppDomainAppPath + "exes\\alarm.exe";
      //      pExecuteEXE.StartInfo.WorkingDirectory = HttpRuntime.AppDomainAppPath + "exes\\";
      //      pExecuteEXE.StartInfo.Arguments = User.Identity.GetUserName() + " " +
      //                                        id.ToString() + " " +
      //                                        sensor1 + " " +
      //                                        sensor2 + " " +
      //                                        sensor3 + " " +
      //                                        sensor4 + " " +
      //                                        sensor5 + " " +
      //                                        sensor6;
      //
      //      pExecuteEXE.StartInfo.UseShellExecute = false;
      //      pExecuteEXE.Start();
      //      pExecuteEXE.WaitForExit();
      //
      //      // check reslut.csv and update value back to database.
      //      string resultFile = HttpRuntime.AppDomainAppPath + "users\\" +
      //                        User.Identity.GetUserName() + "\\" +
      //                        device.ID + "\\" + "results.csv";
      //
      //      List<int> listResults = new List<int>();
      //      if (System.IO.File.Exists(resultFile))
      //      {
      //        using (StreamReader SR = new StreamReader(resultFile))
      //        {
      //          string Line;
      //          while ((Line = SR.ReadLine()) != null)
      //          {
      //            var results = Line.Split(',');
      //            listResults.Add(int.Parse(results[0]));
      //          }
      //        }
      //        ViewBag.status = listResults[5];
      //      }
      ViewBag.status = 1;
      return View("AlarmDecision", device);
    }

    [Authorize]
    public async Task<ActionResult> CloudComparison(int id)
    {
      Device device = await _dbContext.Devices.FindAsync(id);
      return View("CloudComparison", device);
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
