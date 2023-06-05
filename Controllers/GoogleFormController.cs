using AMSAPI.HelperClasses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AMSAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoogleFormController : ControllerBase
    {
        GoogleFormHelper googleFormHelper;
        public GoogleFormController(IHostEnvironment hostEnvironment) { 
            googleFormHelper = new GoogleFormHelper(hostEnvironment);

        }
        [HttpGet]
        public IActionResult Get()
        {
            string formURL = googleFormHelper.CreateForm();
            return Ok(formURL);
        }
        [HttpPost]
        public void Post([FromBody] string value)
        {

        }
    }
}
