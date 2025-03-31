using System;
using System.Data.SqlClient;
using System.Threading.Tasks.Dataflow;

namespace DAO;
public class PaymentgatewayDAO : BasePaymentDAO
{
    public int Save(PaymentModel entity)
    {
        string query = "";
        query = @"  INSERT INTO [PaymentGatewayTransaction](ActivityName,ActivityCode,ActivitySource,[locale],[bill_to_address_line1],[bill_to_address_city],[bill_to_address_country],[bill_to_email],[customer_lastname],bill_to_forename,bill_to_surname,currency,amount,ccEmail,Notes,[UpdateDate],[CreateDate])
                            VALUES (N'" + entity.ActivityName + @"',N'" + entity.ActivityCode + @"',N'" + entity.ActivitySource + @"',N'" + entity.locale + @"',N'" + entity.bill_to_address_line1 + @"',N'" + entity.bill_to_address_city + @"',N'" + entity.bill_to_address_country + @"',N'" + entity.bill_to_email + @"',N'" + entity.customer_lastname + @"',N'" + entity.bill_to_forename + @"',N'" + entity.bill_to_surname + @"',N'" + entity.currency + @"'," + entity.amount + @",N'" + entity.ccEmail + @"',N'" + entity.Notes + @"',GETDATE(),GETDATE()) ";
        query += " SELECT @@Identity";

        return int.Parse(db.ExecuteScalar(query).ToString());
    }
}
