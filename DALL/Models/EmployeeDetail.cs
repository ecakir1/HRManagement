    namespace DAL.Models
    {
        public class EmployeeDetail : Base
        {
            public Guid EmployeeDetailId { get; set; }

            public Guid EmployeeId { get; set; }
            public Employee Employee { get; set; }

            public string Address { get; set; }

            public string Position { get; set; }
            public string Department { get; set; }
            public string City { get; set; }

            public string Education { get; set; }
            public string Certifications { get; set; }
            public string Experience { get; set; }

            public int RemainingLeaveDays {  get; set; }
        }
    }
