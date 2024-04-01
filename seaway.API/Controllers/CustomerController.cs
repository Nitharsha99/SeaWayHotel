using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using seaway.API.Configurations;
using seaway.API.Manager;
using seaway.API.Models;
using System.Diagnostics;

namespace seaway.API.Controllers
{
    [Route("api/Customer")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<LogHandler> _logger;
        private readonly IConfiguration _configuration;
        LogHandler _log;
        CustomerManager _customerManager;

        public CustomerController(ILogger<LogHandler> logger, IConfiguration configuration, LogHandler log, CustomerManager customerManager)
        {
            _logger = logger;
            _configuration = configuration;
            _log = log;
            _customerManager = customerManager;
        }

        [HttpPost]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult NewCustomer([FromForm] Customer customer)
        {
            try
            {
                if(customer != null && (customer.PassportNo != null || customer.NIC != null))
                {
                    customer.PassportNo = customer.PassportNo ?? null;
                    customer.NIC = customer.NIC ?? null;


                    _customerManager.PostCustomer(customer);

                    string requestUrl = HttpContext.Request.Path.ToString();
                    string responseBody = JsonConvert.SerializeObject(customer);

                    _log.setLogTrace(new HttpRequestMessage(), new HttpResponseMessage(), requestUrl, responseBody);
                }
                return Ok(customer);
            }
            catch (Exception ex)
            {
                _logger.LogError("An exception occurred while inserting new customer data : " + ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
