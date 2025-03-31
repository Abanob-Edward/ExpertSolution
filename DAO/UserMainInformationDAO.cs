using System;
using System.Data.SqlClient;
using System.Threading.Tasks.Dataflow;

namespace DAO;
public class UserMainInformationDAO : BaseDAO, IUserMainInformationDAO
{
    public int Save(UserMainInformation entity)
    {
        try
        {
            if (db == null || string.IsNullOrEmpty(db.ConnectionString))
            {
                db = new SqlConnection(Connection);

            }
            string query = @" INSERT INTO UserMainInformation
                                       (ContactFullName,GUID,Phone,[Email],[Password],UserType,[Active],[CreateDate])
                                 VALUES
                                       (N'" + entity.ContactFullName + @"'
                                       ,N'" + entity.GUID + @"'
                                       ,N'" + entity.Phone + @"'
                                       ,N'" + entity.Email + @"'
                                       ,N'" + entity.Password + @"'
                                       ," + entity.UserType + @"
                                       ,1
                                       ,GetDate()
                                       )";
            query += @" SELECT SCOPE_IDENTITY()";
            var result = db.ExecuteScalar(query);
            if (result == null)
                return 0;
            else
                return Convert.ToInt32(result);
        }
        catch (Exception ex)
        {
            return 0;
        }
    }
    public int SaveEvaluator(UserMainInformation entity, bool IsUpdate)
    {
        try
        {
            string query = "";
            if (db == null || string.IsNullOrEmpty(db.ConnectionString))
            {
                db = new SqlConnection(Connection);

            }
            if (!IsUpdate)
            {
                query = @" INSERT INTO UserMainInformation
                                       (ContactFullName,GUID,[CountryId],EvaluatorGroupId,Phone,[Email],[Password],UserType,[Active],[CreateDate])
                                 VALUES
                                       (N'" + entity.ContactFullName + @"'
                                       ,N'" + entity.GUID + @"'
                                       ,N'" + entity.CountryId + @"'
                                       ,N'" + entity.EvaluatorGroupId + @"'
                                       ,N'" + entity.Phone + @"'
                                       ,N'" + entity.Email + @"'
                                       ,N'" + entity.Password + @"'
                                       ," + entity.UserType + @"
                                       ,1
                                       ,GetDate()
                                       )";
                query += @" SELECT SCOPE_IDENTITY()";
            }
            else
            {
                query = @"  UPDATE UserMainInformation
                            SET ContactFullName = N'" + entity.ContactFullName + @"'
                                , CountryId = N'" + entity.CountryId + @"'
                                , EvaluatorGroupId = N'" + entity.EvaluatorGroupId + @"'
                                , Phone = N'" + entity.Phone + @"'
                                , Email = N'" + entity.Email + @"'
                                , Password = N'" + entity.Password + @"'

                                    ";
                query += @"WHERE GUID ='" + entity.GUID + @"'; ";
                query += @"SELECT Id FROM UserMainInformation ";
                query += @"WHERE GUID ='" + entity.GUID + @"'";

            }
            var result = db.ExecuteScalar(query);
            if (result == null)
                return 0;
            else
                return Convert.ToInt32(result);
        }
        catch (Exception ex)
        {
            return 0;
        }
    }

    public UserMainInformation GetByEmail(string Email)
    {
        if (db == null || string.IsNullOrEmpty(db.ConnectionString))
        {
            db = new SqlConnection(Connection);

        }

        UserMainInformation obj = new UserMainInformation();
        try
        {
            string query = @" SELECT *
                            FROM UserMainInformation
                            where  LOWER(Email) like '%" + Email.ToLower() + "%' ";
            List<UserMainInformation> lstuser = db.Query<UserMainInformation>(query).ToList();
            if (lstuser.Count > 0)
                obj = lstuser.FirstOrDefault();
        }
        catch (Exception ex)
        {

        }


        return obj;
    }
    public UserMainInformation GetByGuid(string guid)
    {
        if (db == null || string.IsNullOrEmpty(db.ConnectionString))
        {
            db = new SqlConnection(Connection);

        }

        UserMainInformation obj = new UserMainInformation();
        try
        {
            string query = @" SELECT *
                            FROM UserMainInformation
                            where GUID = '" + guid + "' ";
            List<UserMainInformation> lstuser = db.Query<UserMainInformation>(query).ToList();
            if (lstuser.Count > 0)
                obj = lstuser.FirstOrDefault();
        }
        catch (Exception ex)
        {

        }


        return obj;
    }
    public UserMainInformation GetByActivateKey(string ActivateKey)
    {

        UserMainInformation obj = new UserMainInformation();
        try
        {
            string query = @" SELECT *
                            FROM UserMainInformation
                            where  ActivateKey ='" + ActivateKey + "' ";
            List<UserMainInformation> lstuser = db.Query<UserMainInformation>(query).ToList();
            if (lstuser.Count > 0)
                obj = lstuser.FirstOrDefault();
        }
        catch (Exception ex)
        {

        }


        return obj;
    }
    public UserMainInformation GetByUserNameAndPassword(string Email, string password)
    {

        UserMainInformation obj = new UserMainInformation();
        try
        {
            string query = @" SELECT *
                            FROM UserMainInformation
                            where  Email='" + Email + "' and Password='" + password + "'";
            List<UserMainInformation> lstuser = db.Query<UserMainInformation>(query).ToList();
            if (lstuser.Count > 0)
                obj = lstuser.FirstOrDefault();
        }
        catch (Exception ex)
        {

        }


        return obj;
    }
    public int UpdatePassword(string OldPassword, string NewPassword, string guid)
    {
        int Rid = 0;
        string query = @" UPDATE UserMainInformation SET Password = '" + NewPassword + "' WHERE GUID='" + guid + "' and Password='" + OldPassword + "'";
        using (db)
        {
            Rid = db.Execute(query.ToString());
        }
        return Rid;
    }
    public int ReterivePasswordByEmail(string Email, string ActivateKey)
    {
        int Rid = 0;
        string query = @" UPDATE UserMainInformation SET ActivateKey = '" + ActivateKey + "' WHERE Email='" + Email + "'";
        using (db)
        {
            Rid = db.Execute(query.ToString());
        }
        return Rid;
    }

    public int UpdatePasswordByActivateKey(string ActivateKey, string NewPassword)
    {
        int Rid = 0;
        string query = @" UPDATE UserMainInformation SET Password = '" + NewPassword + "' where  ActivateKey ='" + ActivateKey + "' ";
        using (db)
        {
            Rid = db.Execute(query.ToString());
        }
        return Rid;
    }
}
