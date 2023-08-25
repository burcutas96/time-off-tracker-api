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
          var result=  _permissionService.SGetById(id);
            return Ok(result);
        }


        [HttpPut("update")]
        public IActionResult TimeOffUpdate(Permission permission)
        {
            _permissionService.SUpdate(permission);
            return Ok();
        }



        [HttpDelete("delete/{id}")]
        public IActionResult TimeOffDelete(int ID)
        {
            var resultGet = _permissionService.SGetById(ID);
            if (resultGet == null)
            {
                return BadRequest();
            }
            else
            {
                _permissionService.SDelete(resultGet);
                return Ok("Kullanıcı Başarıyla Silindi!");
            }

        }


        [HttpGet("getallmanager/{id}")]
        public IActionResult TimeOffManagerList(int ID)
        {
            var result = _permissionService.SGetAllManagerId(ID);

            if (result.Count == 0)
            {
                return Ok(
                    new { 
                        Message = "Bu manager'a ait bir izin gönderilmemiş!",  
                        Data = result
                    });
            }
            return Ok(result);
        }


        [HttpGet("getallemployee/{id}")]
        public IActionResult TimeOffEmployeeList(int ID)
        {
            var result = _permissionService.SGetAllEmployeeId(ID);

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
            //Todo: Eğer EmployeeId, bir manager rolüne karşılık geliyorsa izin oluşturulamasın. Ya da bunun üzerine sonra tartışalım

            var managerId = _userService.SGetById(permissionDto.ManagerId);
            
            if (managerId.UserRole != "Manager")
            {
                return BadRequest("Lütfen yönetici rolünde bir seçim yapın!");  //Todo: Bu mesaj değiştirilebilir
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

            var result = _permissionService.SInsertPermission(permission);

            if (result)
            {
                return Ok("İzin Başarıyla Gönderildi!");
            }
            return BadRequest("İzin Tarihleri Geçersiz!");    //Todo: Bu mesaj değiştirilebilir
        }
    }
}
