using AMSAPI.HelperClasses;
using AMSAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AMSAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        GoogleSheetsHelper googleSheetsHelper;
        public AttendanceController(IHostEnvironment hostEnvironment)
        {
             googleSheetsHelper = new GoogleSheetsHelper(hostEnvironment);
        }
        [HttpPost]
        [Authorize(AuthenticationSchemes = "BasicAuthentication")]
        public IActionResult Post([FromBody] MobileAttendance data)
        {
            // Process the data and perform any necessary actions
            AttendanceSpreadsheet attendanceSpreadsheet = new AttendanceSpreadsheet(Users.GetGoogleSheetId(User), "StudentsAttendence", googleSheetsHelper);
            attendanceSpreadsheet.UpdateAttendanceSpreadsheet(data);
            // Return a success response
            return Ok("Data received successfully");
        }
    }
}
