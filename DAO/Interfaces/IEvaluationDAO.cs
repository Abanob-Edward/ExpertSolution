namespace DAO.Interfaces
{
    public interface IEvaluationDAO
    {
        public List<ApplicationFormUI> GetAll();
        public int SaveEvaluationDegree(ApplicationEvaluation entity);
        public ApplicationEvaluation GetEvaluationDegree(ApplicationEvaluation entity);
        public bool FinishEvaluationDegree(ApplicationEvaluation entity);
        public DataTable GetEvaluatedResult();
        public List<ApplicationFormUI> GetByEvaluationGroup(int evaluatorGroupId);
        public List<ApplicationFormUI> GetEvaluators(int evaluatorGroupId);
        public ApplicationEvaluation GetEvaluationSummery(string ApplicationUserGUID, string ApplicationFormGUID);
    }
}