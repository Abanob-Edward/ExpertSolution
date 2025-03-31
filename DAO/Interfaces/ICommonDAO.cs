using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.Interfaces
{
    public interface ICommonDAO
    {
        public List<Lookup> GetCountry();
        public List<LookUpQuestion> GetQuestions();
        public List<EvaluatorGroup> GetEvaluatorGroup();
        public List<Lookup> GetLookup(string tblName);
    }
}
