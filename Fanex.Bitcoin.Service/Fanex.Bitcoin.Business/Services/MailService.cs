using Fanex.MailDelivery.Core.Models;

namespace Fanex.Bitcoin.Business.Services
{
    public interface MailService
    {
        Mail SenMail(string fileName);
    }
}