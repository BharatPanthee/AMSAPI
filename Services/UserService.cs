using AMSAPI.HelperClasses;
using AMSAPI.Models;

namespace AMSAPI.Services
{
    public class UserService : IUserService
    {
        public User? Authenticate(string username, string password, string? clientCode, IHostEnvironment hostEnvironment)
        {
            GoogleSheetsHelper googleSheetsHelper;
            googleSheetsHelper = new GoogleSheetsHelper(hostEnvironment);
            Clients clients = new Clients(googleSheetsHelper);
            string? googleSheetId = clients.GetGoogleSheetId(clientCode);
            if(string.IsNullOrWhiteSpace(googleSheetId))
            {
                return new User { IsValid = false, Error = "Invalid Client" };
            }
            GoogleSheetsHelper userGoogleSheetsHelper;
            userGoogleSheetsHelper = new GoogleSheetsHelper(hostEnvironment,clientCode);
            Users users = new Users(googleSheetId,"Users",userGoogleSheetsHelper);
            int? userId;
            var result = users.ValidateUser(username,password, out userId);
            if(!result)
            {
                return new User { IsValid = false, Error = "Invalid Username or Password" };
            }
            return new User {Id = userId??0, IsValid = true, Username = username, Password = password, GoogleSheetId = googleSheetId };
        }
    }
}
