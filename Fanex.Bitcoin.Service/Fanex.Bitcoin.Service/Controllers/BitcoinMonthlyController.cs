using Fanex.Bitcoin.Business.Services;
using Fanex.MailDelivery.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.Bitcoin.Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BitcoinMonthlyController : ControllerBase
    {
        private readonly MainService service;

        public BitcoinMonthlyController(MainService service)
        {
            this.service = service;
        }

        [HttpGet]
        public JobMessage Get()
        {
            return service.Execute(false);
        }
    }
}