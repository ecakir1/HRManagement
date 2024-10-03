using Microsoft.AspNetCore.Identity;

namespace DAL.Models
{
    public class Employee : IdentityUser<Guid>
    {



        public bool IsActive { get; set; }



        public DateTime Created_At { get; set; }
        public DateTime Updated_At { get; set; }

        public virtual ICollection<LeaveRequest> LeaveRequests { get; set; }
        public virtual ICollection<ExpenseRequest> ExpenseRequests { get; set; }
    }
}
