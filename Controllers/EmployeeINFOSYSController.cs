using Microsoft.AspNetCore.Mvc;
using EmployeeSystem.BL;
using EmployeeSystem.DL;
using EmployeeSystem.Models;

namespace EmployeeManagement.Controllers
{
    [ApiController]
    [Route("api/employees")]
    public class EMPLOYEEINFOSYSController : ControllerBase
    {
        private readonly EmployeeLogic _logic;

        public EMPLOYEEINFOSYSController()
        {
            EmployeeData data = new EmployeeData();
            _logic = new EmployeeLogic(data);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_logic.GetData().EmployeeList);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            var emp = _logic.GetData().EmployeeList.Find(e => e.Id == id);
            if (emp == null) return NotFound();
            return Ok(emp);
        }
        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            try
            {
                employee.Status = "Active";

                var result = _logic.HireEmployee(
                    employee.Id,
                    employee.Name,
                    employee.Position,
                    employee.Department
                );

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPut("{id}")]
        public IActionResult Update(string id, [FromBody] UpdateEmployeeDto dto)
        {
            if (dto == null)
                return BadRequest("Invalid request body");

            var result = _logic.UpdateEmployee(
                id,
                dto.Name,
                dto.Position,
                dto.Department
            );

            if (result == "")
                return NotFound("Employee not found");

            return Ok(result);
        }

        [HttpPut("{id}/transfer")]
        public IActionResult Transfer(string id, [FromBody] string newDepartment)
        {
            var result = _logic.TransferEmployee(id, newDepartment);
            if (result == "") return NotFound();
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var result = _logic.DeleteEmployee(id);
            if (result == "") return NotFound();
            return Ok(result);
        }
    }
}