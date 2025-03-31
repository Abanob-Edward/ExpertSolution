namespace DAO;
public class CommonDAO : BaseDAO, ICommonDAO
{
    public List<Lookup> GetCountry()
    {
        string query = @"  SELECT * FROM Country ORDER BY  Name";
        return db.Query<Lookup>(query).ToList();
    }
    public  List<LookUpQuestion> GetQuestions()
    {
        string query = @"  SELECT * FROM LookUpQuestion ORDER BY [order]";
        return db.Query<LookUpQuestion>(query).ToList();
    }

    public List<EvaluatorGroup> GetEvaluatorGroup()
    {
        string query = @"  SELECT * FROM EvaluatorGroup Where InActive = 0";
        return db.Query<EvaluatorGroup>(query).ToList();
    }
    public List<Lookup> GetLookup(string tblName)
    {
        string query = @"  SELECT * FROM "+tblName;
        query += @" WHERE ISNULL(InActive,0) = 0 Order BY orders  ";
        return db.Query<Lookup>(query).ToList();
    }

   
}
