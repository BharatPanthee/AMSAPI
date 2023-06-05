using Google.Apis.Sheets.v4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AMSAPI.Models;
using Google.Apis.Sheets.v4.Data;

namespace AMSAPI.HelperClasses
{
    public class AttendanceSpreadsheet
    {
        SpreadsheetsResource.ValuesResource _googleSheetValues;
        string _spreadsheetId;
        string _spreadsheetName;
        public AttendanceSpreadsheet(string spreadsheetId, string sheetName, GoogleSheetsHelper googleSheetsHelper)
        {
            _googleSheetValues = googleSheetsHelper.Service.Spreadsheets.Values;
            _spreadsheetId = spreadsheetId ;
            _spreadsheetName = sheetName ;
        }
        public List<StudentAttendence> GetStudentsAttendance(string studentClass, string section )
        {
            var range = $"{_spreadsheetName}!A{2}:D{4}"; //First row is header
            var request = _googleSheetValues.Get(_spreadsheetId, range);
            var response = request.Execute();
            var values = response.Values;
            List<StudentAttendence> studentAttendences = new List<StudentAttendence>();
            foreach (var attendance in values)
            {
                var _student = new Student();
                _student.Id = (string?)attendance[0];
                _student.Class = (string?)attendance[2];
                _student.Section = (string)attendance[3];
                var studentAttendance = new StudentAttendence();
                studentAttendences.Add(studentAttendance);

            }
            return studentAttendences;
        }
        public string UpdateAttendanceSpreadsheet(MobileAttendance mobileAttendances)
        {
            List<StudentAttendence> studentAttendences = new List<StudentAttendence>();
            if (mobileAttendances is not null && mobileAttendances.AbsentStudents is not null)
            {
                foreach(var absentee in mobileAttendances.AbsentStudents)
                {
                    StudentAttendence _studentAttendence = new StudentAttendence();
                    _studentAttendence.StudentId = absentee.Key;
                    _studentAttendence.Period = mobileAttendances.Period;
                    _studentAttendence.Class = mobileAttendances.ClassId;
                    _studentAttendence.Date = mobileAttendances.ClassDateTime;
                    _studentAttendence.Reason = absentee.Value;
                    _studentAttendence.Status = 0;
                    _studentAttendence.AttendanceTakenBy = mobileAttendances.AttendanceTakenBy;
                    studentAttendences.Add(_studentAttendence);
                }
            }
            // Define the range where data will be appended
            var range = $"{_spreadsheetName}!A:H";

            // Create the AppendRequest
            foreach(var _stdAttendance in studentAttendences)
            {
                if(_stdAttendance is not null)
                {
                    // Convert data to ValueRange
                    var valueRange = new ValueRange
                    {
                        Values = new List<IList<object>>
                        {
                            new List<object>
                            {
                                _stdAttendance.StudentId,
                                _stdAttendance.Class,
                                _stdAttendance.Date.ToString("yyyy-MM-dd HH:mm:ss"),
                                _stdAttendance.Period,
                                _stdAttendance.Status,
                                _stdAttendance.Reason,
                                _stdAttendance.Remarks,
                                _stdAttendance.AttendanceTakenBy
                            }
                        }
                    };
                    var appendRequest = _googleSheetValues.Append(valueRange, _spreadsheetId, range);
                    appendRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;
                    // Execute the AppendRequest
                    var appendResponse = appendRequest.Execute();
                }
             
            }
            return "";
        }
        
    }
    

}
