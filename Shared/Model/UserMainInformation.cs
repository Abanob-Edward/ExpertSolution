using Microsoft.AspNetCore.Http;

namespace Shared.Model
{
    public class UserMainInformation
    {
        public int Id { get; set; }
        public int UserType { get; set; }
        public string GUID { get; set; }
        public string JobTitle { get; set; }
        public int CountryId { get; set; }
        public int EvaluatorGroupId { get; set; }
        public string Filename { get; set; }
        public string BirthDate { get; set; }
        public string NationalId { get; set; }
        public string WorkOn { get; set; }
        public int NationalityId { get; set; }
        public string ContactFullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ApplicationFormStatus { get; set; }
        public string EvalautionStatus { get; set; }
        public string FileDisplayName { get; set; }


    }

}
