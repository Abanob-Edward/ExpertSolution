using Microsoft.AspNetCore.Http;

namespace Shared.Model
{
    public class ProfessionalQualifications
    {
        public int Id { get; set; }
        public string UserGUID { get; set; }
        public string Name { get; set; }
        public string Place { get; set; }
        public string Type { get; set; }
        public string Organization { get; set; }
        public string Notes { get; set; }
        public string Filename { get; set; }
        public string FileDisplayName { get; set; }
        public int TrainingType { get; set; }
        public DateTime UpdateDate { get; set; }
        public string TrainingTypeName { get; set; }

    }

}
