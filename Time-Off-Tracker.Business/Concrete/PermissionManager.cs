using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using Time_Off_Tracker.Business.Abstract;
using Time_Off_Tracker.DAL.Abstract;
using Time_Off_Tracker.Entity.Concrete;

namespace Time_Off_Tracker.Business.Concrete
{
    public class PermissionManager : IPermissionService
    {
        private readonly IPermissionDal _permissionDal;

        public PermissionManager(IPermissionDal permissionDal)
        {
            _permissionDal = permissionDal;
        }

        public List<Permission> SGetAllManagerId(int id)
        {
            return _permissionDal.GetList(p => p.ManagerID == id);
        }

        public List<Permission> SGetAllEmployeeId(int id)
        {
            return _permissionDal.GetList(p => p.EmployeID == id);
        }

        public void SDelete(Permission t)
        {
            _permissionDal.Delete(t);
        }

        public Permission SGetById(int id)
        {
            return _permissionDal.GetById(id);
        }

        public Permission SGetByUsername(string username)
        {
            throw new NotImplementedException();
        }

        public List<Permission> SGetList()
        {
            return _permissionDal.GetList();
        }

        public void SInsert(Permission t)
        {

        }

        public Tuple<bool, string> SInsertPermission(Permission t)
        {
            _permissionDal.Insert(t);                  
            return new Tuple<bool, string>(true, "Çoklu seçim yapıldı. İzin gönderildi");
            
        }

        public void SUpdate(Permission t)
        {
            _permissionDal.Update(t);
        }

        public void TimeOffTypeUpdate(int id, string timeOffType)
        {
            Permission permission = SGetById(id);
            permission.TimeOffType = timeOffType;
            SUpdate(permission);
        }

        public List<Permission> GetAllAccept(DateTime date)
        {
            return _permissionDal.GetList(p => p.StartDate <= date.Date && p.EndDate >= date.Date && p.TimeOffType == "Accept").ToList();
        }
        
        public List<Permission> GetAllRejected(DateTime date)
        {
            return _permissionDal.GetList(p => p.StartDate <= date.Date && p.EndDate >= date.Date && p.TimeOffType == "Rejected").ToList();
        }
    }
}
