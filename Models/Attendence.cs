using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMSAPI.Models
{
    public class Attendence
    {
        public DateTime Date { get; set; } = DateTime.Now;
       public short Status { get; set; }
       public string? Reason { get; set; } 
       public string? Remarks { get; set; }   
    }

    public class StudentAttendence : Attendence
    {
        public string? StudentId { get; set; }
        public string? Class { get; set; }
        public string? Period { get; set; }
        public string? AttendanceTakenBy { get; set; }


    }

    public class StaffAttendence : Attendence
    {
        private Staff Staff { get; set; }

        public StaffAttendence(Staff staff)
        {
            Staff = staff;
        }

    }
    //public static class StudentAttendanceMapper
    //{
    //    public static List<Client> MapFromRangeData(IList<IList<object>> values)
    //    {
    //        var clients = new List<Client>();
    //        foreach (var value in values)
    //        {
    //            Client client = new()
    //            {
    //                Id = value[0].ToString(),
    //                Name = value[1].ToString(),
    //                Code = value[2].ToString(),
    //                Details = value[3].ToString(),
    //                StartDate = string.IsNullOrEmpty(value[4].ToString()) ? DateTime.MinValue : Convert.ToDateTime(value[4].ToString()),
    //                EndDate = string.IsNullOrEmpty(value[5].ToString()) ? DateTime.MinValue : Convert.ToDateTime(value[5].ToString()),
    //                Status = Convert.ToInt16((value[6] ?? 0).ToString()),
    //                UserId = value[7].ToString(),
    //                Password = value[8].ToString(),
    //                GoogleSheetId = value[9].ToString(),
    //            };
    //            clients.Add(client);
    //        }
    //        return clients;
    //    }
    //}
    public class MobileAttendance
    {
        public string? ClassId { get; set; }
        public string? Period { get; set; }
        public string? AttendanceTakenBy { get; set; }
        public bool AllAbsent { get; set; }
        public string? AllAbsentReason { get; set; }
        public Dictionary<string, string>? AbsentStudents { get; set; }
        public DateTime ClassDateTime { get; set; }
    }
}
