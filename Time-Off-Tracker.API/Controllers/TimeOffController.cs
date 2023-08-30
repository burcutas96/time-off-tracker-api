using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Time_Off_Tracker.Business.Abstract;
using Time_Off_Tracker.DTO.Concrete;
using Time_Off_Tracker.Entity.Concrete;

namespace Time_Off_Tracker.API.Controllers
{
    [Route("time-off")]
    [ApiController]
    public class TimeOffController : ControllerBase
    {
        private IPermissionService _permissionService;
        private IUserService _userService;

        public TimeOffController(IPermissionService permissionService, IUserService userService)
        {
            _permissionService = permissionService;
            _userService = userService;
        }


        [HttpGet("{id}")]
        public IActionResult TimeOffGet(int id)
        {
            var result = _permissionService.SGetById(id);
            return Ok(result);
        }


        [HttpGet("getallemployee/{id}")]
        public IActionResult TimeOffEmployeeList(int id)
        {
            var result = _permissionService.SGetAllEmployeeId(id);

            if (result.Count == 0)
            {
                return Ok(
                    new
                    {
                        Message = "Bu employee'a ait bir izin gönderilmemiş!",
                        Data = result
                    });
            }
            return Ok(result);
        }


        [HttpGet("getallmanager/{id}")]
        public IActionResult TimeOffManagerList(int id)
        {
            var resultAllManager = _permissionService.SGetAllManagerId(id);
            if (resultAllManager.Count == 0)
            {
                return Ok(
                    new
                    {
                        Message = "Bu manager'a ait bir izin gönderilmemiş!",
                        Data = resultAllManager
                    });
            }

            List<object> modifiedResults = new List<object>();

            foreach (var item in resultAllManager)
            {
                var employeeName = _userService.SGetById(item.EmployeID).UserName;

                var modifiedItem = new
                {
                    item.ID,
                    item.EmployeID,
                    employeeName,
                    item.ManagerID,
                    item.TimeOffType,
                    item.Description,
                    item.StartDate,
                    item.EndDate
                };
                modifiedResults.Add(modifiedItem);
            }
            return Ok(modifiedResults);
        }


        [HttpDelete("delete/{id}")]
        public IActionResult TimeOffDelete(int id)
        {
            var resultGet = _permissionService.SGetById(id);

            var user = _userService.SGetById(resultGet.EmployeID);

            TimeSpan difference = resultGet.EndDate.Subtract(resultGet.StartDate);
            var number = difference.Days + 1;
            user.RamainingDayOff += number;

            _userService.SUpdate(user);

            if (resultGet == null)
            {
                return BadRequest();
            }
            else
            {
                _permissionService.SDelete(resultGet);
                return Ok("İzin Başarıyla Silindi!");
            }
        }


        [HttpPut("updateTimeOff-Rejected/{id}")]
        public IActionResult TimeOffUpdateRejected([FromRoute] int id)
        {
            var permission = _permissionService.SGetById(id);
            var user = _userService.SGetById(permission.EmployeID);

            TimeSpan difference = permission.EndDate.Subtract(permission.StartDate);
            var number = difference.Days + 1;
            user.RamainingDayOff += number;

            _userService.SUpdate(user);

            _permissionService.TimeOffTypeUpdate(id, "Rejected");
            return Ok("TimeOffType Güncellendi");
        }


        [HttpPut("updateTimeOff-Accept/{id}")]
        public IActionResult TimeOffUpdateAccept([FromRoute] int id)
        {
            _permissionService.TimeOffTypeUpdate(id, "Accept");
            return Ok("TimeOffType Güncellendi");
        }


        [HttpGet("getallaccept/{date}")]
        public IActionResult GetAllAccept(DateTime date)
        {
            var accepts = _permissionService.GetAllAccept(date);

            if (accepts.Count == 0)
            {
                return Ok("Bu tarihte izinli kimse yok!");
            }


            List<object> modifiedResults = new List<object>();
            foreach (var accept in accepts)
            {
                var employeeName = _userService.SGetById(accept.EmployeID).UserName;
                var managerName = _userService.SGetById(accept.ManagerID).UserName;

                var modifiedItem = new
                {
                    accept.ID,
                    accept.EmployeID,
                    employeeName,
                    accept.ManagerID,
                    managerName,
                    accept.TimeOffType,
                    accept.Description,
                    accept.StartDate,
                    accept.EndDate
                };
                modifiedResults.Add(modifiedItem);
            }
            return Ok(modifiedResults);
        }


        [HttpGet("getallrejected/{date}")]
        public IActionResult GetAllRejected(DateTime date)
        {
            var rejecteds = _permissionService.GetAllRejected(date);

            if (rejecteds.Count == 0)
            {
                return Ok("Reddilen hiçbir izin yok!");
            }
            return Ok(rejecteds);
        }


        [HttpPut("update")]
        public IActionResult TimeOffUpdate(Permission permission)
        {
            _permissionService.SUpdate(permission);
            return Ok("İzin Başarıyla Güncellendi!");
        }


        [HttpPost("add")]
        public IActionResult TimeOffAdd(PermissionDto permissionDto)
        {

            var user = _userService.SGetById(permissionDto.EmployeeId);

            if (user == null)
            {
                return NotFound("Kullanıcı bulunamadı");
            }

            TimeSpan difference = permissionDto.EndDate.Subtract(permissionDto.StartDate);
            var number = difference.Days + 1;

            if (number > 30)
            {
                return BadRequest("Yetersiz izin hakkı");
            }
            

            if (permissionDto.StartDate > permissionDto.EndDate)
            {
                return BadRequest("İzin başlangıç tarihi bitiş tarihinden önce olmalıdır");
            }
            

            if (permissionDto.StartDate < DateTime.Today)
            {
                return BadRequest("İzin başlangıç tarihi bugünden önce olamaz");
            }


            var managerId = _userService.SGetById(permissionDto.ManagerId);

            if (managerId.UserRole != "Manager")
            {
                return BadRequest("Yönetici Bulunamadı!");
            }

            Permission permission = new()
            {
                EmployeID = permissionDto.EmployeeId,
                ManagerID = permissionDto.ManagerId,
                Description = permissionDto.Description,
                StartDate = permissionDto.StartDate,
                EndDate = permissionDto.EndDate,
                TimeOffType = "Pending"
            };
            user.RamainingDayOff -= number; 
            _userService.SUpdate(user);

            var result = _permissionService.SInsertPermission(permission);

            if (result.Item1)
            {
                return Ok(result.Item2);
            }
            return BadRequest(result.Item2);
        }
    }
}
