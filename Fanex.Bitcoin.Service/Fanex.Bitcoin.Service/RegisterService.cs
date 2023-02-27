using Fanex.Bitcoin.Business.Models;
using Fanex.Bitcoin.Business.Services;
using Fanex.Data;
using Fanex.Data.Repository;
using Fanex.Logging;
using Fanex.Logging.Sentry.RavenClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using OfficeOpenXml;
using SharpRaven;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Fanex.Bitcoin.Service
{
    public static class RegisterService
    {
        public static void RegisterServices(IConfiguration configuration, IServiceCollection services)
        {
            LoadConfig();
            SentryRegister();
            ConfigureFanexData(configuration);
            DependencyInjection(services);
        }

        private static void LoadConfig()
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);

            var text = File.ReadAllText("appsettings.json");
            AppConfigService.AppConfig = JsonConvert.DeserializeObject<ConfigSettingModel>(text);
        }

        private static void SentryRegister()
        {
            var sentry = new SentryLogging(new SentryEngineOptions
            {
                Dsn = new Dsn(AppConfigService.AppConfig.SentryClientKey)
            });

            LogManager
               .SetDefaultLogCategory(AppConfigService.AppConfig.CategorySentryLog)
               .Use(new List<ILogEngine>
               {
                    sentry
               });
        }

        private static void ConfigureFanexData(IConfiguration Configuration)
        {
            var connections = Configuration.GetSection("ConnectionStrings")
                .GetChildren()
                .AsEnumerable()
                .ToDictionary
                (
                     x => x.Key,
                     y => y.Value
                );

            DbSettingProviderManager
             .StartNewSession()
             .UseConnectionStrings(connections)
             .UseDefaultDbSettingProvider(Configuration.GetSection("StoredProcedurePath").Value)
             .Run();
        }

        private static void DependencyInjection(IServiceCollection services)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            services.AddSingleton<Log, LogImp>();
            services.AddSingleton<MainService, MainServiceImp>();
            services.AddSingleton<MailProviderService, MailProviderServiceImp>();
            services.AddSingleton<DataService, DataServiceImp>();
            services.AddSingleton<IDynamicRepository, DynamicRepository>();
            services.AddSingleton<ExcelProfilerService, ExcelProfilerServiceImp>();
            services.AddSingleton<DateServiceProvider, DateServiceProviderImp>();
        }
    }
}