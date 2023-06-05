using AMSAPI.HelperClasses;
using AMSAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static System.Collections.Specialized.BitVector32;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AMSAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassesController : ControllerBase
    {
        GoogleSheetsHelper googleSheetsHelper;
        public ClassesController(IHostEnvironment hostEnvironment)
        {
            googleSheetsHelper = new GoogleSheetsHelper(hostEnvironment);
        }
        // GET: api/<ClassesController>
        [Authorize(AuthenticationSchemes = "BasicAuthentication")]
        [HttpGet]
        public IActionResult Get()
        {
            Classes classSpreadsheet = new(Users.GetGoogleSheetId(User), "Classes", googleSheetsHelper);
            var classes = classSpreadsheet.GetListOfClasses();
            return new JsonResult(classes);
        }

        // GET api/<ClassesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ClassesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ClassesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ClassesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
