using AMSAPI.HelperClasses;
using AMSAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AMSAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        GoogleSheetsHelper googleSheetsHelper;
        public UsersController(IHostEnvironment hostEnvironment)
        {
            googleSheetsHelper = new GoogleSheetsHelper(hostEnvironment);
        }
        [HttpPost]
        [Authorize(AuthenticationSchemes = "BasicAuthentication")]
        public IActionResult Post([FromBody] MobileUserLogin data)
        {
            int userId = 0;
            string? userName = "";
            string googleSheetId = "";
            foreach (var claim in User.Claims)
            {
                if (claim.ValueType == "userid")
                    userId = Int32.Parse(claim.Value);
                else if (claim.ValueType == "username")
                    userName = claim.Value;
                else if (claim.ValueType == "googlesheetid")
                    googleSheetId = claim.Value;
            }
            Users users = new Users(googleSheetId, "Users", googleSheetsHelper);
            var result = users.UpdateUserLogin(userId, userName, data.UniqueId);
            if(result)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
            
        }
    }
}
