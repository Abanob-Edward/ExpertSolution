namespace ExpertSolution.Extension
{
    public static class StartupServices
    {
        public static void Startup(this IServiceCollection services, IConfiguration Configuration)
        {
            Configuration.Bind("AppSettings", AppSettings.Instance);
            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
            services.Configure<ConfigConstants>(Configuration.GetSection("ConfigConstants"));
            services.AddTransient<IMailService, MailService>();
            services.AddTransient<IUserMainInformationDAO, UserMainInformationDAO>();
            services.AddTransient<IApplicationFormDAO, ApplicationFormDAO>();
            services.AddTransient<IFormsDAO, FormsDAO>();
            services.AddTransient<ICommonDAO, CommonDAO>();
            services.AddTransient<IEvaluationDAO, EvaluationDAO>();


        }
    }
}
