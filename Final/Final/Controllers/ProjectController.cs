using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Final.Models;
using Final.ViewModels;

namespace Final.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        private UATContext db = new UATContext();

        //
        // GET: /Project/
        [Authorize]
        public ActionResult Index()
        {
            var project = db.Project.Include(p => p.User);

            return View(project.ToList());
        }

        public ActionResult ViewUATs(int id = 0)
        {
            ProjectModel projectmodel = db.Project.Find(id);
            var uat = projectmodel.UAT.ToList();
            
            if (uat == null)
            {
                return HttpNotFound();
            }

            return View(uat.ToList());
        }
        //
        // GET: /Project/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int id = 0)
        {
            ProjectModel projectmodel = db.Project.Find(id);
            if (projectmodel == null)
            {
                return HttpNotFound();
            }
            return View(projectmodel);
        }

        //
        // GET: /Project/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.UserID = new SelectList(db.User, "UserID", "FirstName");
            return View();
        }

        //
        // POST: /Project/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProjectModel projectmodel)
        {
            if (ModelState.IsValid)
            {
                db.Project.Add(projectmodel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserID = new SelectList(db.User, "UserID", "FirstName", projectmodel.UserID);
            return View(projectmodel);
        }

        //
        // GET: /Project/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id = 0)
        {
            ProjectModel projectmodel = db.Project.Find(id);
            if (projectmodel == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserID = new SelectList(db.User, "UserID", "FirstName", projectmodel.UserID);
            return View(projectmodel);
        }

        //
        // POST: /Project/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProjectModel projectmodel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(projectmodel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserID = new SelectList(db.User, "UserID", "FirstName", projectmodel.UserID);
            return View(projectmodel);
        }

        //
        // GET: /Project/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id = 0)
        {
            ProjectModel projectmodel = db.Project.Find(id);
            if (projectmodel == null)
            {
                return HttpNotFound();
            }
            return View(projectmodel);
        }

        //
        // POST: /Project/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProjectModel projectmodel = db.Project.Find(id);
            db.Project.Remove(projectmodel);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        //[HttpPost]
        public ActionResult SendMail(EmailModel mail, int id = 0)
        {

            UATModel uatmodel = db.UAT.Find(id);
            int u_id = uatmodel.Project.UserID;

            UserModel usermodel = db.User.Find(u_id);

            //try
            //{
            if (ModelState.IsValid)
            {
                var username = usermodel.FirstName + " " + usermodel.LastName;
                mail.Mail_Date = System.DateTime.Now;
                //mail.AnganwadiId = 1;
                mail.Mail_From = "adammiller001@gmail.com";
                mail.Mail_Subject = "New UAT is available for you";
                MailMessage message = new MailMessage();
                message.From = new MailAddress(mail.Mail_From);
                message.To.Add(new MailAddress(usermodel.Email));
                message.Subject = mail.Mail_Subject;
                //message.Body = mail.Mail_Contents;
                message.Body = "Hello " + username + ", \n A new UAT is available for you to view. Please click here to view it: http://localhost:1865/Project/ViewUATs/" + uatmodel.UATID + ". \n\n\n Do not reply on this email. This is only for notification purpose. Good Day!\n\n Sent at:" + DateTime.Now;

                //message.Body = "Hello, " + usermodel.FirstName + " " + usermodel.LastName + ". A new UAT is available for you to view. Please check it out.";
                SmtpClient client = new SmtpClient();
                client.Send(message);
                //return View("Thanks", mail);
                ViewBag.name = uatmodel.Name;
                uatmodel.StatusD = 1;
                db.Entry(uatmodel).State = EntityState.Modified;
                db.SaveChanges();
                return Redirect("/Project/ViewUATs/" + uatmodel.UATID);
            }
            else
            {
                return View(uatmodel);
            }
        }
        //catch (Exception e)
        //{
        //    return View(e);
        //}
        //}
    }
}