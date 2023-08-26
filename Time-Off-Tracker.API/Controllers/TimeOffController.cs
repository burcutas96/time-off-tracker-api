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


        [HttpPut("update")]
        public IActionResult TimeOffUpdate(Permission permission)
        {
            _permissionService.SUpdate(permission);
            return Ok("İzin Başarıyla Güncellendi!");
        }



        [HttpDelete("delete/{id}")]
        public IActionResult TimeOffDelete(int id)
        {
            var resultGet = _permissionService.SGetById(id);
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


        [HttpGet("getallmanager/{id}")]
        public IActionResult TimeOffManagerList(int id)
        {
            var result = _permissionService.SGetAllManagerId(id);

            if (result.Count == 0)
            {
                return Ok(
                    new
                    {
                        Message = "Bu manager'a ait bir izin gönderilmemiş!",
                        Data = result
                    });
            }
            return Ok(result);
        }
        [HttpPut("updateTimeOff-Rejected/{id}")]
        public IActionResult TimeOffUpdateRejected([FromRoute] int id)
        {
            _permissionService.TimeOffTypeUpdate(id, "Rejected");
            return Ok("TimeOffType Güncellendi");
        }

        [HttpPut("updateTimeOff-Accept/{id}")]
        public IActionResult TimeOffUpdateAccept([FromRoute] int id)
        {
            _permissionService.TimeOffTypeUpdate(id, "Accept");
            return Ok("TimeOffType Güncellendi");
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
                        Message = "Bu employee'e ait bir izin gönderilmemiş!",
                        Data = result
                    });
            }
            return Ok(result);
        }


        [HttpPost("add")]
        public IActionResult TimeOffAdd(PermissionDto permissionDto)
        {

            var user = _userService.SGetById(permissionDto.EmployeeId);

            if (user == null)
            {
                return NotFound("Kullanıcı bulunamadı");
            }

            if (user.RamainingDayOff < permissionDto.NumberOfDays)
            {
                return BadRequest("Yetersiz izin hakkı");
            }
            //Todo: Eğer EmployeeId, bir manager rolüne karşılık geliyorsa izin oluşturulamasın. Ya da bunun üzerine sonra tartışalım

            var managerId = _userService.SGetById(permissionDto.ManagerId);

            if (managerId.UserRole != "Manager")
            {
                return BadRequest("Yönetici Bulunamadı!");  //Todo: Bu mesaj değiştirilebilir
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
            user.RamainingDayOff -= permissionDto.NumberOfDays;
            _userService.SUpdate(user);



            var result = _permissionService.SInsertPermission(permission);

            if (result)
            {
                return Ok("İzin Başarıyla Gönderildi!");
            }
            return BadRequest("İzin Tarihleri Geçersiz!");    //Todo: Bu mesaj değiştirilebilir

        }
    }
}
