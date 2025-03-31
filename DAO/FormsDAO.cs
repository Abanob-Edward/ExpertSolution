using System;
using System.Data.SqlClient;
using System.Threading.Tasks.Dataflow;
using static Dapper.SqlMapper;

namespace DAO;
public class FormsDAO : BaseDAO, IFormsDAO
{
    public int SaveExpertQuestionAnswers(List<ExpertQuestionAnswer> answers)
    {
        try
        {
            if (db == null || string.IsNullOrEmpty(db.ConnectionString))
            {
                db = new SqlConnection(Connection);
            }

            if (answers == null || answers.Count == 0)
                return 0; // No answers provided

            string expertGUID = answers[0].expertGUID; // Get ExpertGUID from first answer

            using (var connection = new SqlConnection(Connection))
            {
                connection.Open(); // ✅ Ensure the connection is open
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Delete all existing answers for this ExpertGUID
                        string deleteQuery = "DELETE FROM ExpertQuestionAnswer WHERE ExpertGUID = @ExpertGUID;";
                        connection.Execute(deleteQuery, new { ExpertGUID = expertGUID }, transaction);

                        // Insert new answers
                        string insertQuery = @"
                        INSERT INTO ExpertQuestionAnswer (ExpertGUID, QuestionID, Answer)
                        VALUES (@ExpertGUID, @QuestionID, @Answer);";

                        int rowsInserted = 0;
                        foreach (var ans in answers)
                        {
                            rowsInserted += connection.Execute(insertQuery, new
                            {
                                ExpertGUID = ans.expertGUID,
                                QuestionID = ans.questionID,
                                Answer = ans.answer
                            }, transaction);
                        }

                        transaction.Commit();
                        return rowsInserted; // Return number of inserted rows
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine("Database Error: " + ex.Message);
                        return 0; // Return 0 if something fails
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Critical Database Error: " + ex.Message);
            return 0;
        }
    }

    public List<ExpertQuestionAnswer> getExpertQuestionAnswers(string ExpertGUID)
    {
        try
        {
            if (db == null || string.IsNullOrEmpty(db.ConnectionString))
            {
                db = new SqlConnection(Connection);
            }

            string query = @"
                SELECT Id, ExpertGUID, QuestionID, Answer
                FROM ExpertQuestionAnswer
                WHERE ExpertGUID = @ExpertGUID;";

            return db.Query<ExpertQuestionAnswer>(query, new { ExpertGUID }).ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Database Error: " + ex.Message);
            return new List<ExpertQuestionAnswer>(); // Return empty list on error
        }
    }


    #region ProfessionalQualifications
    public int SaveProfessionalQualifications(ProfessionalQualifications entity)
    {
        try
        {
            if (db == null || string.IsNullOrEmpty(db.ConnectionString))
            {
                db = new SqlConnection(Connection);

            }
            if (entity.Id <= 0)
            {


                string query = @" INSERT INTO ApplicationFormProfessionalqualifications
                                 (UserGUID,TrainingType,Name,Place,Type,Organization,Notes,UpdateDate)
                                 VALUES
                                       (N'" + entity.UserGUID.Trim() + @"'
                                       ,N'" + entity.TrainingType + @"'
                                       ,N'" + entity.Name.Trim() + @"'
                                       ,N'" + entity.Place.Trim() + @"'
                                       ,N'" + entity.Type.Trim() + @"'
                                       ,N'" + entity.Organization.Trim() + @"'
                                       ,N'" + entity.Notes + @"'
                                       ,GetDate()
                                       )";
                query += @" SELECT SCOPE_IDENTITY()";
                var result = db.ExecuteScalar(query);
                if (result == null)
                    return 0;
                else
                    return Convert.ToInt32(result);
            }
            else
            {
                string query = @" UPDATE [dbo].[ApplicationFormProfessionalqualifications]
                                    SET [Name] = N'" + entity.Name.Trim() + @"'
                                        ,[TrainingType] =   N'" + entity.TrainingType + @"'
                                        ,[Place] =   N'" + entity.Place.Trim() + @"'
                                        ,[Type] =     N'" + entity.Type.Trim() + @"'
                                        ,[Organization] =      N'" + entity.Organization.Trim() + @"'
                                        ,[Notes] =    N'" + entity.Notes + @"'
                                        where  Id ='" + entity.Id + "' ";
                int result = db.Execute(query);
                return entity.Id;
            }
        }
        catch (Exception ex)
        {
            return 0;
        }
    }
    public List<ProfessionalQualifications> GetALLProfessionalQualifications(string UserGUID)
    {
        try
        {
            string query = @"  SELECT       u.*, tt.name TrainingTypeName from  ApplicationFormProfessionalqualifications u
                                left join lookupTrainingType tt on u.TrainingType = tt.Id 
                                where UserGUID ='" + UserGUID + "'";

            return db.Query<ProfessionalQualifications>(query).ToList();


        }
        catch (Exception ex)
        {
            return null;
        }


    }
    public ProfessionalQualifications GetByIdProfessionalQualifications(int id)
    {
        try
        {
            string query = @"  SELECT        u.*,ISNULL('uploads/'+a.Name,'#') as filename,ISNULL(a.DisplayName,N'') as FileDisplayName, tt.name TrainingTypeName
                               from  ApplicationFormProfessionalqualifications u
                                 left join Attachment a on u.Id = a.RealtedId and a.Code= 'PQ'
                                 left join lookupTrainingType tt on u.TrainingType = tt.Id 
                                where u.Id =" + id;
            return db.Query<ProfessionalQualifications>(query).ToList().FirstOrDefault();


        }
        catch (Exception ex)
        {
            return null;
        }


    }
    public int DeleteProfessionalQualifications(int id)
    {
        try
        {
            if (db == null || string.IsNullOrEmpty(db.ConnectionString))
            {
                db = new SqlConnection(Connection);

            }
            string query = @" DELETE FROM ApplicationFormProfessionalqualifications where Id = " + id;
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
    #endregion
    #region AcademicQualifications
    public int SaveAcademicQualifications(AcademicQualifications entity)
    {
        try
        {
            if (db == null || string.IsNullOrEmpty(db.ConnectionString))
            {
                db = new SqlConnection(Connection);

            }
            if (entity.Id <= 0)
            {


                string query = @" INSERT INTO ApplicationFormAcademicQualifications
                                 (UserGUID,Name,Place,DegreeScience,Specialist,Year,Degree,CountryId,Notes,UpdateDate)
                                 VALUES
                                       (N'" + entity.UserGUID.Trim() + @"'
                                       ,N'" + entity.Name.Trim() + @"'
                                       ,N'" + entity.Place.Trim() + @"'
                                       ,N'" + entity.DegreeScience + @"'
                                       ,N'" + entity.Specialist.Trim() + @"'
                                       ,N'" + entity.Year + @"'
                                       ,N'" + entity.Degree.Trim() + @"'
                                       ,N'" + entity.CountryId + @"'
                                       ,N'" + entity.Notes + @"'
                                       ,GetDate()
                                       )";
                query += @" SELECT SCOPE_IDENTITY()";
                var result = db.ExecuteScalar(query);
                if (result == null)
                    return 0;
                else
                    return Convert.ToInt32(result);
            }
            else
            {
                string query = @" UPDATE [dbo].[ApplicationFormAcademicQualifications]
                                    SET [Name] = N'" + entity.Name.Trim() + @"'
                                        ,[Place] =   N'" + entity.Place.Trim() + @"'
                                        ,[DegreeScience] =     N'" + entity.DegreeScience + @"'
                                        ,[Specialist] =     N'" + entity.Specialist.Trim() + @"'
                                        ,[Year] =      N'" + entity.Year + @"'
                                        ,[Degree] =      N'" + entity.Degree.Trim() + @"'
                                        ,[CountryId] =      N'" + entity.CountryId + @"'
                                        ,[Notes] =    N'" + entity.Notes + @"'
                                        where  Id ='" + entity.Id + "' ";
                int result = db.Execute(query);
                return entity.Id;
            }
        }
        catch (Exception ex)
        {
            return 0;
        }
    }
    public List<AcademicQualifications> GetALLAcademicQualifications(string UserGUID)
    {
        try
        {
            string query = @"  SELECT       u.*, tt.name DegreeName   from  ApplicationFormAcademicQualifications u
                                    left join lookupScienceDegree tt on u.DegreeScience = tt.Id                                
                                    where UserGUID ='" + UserGUID + "'";
            return db.Query<AcademicQualifications>(query).ToList();


        }
        catch (Exception ex)
        {
            return null;
        }


    }
    public AcademicQualifications GetByIdAcademicQualifications(int id)
    {
        try
        {
            string query = @"  SELECT        u.*,ISNULL('uploads/'+a.Name,'#') as filename,ISNULL(a.DisplayName,N'') as FileDisplayName
                                from  ApplicationFormAcademicQualifications u
                                 left join Attachment a on u.Id = a.RealtedId and a.Code= 'AQ'
                                 
                                where u.Id =" + id;

            return db.Query<AcademicQualifications>(query).ToList().FirstOrDefault();


        }
        catch (Exception ex)
        {
            return null;
        }


    }
    public int DeleteAcademicQualifications(int id)
    {
        try
        {
            if (db == null || string.IsNullOrEmpty(db.ConnectionString))
            {
                db = new SqlConnection(Connection);

            }
            string query = @" DELETE FROM ApplicationFormAcademicQualifications where Id = " + id;
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
    #endregion
    #region CertificateQualifications
    public int SaveCertificateQualifications(CertificateQualifications entity)
    {
        try
        {
            if (db == null || string.IsNullOrEmpty(db.ConnectionString))
            {
                db = new SqlConnection(Connection);

            }
            if (entity.Id <= 0)
            {


                string query = @" INSERT INTO ApplicationFormCertificateQualifications
                                 (UserGUID,Name,Place,Year,Degree,Notes,UpdateDate)
                                 VALUES
                                       (N'" + entity.UserGUID.Trim() + @"'
                                       ,N'" + entity.Name.Trim() + @"'
                                       ,N'" + entity.Place.Trim() + @"'
                                       ,N'" + entity.Year + @"'
                                       ,N'" + entity.Degree.Trim() + @"'
                                       ,N'" + entity.Notes + @"'
                                       ,GetDate()
                                       )";
                query += @" SELECT SCOPE_IDENTITY()";
                var result = db.ExecuteScalar(query);
                if (result == null)
                    return 0;
                else
                    return Convert.ToInt32(result);
            }
            else
            {
                string query = @" UPDATE [dbo].[ApplicationFormCertificateQualifications]
                                    SET [Name] = N'" + entity.Name.Trim() + @"'
                                        ,[Place] =   N'" + entity.Place.Trim() + @"'
                                        ,[Year] =      N'" + entity.Year + @"'
                                        ,[Degree] =      N'" + entity.Degree.Trim() + @"'
                                        ,[Notes] =    N'" + entity.Notes + @"'
                                        where  Id ='" + entity.Id + "' ";
                int result = db.Execute(query);
                return entity.Id;
            }
        }
        catch (Exception ex)
        {
            return 0;
        }
    }
    public List<CertificateQualifications> GetALLCertificateQualifications(string UserGUID)
    {
        try
        {
            string query = @"  SELECT       * from  ApplicationFormCertificateQualifications where UserGUID ='" + UserGUID + "'";
            return db.Query<CertificateQualifications>(query).ToList();


        }
        catch (Exception ex)
        {
            return null;
        }


    }
    public CertificateQualifications GetByIdCertificateQualifications(int id)
    {
        try
        {
            string query = @"  SELECT        u.*,ISNULL('uploads/'+a.Name,'#') as filename,ISNULL(a.DisplayName,N'') as FileDisplayName  
                                from  ApplicationFormCertificateQualifications u
                                 left join Attachment a on u.Id = a.RealtedId and a.Code= 'CQ'
                                where u.Id =" + id;
            return db.Query<CertificateQualifications>(query).ToList().FirstOrDefault();


        }
        catch (Exception ex)
        {
            return null;
        }


    }
    public int DeleteCertificateQualifications(int id)
    {
        try
        {
            if (db == null || string.IsNullOrEmpty(db.ConnectionString))
            {
                db = new SqlConnection(Connection);

            }
            string query = @" DELETE FROM ApplicationFormCertificateQualifications where Id = " + id;
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
    #endregion
    #region Table Lookup
    public int CreateTableRow(TableForms entity)
    {
        try
        {
            if (db == null || string.IsNullOrEmpty(db.ConnectionString))
            {
                db = new SqlConnection(Connection);

            }


            string query = @" INSERT INTO " + entity.TableName + @"
                                 (Number,UserGUID)
                                 VALUES
                                       (N'" + entity.Number.Trim() + @"'
                                        ,N'" + entity.UserGUID.Trim() + @"'
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
    public int DeleteTableRow(TableForms entity)
    {
        try
        {
            if (db == null || string.IsNullOrEmpty(db.ConnectionString))
            {
                db = new SqlConnection(Connection);

            }


            string query = @" delete from " + entity.TableName + @"
                              where id= " + entity.Id;

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

    public int UpdateTableField(TableForms entity)
    {
        try
        {
            if (db == null || string.IsNullOrEmpty(db.ConnectionString))
            {
                db = new SqlConnection(Connection);

            }


            string query = @" UPDATE " + entity.TableName + @"
                                    SET " + entity.ColumName + " = N'" + entity.ColumValue.Trim() + @"'
                                        where id= " + entity.Id;
            int result = db.Execute(query);
            return entity.Id;

        }
        catch (Exception ex)
        {
            return 0;
        }
    }

  
    #endregion
}
