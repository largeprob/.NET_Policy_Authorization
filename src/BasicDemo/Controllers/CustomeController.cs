using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BasicDemo.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    [Authorize]
    public class CustomeController : ControllerBase
    {
        private readonly CustomerService _customerService;

        public CustomeController(CustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        [Authorize(Policy = "Customer.List")]
        public ActionResult All()
        {
            return Ok(_customerService.All());
        }

        [HttpPost]
        [Authorize(Policy = "Customer.Add")]
        public ActionResult Add()
        {
            _customerService.Add();
            return Ok();
        }

        [HttpPost]
        [Authorize(Policy = "Customer.Update")]
        public ActionResult Update()
        {
            _customerService.Update();
            return Ok();
        }

        [HttpDelete]
        [Authorize(Policy = "Customer.Delete")]
        public ActionResult Delete()
        {
            _customerService.Delete();  
            return Ok();
        }
    }
}
