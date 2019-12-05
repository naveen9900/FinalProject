using LoginTestDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace LoginTestDemo.Controllers
{
    public class HomeController : Controller
    {
        LoginEntities entity = new LoginEntities();

        public ActionResult HomePage()
        {
            return View();
        }
        // GET: Home
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(tblLogin log, tblSkill data)
        {
                  var obj = entity.tblLogins.Where(a => a.eUsername.Equals(log.eUsername) && a.ePassword.Equals(log.ePassword)).FirstOrDefault();
                if (obj != null)
                {
                    Session["eLoginId"] = obj.eLoginId.ToString();
                    Session["eUsername"] = obj.eUsername.ToString();
                    Session["eFirstName"] = obj.eFirstName.ToString();
                    FormsAuthentication.SetAuthCookie(log.eUsername, false);
                    if (obj.Role == "Manager")
                    {
                        return RedirectToAction("Dashboard_Manager", data);
                    }
                    else if (obj.Role == "Employee")
                    {
                        return RedirectToAction("Dashboard_Employee", data);
                    }

                }

                ModelState.AddModelError("", "Invalid Login Details!");
                return View("Login");
            
        }

        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(tblLogin data)
        {
            if (ModelState.IsValid)
            {
                entity.tblLogins.Add(data);
                entity.SaveChanges();
                return RedirectToAction("Login");
            }
            return View(data);
        }

        public ActionResult User_Data_Entry()
        {

            return View();
        }

        [HttpPost]
        public ActionResult User_Data_Entry(tblSkill data)
        {

            if (Session["eLoginId"] != null)
            {
                entity.tblSkills.Add(data);
                data.skUsername = Session["eUsername"].ToString();
                entity.SaveChanges();

                return RedirectToAction("Dashboard_Employee", data);
            }
            ViewBag.Message = "Something Went Wrong";
            return View();
        }


        public ActionResult Dashboard_Employee(tblSkill data)
        {
            Session["skusername"] = Session["eUsername"];
            ViewBag.Name = Session["eFirstName"].ToString();
            
            List<tblSkill> userdata = entity.tblSkills.Where(x => x.skUsername == data.skUsername).ToList();
            return View(userdata);
        }

        public ActionResult Dashboard_Manager()
        {
            ViewBag.Display = "Manager Dashboard";
            return View();
        }

        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}