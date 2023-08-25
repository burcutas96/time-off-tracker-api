using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Time_Off_Tracker.Entity.Concrete
{
    public class Permission
    {
        public int ID { get; set; }
        public int EmployeID { get; set; }
        public int ManagerID { get; set; }
        public string TimeOffType { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
       

    }
}
