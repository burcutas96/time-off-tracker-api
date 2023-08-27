using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Time_Off_Tracker.Entity.Concrete;

namespace Time_Off_Tracker.Business.Abstract
{
    public interface IPermissionService: IGenericService<Permission>
    {
        List<Permission> SGetAllManagerId(int id);
        List<Permission> SGetAllEmployeeId(int id);
        List<Permission> GetAllAccept(DateTime date);
        List<Permission> GetAllRejected(DateTime date);
        Tuple<bool, string> SInsertPermission(Permission permission);
        void TimeOffTypeUpdate(int id, string timeOffType);
    }
}
