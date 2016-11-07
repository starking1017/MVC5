using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MVC5.Models;

namespace MVC5.Controllers
{
  public class UploadController : Controller
  {
    private readonly ApplicationDbContext _dbContext;

    public UploadController()
    {
      _dbContext = new ApplicationDbContext();
    }
    #region AsyncFilesUpload
    public async Task<ActionResult> SaveDeviceFailure(IEnumerable<HttpPostedFileBase> files,
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
            var tempFileNames = "devicefailure" +
                                fileName.Substring(fileName.IndexOf(".", StringComparison.Ordinal));
            var destinationfilePath = Path.Combine(destinationPath, tempFileNames);

            file.SaveAs(destinationfilePath);
          }
        }
      }
      // Return an empty string to signify success
      return Content("");
    }

    public async Task<ActionResult> SaveDeviceRisk(IEnumerable<HttpPostedFileBase> files,
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
            var tempFileNames = "devicefcost" +
                                fileName.Substring(fileName.IndexOf(".", StringComparison.Ordinal));
            var destinationfilePath = Path.Combine(destinationPath, tempFileNames);

            file.SaveAs(destinationfilePath);
          }
        }
      }
      // Return an empty string to signify success
      return Content("");
    }

    public async Task<ActionResult> SaveSensorHistoryData(IEnumerable<HttpPostedFileBase> files,
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
            var tempFileNames = "alarmhistory" +
                                fileName.Substring(fileName.IndexOf(".", StringComparison.Ordinal));
            var destinationfilePath = Path.Combine(destinationPath, tempFileNames);

            file.SaveAs(destinationfilePath);
          }
        }
      }
      // Return an empty string to signify success
      return Content("");
    }

    public ActionResult RemoveDeviceFailure(string[] fileNames)
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
  }
}