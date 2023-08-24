using Microsoft.AspNetCore.Mvc;
using Time_Off_Tracker.Business.Abstract;
using Time_Off_Tracker.Entity.Concrete;
using Time_Off_Tracker.Entity.Dto;

namespace Time_Off_Tracker.API.Controllers
{
    [Route("time-off")]
    [ApiController]
    public class TimeOffController : ControllerBase
    {
        private IPermissionService _permissionService;

        public TimeOffController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
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



        [HttpDelete("delete/{ID}")]
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


        [HttpGet("getall")]
        public IActionResult TimeOffList()
        {
            var result = _permissionService.SGetList();
            return Ok(result);
        }


        [HttpPost("add")]
        public IActionResult TimeOffAdd(PermissionDto permissionDto)
        {
            Permission permission = new()
            {
                EmployeID = permissionDto.EmployeeId,
                ManagerID = permissionDto.ManagerId,
                Description = permissionDto.Description,
                StartDate = permissionDto.StartDate,
                EndDate = permissionDto.EndDate,
                TimeOffType = "Pending"
            };

            _permissionService.SInsert(permission);
            return Ok();
        }
    }
}
