using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Hotel_Reservation_System.Models;

namespace Hotel_Reservation_System.Controllers
{
    [Authorize]
    public class Room_UsageController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public async Task<ActionResult> Index()
        {
            return View(await db.Room_Usage.ToListAsync());
        }

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room_Usage room_Usage = await db.Room_Usage.FindAsync(id);
            if (room_Usage == null)
            {
                return HttpNotFound();
            }
            return View(room_Usage);
        }

        public ActionResult Create()
        {
            return View(new Room_Usage());
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Room_Id,Guest_Id,Hours,Checkin")] Room_Usage room_Usage)
        {
            if (room_Usage != null)
            {
                room_Usage.Date = DateTime.Now;
            }
            if (ModelState.IsValid)
            {
                List<Room_Usage> usages = db.Room_Usage.ToList().Where(x => x.Room_Id == room_Usage.Room_Id).ToList();
                foreach (var item in usages)
                {
                    if ((item.Checkin >= room_Usage.Checkin && item.Checkin <= room_Usage.CheckOut) ||
                        item.Checkin.AddHours(room_Usage.Hours) >= room_Usage.Checkin)
                    {
                        ModelState.AddModelError("", "Room is not Availble!");
                        return View(room_Usage);
                    }
                }


                db.Room_Usage.Add(room_Usage);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(room_Usage);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room_Usage room_Usage = await db.Room_Usage.FindAsync(id);
            if (room_Usage == null)
            {
                return HttpNotFound();
            }
            return View(room_Usage);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Room_Id,Guest_Id,Isactive,Date")] Room_Usage room_Usage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(room_Usage).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(room_Usage);
        }

        
        public async Task<JsonResult> Rooms(string RType)
        {
            if (!string.IsNullOrEmpty(RType))
            {
                List<Room> rooms = await db.Rooms.ToListAsync();
                rooms = rooms.ToList().Where(x => x.RType.Trim().ToLower() == 
                            RType.Trim().ToLower()).ToList();
                return Json(new {Rooms = rooms });
            }
            return Json(new {Rooms = new List<Room>() });
        }

        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room_Usage room_Usage = await db.Room_Usage.FindAsync(id);
            if (room_Usage == null)
            {
                return HttpNotFound();
            }
            return View(room_Usage);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Room_Usage room_Usage = await db.Room_Usage.FindAsync(id);
            db.Room_Usage.Remove(room_Usage);
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
