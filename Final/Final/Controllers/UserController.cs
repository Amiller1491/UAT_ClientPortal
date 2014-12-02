using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Final.Models;
using System.Web.Helpers;

namespace Final.Controllers
{
    [Authorize(Roles= "Admin")]
    public class UserController : Controller
    {
          
        private UATContext db = new UATContext();

        //
        // GET: /User/
        
        public ActionResult Index()
        {
            var user = db.User.Include(u => u.Role);
            return View(user.ToList());
        }

        //
        // GET: /User/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int id = 0)
        {
            UserModel usermodel = db.User.Find(id);
            if (usermodel == null)
            {
                return HttpNotFound();
            }
            return View(usermodel);
        }

        //
        // GET: /User/Edit/5
        //[Authorize(Roles="Admin")]
        public ActionResult ResetPassword(int id = 0)
        {
            UserModel usermodel = db.User.Find(id);
            ViewBag.useremail = usermodel.Email;
            if (usermodel == null)
            {
                return HttpNotFound();
            }

            return View(usermodel);
        }

        //
        // POST: /User/Edit/5

        [HttpPost]
        public ActionResult ResetPassword(UserModel usermodel)
        {
            if (ModelState.IsValid)
            {
                usermodel.Password = Crypto.HashPassword(usermodel.Password);
                db.Entry(usermodel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(usermodel);
        }


        //
        // GET: /User/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.RoleID = new SelectList(db.Role, "RoleID", "Name");
            return View();
        }

        //
        // POST: /User/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserModel usermodel)
        {
            if (ModelState.IsValid)
            {
                usermodel.Password = Crypto.HashPassword(usermodel.Password);
                db.User.Add(usermodel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.RoleID = new SelectList(db.Role, "RoleID", "Name", usermodel.RoleID);
            return View(usermodel);
        }

        //
        // GET: /User/Edit/5

        public ActionResult Edit(int id = 0)
        {
            UserModel usermodel = db.User.Find(id);
            if (usermodel == null)
            {
                return HttpNotFound();
            }
            ViewBag.RoleID = new SelectList(db.Role, "RoleID", "Name", usermodel.RoleID);
            return View(usermodel);
        }

        //
        // POST: /User/Edit/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserModel usermodel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(usermodel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RoleID = new SelectList(db.Role, "RoleID", "Name", usermodel.RoleID);
            return View(usermodel);
        }

        //
        // GET: /User/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id = 0)
        {
            UserModel usermodel = db.User.Find(id);
            if (usermodel == null)
            {
                return HttpNotFound();
            }
            return View(usermodel);
        }

        //
        // POST: /User/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserModel usermodel = db.User.Find(id);
            db.User.Remove(usermodel);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}