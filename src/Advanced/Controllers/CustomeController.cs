using Advanced.Authorizations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Advanced.Controllers
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
        [PermissionAttribute("Customer.List")]
        public ActionResult All()
        {
            return Ok(_customerService.All());
        }

        [HttpPost]
        [PermissionAttribute("Customer.Add")]
        public ActionResult Add()
        {
            _customerService.Add();
            return Ok();
        }

        [HttpPost]
        [PermissionAttribute("Customer.Add")]
        public ActionResult Update()
        {
            _customerService.Update();
            return Ok();
        }

        [HttpDelete]
        [PermissionAttribute("Customer.Add")]
        public ActionResult Delete()
        {
            _customerService.Delete();  
            return Ok();
        }
    }
}
