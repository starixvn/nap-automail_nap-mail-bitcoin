using Fanex.Bitcoin.Business.Services;
using Fanex.MailDelivery.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.Bitcoin.Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BitcoinDailyController : ControllerBase
    {
        private readonly MainService service;

        public BitcoinDailyController(MainService service)
        {
            this.service = service;
        }

        [HttpGet]
        public JobMessage Get()
        {
            return service.Execute(true);
        }
    }
}