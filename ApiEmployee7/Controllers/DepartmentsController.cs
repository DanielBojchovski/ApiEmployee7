using ApiEmployee7.Models;
using ApiEmployee7.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiEmployee7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentsController(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        [HttpGet("GetDepartments", Name = "Print all departments./ Печати ги сите сектори.")]
        [ResponseCache(Duration = 10)]
        public async Task<ActionResult> GetDepartments()
        {
            try
            {
                return Ok(await _departmentRepository.GetDepartments());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database./ Грешка при превземањето податоци од базата");
            }
        }

        [HttpGet("GetDepartment/{id:int}", Name = "Search department by id./ Пребарај сектор по идентификационен број.")]
        [ResponseCache(Duration = 10)]
        public async Task<ActionResult<Department>> GetDepartment(int id)
        {
            try
            {
                var result = await _departmentRepository.GetDepartment(id);

                if (result == null)
                {
                    return NotFound($"Department with id = {id} was not found./ Сектор со број = {id} не беше пронајден");
                }

                return result;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database./ Грешка при превземањето на податоци од базата");
            }
        }
    }
}
