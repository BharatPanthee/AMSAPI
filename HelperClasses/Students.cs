using AMSAPI.Models;
using Google.Apis.Sheets.v4;

namespace AMSAPI.HelperClasses
{
    public class Students
    {
        SpreadsheetsResource.ValuesResource _googleSheetValues;
        string _spreadsheetId;
        string _spreadsheetName;
        public Students(string spreadsheetId, string sheetName, GoogleSheetsHelper googleSheetsHelper)
        {
            _googleSheetValues = googleSheetsHelper.Service.Spreadsheets.Values;
            _spreadsheetId = spreadsheetId;
            _spreadsheetName = sheetName;
        }
        public List<Student> GetStudents(string studentClass, string section)
        {
            //var range = $"{_spreadsheetName}!A{2}:D{4}"; //First row is header
            // Define the request to retrieve all the data from the sheet
            var request = _googleSheetValues.Get(_spreadsheetId, _spreadsheetName);

            var response = request.Execute();
            var values = response.Values;
            List<Student> students = new List<Student>();
            if (values != null && values.Count > 0)
            {
                foreach (var student in values)
                {
                    var _student = new Student();
                    _student.Id = (string?)student[0];
                    _student.FirstName = (string?)student[1];
                    _student.LastName = (string?)student[1];
                    _student.Class = (string?)student[2];
                    _student.Section = (string)student[3];
                    students.Add(_student);

                }
                students.RemoveAt(0);
            }

            return students;
        }
        public List<Person> GetStudentsWithoutClassInfo(string studentClass, string section)
        {
            // Define the request to retrieve all the data from the sheet
            var request = _googleSheetValues.Get(_spreadsheetId, _spreadsheetName);
            var response = request.Execute();
            var values = response.Values;
            List<Person> students = new List<Person>();
            if (values != null && values.Count > 0)
            {
                for (int i = 1; i < values.Count; i++)
                {
                    if (values[i][3].ToString() == studentClass)
                    {
                        var _student = new Person();
                        _student.Id = (string?)values[i][0];
                        _student.FirstName = (string?)values[i][1];
                        _student.LastName = (string?)values[i][2];
                        students.Add(_student);
                    }
                }
                //students.RemoveAt(0);
            }
            return students;
        }
        public List<Person> GetStudentsWithoutClassInfo(string studentClass)
        {
            // Define the request to retrieve all the data from the sheet
            var request = _googleSheetValues.Get(_spreadsheetId, _spreadsheetName);
            var response = request.Execute();
            var values = response.Values;
            List<Person> students = new List<Person>();
            if (values != null && values.Count > 0)
            {
                for (int i = 1; i < values.Count; i++)
                {
                    if (values[i][3].ToString() == studentClass)
                    {
                        var _student = new Person();
                        _student.Id = (string?)values[i][0];
                        _student.FirstName = (string?)values[i][1];
                        _student.LastName = (string?)values[i][2];
                        students.Add(_student);
                    }
                }
                //students.RemoveAt(0);
            }
            return students;
        }
    }
}
