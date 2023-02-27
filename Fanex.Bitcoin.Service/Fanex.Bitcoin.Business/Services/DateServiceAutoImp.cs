using System;

namespace Fanex.Bitcoin.Business.Services
{
    public class DateServiceAutoImp : DateService
    {
        public DateTime ToDate
        {
            get
            {
                return Convert.ToDateTime($"{DateTime.Now:yyyy/MM/01}").AddDays(-1);
            }
        }

        public DateTime FromDate
        {
            get
            {
                var date = DateTime.Now.AddDays(-1).AddMonths(-1);
                return Convert.ToDateTime($"{date:yyyy/MM/01}");
            }
        }

        public DateTime Yesterday
        {
            get
            {
                return Convert.ToDateTime($"{DateTime.Now.AddDays(-1):yyyy/MM/dd}");
            }
        }
    }
}