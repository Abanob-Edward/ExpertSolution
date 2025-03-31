using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using static Dapper.SqlMapper;

namespace ExpertSolution.Controllers
{
    public class ApplicationFormController : BaseController
    {
        private readonly IApplicationFormDAO _ApplicationFormDAO;
        private readonly IMailService _mailService;
        private readonly ConfigConstants _configConstants;
        private readonly IFormsDAO _FormsDAO;
        private readonly ICommonDAO _CommonDAO;

        public ApplicationFormController(IApplicationFormDAO applicationFormDAO, IFormsDAO formsDAO, ICommonDAO commonDAO,IMailService mailService, IOptions<ConfigConstants> configConstants)
        {
            _ApplicationFormDAO = applicationFormDAO;
            _mailService = mailService;
            _configConstants = configConstants.Value;
            _FormsDAO = formsDAO;
            _CommonDAO = commonDAO;

        }
        public IActionResult Index(string id = "", string evaluator = "",string action="")
        {
            UserMainInformation obj = UserInfo;
            if (obj == null || obj.Id <= 0)
            {
                return new RedirectResult("~/account/Login");
            }
            ViewBag.Id = id;
            ViewBag.Evaluator = evaluator;
            if (!string.IsNullOrEmpty(id) && obj.UserType == 1)
            {
                return new RedirectResult("~/account/Login");
            }
            //if (string.IsNullOrEmpty(obj.WorkOn) && action.ToLower() != "index")
            //{
            //    return View("About");
            //}
            return View();
        }
        public IActionResult About()
        {
            return View();
        }
        public ActionResult SaveMainInformation(UserMainInformation UserMainInformation)
        {
            var dic = new Dictionary<string, object>();

            try
            {
                int id = _ApplicationFormDAO.SaveMainInformation(UserMainInformation);
                if (id > 0)
                {
                    dic.Add("Valid", true);
                }
                else
                {
                    dic.Add("Valid", false);
                }

            }
            catch (Exception ex)
            {

                dic.Add("Valid", false);
                dic.Add("ServiceError", ex.Message);
            }
            return Json(Utility.GetExtendedJsonResult(dic));


        }
        #region QuastionsYesOrNo
        [HttpGet]
        public ActionResult GetExpertQuestionAnswers(string expertGUID)
        {
            var dic = new Dictionary<string, object>();

            try
            {
                if (string.IsNullOrEmpty(expertGUID))
                {
                    dic.Add("Valid", false);
                    dic.Add("Message", "ExpertGUID is required.");
                    return Json(Utility.GetExtendedJsonResult(dic));
                }

                List<ExpertQuestionAnswer> answers = _FormsDAO.getExpertQuestionAnswers(expertGUID);

                if (answers.Count > 0)
                {
                    dic.Add("Valid", true);
                    dic.Add("result", answers);
                }
                else
                {
                    dic.Add("Valid", false);
                }
            }
            catch (Exception ex)
            {
                dic.Add("Valid", false);
                dic.Add("ServiceError", ex.Message);
            }

            return Json(Utility.GetExtendedJsonResult(dic));
        }


        [HttpPost]
        public ActionResult SaveExpertQuestionAnswers([FromBody] List<ExpertQuestionAnswer> Answers)
        {
            var dic = new Dictionary<string, object>();

            try
            {
                int id = _FormsDAO.SaveExpertQuestionAnswers(Answers);
                if (id > 0)
                {
                    dic.Add("Valid", true);
                    dic.Add("id", id);
                }
                else
                {
                    dic.Add("Valid", false);
                }
            }
            catch (Exception ex)
            {

                dic.Add("Valid", false);
                dic.Add("ServiceError", ex.Message);
            }
            return Json(Utility.GetExtendedJsonResult(dic));


        }
        #endregion

        #region Professionalqualifications
        public ActionResult SaveProfessionalqualifications(ProfessionalQualifications entity)
        {
            var dic = new Dictionary<string, object>();

            try
            {
                int id = _FormsDAO.SaveProfessionalQualifications(entity);
                if (id > 0)
                {
                    dic.Add("Valid", true);
                    dic.Add("id", id);
                }
                else
                {
                    dic.Add("Valid", false);
                }

            }
            catch (Exception ex)
            {

                dic.Add("Valid", false);
                dic.Add("ServiceError", ex.Message);
            }
            return Json(Utility.GetExtendedJsonResult(dic));


        }
        public ActionResult GetALLProfessionalQualifications(ProfessionalQualifications entity)
        {
            var dic = new Dictionary<string, object>();

            try
            {
                List<ProfessionalQualifications> lst = _FormsDAO.GetALLProfessionalQualifications(entity.UserGUID);

                if (lst.Count > 0)
                {
                    dic.Add("Valid", true);
                    dic.Add("result", lst);
                }
                else
                {
                    dic.Add("Valid", false);
                }

            }
            catch (Exception ex)
            {

                dic.Add("Valid", false);
                dic.Add("ServiceError", ex.Message);
            }
            return Json(Utility.GetExtendedJsonResult(dic));


        }
        public ActionResult GetProfessionalqualificationsById(ProfessionalQualifications entity)
        {
            var dic = new Dictionary<string, object>();

            try
            {
                ProfessionalQualifications obj = _FormsDAO.GetByIdProfessionalQualifications(entity.Id);
                dic.Add("Valid", true);
                dic.Add("result", obj);


            }
            catch (Exception ex)
            {

                dic.Add("Valid", false);
                dic.Add("ServiceError", ex.Message);
            }
            return Json(Utility.GetExtendedJsonResult(dic));


        }
        public ActionResult DeleteProfessionalqualifications(ProfessionalQualifications entity)
        {
            var dic = new Dictionary<string, object>();

            try
            {
                int x = _FormsDAO.DeleteProfessionalQualifications(entity.Id);
                dic.Add("Valid", true);


            }
            catch (Exception ex)
            {

                dic.Add("Valid", false);
                dic.Add("ServiceError", ex.Message);
            }
            return Json(Utility.GetExtendedJsonResult(dic));


        }
        #endregion

        #region AcademicQualifications
        public ActionResult SaveAcademicQualifications(AcademicQualifications entity)
        {
            var dic = new Dictionary<string, object>();

            try
            {
                int id = _FormsDAO.SaveAcademicQualifications(entity);
                if (id > 0)
                {
                    dic.Add("Valid", true);
                    dic.Add("id", id);
                }
                else
                {
                    dic.Add("Valid", false);
                }

            }
            catch (Exception ex)
            {

                dic.Add("Valid", false);
                dic.Add("ServiceError", ex.Message);
            }
            return Json(Utility.GetExtendedJsonResult(dic));


        }
        public ActionResult GetALLAcademicQualifications(AcademicQualifications entity)
        {
            var dic = new Dictionary<string, object>();

            try
            {
                List<AcademicQualifications> lst = _FormsDAO.GetALLAcademicQualifications(entity.UserGUID);

                if (lst.Count > 0)
                {
                    dic.Add("Valid", true);
                    dic.Add("result", lst);
                }
                else
                {
                    dic.Add("Valid", false);
                }

            }
            catch (Exception ex)
            {

                dic.Add("Valid", false);
                dic.Add("ServiceError", ex.Message);
            }
            return Json(Utility.GetExtendedJsonResult(dic));


        }
        public ActionResult GetAcademicQualificationsById(AcademicQualifications entity)
        {
            var dic = new Dictionary<string, object>();

            try
            {
                AcademicQualifications obj = _FormsDAO.GetByIdAcademicQualifications(entity.Id);
                dic.Add("Valid", true);
                dic.Add("result", obj);


            }
            catch (Exception ex)
            {

                dic.Add("Valid", false);
                dic.Add("ServiceError", ex.Message);
            }
            return Json(Utility.GetExtendedJsonResult(dic));


        }
        public ActionResult DeleteAcademicQualifications(AcademicQualifications entity)
        {
            var dic = new Dictionary<string, object>();

            try
            {
                int x = _FormsDAO.DeleteAcademicQualifications(entity.Id);
                dic.Add("Valid", true);


            }
            catch (Exception ex)
            {

                dic.Add("Valid", false);
                dic.Add("ServiceError", ex.Message);
            }
            return Json(Utility.GetExtendedJsonResult(dic));


        }
        #endregion

        #region CertificateQualifications
        public ActionResult SaveCertificateQualifications(CertificateQualifications entity)
        {
            var dic = new Dictionary<string, object>();

            try
            {
                int id = _FormsDAO.SaveCertificateQualifications(entity);
                if (id > 0)
                {
                    dic.Add("Valid", true);
                    dic.Add("id", id);
                }
                else
                {
                    dic.Add("Valid", false);
                }

            }
            catch (Exception ex)
            {

                dic.Add("Valid", false);
                dic.Add("ServiceError", ex.Message);
            }
            return Json(Utility.GetExtendedJsonResult(dic));


        }
        public ActionResult GetALLCertificateQualifications(CertificateQualifications entity)
        {
            var dic = new Dictionary<string, object>();

            try
            {
                List<CertificateQualifications> lst = _FormsDAO.GetALLCertificateQualifications(entity.UserGUID);

                if (lst.Count > 0)
                {
                    dic.Add("Valid", true);
                    dic.Add("result", lst);
                }
                else
                {
                    dic.Add("Valid", false);
                }

            }
            catch (Exception ex)
            {

                dic.Add("Valid", false);
                dic.Add("ServiceError", ex.Message);
            }
            return Json(Utility.GetExtendedJsonResult(dic));


        }
        public ActionResult GetCertificateQualificationsById(CertificateQualifications entity)
        {
            var dic = new Dictionary<string, object>();

            try
            {
                CertificateQualifications obj = _FormsDAO.GetByIdCertificateQualifications(entity.Id);
                dic.Add("Valid", true);
                dic.Add("result", obj);


            }
            catch (Exception ex)
            {

                dic.Add("Valid", false);
                dic.Add("ServiceError", ex.Message);
            }
            return Json(Utility.GetExtendedJsonResult(dic));


        }
        public ActionResult DeleteCertificateQualifications(CertificateQualifications entity)
        {
            var dic = new Dictionary<string, object>();

            try
            {
                int x = _FormsDAO.DeleteCertificateQualifications(entity.Id);
                dic.Add("Valid", true);


            }
            catch (Exception ex)
            {

                dic.Add("Valid", false);
                dic.Add("ServiceError", ex.Message);
            }
            return Json(Utility.GetExtendedJsonResult(dic));


        }
        #endregion

        #region Table Lookup
        public ActionResult CreateTableRow(TableForms entity)
        {
            var dic = new Dictionary<string, object>();

            try
            {
                int id = _FormsDAO.CreateTableRow(entity);
                if (id > 0)
                {
                    dic.Add("Valid", true);
                    dic.Add("id", id);
                }
                else
                {
                    dic.Add("Valid", false);
                }

            }
            catch (Exception ex)
            {

                dic.Add("Valid", false);
                dic.Add("ServiceError", ex.Message);
            }
            return Json(Utility.GetExtendedJsonResult(dic));


        }
        public ActionResult DeleteTableRow(TableForms entity)
        {
            var dic = new Dictionary<string, object>();

            try
            {
                int x = _FormsDAO.DeleteTableRow(entity);
                dic.Add("Valid", true);


            }
            catch (Exception ex)
            {

                dic.Add("Valid", false);
                dic.Add("ServiceError", ex.Message);
            }
            return Json(Utility.GetExtendedJsonResult(dic));


        }
        public ActionResult UpdateTableField(TableForms entity)
        {
            var dic = new Dictionary<string, object>();

            try
            {
                int id = _FormsDAO.UpdateTableField(entity);
                if (id > 0)
                {
                    dic.Add("Valid", true);
                    dic.Add("id", id);
                }
                else
                {
                    dic.Add("Valid", false);
                }

            }
            catch (Exception ex)
            {

                dic.Add("Valid", false);
                dic.Add("ServiceError", ex.Message);
            }
            return Json(Utility.GetExtendedJsonResult(dic));


        }
        public ActionResult GetLookup(string TableName)
        {
            var dic = new Dictionary<string, object>();
            List<Lookup> lst = new List<Lookup>();
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString(TableName)))
                {
                    lst = _CommonDAO.GetLookup(TableName);
                    HttpContext.Session.SetString(TableName, JsonSerializer.Serialize(lst));
                }
                else
                {
                    lst = JsonSerializer.Deserialize<List<Lookup>>(HttpContext.Session.GetString(TableName));
                }


                if (lst.Count > 0)
                {
                    dic.Add("Valid", true);
                    dic.Add("result", lst);
                }
                else
                {
                    dic.Add("Valid", false);
                }

            }
            catch (Exception ex)
            {

                dic.Add("Valid", false);
                dic.Add("ServiceError", ex.Message);
            }
            return Json(Utility.GetExtendedJsonResult(dic));


        }
        #endregion Table Lookup




        public ActionResult SaveApplicationFormService(List<ApplicationFormService> servicearray)
        {
            var dic = new Dictionary<string, object>();

            try
            {
                int id = _ApplicationFormDAO.SaveApplicationFormService(servicearray);
                if (id > 0)
                {
                    dic.Add("Valid", true);
                }
                else
                {
                    dic.Add("Valid", false);
                }

            }
            catch (Exception ex)
            {

                dic.Add("Valid", false);
                dic.Add("ServiceError", ex.Message);
            }
            return Json(Utility.GetExtendedJsonResult(dic));


        }
        public ActionResult SaveApplicationEvaluation(List<ApplicationFormEvaluation> Evalarray)
        {
            var dic = new Dictionary<string, object>();

            try
            {
                int id = _ApplicationFormDAO.SaveApplicationEvaluation(Evalarray);
                if (id > 0)
                {
                    dic.Add("Valid", true);
                }
                else
                {
                    dic.Add("Valid", false);
                }

            }
            catch (Exception ex)
            {

                dic.Add("Valid", false);
                dic.Add("ServiceError", ex.Message);
            }
            return Json(Utility.GetExtendedJsonResult(dic));


        }
        [HttpPost]
        public async Task<IActionResult> SaveAttach()
        {
            AttachmentUI attchObj = new AttachmentUI();
            var dic = new Dictionary<string, object>();
            string[] extensions = new string[] { ".png", ".jpg", ".jpeg", ".doc", ".docx", ".pdf", ".xls", ".xlsx", ".ppt", ".pptx", ".gif" };
            string FileName = "";
            if (Request.Form.Files.Count > 0)
            {
                try
                {
                    foreach (var file in Request.Form.Files)
                    {
                        string file_ext = file.FileName.ToLower().Substring(file.FileName.LastIndexOf("."));
                        if (extensions.Contains(file_ext))
                        {
                            string[] paramArray = file.Name.Split('#');
                            FileName = (Guid.NewGuid()).ToString() + file_ext;
                            attchObj.UserGUID = paramArray[0];
                            attchObj.Code = paramArray[1];
                            attchObj.RealtedId = int.Parse(paramArray[2]);
                            attchObj.Name = FileName;
                            attchObj.DisplayName = file.FileName;
                            string attachPath = "Uploads/" + FileName;
                            int id = _ApplicationFormDAO.SaveAttachment(attchObj);
                            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", attachPath);
                            using (var stream = new FileStream(path, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }
                            dic.Add("Valid", true);
                            dic.Add("Entity", attchObj);
                            dic.Add("AttachPath", attachPath);
                        }
                    }
                }
                catch //(Exception ex)
                {
                    dic.Add("Valid", false);
                    dic.Add("ErrorMessage", "تعذر إضافة الملف يرجى إضافة ملف أخر");
                    return Json(Utility.GetExtendedJsonResult(dic));
                }

            }
            else
            {
                dic.Add("Valid", false);
            }
            return Json(Utility.GetExtendedJsonResult(dic));

        }

        [HttpPost]
        public ActionResult ApplicationFormChangeStatus(string UserGUID, string Status)
        {
            var dic = new Dictionary<string, object>();
            int id = _ApplicationFormDAO.UpdateStatus(UserGUID, Status);
            if (id <= 0)
            {
                dic.Add("Valid", false);
            }
            else
            {
                try
                {
                    dic.Add("Valid", true);
                    if (Status == "Finished")
                    {
                        SendEmailFinishApplication();
                        string paymentId = SavePaymentTransaction();
                        dic.Add("paymentId", paymentId);
                        //if (!string.IsNullOrEmpty(url))
                        //  return Redirect(url);

                    }
                }
                catch (Exception ex)
                {
                    dic.Add("Valid", false);
                    dic.Add("ServiceError", ex.Message);
                }
            }
            return Json(Utility.GetExtendedJsonResult(dic));

        }
        [HttpPost]
        public ActionResult ChangeGroup(string UserGUID, int EvaluatorGroupId)
        {
            var dic = new Dictionary<string, object>();
            try
            {
                int id = _ApplicationFormDAO.ChangeGroup(UserGUID, EvaluatorGroupId);
                if (id <= 0)
                    dic.Add("Valid", false);
                else
                    dic.Add("Valid", true);
            }
            catch (Exception ex)
            {
                dic.Add("Valid", false);
                dic.Add("ServiceError", ex.Message);
            }
            return Json(Utility.GetExtendedJsonResult(dic));
        }

        private void SendEmailFinishApplication()
        {
            string attachPath = "EmailTemplate/FinishApplication.html";
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", attachPath);
            var fileContents = System.IO.File.ReadAllText(path);
            fileContents = EmailSMSHelper.ReplaceTemplateFields(UserInfo, fileContents);
            MailRequest request = new MailRequest();
            request.Subject = "انهاء استمارة التسجيل";
            request.Body = fileContents;
            request.ToEmail = UserInfo.Email;
            _mailService.SendEmailAsync(request);
        }
        private string SavePaymentTransaction()
        {
            PaymentModel modelPaymentModel = new PaymentModel();
            modelPaymentModel.ActivityName = "مبادرة اعتماد الخبراء  في مجال التواصل والعلاقات العامة";
            modelPaymentModel.ActivityCode = "0";
            modelPaymentModel.ActivitySource = "AradoWebSite";
            modelPaymentModel.bill_to_email = UserInfo.Email;
            modelPaymentModel.customer_lastname = UserInfo.ContactFullName;
            modelPaymentModel.amount = float.Parse(_configConstants.Fees);
            modelPaymentModel.currency = "USD";
            modelPaymentModel.ccEmail = UserInfo.Email;

            int idPaymentModel = new PaymentgatewayDAO().Save(modelPaymentModel);
            return idPaymentModel.ToString();
        }

    }
}
