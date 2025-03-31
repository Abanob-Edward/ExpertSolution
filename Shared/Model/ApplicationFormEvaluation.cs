using Microsoft.AspNetCore.Http;

namespace Shared.Model
{
    public class ApplicationFormEvaluation
    {
        public string UserGUID { get; set; }
        public string Code { get; set; }
        public string AnswerFirst { get; set; }
        public string AnswerSecond { get; set; }

        public string AnswerThird { get; set; }
        public int Answer { get; set; }

        public int Number { get; set; }

    }

    public class ApplicationFormEvaluationView
    {
        public string Code { get; set; }
        public List<ApplicationFormEvaluation> ApplicationFormEvaluationLst { get; set; }
        public List<AttachmentUI> AttachmentLst { get; set; }
    }

    public class ApplicationEvaluationView
    {
        public string Code { get; set; }
        public int Number { get; set; }
        public List<ApplicationEvaluation> ApplicationEvaluationLst { get; set; }
    }

}
