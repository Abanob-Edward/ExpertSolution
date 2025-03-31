using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Threading;
using Dapper;
using System.Linq;
using Shared.Helper;

namespace DAO
{
    public class BasePaymentDAO
    {
        protected IDbConnection db;
        public static string Connection = AppSettings.Instance.ProviderConnectionPayment;
        
        public BasePaymentDAO()
        {
            db = new SqlConnection(Connection);
        }
        
        
    }
}
