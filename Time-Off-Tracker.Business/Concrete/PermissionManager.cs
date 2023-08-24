using System;
using System.Collections.Generic;
using System.Linq;
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
            t.ID = 0;
            _permissionDal.Insert(t);
        }

        public void SUpdate(Permission t)
        {
           _permissionDal.Update(t);
        }
    }
}
