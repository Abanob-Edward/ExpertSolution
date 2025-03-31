using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.Interfaces
{
    public interface IUserMainInformationDAO
    {
        public int Save(UserMainInformation entity);
        public int SaveEvaluator(UserMainInformation entity, bool IsUpdate);
      
        public UserMainInformation GetByEmail(string Email);
        public UserMainInformation GetByUserNameAndPassword(string Email, string password);
        public int UpdatePassword(string OldPassword, string NewPassword, string guid);
        public int UpdatePasswordByActivateKey(string ActivateKey, string NewPassword);
        public int ReterivePasswordByEmail(string Email, string ActivateKey);
        public UserMainInformation GetByActivateKey(string ActivateKey);
        public UserMainInformation GetByGuid(string guid);
    }
}
