using Microsoft.AspNetCore.Http;

namespace Shared.Model
{
    public class ApplicationEvaluation
    {
        public int Id { get; set; }
        public string ApplicationUserGUID { get; set; }
        public string Evaluator { get; set; }
        public string ApplicationFormGUID { get; set; }
        public string Code { get; set; }
        public string EvalautionStatus { get; set; }
        public string DegreeNotes { get; set; }
        public int Number { get; set; }
        public int Degree { get; set; }
        public int DegreeReason { get; set; }

    }

}
