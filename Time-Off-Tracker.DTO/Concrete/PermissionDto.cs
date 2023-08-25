using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Time_Off_Tracker.DTO.Concrete
{
    public class PermissionDto
    {
        public int EmployeeId { get; set; }
        public int ManagerId { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int NumberOfDays { get; set; } // Talep edilen izin gün sayısı
    }
}
