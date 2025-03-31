namespace Shared.Helper;
public sealed class AppSettings
{
    private static AppSettings _instance;
    private static readonly object padlock = new object();
    private AppSettings()
    { }

    public static AppSettings Instance
    {
        get
        {
            lock (padlock)
            {
                if (_instance == null)
                {
                    _instance = new AppSettings();
                }
                return _instance;
            }
        }
    }

    // Database Provider Name : SqlServer, PostgreSql, MySql
    public string ProviderName { get; set; }
    // Database Connection String
    public string ProviderConnection { get; set; }
    public string ProviderConnectionPayment { get; set; }


    public string MainDatabase { get; set; }
    public string FranchiseDatabase { get; set; }

    #region Email Configeration
    public string EmailHost { get; set; }
    public string EmailPort { get; set; }
    public string EmailUserName { get; set; }
    public string EmailPassword { get; set; }
    public string EmailFrom { get; set; }
    public string EmailSSL { get; set; }
    public string EmailDisplayName { get; set; }
    #endregion

    #region SMS Configeration
    public string SenderID { get; set; }
    public string SMSURL { get; set; }
    public string SMSUserName { get; set; }
    public string SMSPassword { get; set; }
    public string SMSWebSite { get; set; }
    #endregion

}


