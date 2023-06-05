using AMSAPI.HelperClasses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AMSAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        GoogleSheetsHelper googleSheetsHelper;
        private const string sheetName = "Students";
        public ClientsController(IHostEnvironment hostEnvironment)
        {
            googleSheetsHelper = new GoogleSheetsHelper(hostEnvironment);
        }
        // GET: api/<ClientController>
        [Authorize(AuthenticationSchemes = "BasicAuthentication")]
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ClientController>/5
        [Authorize(AuthenticationSchemes = "BasicAuthentication")]
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            Clients clients = new Clients(googleSheetsHelper);
            var client =  clients.GetClient(id);

            return Ok(client);
        }

        // POST api/<ClientController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ClientController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ClientController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
