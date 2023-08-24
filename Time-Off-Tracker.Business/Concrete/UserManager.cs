using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Time_Off_Tracker.Business.Abstract;
using Time_Off_Tracker.DAL.Abstract;
using Time_Off_Tracker.DAL.Concrete;
using Time_Off_Tracker.Entity.Concrete;

namespace Time_Off_Tracker.Business.Concrete
{
    public class UserManager : IUserService
    {
        private readonly IUserDal _userDal;

        public UserManager(IUserDal userDal)
        {
            _userDal = userDal;
        }

        public void SDelete(User t)
        {
            _userDal.Delete(t);
        }

        public User SGetById(int id)
        {
           return _userDal.GetById(id);
        }

        public List<User> SGetList()
        {
           return _userDal.GetList();
        }

        public void SInsert(User t)
        {
            t.ID = 0;
            _userDal.Insert(t);
        }

        public void SUpdate(User t)
        {
         _userDal.Update(t);
        }

        public User SGetByUsername(string username)
        {
            return _userDal.GetList().FirstOrDefault(x => x.UserName == username);
        }

        public User SGetByEmail(string email)
        {
            return _userDal.GetList().SingleOrDefault(x => x.UserEmail == email);
        }
    }
}
