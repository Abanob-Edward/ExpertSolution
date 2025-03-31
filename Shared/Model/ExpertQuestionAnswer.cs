using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Model
{
    public   class ExpertQuestionAnswer
    {
        public int? Id { get; set; }
        public string expertGUID { get; set;}
        public int questionID { get; set; }
        public bool answer { get; set; }

    }
}
