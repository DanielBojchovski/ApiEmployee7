using ApiEmployee7.Models;
using ApiEmployee7.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiEmployee7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeRepository _empRepository;
        private readonly IMapper _mapper;
        public EmployeesController(IEmployeeRepository empRepository, IMapper mapper)
        {
            _empRepository = empRepository;
            _mapper = mapper;
        }

        //COMMANDS

        [HttpPost("PostEmployees", Name = "Enter Employee./ Внеси вработен.")]
        public async Task<ActionResult<Employee>> PostEmployees(Employee employee)
        {
            try
            {
                if (employee == null)
                    return BadRequest();

                var emp = await _empRepository.GetEmployeeByEmail(employee.Email);

                if (emp != null)
                {
                    ModelState.AddModelError("Email", "Employee email already in use./ Веќе имаме вработен со оваа email адреса");
                    return BadRequest(ModelState);
                }

                var createdEmployee = await _empRepository.Create(employee);

                return CreatedAtAction(nameof(GetEmployee),
                    new { id = createdEmployee.EmployeeId }, createdEmployee);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error creating new employee record./ Грешка при креирање нов вработен.");
            }
        }

        [HttpPut("UpdateEmployee/{id:int}", Name = "Update employee./ Измени податоци за вработен.")]
        public async Task<ActionResult<Employee>> UpdateEmployee(int id, Employee employee)
        {
            try
            {
                if (id != employee.EmployeeId)
                    return BadRequest("Employee ID mismatch./ Неусогласеност помеѓу индентификациониот број и вработениот");

                var employeeToUpdate = await _empRepository.Get(id);

                if (employeeToUpdate == null)
                {
                    return NotFound($"Employee with Id = {id} not found./ Вработен со број = {id} не беше пронајден");
                }

                return await _empRepository.Update(employee);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error updating employee record./ Грешка при ажурирање на вработениот");
            }
        }

        [HttpDelete("DeleteEmployee/{id:int}", Name = "Delete employee./ Избриши вработен.")]
        public async Task<ActionResult> DeleteEmployee(int id)
        {
            try
            {
                var employeeToDelete = await _empRepository.Get(id);

                if (employeeToDelete == null)
                {
                    return NotFound($"Employee with Id = {id} not found./ Вработен со број = {id} не беше пронајден");
                }

                await _empRepository.Delete(id);

                return Ok($"Employee with Id = {id} deleted./ Вработен со број = {id} е избришан");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error deleting employee record. No employee was deleted./ Грешка при бришење на вработен. Ниту еден вработен не беше избришан");
            }
        }

        //QUERIES

        [HttpGet("GetEmployees", Name = "Print all employees./ Печати ги сите вработени.")]
        public async Task<ActionResult> GetEmployees()
        {
            try
            {
                return Ok(await _empRepository.Get());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database./ Грешка при превземањето на податоци од базата");
            }
        }

        [HttpGet("GetEmployeesDTO", Name = "Print all employees with DTO./ Печати ги сите вработени.")]
        public async Task<ActionResult> GetEmployeesDTO()
        {
            try
            {
                var employeesDTO = _mapper.Map<IEnumerable<EmployeeDTO>>(await _empRepository.Get());
                return Ok(employeesDTO);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database./ Грешка при превземањето на податоци од базата");
            }
        }

        [HttpGet("GetEmployee/{id:int}", Name = "Get employee by id./ Пребарај вработен по идентификационен број.")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            try
            {
                var result = await _empRepository.Get(id);

                if (result == null)
                {
                    return NotFound($"Employee with id = {id} does not exist./ Вработен со идентификационен број = {id} не постои.");
                }

                return result;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database./ Грешка при превземањето на податоци од базата");
            }
        }

        [HttpGet("SearchByName/{name}", Name = "Search the employee by name./ Пребарај го вработениот по име.")]
        public async Task<ActionResult<IEnumerable<Employee>>> SearchByName(string name)
        {
            try
            {
                var result = await _empRepository.SearchByName(name);
                if (result.Any())
                {
                    return Ok(result);
                }
                return NotFound($"Employee with name = {name} was not found./ Вработен со име = {name} не беше пронајден");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                "Error retrieving data from the database./ Грешка при превземањето на податоци од базата");
            }
        }

        [HttpGet("SearchBySkill/{skill}", Name = "Search the employees by skill./ Пребарај ги вработените по вештина.")]
        public async Task<ActionResult<IEnumerable<Employee>>> SearchBySkill(string skill)
        {
            try
            {
                var result = await _empRepository.SearchBySkill(skill);

                if (result.Any())
                {
                    return Ok(result);
                }

                return NotFound($"Employee with skill = {skill} was not found./ Вработен со вештина = {skill} не беше пронајден");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                "Error retrieving data from the database./ Грешка при превземањето на податоци од базата");
            }
        }

        [HttpGet("GetAverageSalary", Name = "Print average salary in the company./ Печати просечна плата во компанијата.")]
        public async Task<double> GetAverageSalary()
        {
            return await _empRepository.GetAverageSalary();
        }

        [HttpGet("GetTotalSumOfSalaries", Name = "Print sum of all salaries in the company./ Печати ја сумата од платите во компанијата.")]
        public async Task<double> GetTotalSumOfSalaries()
        {
            return await _empRepository.GetTotalSumOfSalaries();
        }

        [HttpGet("GetSalariesBiggerThen/{salary:double}", Name = "Search the employees with salary bigger than your value./ Пребарај ги вработените со плата поголема од вашата вредност.")]
        public async Task<ActionResult<IEnumerable<Employee>>> GetSalariesBiggerThen(double salary)
        {
            try
            {
                var result = await _empRepository.GetSalariesBiggerThen(salary);

                if (result.Any())
                {
                    return Ok(result);
                }

                return NotFound($"No employee with salary bigger than {salary} was found./ Нема вработен со плата поголема од {salary}");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                "Error retrieving data from the database./ Грешка при превземањето на податоци од базата");
            }
        }

        [HttpGet("GetSalariesLessThen/{salary:double}", Name = "Search the employees with salary smaller than your value./ Пребарај ги вработените со плата помала од вашата вредност.")]
        public async Task<ActionResult<IEnumerable<Employee>>> GetSalariesLessThen(double salary)
        {
            try
            {
                var result = await _empRepository.GetSalariesLessThen(salary);

                if (result.Any())
                {
                    return Ok(result);
                }

                return NotFound($"No employee with salary smaller than {salary} was found./ Нема вработен со плата помала од {salary}");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                "Error retrieving data from the database./ Грешка при превземањето на податоци од базата");
            }
        }

        [HttpGet("GetSalariesInRange/{startPoint:double}/{endPoint:double}", Name = "Search the employees with salary within your range./ Пребарај ги вработените со плата во опсег на вашите вредности.")]
        public async Task<ActionResult<IEnumerable<Employee>>> GetSalariesInRange(double startPoint, double endPoint)
        {
            try
            {
                var result = await _empRepository.GetSalariesInRange(startPoint, endPoint);

                if (result.Any())
                {
                    return Ok(result);
                }

                return NotFound($"No employees with salary within range: {startPoint} - {endPoint} was found./ Нема вработени со плата во опсег {startPoint} - {endPoint}");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                "Error retrieving data from the database./ Грешка при превземањето на податоци од базата");
            }
        }

        [HttpGet("GetAgeLessThen/{age:int}", Name = "Search the employees with age smaller than your value./ Пребарај ги вработените со помалку години од вашата вредност.")]
        public async Task<ActionResult<IEnumerable<Employee>>> GetAgeLessThen(int age)
        {
            try
            {
                var result = await _empRepository.GetAgeLessThen(age);

                if (result.Any())
                {
                    return Ok(result);
                }

                return NotFound($"No employee with age smaller than {age} was found./ Нема вработен со години помалку од {age}");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                "Error retrieving data from the database./ Грешка при превземањето на податоци од базата");
            }
        }

        [HttpGet("GetAgeBiggerThen/{age:int}", Name = "Search the employees with age bigger than your value./ Пребарај ги вработените со повеќе години од вашата вредност.")]
        public async Task<ActionResult<IEnumerable<Employee>>> GetAgeBiggerThen(int age)
        {
            try
            {
                var result = await _empRepository.GetAgeBiggerThen(age);

                if (result.Any())
                {
                    return Ok(result);
                }

                return NotFound($"No employee with age bigger than {age} was not found./ Нема вработен со години повеќе од {age}");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                "Error retrieving data from the database./ Грешка при превземањето на податоци од базата");
            }
        }

        [HttpGet("GetAgeInRange/{startPoint:int}/{endPoint:int}", Name = "Search the employees with age within your range./ Пребарај ги вработените со години во опсег на вашите вредности.")]
        public async Task<ActionResult<IEnumerable<Employee>>> GetAgeInRange(int startPoint, int endPoint)
        {
            try
            {
                var result = await _empRepository.GetAgeInRange(startPoint, endPoint);

                if (result.Any())
                {
                    return Ok(result);
                }

                return NotFound($"No employees with age within range: {startPoint} - {endPoint} was found./ Нема вработени со години во опсег {startPoint} - {endPoint}");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                "Error retrieving data from the database./ Грешка при превземањето на податоци од базата");
            }
        }

        [HttpGet("SearchByDepartmentName/{departmentName}", Name = "Search the employees by department./ Пребарај ги вработените по сектор.")]
        public async Task<ActionResult<IEnumerable<Employee>>> SearchByDepartmentName(string departmentName)
        {
            try
            {
                var result = await _empRepository.SearchByDepartmentName(departmentName);

                if (result.Any())
                {
                    return Ok(result);
                }

                return NotFound($"No employee was found in department = {departmentName}./ Нема вработени во сектор = {departmentName}.");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                "Error retrieving data from the database./ Грешка при превземањето на податоци од базата");
            }
        }

        [HttpGet("SearchByGender/{gender}", Name = "Search the employees by gender./ Пребарај ги вработените по пол.")]
        public async Task<ActionResult<IEnumerable<Employee>>> SearchByGender(string gender)
        {
            try
            {
                var result = await _empRepository.SearchByGender(gender);

                if (result.Any())
                {
                    return Ok(result);
                }

                return NotFound($"No employee was found with gender = {gender}./ Нема вработени со пол = {gender}.");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                "Error retrieving data from the database./ Грешка при превземањето на податоци од базата");
            }
        }

        [HttpGet("OrderByAgeDescending", Name = "Order the employees by age descending./ Пребарај ги вработените по возраст опаѓачки.")]
        public async Task<ActionResult<IEnumerable<Employee>>> OrderByAgeDescending()
        {
            try
            {
                var result = await _empRepository.OrderByAgeDescending();

                return Ok(result);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                "Error retrieving data from the database./ Грешка при превземањето на податоци од базата");
            }
        }

        [HttpGet("OrderByAgeAscending", Name = "Order the employees by age ascending./ Пребарај ги вработените по возраст растечки.")]
        public async Task<ActionResult<IEnumerable<Employee>>> OrderByAgeAscending()
        {
            try
            {
                var result = await _empRepository.OrderByAgeAscending();

                return Ok(result);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                "Error retrieving data from the database./ Грешка при превземањето на податоци од базата");
            }
        }

        [HttpGet("OrderBySalaryDescending", Name = "Order the employees by salary descending./ Пребарај ги вработените по плата опаѓачки.")]
        public async Task<ActionResult<IEnumerable<Employee>>> OrderBySalaryDescending()
        {
            try
            {
                var result = await _empRepository.OrderBySalaryDescending();

                return Ok(result);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                "Error retrieving data from the database./ Грешка при превземањето на податоци од базата");
            }
        }

        [HttpGet("OrderBySalaryAscending", Name = "Order the employees by salary ascending./ Пребарај ги вработените по плата .")]
        public async Task<ActionResult<IEnumerable<Employee>>> OrderBySalaryAscending()
        {
            try
            {
                var result = await _empRepository.OrderBySalaryAscending();

                return Ok(result);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                "Error retrieving data from the database./ Грешка при превземањето на податоци од базата");
            }
        }
    }
}
