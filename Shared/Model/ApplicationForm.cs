using Microsoft.AspNetCore.Http;

namespace Shared.Model
{
    public class ApplicationForm
    {
        public UserMainInformation MainInformation { get; set; }
        public List<ApplicationFormService> ApplicationFormServiceLst { get; set; }
        public List<ApplicationFormSocialLinks> SocialLinksLst { get; set; }
        public List<ApplicationFormLanguage> languageLst { get; set; }
        public List<ApplicationFormComputerSkills> ComSkillsLst { get; set; }
        public List<ApplicationFormEvaluation> ApplicationFormEvaluationLst { get; set; }
        public List<AttachmentUI> AttachmentLst { get; set; }
        public List<ApplicationEvaluation> ApplicationEvaluationLst { get; set; }

    }
    public class ApplicationFormUI : UserMainInformation
    {
        public string CountryName { get; set; }
        public string GroupName { get; set; }
        public string ApplicationFormGUID { get; set; }

    }
    public class ApplicationFormFilter
    {
        public string Status { get; set; }
        public string CountryId { get; set; }
        public int OrganizationSize { get; set; }
        public int EvaluatorGroupId { get; set; }
    }
    public class ApplicationFormCount
    {
        public int OrganizationSize { get; set; }
        public string CountryName { get; set; }
        public string CountryId { get; set; }
        public int Total { get; set; }
        public int InProgress { get; set; }
        public int Finished { get; set; }
        public int Cancelled { get; set; }
        public int Accepted { get; set; }
        public int Paid { get; set; }
        public int TotalMedium { get; set; }
        public int TotalSmall { get; set; }
    }
}
