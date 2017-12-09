using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication4.Controllers
{
    public class userController : Controller
    {
        // GET: user
        public ActionResult Index()
        {
            using (BankEntities db = new BankEntities())
            {
                return View(db.SystemUser1.ToList());
            }  
        }

        public ActionResult AddUser()
        {
            return View();
        }

       [HttpPost]
        public ActionResult AddUser(SystemUser1 User) {
            if (ModelState.IsValid) {
                CustomerController customer = new CustomerController();
                customer.Post(User);
                ModelState.Clear();
                ViewBag.message = "the user was added";
            }
            return View();
        }
        public ActionResult login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult login(SystemUser1 user1)
        {
            using(BankEntities db= new BankEntities())
            {
                var myUser = db.SystemUser1.Single(u => u.ID == user1.ID && u.Pasword == user1.Pasword); 
                if(myUser != null)
                {
                    Session["userID"] = myUser.ID.ToString();
                    Session["userType"] = myUser.UserType.ToString();
                    return RedirectToAction("Loggedin");
                }
                else
                {
                    ModelState.AddModelError("", " this user dosent exist");
                }
            }
            return View();
        }

        public ActionResult loggedin()
        {
            if (Session["userType"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("login");
            }
        }
    }
}