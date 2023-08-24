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
            _permissionService.SGetById(id);
            return Ok();
        }


        [HttpPut("update")]
        public IActionResult TimeOffUpdate(Permission permission)
        {
            _permissionService.SUpdate(permission);
            return Ok();
        }


        [HttpDelete("delete")]
        public IActionResult TimeOffDelete(Permission permission)
        {
            _permissionService.SDelete(permission);
            return Ok();
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
