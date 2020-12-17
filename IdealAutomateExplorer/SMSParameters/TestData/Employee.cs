using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataGridFilterTest.TestData
{
    public class Employee
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

        public Guid? EmployeeGuid { get; set; }

        public int WorkExperience { get; set; }

        public EmployeePosition Position { get; set; }
        public int EmployeeStatusId { get; set; }

        public bool IsInterviewed { get; set; }

        public DateTime DateOfBirth { get; set; }
    }

    public class EmployeePosition
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class EmployeeStatus
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
