using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Web;
using Newtonsoft.Json;
using Shared.Model;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Http.Extensions;

namespace ExpertSolution.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IUserMainInformationDAO _UserDAO;
        private readonly IMailService _mailService;
        public AccountController(IUserMainInformationDAO userDAO, IMailService mailService)
        {
            _UserDAO = userDAO;
            _mailService = mailService;
        }
        public IActionResult Login()
        {
            ClearSavedInformation();
            return View();
        }
        public ActionResult ChangePassword()
        {
            return View();
        }
        public ActionResult Landing()
        {
            return View();
        }
        public ActionResult Terms()
        {
            return View();
        }
        public ActionResult LoginByUserNamePassword(UserMainInformation UserMainInformation)
        {


            var dic = new Dictionary<string, object>();
            UserMainInformation userobj = _UserDAO.GetByUserNameAndPassword(UserMainInformation.Email, UserMainInformation.Password);
            if (userobj.Id > 0)
            {
                dic.Add("Valid", true);
                //dic.Add("User_Type", userobj.User_Type);
                setloginUser(userobj);
                //if (userobj.User_Type == (int)User_Type.Admin)
                //    return new RedirectResult("~/admin");
            }
            else
                dic.Add("Valid", false);
            return Json(Utility.GetExtendedJsonResult(dic));
        }
        public ActionResult AccountSave(UserMainInformation UserMainInformation)
        {
            var dic = new Dictionary<string, object>();
            UserMainInformation userobj = _UserDAO.GetByEmail(UserMainInformation.Email);
            if (userobj.Id > 0)
            {
                dic.Add("Valid", false);
            }
            else
            {
                try
                {
                    UserMainInformation.GUID = Guid.NewGuid().ToString();
                    UserMainInformation.UserType = 1;
                    int id = _UserDAO.Save(UserMainInformation);
                    dic.Add("Valid", true);
                    dic.Add("Id", id);
                    UserMainInformation.Id = id;
                    SendEmailNewUser(UserMainInformation);

                }
                catch (Exception ex)
                {

                    dic.Add("Valid", false);
                    dic.Add("ServiceError", ex.Message);
                }
            }
            return Json(Utility.GetExtendedJsonResult(dic));


        }
        private void SendEmailNewUser(UserMainInformation model)
        {
            string attachPath = "EmailTemplate/Registertaion.html";
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", attachPath);
            var fileContents = System.IO.File.ReadAllText(path);
            fileContents = EmailSMSHelper.ReplaceTemplateFields(model, fileContents);
            MailRequest request = new MailRequest();
            request.Subject = "التسجيل فى مبادرة اعتماد الخبراء  في مجال التواصل والعلاقات العامة";
            request.Body = fileContents;
            request.ToEmail = model.Email;
            _mailService.SendEmailAsync(request);
        }
        private void SendEmailForgetPassword(UserMainInformation model, string guid)
        {
            string attachPath = "EmailTemplate/ForgetPassword.html";
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", attachPath);
            var fileContents = System.IO.File.ReadAllText(path);
            string URL =Request.Scheme +":/"+ Request.Host.ToUriComponent()+ "/Account/ForgetPassword/" + guid;
            fileContents = EmailSMSHelper.ReplaceTemplateFields(model, fileContents,URL);
            MailRequest request = new MailRequest();
            request.Subject = "استعادة كلمة المرور";
            request.Body = fileContents;
            request.ToEmail = model.Email;
            _mailService.SendEmailAsync(request);
        }
        public ActionResult UpdatePassword(string OldPassword = "", string NewPassword = "")
        {
            var dic = new Dictionary<string, object>();
            int Rid = _UserDAO.UpdatePassword(OldPassword, NewPassword, UserInfo.GUID);
            if (Rid > 0)
            {
                dic.Add("Valid", true);
            }
            else
            {
                dic.Add("Valid", false);

            }
            return Json(Utility.GetExtendedJsonResult(dic));

        }
        public ActionResult ReterivePasswordByEmail(string Email)
        {
            var dic = new Dictionary<string, object>();
            string guid = Guid.NewGuid().ToString();

            int Rid = _UserDAO.ReterivePasswordByEmail(Email,guid);
            if (Rid > 0)
            {
                UserMainInformation userobj = _UserDAO.GetByEmail(Email);
                SendEmailForgetPassword(userobj, guid);

                dic.Add("Valid", true);
            }
            else
            {
                dic.Add("Valid", false);

            }
            return Json(Utility.GetExtendedJsonResult(dic));

        }
        void setloginUser(UserMainInformation userobj)
        {
            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddDays(1);
            String seralizeSession = JsonConvert.SerializeObject(userobj);
            Response.Cookies.Append("ExpertUserInfo", HttpUtility.UrlEncode(seralizeSession), option);
        }
        public ActionResult SignOut(string type = "")
        {
            ClearSavedInformation();
            return new RedirectResult("~/account/Login");

        }
        private void ClearSavedInformation()
        {
            //HttpContext.Session.Clear();
            foreach (var cookie in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookie);
            }
        }
        public IActionResult Register()
        {
            return View();
        }
        public IActionResult ForgetPassword(string id = "")
        {
            ViewData["ViewMode"] = "";
            if (!string.IsNullOrEmpty(id))
            {
                UserMainInformation userobj = _UserDAO.GetByActivateKey(id);
                if (userobj.Id > 0)
                {
                    ViewData["ViewMode"] = id;
                }
                else
                {
                    ViewData["ViewMode"] = "Error";
                }

            }

            return View();
        }
        public ActionResult UpdatePasswordByActivateKey(string ActivateKey, string NewPassword)
        {
            var dic = new Dictionary<string, object>();
            int Rid = _UserDAO.UpdatePasswordByActivateKey(ActivateKey, NewPassword);
            if (Rid > 0)
            {
                dic.Add("Valid", true);
            }
            else
            {
                dic.Add("Valid", false);

            }
            return Json(Utility.GetExtendedJsonResult(dic));

        }
    }
}
