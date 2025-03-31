using ExpertSolution.Extensions;
using ExpertSolution.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using System.Diagnostics;
using System.Reflection;

namespace ExpertSolution.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMailService _mailService;

        public HomeController(ILogger<HomeController> logger, IMailService mailService)
        {
            _logger = logger;
            _mailService = mailService;
        }
        public IActionResult Index()
        {
            UserMainInformation obj = UserInfo;
            if (obj == null || obj.Id <= 0)
            {
                return new RedirectResult("~/account/landing");
            }
            //testSendMail(obj, "Registertaion.html");
            if (obj.UserType == 2)
                return new RedirectResult("~/ApplicationFormManage/Dashboard");
            else if (obj.UserType == 3)
                return new RedirectResult("~/Evaluation");
            //else if (obj.UserType ==1 && string.IsNullOrEmpty(obj.WorkOn))
            //    return new RedirectResult("~/ApplicationForm/About");
            else
                return new RedirectResult("~/ApplicationForm");
        }

        private void testSendMail(UserMainInformation model, string FileName)
        {
            string attachPath = "EmailTemplate/" + FileName;
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", attachPath);
            var fileContents = System.IO.File.ReadAllText(path);
            fileContents = EmailSMSHelper.ReplaceTemplateFields(model, fileContents);
            MailRequest request = new MailRequest();
            request.Subject = "التسجيل فى مبادرة اعتماد الخبراء  في مجال التواصل والعلاقات العامة";
            request.Body = fileContents;
            request.ToEmail = "yahmed@arado.org ";

            _mailService.SendEmailAsync(request);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}