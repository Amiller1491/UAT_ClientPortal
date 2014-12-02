using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Web.Mvc;
using Final.Models;

namespace Final.Controllers
{
    [Authorize]
    public class TestCaseController : Controller
    {
        private UATContext db = new UATContext();

        //
        // GET: /TestCase/
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var testcase = db.TestCase.Include(t => t.UAT).Include(t => t.Status);
            return View(testcase.ToList());
        }

        //
        // GET: /TestCase/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int id = 0)
        {
            TestCaseModel testcasemodel = db.TestCase.Find(id);
            if (testcasemodel == null)
            {
                return HttpNotFound();
            }
            return View(testcasemodel);
        }

        //
        // GET: /TestCase/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.UATID = new SelectList(db.UAT, "UATID", "Name");
            ViewBag.StatusD = new SelectList(db.Status, "StatusD", "StatusType");
            return View();
        }

        //
        // POST: /TestCase/Create
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TestCaseModel testcasemodel)
        {
            if (ModelState.IsValid)
            {
                db.TestCase.Add(testcasemodel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UATID = new SelectList(db.UAT, "UATID", "Name", testcasemodel.UATID);
            ViewBag.StatusD = new SelectList(db.Status, "StatusD", "StatusType", testcasemodel.StatusD);
            return View(testcasemodel);
        }

        //
        // GET: /TestCase/Edit/5
        
        public ActionResult Edit(int id = 0)
        {
            TestCaseModel testcasemodel = db.TestCase.Find(id);
            if (testcasemodel == null)
            {
                return HttpNotFound();
            }
            ViewBag.UATID = new SelectList(db.UAT, "UATID", "Name", testcasemodel.UATID);
            ViewBag.StatusD = new SelectList(db.Status, "StatusD", "StatusType", testcasemodel.StatusD);
            return View(testcasemodel);
        }

        //
        // POST: /TestCase/Edit/5
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TestCaseModel testcasemodel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(testcasemodel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UATID = new SelectList(db.UAT, "UATID", "Name", testcasemodel.UATID);
            ViewBag.StatusD = new SelectList(db.Status, "StatusD", "StatusType", testcasemodel.StatusD);
            return View(testcasemodel);
        }
        //
        // GET: /TestCase/ClientEdit/5
        
        public ActionResult ClientEdit(int id = 0)
        {
            TestCaseModel testcasemodel = db.TestCase.Find(id);

            if (testcasemodel == null)
            {
                return HttpNotFound();
            }
            ViewBag.UATID = new SelectList(db.UAT, "UATID", "Name", testcasemodel.UATID);
            ViewBag.StatusD = new SelectList(db.Status, "StatusD", "StatusType", testcasemodel.StatusD);
            return View(testcasemodel);
        }
        //
        // POST: /TestCase/ClientEdit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ClientEdit(TestCaseModel testcasemodel)
        {
            var nullprev = testcasemodel.StatusD;
           UATModel uat = db.UAT.Find(testcasemodel.UATID);
           int u_id = uat.Project.UserID;
           UserModel usermodel = db.User.Find(u_id);
           
           if(User.IsInRole("Client")){
               //SendMail(usermodel);
           }

            if (ModelState.IsValid)
            {
                if (testcasemodel.StatusD == null)
                {
                    testcasemodel.StatusD = 1;
                }
                
                db.Entry(testcasemodel).State = EntityState.Modified;
                db.SaveChanges();
                return Redirect("/UAT/ViewTestCases/"+ testcasemodel.UATID);
            }
            ViewBag.UATID = new SelectList(db.UAT, "UATID", "Name", testcasemodel.UATID);
            ViewBag.StatusD = new SelectList(db.Status, "StatusD", "StatusType", testcasemodel.StatusD);
            return View(testcasemodel);
        }

        //[HttpPost]
        public void SendMail(UserModel usermodel)
        {
            EmailModel mail = new EmailModel();
            //UATModel uatmodel = db.UAT.Find(id);
            //int u_id = uatmodel.Project.UserID;

            //usermodel = db.User.Find(u_id);

            try
            {
                if (ModelState.IsValid)
                {
                    string name = usermodel.FirstName + " " + usermodel.LastName;
                    mail.Mail_Date = System.DateTime.Now;
                    //mail.AnganwadiId = 1;
                    mail.Mail_From = "adammiller001@gmail.com";
                    mail.Mail_Subject = name + " has changed TestCase";
                    mail.Mail_Date = DateTime.Now;
                    MailMessage message = new MailMessage();
                    message.From = new MailAddress(mail.Mail_From);
                    message.To.Add("greenhill10@yahoo.com");
                    message.Subject = mail.Mail_Subject;
                    //message.Body = mail.Mail_Contents;
                    message.Body = "Hello admin, \n\n\n A TestCase has been changed by "  + name + "\n\n\n Do not reply on this email. This is only for notification purpose. Good Day!\n\n Sent at:" + DateTime.Now;

                    //message.Body = "Hello, " + usermodel.FirstName + " " + usermodel.LastName + ". A new UAT is available for you to view. Please check it out.";
                    SmtpClient client = new SmtpClient();
                    client.Send(message);
                    //return View("Thanks", mail);

                }
                else
                {

                }
            }
            catch (Exception e)
            {
                //throw Exception e;
            }
        }

        //
        // GET: /TestCase/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id = 0)
        {
            TestCaseModel testcasemodel = db.TestCase.Find(id);
            if (testcasemodel == null)
            {
                return HttpNotFound();
            }
            return View(testcasemodel);
        }

        //
        // POST: /TestCase/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TestCaseModel testcasemodel = db.TestCase.Find(id);
            db.TestCase.Remove(testcasemodel);
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