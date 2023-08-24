using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Time_Off_Tracker.Entity.Dto
{
    public class PermissionDto
    {
        public int EmployeeId { get; set; }
        public int ManagerId { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
