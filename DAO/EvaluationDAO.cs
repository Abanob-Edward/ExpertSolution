using Dapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAO;
public class EvaluationDAO : BaseDAO, IEvaluationDAO
{
    public List<ApplicationFormUI> GetAll()
    {
        string query = @"   SELECT        UserMainInformation.Id, UserMainInformation.GUID, UserMainInformation.OrganizationName, UserMainInformation.CountryId, UserMainInformation.City, UserMainInformation.EstablishmentDate, 
                        UserMainInformation.RegisterationNo, UserMainInformation.RegisteratedBy, UserMainInformation.OrganizationSize, UserMainInformation.OrganizationPhone, UserMainInformation.Website, UserMainInformation.SocialSites, 
                        UserMainInformation.ContactFullName, UserMainInformation.ContactTitle, UserMainInformation.ContactMobile, UserMainInformation.ContactPhone, UserMainInformation.ContactEmail, UserMainInformation.Email, 
                        UserMainInformation.ApplicationFormStatus, UserMainInformation.Active, UserMainInformation.CreateDate,
                        Country.Name CountryName, eg.Name GroupName
                        FROM            UserMainInformation INNER JOIN
                        Country ON UserMainInformation.CountryId = Country.Id
                        LEFT JOIN EvaluatorGroup eg on eg.Id = UserMainInformation.EvaluatorGroupId
						where usertype = 3 ";
        query += @"   order by  UserMainInformation.OrganizationName ";
        return db.Query<ApplicationFormUI>(query).ToList();
    }
    public List<ApplicationFormUI> GetByEvaluationGroup(int evaluatorGroupId)
    {
        string query = @"   SELECT        UserMainInformation.Id, UserMainInformation.GUID, UserMainInformation.OrganizationName, UserMainInformation.CountryId, UserMainInformation.City, UserMainInformation.EstablishmentDate, 
                        UserMainInformation.RegisterationNo, UserMainInformation.RegisteratedBy, UserMainInformation.OrganizationSize, UserMainInformation.OrganizationPhone, UserMainInformation.Website, UserMainInformation.SocialSites, 
                        UserMainInformation.ContactFullName, UserMainInformation.ContactTitle, UserMainInformation.ContactMobile, UserMainInformation.ContactPhone, UserMainInformation.ContactEmail, UserMainInformation.Email, 
                        UserMainInformation.ApplicationFormStatus, UserMainInformation.Active, UserMainInformation.CreateDate,
                        Country.Name CountryName, eg.Name GroupName,UserMainInformation.EvaluatorGroupId
                        FROM            UserMainInformation INNER JOIN
                        Country ON UserMainInformation.CountryId = Country.Id
                        LEFT JOIN EvaluatorGroup eg on eg.Id = UserMainInformation.EvaluatorGroupId
						where usertype = 1  AND UserMainInformation.EvaluatorGroupId=" + evaluatorGroupId;
        query += @"   order by  UserMainInformation.OrganizationName ";
        return db.Query<ApplicationFormUI>(query).ToList();
    }
    public List<ApplicationFormUI> GetEvaluators(int evaluatorGroupId)
    {
        string query = @"   SELECT        UserMainInformation.Id, UserMainInformation.GUID, 
                            UserMainInformation.ContactFullName
                            FROM            UserMainInformation 
						    where usertype = 3 AND UserMainInformation.EvaluatorGroupId=" + evaluatorGroupId;
        query += @"   order by  UserMainInformation.ContactFullName ";
        return db.Query<ApplicationFormUI>(query).ToList();
    }
    public DataTable GetEvaluatedResult()
    {
        DataTable dt = new DataTable();
        string query = @"   DECLARE @cols NVARCHAR(MAX), @query NVARCHAR(MAX);
                            SET @cols = STUFF(
                                             (
                                                 SELECT DISTINCT
                                                        ','+QUOTENAME(u.ContactFullName)
                                                 FROM [dbo].ApplicationEvaluationSummery c
					                             inner join UserMainInformation u on u.GUID = c.ApplicationUserGUID
					                             FOR XML PATH(''), TYPE
                                             ).value('.', 'nvarchar(max)'), 1, 1, '');
                            SET @query = 'SELECT [ApplicationFormGUID], '+@cols+' INTO #result from (SELECT [ApplicationFormGUID],
          
                                       [EvalautionStatus] AS [EvalautionStatus],
                                       u.ContactFullName AS [Evaluator]
                                 FROM [dbo].ApplicationEvaluationSummery c
	                             inner join UserMainInformation u on u.GUID = c.ApplicationUserGUID
                                )x pivot (max(EvalautionStatus) for Evaluator in ('+@cols+')) p';
	                            set @query +=' select r.*, u.[OrganizationName]
	                            ,Country.Name CountryName ,u.OrganizationSize,u.City,u.OrganizationPhone,u.Email,u.ContactFullName
	                            from  #result r
	                            inner join [dbo].[UserMainInformation] u on r.[ApplicationFormGUID] = u.GUID
	                             INNER JOIN Country ON u.CountryId = Country.Id
	                            drop table  #result'
	                            --print @query
                            EXECUTE (@query); ";
        using (SqlConnection sqlConnection =
        new SqlConnection(Connection))
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(
                query, sqlConnection);
            adapter.Fill(dt);

        }
        return dt;
        //query += @"";
        //return db.Query<ApplicationFormUI>(query).ToList();
    }
    public int SaveEvaluationDegree(ApplicationEvaluation entity)
    {
        try
        {
            string query = "";
            if (db == null || string.IsNullOrEmpty(db.ConnectionString))
            {
                db = new SqlConnection(Connection);

            }

            query = @" DELETE FROM ApplicationEvaluation 
                        WHERE   ApplicationUserGUID = N'" + entity.ApplicationUserGUID + @"'
                        AND     ApplicationFormGUID =  N'" + entity.ApplicationFormGUID + @"'
                        AND     Code =  N'" + entity.Code + @"'
                        AND     Number =  N'" + entity.Number + @"'";
            query += @" DELETE FROM ApplicationEvaluationSummery 
                        WHERE   ApplicationUserGUID = N'" + entity.ApplicationUserGUID + @"'
                        AND     ApplicationFormGUID =  N'" + entity.ApplicationFormGUID + @"'";

            query += @" INSERT INTO ApplicationEvaluationSummery
                                       (ApplicationUserGUID,ApplicationFormGUID,EvalautionStatus,CreateDate)
                                 VALUES
                                       (N'" + entity.ApplicationUserGUID + @"'
                                       ,N'" + entity.ApplicationFormGUID + @"'
                                       ,N'InProgress'
                                       ,GetDate()
                                       )";

            query += @" INSERT INTO ApplicationEvaluation
                                       (ApplicationUserGUID,ApplicationFormGUID,Code,Number,Degree,DegreeReason,DegreeNotes,CreateDate)
                                 VALUES
                                       (N'" + entity.ApplicationUserGUID + @"'
                                       ,N'" + entity.ApplicationFormGUID + @"'
                                       ,N'" + entity.Code + @"'
                                       ,N'" + entity.Number + @"'
                                       ,N'" + entity.Degree + @"'
                                       ,N'" + entity.DegreeReason + @"'
                                       ,N'" + entity.DegreeNotes + @"'
                                       ,GetDate()
                                       )";
            query += @" SELECT SCOPE_IDENTITY() ";

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
    public bool FinishEvaluationDegree(ApplicationEvaluation entity)
    {
        try
        {
            string query = "";
            if (db == null || string.IsNullOrEmpty(db.ConnectionString))
            {
                db = new SqlConnection(Connection);
            }
            query = @" SELECT COUNT(Id) FROM ApplicationEvaluation 
                        WHERE   ApplicationUserGUID = N'" + entity.ApplicationUserGUID + @"'
                        AND     ApplicationFormGUID =  N'" + entity.ApplicationFormGUID + @"'";

            var count = db.ExecuteScalar(query);
            if (int.Parse(count.ToString()) == 37)
            {
                query = @" DELETE FROM ApplicationEvaluationSummery 
                        WHERE   ApplicationUserGUID = N'" + entity.ApplicationUserGUID + @"'
                        AND     ApplicationFormGUID =  N'" + entity.ApplicationFormGUID + @"'";

                query += @" INSERT INTO ApplicationEvaluationSummery
                                       (ApplicationUserGUID,ApplicationFormGUID,EvalautionStatus,CreateDate)
                                 VALUES
                                       (N'" + entity.ApplicationUserGUID + @"'
                                       ,N'" + entity.ApplicationFormGUID + @"'
                                       ,N'Finished'
                                       ,GetDate()
                                       )";
                var result = db.Execute(query);
                return true;
            }
            else
                return false;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public ApplicationEvaluation GetEvaluationDegree(ApplicationEvaluation entity)
    {
        ApplicationEvaluation obj = new ApplicationEvaluation();
        try
        {
            string query = "";
            if (db == null || string.IsNullOrEmpty(db.ConnectionString))
            {
                db = new SqlConnection(Connection);

            }

            query = @" SELECT * FROM ApplicationEvaluation 
                        WHERE ";
            if (!string.IsNullOrEmpty(entity.Evaluator))
            {
                query += @"  ApplicationUserGUID = N'" + entity.Evaluator + @"'";
            }
            else
            {
                query += @"  ApplicationUserGUID = N'" + entity.ApplicationUserGUID + @"'";
            }
            query += @"  AND     ApplicationFormGUID =  N'" + entity.ApplicationFormGUID + @"'
                        AND     Code =  N'" + entity.Code + @"'
                        AND     Number =  N'" + entity.Number + @"'";

            List<ApplicationEvaluation> lstuser = db.Query<ApplicationEvaluation>(query).ToList();
            if (lstuser.Count > 0)
                obj = lstuser.FirstOrDefault();



        }
        catch (Exception ex)
        {

        }
        return obj;
    }

    public ApplicationEvaluation GetEvaluationSummery(string ApplicationUserGUID, string ApplicationFormGUID)
    {
        ApplicationEvaluation obj = new ApplicationEvaluation();
        try
        {
            string query = "";
            if (db == null || string.IsNullOrEmpty(db.ConnectionString))
            {
                db = new SqlConnection(Connection);

            }

            query = @" select sum(ae.Degree) Degree,aes.EvalautionStatus from ApplicationEvaluation ae
                        inner join ApplicationEvaluationSummery aes on ae.ApplicationFormGUID = aes.ApplicationFormGUID
                        and ae.ApplicationUserGUID = aes.ApplicationUserGUID
                        where ae.ApplicationUserGUID = '" + ApplicationUserGUID + @"'
                        AND ae.ApplicationFormGUID = '" + ApplicationFormGUID + @"'
                        group by aes.EvalautionStatus ";

            List<ApplicationEvaluation> lst = db.Query<ApplicationEvaluation>(query).ToList();
            if (lst.Count > 0)
                obj = lst.FirstOrDefault();



        }
        catch (Exception ex)
        {

        }
        return obj;
    }

}
