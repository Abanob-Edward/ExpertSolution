using System.ComponentModel;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Shared.Helper;

    public class Utility
{
       
    public static string GetMd5Hash(string text)
    {
        MD5 md5 = MD5.Create();
        byte[] inputBytes = Encoding.Unicode.GetBytes(text.ToLower());
        byte[] hash = md5.ComputeHash(inputBytes);
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < hash.Length; i++)
        {
            sb.Append(hash[i].ToString("X2"));
        }
        return sb.ToString();
    }
    public static string GenerateFileName(string FileName)
    {
        return DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + FileName;
    }
    public static string GenerateUniqueString()
    {
        long ticks = DateTime.Now.Ticks;
        byte[] bytes = BitConverter.GetBytes(ticks);
        string id = Convert.ToBase64String(bytes)
                                .Replace('+', '_')
                                .Replace('/', '-')
                                .TrimEnd('=');
        return id;
    }
    public static string Wrap(string Str)
    {
        if (string.IsNullOrEmpty(Str))
            return "";
        else
        {
            try
            {
                if (Str.IndexOf("'") != -1)
                    Str = Str.Replace("'", "`");
            }
            catch { }
            return Str;
        }
    }
   
    public static string GetColumns<T>()
    {
        string columns = string.Empty;
        foreach (var pro in typeof(T).GetProperties())
        {
            if (string.IsNullOrEmpty(columns))
                columns = pro.Name;
            else
                columns += "," + pro.Name;
        }
        return columns;
    }
    public static object GetColumnValue(object obj, string propertyName)
    {
        object result = obj.GetType().GetProperty(propertyName).GetValue(obj);
        if (obj.GetType().GetProperty(propertyName).PropertyType.Name.Equals("String")
            ||
            obj.GetType().GetProperty(propertyName).PropertyType.Name.Equals("TimeSpan"))
        {
            if (result == null)
                result = "NULL";
            else
            {
                if (result.ToString().Contains("'"))
                    result = result.ToString().Replace("'", "");
                result = "N'" + result + "'";
            }
        }
        else if (obj.GetType().GetProperty(propertyName).PropertyType.Name.Equals("DateTime")
                 ||
                 obj.GetType().GetProperty(propertyName).PropertyType.Name.Equals("Date"))
        {
            DateTime temp_date = Convert.ToDateTime(obj.GetType().GetProperty(propertyName).GetValue(obj));

            if (temp_date <= DateTime.MinValue || temp_date >= DateTime.MaxValue)
                result = "NULL";
            else
                result = "'" + temp_date.ToString("yyyy/MM/dd hh:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture) + "'";
        }
        else if (obj.GetType().GetProperty(propertyName).PropertyType.Name.Equals("Boolean"))
            result = Convert.ToByte(obj.GetType().GetProperty(propertyName).GetValue(obj));
        return result;
    }

    public static string Base64Encode(string plainText)
    {
        var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
        return Convert.ToBase64String(plainTextBytes);
    }

    public static string Base64Decode(string base64EncodedData)
    {
        var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
        return Encoding.UTF8.GetString(base64EncodedBytes);
    }

    public static dynamic GetJsonResult(bool isValid = true, bool isAuthorized = true, string errorMsg = "")
    {
        return new { IsValid = isValid, IsAuthorized = isAuthorized, Error = errorMsg };
    }
    public static object GetExtendedJsonResult(Dictionary<string, object> dict)
    {
        Dictionary<string, object> newDict = Utility.Dyn2Dict(GetJsonResult());
        foreach (var item in dict)
        {
            newDict.Add(item.Key, item.Value);
        }
        return newDict;
    }
    public static Dictionary<string, object> Dyn2Dict(dynamic dynObj)
    {
        var dictionary = new Dictionary<string, object>();
        foreach (PropertyDescriptor propertyDescriptor in TypeDescriptor.GetProperties(dynObj))
        {
            object obj = propertyDescriptor.GetValue(dynObj);
            dictionary.Add(propertyDescriptor.Name, obj);
        }
        return dictionary;
    }
    public static string GetMessageError(Exception ex)
    {
        string error = "";
        if (ex.InnerException == null)
            error = ex.Message;
        else
            return GetMessageError(ex.InnerException);
        return error;
    }
    public static string RemoveSpecialCharacters(string str)
    {
        if (string.IsNullOrEmpty(str))
            return "";

        // handle arabic characters
        string pattern = @"^[\u0621-\u064A]+$";
        Regex rg = new Regex(pattern);
        
        StringBuilder sb = new StringBuilder();
        foreach (char c in str)
        {
            if ((c >= '0' && c <= '9') 
                || (c >= 'A' && c <= 'Z') 
                || (c >= 'a' && c <= 'z') 
                || c == '.' || c == '_' || c == ' '
                || rg.IsMatch(c.ToString(),0))
            {
                sb.Append(c);
            }
        }
        return sb.ToString();
    }
    public static string RemoveMobilePhoneSpecialCharacters(string str)
    {
        if (string.IsNullOrEmpty(str))
            return "";

        StringBuilder sb = new StringBuilder();
        foreach (char c in str)
        {
            if ((c >= '0' && c <= '9'))
            {
                sb.Append(c);
            }
        }
        return sb.ToString();
    }

    #region convert
    public static bool IsNumeric(string s)
    {
        try
        {
            float output;
            return float.TryParse(s, out output);
        }
        catch { }

        return false;
    }
    public static int ConvertToInt(string val)
    {
        try
        {
            return Convert.ToInt32(val);
        }
        catch { return 0; }
    }
    public static bool ConvertToBool(string val)
    {
        try
        {
            return Convert.ToBoolean(val);
        }
        catch { return false; }
    }
    public static double ConvertToDouble(string val)
    {
        try
        {
            return Convert.ToDouble(val);
        }
        catch { return 0.0; }
    }
    public static float ConvertToSingle(string val)
    {
        try
        {
            return Convert.ToSingle(val);
        }
        catch { return 0; }
    }
    public static string ConvertToString(string val)
    {
        if (val != null)
            return val;
        else
            return "";
    }
    public static DateTime ConvertToDatetime(string val)
    {
        if (val != null)
        {
            try
            {
                return Convert.ToDateTime(val);
            }
            catch {}
        }

        return DateTime.MinValue;
    }
    #endregion

    
}