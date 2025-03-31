using Microsoft.AspNetCore.Http;

namespace Shared.Model
{
    public class TableForms
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public string Code { get; set; }
        public string TableName { get; set; }
        public string UserGUID { get; set; }
        public string ColumName { get; set; }
        public string ColumValue { get; set; }

    }

    public class ApplicationFormSocialLinks
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
        public string SocialId { get; set; }
        public string Link { get; set; }

    }
    public class ApplicationFormLanguage
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LevelName { get; set; }
        public string Number { get; set; }
        public string SocialId { get; set; }
        public string Link { get; set; }
        public string Notes { get; set; }

    }
    public class ApplicationFormComputerSkills
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LevelName { get; set; }
        public string Number { get; set; }
        public string SocialId { get; set; }
        public string Link { get; set; }
        public string Notes { get; set; }

    }


}
