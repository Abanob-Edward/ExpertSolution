using System.Collections.Generic;
using System.Data;
using Dapper;
using System.Linq;
using System.Text;
using System;

namespace DAO;
public class GenericeDAO<T> : BaseDAO
{
    public List<T> GetAll()
    {
        string tableName = "";
        if (typeof(T).DeclaringType != null)
            tableName = typeof(T).DeclaringType.Name;
        else
            tableName = typeof(T).Name;

        using (db)
        {
            string query = "Select " + Utility.GetColumns<T>() + " From [" + tableName + "]";
            return db.Query<T>(query).ToList();
        }
    }
    public T GetById(object id)
    {
        using (db)
        {
            string query = "Select * From " + typeof(T).DeclaringType.Name + " WHERE Id=" + id;
            return db.Query<T>(query).FirstOrDefault();
        }
    }
    public List<T> GetWhere(string condition)
    {
        string tableName = "";
        if (typeof(T).DeclaringType != null)
            tableName = typeof(T).DeclaringType.Name;
        else
            tableName = typeof(T).Name;
        using (db)
        {
            string query = "Select * From [" + tableName + "] WHERE " + condition;
            return db.Query<T>(query).ToList();
        }
    }
    public void UpdateList(List<T> list)
    {
        string tableName = typeof(T).DeclaringType.Name;
        StringBuilder query = new StringBuilder();
        for (int i = 0; i < list.Count; i++)
        {
            T obj = list[i];
            int NetsuiteId = (int)obj.GetType().GetProperty("NetsuiteId").GetValue(obj);
            var proList = obj.GetType().GetProperties().Where(p => p.CanWrite);
            query.Append("UPDATE [" + tableName + "] SET ");
            foreach (var pro in proList)
                query.Append(" " + pro.Name + "=" + Utility.GetColumnValue(obj, pro.Name) + ", ");
            query.Append("[UpdateDate] = GETDATE() WHERE [NetsuiteId] = " + NetsuiteId + " ");
        }
        using (db)
        {
            db.Execute(query.ToString());
        }
    }
    public void UpdateList(List<T> list, string columns)
    {
        string tableName = typeof(T).DeclaringType.Name;
        StringBuilder query = new StringBuilder();
        for (int i = 0; i < list.Count; i++)
        {
            T obj = list[i];
            int NetsuiteId = (int)obj.GetType().GetProperty("NetsuiteId").GetValue(obj);
            string[] cols = columns.Split(',');
            query.Append("UPDATE [" + tableName + "] SET ");
            foreach (var col in cols)
                query.Append(" " + col + "=" + Utility.GetColumnValue(obj, col) + ", ");

            query.Append("[UpdateDate] = GETDATE() WHERE [NetsuiteId] = " + NetsuiteId + " ");
        }
        using (db)
        {
            db.Execute(query.ToString());
        }
    }

    public int SaveObject(T obj, string Check_Exist_Field)
    {

        string tableName = "";
        if (typeof(T).DeclaringType != null)
            tableName = typeof(T).DeclaringType.Name;
        else
            tableName = typeof(T).Name;

        StringBuilder query = new StringBuilder();

        int check_ID = 0;
        int Return_id = 0;
        try
        {
            check_ID = (int)obj.GetType().GetProperty(Check_Exist_Field).GetValue(obj);
            var proList = obj.GetType().GetProperties().Where(p => p.CanWrite);

            query.Append("IF EXISTS(SELECT " + Check_Exist_Field + " FROM [" + tableName + "] WHERE " + Check_Exist_Field + " = '" + check_ID + "') ");
            query.Append("  BEGIN UPDATE [" + tableName + "] SET ");
            foreach (var pro in proList)
                if (pro.Name != "Id")
                    query.Append(" " + pro.Name + "=" + Utility.GetColumnValue(obj, pro.Name) + ", ");
            query.Append("       [UpdateDate] = GETDATE()   WHERE " + Check_Exist_Field + " = '" + check_ID + "' ");
            query.Append("  END ELSE BEGIN INSERT INTO [" + tableName + "] ( ");
            foreach (var pro in proList)
                if (pro.Name != "Id")
                    query.Append(" " + pro.Name + ", ");
            query.Append("  [UpdateDate],[CreateDate]) VALUES ( ");
            foreach (var pro in proList)

                if (pro.Name != "Id")
                    query.Append(" " + Utility.GetColumnValue(obj, pro.Name) + ", ");
            query.Append(" GETDATE(),GETDATE()) select max(" + Check_Exist_Field + ") from [" + tableName + "] END ");

            using (db)
            {
                Return_id = Convert.ToInt32(db.ExecuteScalar(query.ToString()));
            }
        }
        catch //(Exception ex)
        {
        }
        //    }
        return check_ID <= 0 ? Return_id : check_ID;
    }

    public void SaveList(List<T> list, string Check_Exist_Field)
    {

        string tableName = "";
        if (typeof(T).DeclaringType != null)
            tableName = typeof(T).DeclaringType.Name;
        else
            tableName = typeof(T).Name;

        StringBuilder query = new StringBuilder();
        for (int i = 0; i < list.Count; i++)
        {

            try
            {
                T obj = list[i];

                int check_ID = (int)obj.GetType().GetProperty(Check_Exist_Field).GetValue(obj);
                var proList = obj.GetType().GetProperties().Where(p => p.CanWrite);

                query.Append("IF EXISTS(SELECT " + Check_Exist_Field + " FROM [" + tableName + "] WHERE " + Check_Exist_Field + " = '" + check_ID + "') ");
                query.Append("  BEGIN UPDATE [" + tableName + "] SET ");
                foreach (var pro in proList)
                    if (pro.Name != "Id")
                        query.Append(" " + pro.Name + "=" + Utility.GetColumnValue(obj, pro.Name) + ", ");
                query.Append("       [UpdateDate] = GETDATE()   WHERE " + Check_Exist_Field + " = '" + check_ID + "' ");
                query.Append("  END ELSE BEGIN INSERT INTO [" + tableName + "] ( ");
                foreach (var pro in proList)
                    if (pro.Name != "Id")
                        query.Append(" " + pro.Name + ", ");
                query.Append("  [UpdateDate],[CreateDate]) VALUES ( ");
                foreach (var pro in proList)
                    if (pro.Name != "Id")
                        query.Append(" " + Utility.GetColumnValue(obj, pro.Name) + ", ");
                query.Append(" GETDATE(),GETDATE()) END ");
            }
            catch //(Exception ex)
            {
            }
        }
        using (db)
        {
            if (!string.IsNullOrEmpty(query.ToString()))
                db.Execute(query.ToString());
        }
    }
    public void UpdateList(List<T> list, string update_columns, string condition_column)
    {
        string tableName = typeof(T).DeclaringType.Name;
        StringBuilder query = new StringBuilder();
        for (int i = 0; i < list.Count; i++)
        {
            T obj = list[i];
            string[] cols = update_columns.Split(',');
            query.Append("UPDATE [" + tableName + "] SET ");
            foreach (var col in cols)
                query.Append(" " + col + "=" + Utility.GetColumnValue(obj, col) + ", ");

            query.Append("[UpdateDate] = GETDATE() WHERE [" + condition_column + "] = " + Utility.GetColumnValue(obj, condition_column) + " ");
        }
        using (db)
        {
            db.Execute(query.ToString());
        }
    }
    public void UpdateWhereColumnEqual(List<T> list, string columnName, bool netsuiteDate = false)
    {
        string tableName = typeof(T).DeclaringType.Name;
        StringBuilder query = new StringBuilder();
        for (int i = 0; i < list.Count; i++)
        {
            T obj = list[i];
            int columnValue = (int)obj.GetType().GetProperty(columnName).GetValue(obj);
            var proList = obj.GetType().GetProperties().Where(p => p.CanWrite);
            query.Append("UPDATE [" + tableName + "] SET ");
            foreach (var pro in proList)
                query.Append(" " + pro.Name + "=" + Utility.GetColumnValue(obj, pro.Name) + ", ");

            if (netsuiteDate)
                query.Append("[NetsuiteUpdateDate] = GETDATE()");
            else
                query.Append("[UpdateDate] = GETDATE()");

            query.Append(" WHERE [" + columnName + "] = " + columnValue + " ");
        }
        using (db)
        {
            db.Execute(query.ToString());
        }
    }
    public void MultiFieldIntegration(List<T> newList, string extraFields = "")
    {
        List<T> list = new List<T>();
        int totalRecords = 25;
        if (newList.Count < 25)
        {
            totalRecords = newList.Count;
        }

        while (newList.Any())
        {

            list = newList.Take(totalRecords).ToList();
            newList = newList.Skip(totalRecords).ToList();

            string tableName = typeof(T).DeclaringType.Name;
            //string tableName = typeof(T).Name;
            //StringBuilder query = new StringBuilder().Append("UPDATE [" + tableName + "] SET InActive=1; ");
            StringBuilder query = new StringBuilder().Append("");
            for (int i = 0; i < list.Count; i++)
            {
                T obj = list[i];
                string addFields = "";
                string[] fields = extraFields.Split(' ');
                foreach (var field in fields)
                {
                    addFields += " AND " + field + " = " + Utility.GetColumnValue(obj, obj.GetType().GetProperty(field).Name);
                }
                string salesItemQry = "";
                if (tableName == "SalesOrderItems")
                    salesItemQry = " AND Insert_Status = 1 ";
                var pro_list = obj.GetType().GetProperties().Where(x => x.CanWrite);
                query.AppendLine("IF EXISTS(SELECT Id FROM [" + tableName + "] WHERE Id > 0 " + addFields + salesItemQry + ") ");
                query.AppendLine("  BEGIN UPDATE [" + tableName + "] SET ");
                foreach (var pro in pro_list)
                    query.Append(" " + pro.Name + "=" + Utility.GetColumnValue(obj, pro.Name) + ", ");
                query.Append("        [UpdateDate] = GETDATE() WHERE Id > 0 " + addFields + " ");
                query.AppendLine("  END ELSE BEGIN INSERT INTO [" + tableName + "] ( ");
                foreach (var pro in pro_list)
                    query.Append(" " + pro.Name + ", ");
                query.Append(" [UpdateDate],[CreateDate]) VALUES ( ");
                foreach (var pro in pro_list)
                    query.Append(" " + Utility.GetColumnValue(obj, pro.Name) + ", ");
                query.AppendLine(" GETDATE(),GETDATE()) END ");
            }

            try
            {
                db.Execute(query.ToString());
            }
            catch (Exception ex)
            {
            }

        }
    }
    public void TruncateInsert(List<T> newList)
    {
        if (newList.Count > 0)
        {
            string tableName = typeof(T).DeclaringType.Name;
            var proList = typeof(T).GetProperties().Where(p => p.CanWrite);
            List<T> list = new List<T>();
            StringBuilder query;
            int totalRecords = 1000;
            int index = 0;
            if (newList.Count < 1000)
            {
                totalRecords = newList.Count;
            }
            while (newList.Any())
            {
                query = new StringBuilder();
                if (index == 0)
                {
                    query.Append("truncate table  [" + tableName + "] ; ");
                    index++;
                }

                list = newList.Take(totalRecords).ToList();
                newList = newList.Skip(totalRecords).ToList();

                query.Append("  BEGIN INSERT INTO [" + tableName + "] ( ");
                foreach (var pro in proList)
                    query.Append(" " + pro.Name + ", ");
                query.Append(" [UpdateDate],[CreateDate]) VALUES ( ");
                for (int i = 0; i < list.Count; i++)
                {
                    T obj = list[i];
                    foreach (var pro in proList)
                        query.Append(" " + Utility.GetColumnValue(obj, pro.Name) + ", ");
                    if (i == (list.Count - 1))
                        query.Append(" GETDATE(),GETDATE()) ");
                    else
                        query.Append(" GETDATE(),GETDATE()) , (");
                }
                query.Append(" END ");

                try
                {
                    db.Execute(query.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }
    }
    public void DeleteInsert(string tableName, List<T> list, string UserGuid, string Code="",string otherCondition="")
    {
        if (list.Count > 0)
        {
            var proList = typeof(T).GetProperties().Where(p => p.CanWrite);
            StringBuilder query = new StringBuilder();
            query.Append("DELETE FROM [" + tableName + @"] 
                              WHERE UserGUID = '"+UserGuid+"'");
            if (!string.IsNullOrEmpty(Code))
            {
                query.Append(" AND code ='"+Code+"'");
            }
            if (!string.IsNullOrEmpty(otherCondition))
            {
                query.Append(" AND "+ otherCondition);
            }
            query.Append("  BEGIN INSERT INTO [" + tableName + "] ( ");
                foreach (var pro in proList)
                    query.Append(" " + pro.Name + ", ");
            query.Append("UpdateDate) VALUES ( ");
            for (int i = 0; i < list.Count; i++)
                {
                    T obj = list[i];
                    foreach (var pro in proList)
                        query.Append(" " + Utility.GetColumnValue(obj, pro.Name) + ", ");
                    if (i == (list.Count - 1))
                        query.Append(" GETDATE()) ");
                    else
                        query.Append(" GETDATE()) , (");
                }
                query.Append(" END ");

                try
                {
                    db.Execute(query.ToString());
                    query.Clear();
                }
                catch (Exception ex)
                {
                }
            
        }
    }
    public void Insert(T entity)
    {
        string tableName = typeof(T).DeclaringType.Name;
        var proList = typeof(T).GetProperties().Where(p => p.CanWrite);
        StringBuilder query = new StringBuilder();
        query.Append("INSERT INTO [" + tableName + "] ( ");
        foreach (var pro in proList)
            query.Append(" " + pro.Name + ", ");
        query.Append(" InActive,[UpdateDate],[CreateDate]) VALUES ( ");
        foreach (var pro in proList)
            query.Append(" " + Utility.GetColumnValue(entity, pro.Name) + ", ");
        query.Append(" 0,GETDATE(),GETDATE()) ");
        using (db)
        {
            db.Execute(query.ToString());
        }
    }
    public void InsertMultiple(List<T> newList)
    {
        if (newList.Count > 0)
        {
            string tableName = typeof(T).DeclaringType.Name;

            var proList = typeof(T).GetProperties().Where(p => p.CanWrite);
            List<T> list = new List<T>();
            StringBuilder query = new StringBuilder();
            int totalRecords = 200;
            if (newList.Count < 200)
                totalRecords = newList.Count;

            while (newList.Any())
            {
                list = newList.Take(totalRecords).ToList();
                newList = newList.Skip(totalRecords).ToList();

                query.Append("  BEGIN INSERT INTO [" + tableName + "] ( ");
                foreach (var pro in proList)
                    query.Append(" " + pro.Name + ", ");
                query.Append(" [UpdateDate],[CreateDate]) VALUES ( ");
                for (int i = 0; i < list.Count; i++)
                {
                    T obj = list[i];
                    foreach (var pro in proList)
                        query.Append(" " + Utility.GetColumnValue(obj, pro.Name) + ", ");
                    if (i == (list.Count - 1))
                        query.Append(" GETDATE(),GETDATE()) ");
                    else
                        query.Append(" GETDATE(),GETDATE()) , (");
                }
                query.Append(" END ");

                try
                {
                    db.Execute(query.ToString());
                    query.Clear();
                }
                catch (Exception ex)
                {
                }
            }
        }
    }
    public void InsertMultiple(List<T> newList, string date, bool insertDate = false)
    {
        if (newList.Count > 0)
        {
            string tableName = typeof(T).DeclaringType.Name;
            string full_date = " convert(datetime, '" + date + "')";
            var proList = typeof(T).GetProperties().Where(p => p.CanWrite);
            List<T> list = new List<T>();
            StringBuilder query = new StringBuilder();
            int totalRecords = 200;
            if (newList.Count < 200)
                totalRecords = newList.Count;

            while (newList.Any())
            {
                list = newList.Take(totalRecords).ToList();
                newList = newList.Skip(totalRecords).ToList();

                query.Append("  BEGIN INSERT INTO [" + tableName + "] ( ");
                foreach (var pro in proList)
                    query.Append(" " + pro.Name + ", ");
                if (!insertDate)
                    query.Append(" [UpdateDate],[CreateDate]) VALUES ( ");
                else
                    query.Append(" [UpdateDate]) VALUES ( ");
                for (int i = 0; i < list.Count; i++)
                {
                    T obj = list[i];
                    foreach (var pro in proList)
                        query.Append(" " + Utility.GetColumnValue(obj, pro.Name) + ", ");

                    if (!insertDate)
                    {
                        if (i == (list.Count - 1))
                            query.Append(full_date + ", " + full_date + ") ");
                        else
                            query.Append(full_date + ", " + full_date + ") , (");
                    }
                    else
                    {
                        if (i == (list.Count - 1))
                            query.Append(full_date + ") ");
                        else
                            query.Append(full_date + ") , (");
                    }
                }
                query.Append(" END ");

                try
                {
                    db.Execute(query.ToString());
                    query.Clear();
                }
                catch (Exception ex)
                {
                }
            }
        }
    }

    public void TruncateTable()
    {
        string tableName = typeof(T).DeclaringType.Name;
        string query = "TRUNCATE TABLE " + tableName;
        db.Execute(query.ToString());
    }


}


