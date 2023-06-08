using AMSAPI.Models;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
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
        public bool UpdateUserLogin(int Id, string? userId, string? UniqueId)
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
                clientUser = clientUsers.Where(clt => clt.Id == Id && clt.Username == userId).FirstOrDefault();
                if (clientUser == null) return false;
                int index = 2;
                foreach(var value  in values)
                {
                    if (value!=null)
                    {
                        if (clientUser.Id.ToString() == value[0].ToString())
                        {
                            break;
                        }
                    }
                    index++;
                }
                var range = $"{_spreadsheetName}!D{index}:F{index}";
                // Convert data to ValueRange
                var valueRange = new ValueRange
                {
                    Values = new List<IList<object>>
                        {
                            new List<object>
                            {
                                UniqueId??"",
                                clientUser.Validity!=null?(clientUser.Validity??DateTime.Now).ToString("yyyyy-MM-dd") : DateTime.Now.ToString("yyyyy-MM-dd"),
                                DateTime.Now.ToString("yyyyy-MM-dd HH:mm:ss"),
                            }
                        }
                };
                var appendRequest = _googleSheetValues.Update(valueRange, _spreadsheetId, range);
                appendRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
                // Execute the UpdateRequest
                var appendResponse = appendRequest.Execute();
            }
            return true;
        }
        public bool ValidateUser(string userId, string password, out int? Id)
        {
            Id = 0;
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
                Id = clientUser?.Id;
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
