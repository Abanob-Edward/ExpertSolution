using Microsoft.AspNetCore.Http;

namespace Shared.Model
{
    public class AcademicQualifications
    {
        public int Id { get; set; }
        public string UserGUID { get; set; }
        public string Name { get; set; }
        public string Place { get; set; }
        public int DegreeScience { get; set; }
        public int Year { get; set; }
        public string Degree { get; set; }
        public int CountryId { get; set; }
        public string Notes { get; set; }
        public DateTime UpdateDate { get; set; }
        public string Filename { get; set; }
        public string FileDisplayName { get; set; }
        public string DegreeName { get; set; }
        public string Specialist { get; set; }
        
    }

}
