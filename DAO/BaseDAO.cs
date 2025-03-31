using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Threading;
using Dapper;
using System.Linq;
using Shared.Helper;

namespace DAO
{
    public class BaseDAO
    {
        protected IDbConnection db;
        public static string Connection = AppSettings.Instance.ProviderConnection;
        
        public BaseDAO()
        {
            db = new SqlConnection(Connection);
        }
        
        
    }
}
