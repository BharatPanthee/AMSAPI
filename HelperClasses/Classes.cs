using AMSAPI.Models;
using Google.Apis.Sheets.v4;

namespace AMSAPI.HelperClasses
{
    public class Classes
    {
        SpreadsheetsResource.ValuesResource _googleSheetValues;
        string _spreadsheetId;
        string _spreadsheetName;
        public Classes(string spreadsheetId, string sheetName, GoogleSheetsHelper googleSheetsHelper)
        {
            _googleSheetValues = googleSheetsHelper.Service.Spreadsheets.Values;
            _spreadsheetId = spreadsheetId;
            _spreadsheetName = sheetName;
        }

        public List<MobileAppClass> GetListOfClasses()
        {
            var request = _googleSheetValues.Get(_spreadsheetId, _spreadsheetName);
            var response = request.Execute();
            var values = response.Values;
            List<MobileAppClass> classes = new List<MobileAppClass>();
            if (values != null && values.Count > 0)
            {
                for (int i = 1; i < values.Count; i++)
                {
                    var _class = new MobileAppClass();
                    _class.Id = (string?)values[i][0];
                    _class.Name = (string?)values[i][2];
                    classes.Add(_class);
                }
                //students.RemoveAt(0);
            }
            return classes;
        }

    }
}
