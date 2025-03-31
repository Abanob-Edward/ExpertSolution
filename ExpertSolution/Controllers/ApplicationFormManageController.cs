using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Mail;
using System.Resources;

namespace ExpertSolution.Controllers
{
    public class ApplicationFormManageController : BaseController
    {
        private readonly IApplicationFormDAO _ApplicationFormDAO;
        private readonly IEvaluationDAO _evaluationDAO;

        public ApplicationFormManageController(IApplicationFormDAO applicationFormDAO, IEvaluationDAO evaluationDAO)
        {
            _ApplicationFormDAO = applicationFormDAO;
            _evaluationDAO = evaluationDAO;
        }
        

        public IActionResult Index(ApplicationFormFilter model)
        {
            UserMainInformation obj = UserInfo;
            if (obj == null || obj.Id <= 0)
            {
                return new RedirectResult("~/account/Login");
            }
            List <ApplicationFormUI> lst = _ApplicationFormDAO.GetAllApplicationForms(model);
            return View(lst);
        }
        public IActionResult EvaluationResult(int evaluatorGroupId)
        {
            UserMainInformation obj = UserInfo;
            if (obj == null || obj.Id <= 0)
            {
                return new RedirectResult("~/account/Login");
            }
            List<ApplicationFormUI> lst = _evaluationDAO.GetByEvaluationGroup(evaluatorGroupId);
            return View(lst);
        }
        public IActionResult CountryLst()
        {
            UserMainInformation obj = UserInfo;
            if (obj == null || obj.Id <= 0)
            {
                return new RedirectResult("~/account/Login");
            }
            List<ApplicationFormCount> lst = _ApplicationFormDAO.GetApplicationFormCountbyCountry();
            return View(lst);
        }
        public IActionResult DashBoard()
        {
            UserMainInformation obj = UserInfo;
            if (obj == null || obj.Id <= 0)
            {
                return new RedirectResult("~/account/Login");
            }
            return View();
        }
        

    }
}
