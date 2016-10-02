using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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
        return RedirectToAction("Index");
      }
      return View(device);
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

    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        db.Dispose();
      }
      base.Dispose(disposing);
    }
  }
}
