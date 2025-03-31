using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.Interfaces
{
    public interface IApplicationFormDAO
    {
        public ApplicationForm GetApplicationInfo(string Id,UserMainInformation userInfo,string evaluator = "");
        public int SaveMainInformation(UserMainInformation entity);
        public int SaveApplicationFormService(List<ApplicationFormService> servicearray);
        public int SaveApplicationEvaluation(List<ApplicationFormEvaluation> servicearray);
        int SaveAttachment(AttachmentUI attchObj);
        int UpdateStatus(string UserGUID, string Status);
        public List<ApplicationFormUI> GetAllApplicationForms(ApplicationFormFilter model);
        public List<ApplicationFormUI> GetEvaluationApplicationForms(ApplicationFormFilter model, UserMainInformation obj);
        public ApplicationFormCount GetApplicationFormCount();
        public List<ApplicationFormCount> GetApplicationFormCountbyCountry();
        public List<ApplicationFormCount> GetApplicationFormCountryTotal();
        public int ChangeGroup(string UserGUID, int EvaluatorGroupId);
    }
}
