using System;

namespace Fanex.Bitcoin.Business.Services
{
    public class DateServiceManualImp : DateService
    {
        public DateTime ToDate
        {
            get
            {
                return AppConfigService.AppConfig.ToDate;
            }
        }

        public DateTime FromDate
        {
            get
            {
                return AppConfigService.AppConfig.FromDate;
            }
        }

        public DateTime Yesterday
        {
            get
            {
                return AppConfigService.AppConfig.Yesterday;
            }
        }
    }
}