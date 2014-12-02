using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Final.Models;
using System.Net.Mail;

namespace Final.Controllers
{

    [Authorize]
    public class UATController : Controller
    {
        private UATContext db = new UATContext();

        //
        // GET: /UAT/
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var uat = db.UAT.Include(u => u.Project).Include(u => u.Status);
            return View(uat.ToList());
        }

        public ActionResult ViewTestCases(int id = 0)
        {
            UATModel uatmodel = db.UAT.Find(id);
            //so when client views testcase and if test case is "in progress" it wil change it to "Viewed" :Rabin
            if (User.IsInRole("Client"))
            {
                if (uatmodel.StatusD == 5 || uatmodel.StatusD == 1)
                {
                    uatmodel.StatusD = 2;
                    db.Entry(uatmodel).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            
            var testcase = uatmodel.TestCase.ToList();
            var preStatus = uatmodel.StatusD;
            checkUatStat(id);
            var afterStatus = uatmodel.StatusD;
            if (User.IsInRole("Client")) {
            if (preStatus != afterStatus)
            {
                SendMail(uatmodel.UATID);
            }
            }
            
            if (testcase == null)
            {
                return HttpNotFound();
            }

            return View(testcase.ToList());
        }

        //
        // GET: /UAT/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int id = 0)
        {
            UATModel uatmodel = db.UAT.Find(id);
            if (uatmodel == null)
            {
                return HttpNotFound();
            }
            return View(uatmodel);
        }

        //
        // GET: /UAT/Create
        [Authorize(Roles="Admin")]
        public ActionResult Create()
        {

            ViewBag.ProjectID = new SelectList(db.Project, "ProjectID", "Name");
            ViewBag.StatusD = new SelectList(db.Status, "StatusD", "StatusType");
            return View();
        }

        //
        // POST: /UAT/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UATModel uatmodel)
        {
            uatmodel.Started = DateTime.Now;
            uatmodel.LastModified = DateTime.Now;
            uatmodel.StatusD = 5;
            if (ModelState.IsValid)
            {
                db.UAT.Add(uatmodel);
                db.SaveChanges();
                return Redirect("/Project/ViewUATs/" + uatmodel.ProjectID);
            }

            ViewBag.ProjectID = new SelectList(db.Project, "ProjectID", "Name", uatmodel.ProjectID);
            ViewBag.StatusD = new SelectList(db.Status, "StatusD", "StatusType", uatmodel.StatusD);
            return View(uatmodel);
        }

        //
        // GET: /UAT/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id = 0)
        {
            UATModel uatmodel = db.UAT.Find(id);
            if (uatmodel == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProjectID = new SelectList(db.Project, "ProjectID", "Name", uatmodel.ProjectID);
            ViewBag.StatusD = new SelectList(db.Status, "StatusD", "StatusType", uatmodel.StatusD);
            return View(uatmodel);
        }

        //
        // POST: /UAT/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UATModel uatmodel)
        {
            
            if (ModelState.IsValid)
            {
                
                uatmodel.LastModified = DateTime.Now;
                db.Entry(uatmodel).State = EntityState.Modified;
                db.SaveChanges();
                return Redirect("/Project/ViewUATs/" + uatmodel.ProjectID);
            }
            ViewBag.ProjectID = new SelectList(db.Project, "ProjectID", "Name", uatmodel.ProjectID);
            ViewBag.StatusD = new SelectList(db.Status, "StatusD", "StatusType", uatmodel.StatusD);
            return View(uatmodel);
        }

        //
        // GET: /UAT/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id = 0)
        {
            UATModel uatmodel = db.UAT.Find(id);
            if (uatmodel == null)
            {
                return HttpNotFound();
            }
            return View(uatmodel);
        }

        //
        // POST: /UAT/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UATModel uatmodel = db.UAT.Find(id);
            db.UAT.Remove(uatmodel);
            db.SaveChanges();
            return Redirect("/Project/ViewUATs/" + uatmodel.ProjectID);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public void checkUatStat(int id)
        {

         
            int count = 0;
            int state = 1;
            UATModel uatmodel = db.UAT.Find(id);
            var testcase = uatmodel.TestCase.ToList();
            int temp = testcase.Count();
            foreach (var item in testcase)
            {
                if (item.StatusD == 3)  //testcase approved
                {
                    count++;
                }

                if (item.StatusD == 4)  //testcase denied
                {
                    state = 2;
                }
                if (item.StatusD == 5)  //testcase in progress
                {
                    state = 2;
                }

            }

            if (count == temp)
            {
                uatmodel.StatusD = 3;
                
            }

            if (state == 2)
            {
                uatmodel.StatusD = 4;
                
            }
            //if (User.IsInRole("Client"))
            //{
                //SendMail();
            //}
            db.Entry(uatmodel).State = EntityState.Modified;
            db.SaveChanges();
        }
     //[HttpPost]
        
        public ActionResult SendMail(int id)
        {
            EmailModel mail = new EmailModel();
            UATModel uatmodel = db.UAT.Find(id);
            int u_id = uatmodel.Project.UserID;
            
            UserModel usermodel = db.User.Find(u_id);
         
            try
            {
                if (ModelState.IsValid)
                {
                    var username = usermodel.FirstName + " " + usermodel.LastName;
                    mail.Mail_Date = System.DateTime.Now;
                    //mail.AnganwadiId = 1;
                    mail.Mail_From = "adammiller001@gmail.com";
                    mail.Mail_Subject = username +" has changed UAT.";
                    MailMessage message = new MailMessage();
                    message.From = new MailAddress(mail.Mail_From);
                    message.To.Add(new MailAddress("lanceugalde@gmail.com"));
                    message.Subject = mail.Mail_Subject;
                    var statusId = uatmodel.StatusD;
                    var statusName = uatmodel.Status.StatusType;
                    //message.Body = mail.Mail_Contents;
                    message.Body = "Hello Admin, \n\n" + username + ", has changed UAT " + uatmodel.Name + "'s status to " + statusName + ". Please click here. http://localhost:1865/Project/ViewUATs/" + uatmodel.UATID + "  \n\n\n Do not reply on this email. This is only for notification purpose. Good Day!\n\n Sent at:" + DateTime.Now;

                    //message.Body = "Hello, " + usermodel.FirstName + " " + usermodel.LastName + ". A new UAT is available for you to view. Please check it out.";
                    SmtpClient client = new SmtpClient();
                    client.Send(message);
                    //return View("Thanks", mail);
                    ViewBag.name = uatmodel.Name;
                    //uatmodel.StatusD = 1;
                    db.Entry(uatmodel).State = EntityState.Modified;
                    db.SaveChanges();
                    return Redirect("/Project/ViewUATs/" + uatmodel.ProjectID);
                }
                else
                {
                    return View(uatmodel);
                }
            }
            catch (Exception e)
            {
                return View(e);
            }
        }
    }
}
