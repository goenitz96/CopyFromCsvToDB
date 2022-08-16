using Hangfire;
using Hangfire.MySql;

namespace CSV.Helpers;

public static class CronJobHelper
{
    public static void HangFireConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var mysql = configuration.GetConnectionString("MySql");
        services.AddHangfire(x => 
            x.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseStorage(new MySqlStorage(mysql, new MySqlStorageOptions
                {
                    QueuePollInterval = TimeSpan.FromSeconds(10),
                    JobExpirationCheckInterval = TimeSpan.FromHours(1),
                    CountersAggregateInterval = TimeSpan.FromMinutes(5),
                    PrepareSchemaIfNecessary = true,
                    DashboardJobListLimit = 2500,
                    TransactionTimeout = TimeSpan.FromMinutes(2),
                    TablesPrefix = "HangfireJobs"
                })));
    }
}