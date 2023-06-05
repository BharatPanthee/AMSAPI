using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMSAPI.Models
{
    public class Person
    {
        public string? Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }

    public class Student : Person
    {
        public string? Class { get; set; }
        public string? Section { get; set; }
        public string? ParentEmail { get; set; }
        public string? ParentPhone { get; set;}
    }
    public class Staff : Person
    {
        public string? Level { get; set; }
    }
}
