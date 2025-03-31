using Dapper;
using System;
using System.Collections;
using System.Collections.Generic;

namespace DAO;
public class ApplicationFormDAO : BaseDAO, IApplicationFormDAO
{
    public ApplicationForm GetApplicationInfo(string Id, UserMainInformation userInfo, string evaluator = "")
    {

        ApplicationForm obj = new ApplicationForm();

        try
        {
            string queryMain = @" SELECT u.*,ISNULL('uploads/'+a.Name,'#') as filename,ISNULL(a.DisplayName,N'') as FileDisplayName 
                                 FROM UserMainInformation u
                                 left join Attachment a on u.GUID = a.UserGUID and a.Code= 'Main'
                            where  u.GUID ='" + Id + "' ";
            obj.MainInformation = db.Query<UserMainInformation>(queryMain).FirstOrDefault();

            string queryService = @" SELECT a.*,l.Name
                            FROM ApplicationFormSocialLinks a left join lookupSocial l on l.Id = a.SocialId
                            where  a.UserGUID ='" + Id + "' ";
            obj.SocialLinksLst = db.Query<ApplicationFormSocialLinks>(queryService).ToList();

            queryService = @" SELECT a.*,l.Name,ll.Name LevelName
                            FROM ApplicationFormLanguage a left join lookupLanguage l on l.Id = a.LanguageId
							left join lookupLevel ll on ll.Id = a.LevelId
                            where  a.UserGUID ='" + Id + "' ";
            obj.languageLst = db.Query<ApplicationFormLanguage>(queryService).ToList();

            queryService = @" SELECT a.*,l.Name,ll.Name LevelName
                            FROM ApplicationFormComputerSkills a left join lookupComputerSkill l on l.Id = a.ComputerSkillId
							left join lookupLevel ll on ll.Id = a.LevelId
                            where  a.UserGUID ='" + Id + "' ";
            obj.ComSkillsLst = db.Query<ApplicationFormComputerSkills>(queryService).ToList();

            //string queryAttach = @" SELECT *
            //                FROM Attachment
            //                where  UserGUID ='" + Id + "' ";
            //obj.AttachmentLst = db.Query<AttachmentUI>(queryAttach).ToList();

            //string queryEval = @" SELECT *
            //                FROM ApplicationFormEvaluationFirst
            //                where  UserGUID ='" + Id + "' ";
            //queryEval += @" 
            //                UNION SELECT *
            //                FROM ApplicationFormEvaluationSecond
            //                where  UserGUID ='" + Id + "' ";
            //queryEval += @" 
            //                UNION SELECT *
            //                FROM ApplicationFormEvaluationThird
            //                where  UserGUID ='" + Id + "' ";
            //queryEval += @" 
            //                UNION SELECT *
            //                FROM ApplicationFormEvaluationFourth
            //                where  UserGUID ='" + Id + "' ";
            //queryEval += @" 
            //                UNION SELECT *
            //                FROM ApplicationFormEvaluationFifth
            //                where  UserGUID ='" + Id + "' ";
            //queryEval += @" 
            //                UNION SELECT *
            //                FROM ApplicationFormEvaluationSix
            //                where  UserGUID ='" + Id + "' ";
            //queryEval += @" 
            //                UNION SELECT *
            //                FROM ApplicationFormEvaluationSeven
            //                where  UserGUID ='" + Id + "' ";
            //union evaluation second
            //obj.ApplicationFormEvaluationLst = db.Query<ApplicationFormEvaluation>(queryEval).ToList();

            if (userInfo.UserType == 3)
            {
                string queryEvalDegree = @" SELECT *
                            FROM ApplicationEvaluation
                            where  ApplicationUserGUID ='" + userInfo.GUID + "' AND ApplicationFormGUID ='" + Id + "'";
                obj.ApplicationEvaluationLst = db.Query<ApplicationEvaluation>(queryEvalDegree).ToList();
            }
            else if (userInfo.UserType == 2 && !string.IsNullOrEmpty(evaluator))
            {
                string queryEvalDegree = @" SELECT *
                            FROM ApplicationEvaluation
                            where  ApplicationUserGUID = '" + evaluator + "' AND ApplicationFormGUID ='" + Id + "'";
                obj.ApplicationEvaluationLst = db.Query<ApplicationEvaluation>(queryEvalDegree).ToList();
            }

        }
        catch (Exception ex)
        {

        }


        return obj;
    }

    public List<ApplicationFormUI> GetAllApplicationForms(ApplicationFormFilter model)
    {
        string query = @"  SELECT        u.Id, u.GUID, u.CountryId, u.Phone,u.JobTitle,u.NationalId,
                        u.ContactFullName,  u.Email, 
                        u.ApplicationFormStatus, u.Active, u.CreateDate,
                        EvaluatorGroupId
                        FROM  UserMainInformation u
						where usertype=1 ";

        if (!string.IsNullOrEmpty(model.Status))
            query += @"   AND ApplicationFormStatus = N'" + model.Status + "'";
        else
            query += @"   AND ApplicationFormStatus  != 'Cancelled' ";

        if (!string.IsNullOrEmpty(model.CountryId))
            query += @"   AND CountryId = " + model.CountryId;

        query += @"   order by u.ContactFullName ";
        return db.Query<ApplicationFormUI>(query).ToList();
    }

    public List<ApplicationFormUI> GetEvaluationApplicationForms(ApplicationFormFilter model, UserMainInformation obj)
    {
        string query = @"   SELECT        UserMainInformation.Id, UserMainInformation.GUID, UserMainInformation.OrganizationName, UserMainInformation.CountryId, UserMainInformation.City, UserMainInformation.EstablishmentDate, 
                        UserMainInformation.RegisterationNo, UserMainInformation.RegisteratedBy, UserMainInformation.OrganizationSize, UserMainInformation.OrganizationPhone, UserMainInformation.Website, UserMainInformation.SocialSites, 
                        UserMainInformation.ContactFullName, UserMainInformation.ContactTitle, UserMainInformation.ContactMobile, UserMainInformation.ContactPhone, UserMainInformation.ContactEmail, UserMainInformation.Email, 
                        UserMainInformation.ApplicationFormStatus, UserMainInformation.Active, UserMainInformation.CreateDate,
                        Country.Name CountryName, EvaluatorGroupId, ISNULL(aes.EvalautionStatus,'NotStarted') EvalautionStatus
                        FROM  UserMainInformation INNER JOIN Country 
                              ON UserMainInformation.CountryId = Country.Id
                              LEFT JOIN ApplicationEvaluationSummery aes ON aes.ApplicationFormGUID = GUID
                              AND aes.ApplicationUserGUID = '" + obj.GUID + @"'
						where usertype = 1 AND EvaluatorGroupId >0 AND EvaluatorGroupId = " + model.EvaluatorGroupId;
        query += @" ";
        //if (!string.IsNullOrEmpty(model.Status))
        //    query += @"   AND ApplicationFormStatus = N'" + model.Status + "'";
        //else
        //    query += @"   AND ApplicationFormStatus  != 'Cancelled' ";

        //if (!string.IsNullOrEmpty(model.CountryId))
        //    query += @"   AND CountryId = " + model.CountryId;
        //if (model.OrganizationSize > 0)
        //    query += @"   AND  isnull(UserMainInformation.OrganizationSize,2) = " + model.OrganizationSize;

        query += @"   order by UserMainInformation.OrganizationName, Country.Name ";
        return db.Query<ApplicationFormUI>(query).ToList();
    }
    public ApplicationFormCount GetApplicationFormCount()
    {

        ApplicationFormCount obj = new ApplicationFormCount();

        try
        {
            string queryMain = @"  select COUNT(*) AS Total, 
                                   sum(case when ApplicationFormStatus = N'InProgress'  then 1 else 0 end) InProgress,
                                   sum(case when ApplicationFormStatus = N'Paid'  then 1 else 0 end) Paid,
		                           sum(case when ApplicationFormStatus = N'Finished'  then 1 else 0 end) Finished,
                                   sum(case when ApplicationFormStatus = N'Cancelled'  then 1 else 0 end) Cancelled,
                                   sum(case when ApplicationFormStatus = N'Accepted'  then 1 else 0 end) Accepted
		                           from UserMainInformation 	where usertype=1  ";
            obj = db.Query<ApplicationFormCount>(queryMain).FirstOrDefault();


        }
        catch (Exception ex)
        {

        }


        return obj;
    }
    public List<ApplicationFormCount> GetApplicationFormCountbyCountry()
    {
        try
        {
            string query = @"  SELECT        Country.Name CountryName,Country.Id CountryId ,
                         isnull(UserMainInformation.OrganizationSize,2) OrganizationSize, 
						  sum(case when ApplicationFormStatus = N'InProgress'  then 1 else 0 end) InProgress,
		                  sum(case when ApplicationFormStatus = N'Finished'  then 1 else 0 end) Finished,
                          --sum(case when ApplicationFormStatus = N'Cancelled'  then 1 else 0 end) Cancelled,
						  sum(case when ApplicationFormStatus = N'Accepted'  then 1 else 0 end) Accepted
                        FROM            UserMainInformation INNER JOIN
                        Country ON UserMainInformation.CountryId = Country.Id
						where usertype=1  AND ApplicationFormStatus  != 'Cancelled'
						group by Country.Name,Country.Id,  isnull(UserMainInformation.OrganizationSize,2)
						order by Country.Name,Country.Id,  isnull(UserMainInformation.OrganizationSize,2) ";
            return db.Query<ApplicationFormCount>(query).ToList();


        }
        catch (Exception ex)
        {
            return null;
        }


    }
    public List<ApplicationFormCount> GetApplicationFormCountryTotal()
    {
        try
        {
            string query = @"   SELECT        Country.Name CountryName,Country.Id CountryId ,
                                COUNT(UserMainInformation.Id) As Total
                                FROM            UserMainInformation INNER JOIN
                                Country ON UserMainInformation.CountryId = Country.Id
						        where usertype=1  AND ApplicationFormStatus  != 'Cancelled'
						        group by Country.Name,Country.Id
						        order by  COUNT(UserMainInformation.Id) desc ";
            return db.Query<ApplicationFormCount>(query).ToList();


        }
        catch (Exception ex)
        {
            return null;
        }


    }
    public int SaveMainInformation(UserMainInformation entity)
    {
        string query = @" UPDATE [dbo].[UserMainInformation]
                         SET [JobTitle] = N'" + entity.JobTitle + @"'
                            ,[CountryId] = " + entity.CountryId + @"
                            ,[BirthDate] =   N'" + entity.BirthDate + @"'
                            ,[NationalId] =     N'" + entity.NationalId + @"'
                            ,[ContactFullName] =      N'" + entity.ContactFullName + @"'
                            ,[Phone] =        N'" + entity.Phone + @"'
                            ,[NationalityId] =        N'" + entity.NationalityId + @"'
                            ,[WorkOn] =        N'" + entity.WorkOn + @"'
                            where  GUID ='" + entity.GUID + "' ";
        int result = db.Execute(query);
        return result;
    }

    public int SaveApplicationFormService(List<ApplicationFormService> servicearray)
    {
        var result = 0;
        try
        {
            new GenericeDAO<ApplicationFormService>().DeleteInsert("ApplicationFormService", servicearray, servicearray[0].UserGUID);
            result = 1;
        }
        catch (Exception ex) { }
        return result;
    }

    public int SaveApplicationEvaluation(List<ApplicationFormEvaluation> evalArray)
    {
        var result = 0;
        try
        {
            new GenericeDAO<ApplicationFormEvaluation>().DeleteInsert("ApplicationFormEvaluation" + evalArray[0].Code, evalArray, evalArray[0].UserGUID, evalArray[0].Code);
            result = 1;
        }
        catch (Exception ex) { }
        return result;
    }

    public int SaveAttachment(AttachmentUI attchObj)
    {
        List<AttachmentUI> lst = new List<AttachmentUI>();
        var result = 0;
        try
        {
            lst.Add(attchObj);
            new GenericeDAO<AttachmentUI>().DeleteInsert("Attachment", lst, attchObj.UserGUID, attchObj.Code, " RealtedId=" + attchObj.RealtedId);
            result = 1;
        }
        catch (Exception ex) { }
        return result;
    }

    public int UpdateStatus(string UserGUID, string Status)
    {
        int Rid = 0;
        string query = @" UPDATE UserMainInformation SET ApplicationFormStatus = '" + Status + "' WHERE GUID='" + UserGUID + "'";
        using (db)
        {
            Rid = db.Execute(query.ToString());
        }
        return Rid;
    }

    public int ChangeGroup(string UserGUID, int EvaluatorGroupId)
    {
        int Rid = 0;
        string query = @" UPDATE UserMainInformation SET EvaluatorGroupId = '" + EvaluatorGroupId + "' WHERE GUID='" + UserGUID + "'";
        using (db)
        {
            Rid = db.Execute(query.ToString());
        }
        return Rid;
    }


}
