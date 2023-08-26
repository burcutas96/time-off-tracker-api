using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        public int NumberOfDays { get; set; } // Talep edilen izin gün sayısı
    }
}
