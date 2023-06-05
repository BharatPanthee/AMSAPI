using AMSAPI.Models;
using Google.Apis.Sheets.v4;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace AMSAPI.HelperClasses
{
    public class Users
    {
        SpreadsheetsResource.ValuesResource _googleSheetValues;
        string _spreadsheetId;
        string _spreadsheetName = "Users";
        public Users(string spreadsheetId, string sheetName, GoogleSheetsHelper googleSheetsHelper)
        {
            _googleSheetValues = googleSheetsHelper.Service.Spreadsheets.Values;
            _spreadsheetId = spreadsheetId;
            if(sheetName != null)
            {
                _spreadsheetName = sheetName;
            }
        }
        public bool ValidateUser(string userId, string password)

        {
            var request = _googleSheetValues.Get(_spreadsheetId, _spreadsheetName);
            var response = request.Execute();
            var values = response.Values;
            ClientUser? clientUser = null;
            List<ClientUser> clientUsers = new List<ClientUser>();
            if (values != null && values.Count > 0)
            {
                values.RemoveAt(0);
                clientUsers = ClientUserMapper.MapFromRangeData(values);

                clientUser = clientUsers.Where(clt => clt.Username == userId).FirstOrDefault();
            }
            if(clientUser == null)
            {
                return false;
            }
            if(clientUser.Password != password)
            {
                return false;
            }
            return true;
        }
        public static string GetGoogleSheetId(System.Security.Claims.ClaimsPrincipal claimPrincipal)
        {
           
            string googleSheetId = string.Empty;
            foreach (var claim in claimPrincipal.Claims)
            {
                if(claim.ValueType== "googlesheetid")
                    googleSheetId = claim.Value;
            }
            return googleSheetId;

        }
    }
}
