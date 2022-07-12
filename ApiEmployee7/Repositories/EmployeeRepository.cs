using ApiEmployee7.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiEmployee7.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly EmployeeContext _context;
        public EmployeeRepository(EmployeeContext context)
        {
            _context = context;
        }

        //COMMANDS
        public async Task<Employee> Create(Employee employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return employee;
        }

        public async Task<Employee> Update(Employee employee)
        {
            var result = await _context.Employees
                .FirstOrDefaultAsync(e => e.EmployeeId == employee.EmployeeId);

            if (result != null)
            {
                result.FirstName = employee.FirstName;
                result.LastName = employee.LastName;
                result.Salary = employee.Salary;
                result.Email = employee.Email;
                result.DateOfBrith = employee.DateOfBrith;
                result.Gender = employee.Gender;
                result.Skills = employee.Skills;
                result.Password = employee.Password;
                result.ConfirmPassword = employee.ConfirmPassword;
                if (employee.DepartmentId != 0)
                {
                    result.DepartmentId = employee.DepartmentId;
                }
                else if (employee.Department != null)
                {
                    result.DepartmentId = employee.Department.DepartmentId;
                }
                result.PhotoPath = employee.PhotoPath;

                await _context.SaveChangesAsync();

                return result;
            }

            return null;
        }

        public async Task Delete(int employeeId)
        {
            var employeeToDelete = await _context.Employees.FindAsync(employeeId);
            _context.Employees.Remove(employeeToDelete);
            await _context.SaveChangesAsync();
        }

        //QUERIES
        public async Task<IEnumerable<Employee>> Get()
        {
            return await _context.Employees.ToListAsync(); 
        }

        public async Task<Employee> Get(int employeeId)
        {
            return await _context.Employees.FindAsync(employeeId);
        }

        public async Task<Employee> GetEmployeeByEmail(string email)
        {
            return await _context.Employees.FirstOrDefaultAsync(e => e.Email == email);
        }

        public async Task<IEnumerable<Employee>> SearchByName(string name)
        {
            IQueryable<Employee> query = _context.Employees;

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(e => e.FirstName.Contains(name)
                            || e.LastName.Contains(name));
            }
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Employee>> SearchBySkill(string skill)
        {
            IQueryable<Employee> query = _context.Employees;

            if (!string.IsNullOrEmpty(skill))
            {
                query = query.Where(e => e.Skills.Contains(skill));
            }
            return await query.ToListAsync();
        }

        public async Task<double> GetAverageSalary()
        {
            double sum = 0;
            int length = _context.Employees.Count();
            foreach (var employee in _context.Employees)
            {
                sum += employee.Salary;
            }
            await Task.CompletedTask;
            return sum / length;
        }

        public async Task<double> GetTotalSumOfSalaries()
        {
            double sum = 0;
            foreach (var employee in _context.Employees)
            {
                sum += employee.Salary;
            }
            await Task.CompletedTask;
            return sum;
        }

        public async Task<IEnumerable<Employee>> GetSalariesBiggerThen(double salary)
        {
            IQueryable<Employee> query = _context.Employees;

            query = query.Where(e => e.Salary > salary);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetSalariesLessThen(double salary)
        {
            IQueryable<Employee> query = _context.Employees;

            query = query.Where(e => e.Salary < salary);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetSalariesInRange(double startPoint, double endPoint)
        {
            IQueryable<Employee> query = _context.Employees;

            query = query.Where(e => e.Salary >= startPoint && e.Salary <= endPoint);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetAgeLessThen(int age)
        {
            IQueryable<Employee> query = _context.Employees;

            query = query.Where(e => DateTime.Now.Year - e.DateOfBrith.Year <= age);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetAgeBiggerThen(int age)
        {
            IQueryable<Employee> query = _context.Employees;

            query = query.Where(e => DateTime.Now.Year - e.DateOfBrith.Year >= age);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetAgeInRange(int startPoint, int endPoint)
        {
            IQueryable<Employee> query = _context.Employees;

            query = query.Where(e => DateTime.Now.Year - e.DateOfBrith.Year >= startPoint && DateTime.Now.Year - e.DateOfBrith.Year <= endPoint);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Employee>> SearchByDepartmentName(string departmentName)
        {
            IQueryable<Employee> query = _context.Employees;

            if (!string.IsNullOrEmpty(departmentName))
            {
                query = query.Where(e => e.Department.DepartmentName.Contains(departmentName));
            }
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Employee>> SearchByGender(string gender)
        {
            IQueryable<Employee> query = _context.Employees;

            switch (gender.ToLower())
            {
                case "male":
                    query = query.Where(e => e.Gender.Value.Equals(Gender.Male));
                    break;
                case "female":
                    query = query.Where(e => e.Gender.Value.Equals(Gender.Female));
                    break;
                case "other":
                    query = query.Where(e => e.Gender.Value.Equals(Gender.Other));
                    break;
                default:
                    IQueryable<Employee> emptyQuery = new Employee[] { }.AsQueryable();
                    return emptyQuery;
                    break;
            }
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Employee>> OrderByAgeDescending()
        {
            IQueryable<Employee> query = _context.Employees;

            query = query.OrderBy(e => e.DateOfBrith);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Employee>> OrderByAgeAscending()
        {
            IQueryable<Employee> query = _context.Employees;

            query = query.OrderByDescending(e => e.DateOfBrith);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Employee>> OrderBySalaryDescending()
        {
            IQueryable<Employee> query = _context.Employees;

            query = query.OrderByDescending(e => e.Salary);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Employee>> OrderBySalaryAscending()
        {
            IQueryable<Employee> query = _context.Employees;

            query = query.OrderBy(e => e.Salary);

            return await query.ToListAsync();
        }
    }
}
