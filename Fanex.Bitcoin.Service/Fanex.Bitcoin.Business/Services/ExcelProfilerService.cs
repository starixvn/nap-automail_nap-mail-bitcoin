using System.Collections.Generic;

namespace Fanex.Bitcoin.Business.Services
{
    public interface ExcelProfilerService
    {
        ExcelService Execute(bool isDaily);
    }

    public class ExcelProfilerServiceImp : ExcelProfilerService
    {
        private readonly DateServiceProvider dateServiceProvider;

        public ExcelProfilerServiceImp(DateServiceProvider dateServiceProvider)
        {
            this.dateServiceProvider = dateServiceProvider;
        }

        public ExcelService Execute(bool isDaily)
        {
            var excel = new Dictionary<bool, ExcelService>
            {
                { true, new ExcelServiceDailyImp(dateServiceProvider) },
                { false, new ExcelServiceMonthlyImp(dateServiceProvider) },
            };

            return excel[isDaily];
        }
    }
}