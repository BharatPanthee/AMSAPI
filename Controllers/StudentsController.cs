using AMSAPI.HelperClasses;
using AMSAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AMSAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        GoogleSheetsHelper googleSheetsHelper;
        private const string sheetName = "Students";
        private Clients clients;
        public StudentsController(IHostEnvironment hostEnvironment)
        {
            googleSheetsHelper = new GoogleSheetsHelper(hostEnvironment);
            clients = new Clients(googleSheetsHelper);
        }
        [Authorize(AuthenticationSchemes = "BasicAuthentication")]
        [HttpGet("{classId}/{section}")]
        public IActionResult Get(string classId, string section)
        {
            //("17HMruiBj1T-q_TLdt9J_Ii5M19DuH1VhWsuO8umA5sk", "Student", googleSheetsHelper);

            Students studentSpreadsheet = new (Users.GetGoogleSheetId(User), sheetName, googleSheetsHelper);
            var students = studentSpreadsheet.GetStudentsWithoutClassInfo(classId, section);
            return new JsonResult(students);
        }
        
        [Authorize(AuthenticationSchemes = "BasicAuthentication")]
        [HttpGet("basedonclass/{classId}")]
        public IActionResult Get(string classId)
        {
            var user = HttpContext.User;
            var client = clients.GetClient(Request.Headers["ClientCode"]);
            if (client == null)
            {
                return Unauthorized("Invalid Client");
            }
            Students studentSpreadsheet = new("17HMruiBj1T-q_TLdt9J_Ii5M19DuH1VhWsuO8umA5sk", sheetName, googleSheetsHelper);
            var students = studentSpreadsheet.GetStudentsWithoutClassInfo(classId);
            return new JsonResult(students);
        }
   
    }
}
