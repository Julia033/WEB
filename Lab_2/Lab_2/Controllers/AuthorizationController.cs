using Lab_2.Models.ViewModels;
using Lab_2.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Security.Cryptography;
using System.Text;

namespace Lab_2.Controllers
{
    public class AuthorizationController : Controller
    {
        // GET: Authorization
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(UserVM webUser)
        {
            if (ModelState.IsValid)
            {
                using (NazarenkoWEBEntities context = new NazarenkoWEBEntities())
                {
                    User user = null;
                    user = context.Users.Where(u => u.login == webUser.Login).FirstOrDefault();
                    if (user != null)
                    {
                        string passwordHach = ReturnHashCode(webUser.Password + user.salt.ToString().ToUpper());
                        if (passwordHach == user.password_hash)
                        {
                            string user_Role = "";
                            switch (user.user_role)
                            {
                                case 1:
                                    user_Role = "Participant";
                                    break;
                                case 2:
                                    user_Role = "Admin";
                                    break;
                                
                            }

                            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                                1,
                                user.login,
                                DateTime.Now,
                                DateTime.Now.AddDays(1),
                                false,
                                user_Role);
                            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                            HttpContext.Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket));
                            return RedirectToAction("ListOfPeople", "Lab2");
                        }
                    }
                }
            }
            ViewBag.Error = "Пользователя с таким логином и паролем не существует, попробуйте еще раз";
            return View(webUser);
        }

        string ReturnHashCode(string loginAndSalt)
        {
            string hash = "";
            using (SHA1 sha1Hash = SHA1.Create())
            {
                byte[] data = sha1Hash.ComputeHash(Encoding.UTF8.GetBytes(loginAndSalt));
                StringBuilder sBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
                hash = sBuilder.ToString().ToUpper();
            }
            return hash;
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("ListOfPeople", "Lab2");
        }

    }
}