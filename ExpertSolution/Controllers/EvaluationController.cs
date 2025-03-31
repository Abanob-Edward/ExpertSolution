using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Web;
using Newtonsoft.Json;
using Shared.Model;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Http.Extensions;
using System.Reflection;
using DAO.Interfaces;
using DAO;

namespace ExpertSolution.Controllers
{
    public class EvaluationController : BaseController
    {
        private readonly IUserMainInformationDAO _UserDAO;
        private readonly IMailService _mailService;
        private readonly IApplicationFormDAO _ApplicationFormDAO;
        private readonly IEvaluationDAO _evaluationDAO;
        public EvaluationController(IUserMainInformationDAO userDAO, IMailService mailService, IEvaluationDAO evaluationDAO, IApplicationFormDAO applicationFormDAO)
        {
            _UserDAO = userDAO;
            _mailService = mailService;
            _evaluationDAO = evaluationDAO;
            _ApplicationFormDAO = applicationFormDAO;
        }

        public ActionResult AccountSave(UserMainInformation UserMainInformation)
        {
            var dic = new Dictionary<string, object>();
            int id = 0;
            if (string.IsNullOrEmpty(UserMainInformation.GUID))
            {
                UserMainInformation userobj = _UserDAO.GetByEmail(UserMainInformation.Email);
                if (userobj.Id > 0)
                {
                    dic.Add("Valid", false);
                    return Json(Utility.GetExtendedJsonResult(dic));
                }
            }

            try
            {
                if (string.IsNullOrEmpty(UserMainInformation.GUID))
                {
                    UserMainInformation.GUID = Guid.NewGuid().ToString();
                    UserMainInformation.UserType = 3;
                    id = _UserDAO.SaveEvaluator(UserMainInformation, false);
                    SendEmailNewUser(UserMainInformation);
                }
                else
                {
                    id = _UserDAO.SaveEvaluator(UserMainInformation, true);
                }
                dic.Add("Valid", true);
                dic.Add("Id", id);
                UserMainInformation.Id = id;
            }
            catch (Exception ex)
            {

                dic.Add("Valid", false);
                dic.Add("ServiceError", ex.Message);
            }

            return Json(Utility.GetExtendedJsonResult(dic));


        }
        private void SendEmailNewUser(UserMainInformation model)
        {
            string attachPath = "EmailTemplate/RegistertaionEvaluator.html";
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", attachPath);
            var fileContents = System.IO.File.ReadAllText(path);
            fileContents = EmailSMSHelper.ReplaceTemplateFields(model, fileContents);
            MailRequest request = new MailRequest();
            request.Subject = " مبادرة اعتماد الخبراء  في مجال التواصل والعلاقات العامة";
            request.Body = fileContents;
            request.ToEmail = model.Email;
            _mailService.SendEmailAsync(request);
        }


        public IActionResult AddEvaluator(string Id = "")
        {
            UserMainInformation model = new UserMainInformation();
            if (!string.IsNullOrEmpty(Id))
            {
                model = _UserDAO.GetByGuid(Id);
            }

            return View(model);
        }
        public IActionResult Index()
        {
            return new RedirectResult("~/Evaluation/ApplicationList");
           // return View();
        }
        public IActionResult List()
        {
            UserMainInformation obj = UserInfo;
            if (obj == null || obj.Id <= 0)
            {
                return new RedirectResult("~/account/Login");
            }
            List<ApplicationFormUI> lst = _evaluationDAO.GetAll();
            return View(lst);
        }
        public IActionResult ApplicationList(ApplicationFormFilter model)
        {
            UserMainInformation obj = UserInfo;
            if (obj == null || obj.Id <= 0)
            {
                return new RedirectResult("~/account/Login");
            }
            model.EvaluatorGroupId = obj.EvaluatorGroupId;
            List<ApplicationFormUI> lst = _ApplicationFormDAO.GetEvaluationApplicationForms(model,obj);
            return View(lst);
        }
        public ActionResult GetEvaluationDegree(ApplicationEvaluation obj)
        {
            var dic = new Dictionary<string, object>();
            try
            {

                obj.ApplicationUserGUID = UserInfo.GUID;
                var result = _evaluationDAO.GetEvaluationDegree(obj);
                dic.Add("result", result);
                dic.Add("Valid", true);
            }
            catch (Exception ex)
            {

                dic.Add("Valid", false);
                dic.Add("ServiceError", ex.Message);
            }

            return Json(Utility.GetExtendedJsonResult(dic));


        }
        public ActionResult SaveEvaluationDegree(ApplicationEvaluation obj)
        {
            var dic = new Dictionary<string, object>();
            try
            {

                int id = 0;
                obj.ApplicationUserGUID = UserInfo.GUID;
                _evaluationDAO.SaveEvaluationDegree(obj);

                dic.Add("Valid", true);
                dic.Add("Id", id);
            }
            catch (Exception ex)
            {

                dic.Add("Valid", false);
                dic.Add("ServiceError", ex.Message);
            }

            return Json(Utility.GetExtendedJsonResult(dic));


        }
        public ActionResult FinishEvaluationDegree(ApplicationEvaluation obj)
        {
            var dic = new Dictionary<string, object>();
            try
            {

                int id = 0;
                obj.ApplicationUserGUID = UserInfo.GUID;
               var result =  _evaluationDAO.FinishEvaluationDegree(obj);

                dic.Add("Valid", result);
                dic.Add("Id", id);
            }
            catch (Exception ex)
            {

                dic.Add("Valid", false);
                dic.Add("ServiceError", ex.Message);
            }

            return Json(Utility.GetExtendedJsonResult(dic));


        }
    }
}
