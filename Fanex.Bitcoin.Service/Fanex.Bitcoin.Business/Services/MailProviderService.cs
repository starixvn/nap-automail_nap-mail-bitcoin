using System.Collections.Generic;

namespace Fanex.Bitcoin.Business.Services
{
    public interface MailProviderService
    {
        MailService Execute(bool isDaily);
    }

    public class MailProviderServiceImp : MailProviderService
    {
        private readonly DateServiceProvider dateService;

        public MailProviderServiceImp(DateServiceProvider dateService)
        {
            this.dateService = dateService;
        }

        public MailService Execute(bool isDaily)
        {
            var mail = new Dictionary<bool, MailService>
            {
                { true, new  MailServiceDailyImp(dateService) },
                { false , new  MailServiceMonthlyImp(dateService) }
            };

            return mail[isDaily];
        }
    }
}