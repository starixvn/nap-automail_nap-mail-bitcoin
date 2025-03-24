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
    public static class ServiceCollectionExtensions
    {
        public static void AddConfig(this IServiceCollection services)
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);

            var text = File.ReadAllText("appsettings.json");
            AppConfigService.AppConfig = JsonConvert.DeserializeObject<ConfigSettingModel>(text);
        }

     
        public static void AddInternalServices(this IServiceCollection services)
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