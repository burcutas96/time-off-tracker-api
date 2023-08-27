using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Time_Off_Tracker.Entity.Concrete
{
    public class User
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string UserLastName { get; set; }
        public string UserEmail { get; set; }
        public int RamainingDayOff { get; set; } = 30;
        public string UserRole { get; set; }
        public bool UserStatus { get; set; }
        public DateTime UserCreateDate { get; set; }
        public DateTime UserModifiedDate { get; set; }
        public string UserPassword { get; set; }
    }
}
