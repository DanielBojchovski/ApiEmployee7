using ApiEmployee7.Models;

namespace ApiEmployee7.Repositories
{
    public interface IEmployeeRepository
    {
        //COMMANDS
        Task<Employee> Create(Employee employee);
        Task<Employee> Update(Employee employee);
        Task Delete(int employeeId);

        //QUERIES
        Task<IEnumerable<Employee>> Get();
        Task<Employee> Get(int employeeId);
        Task<Employee> GetEmployeeByEmail(string email);
        Task<IEnumerable<Employee>> SearchByName(string name);
        Task<IEnumerable<Employee>> SearchBySkill(string skill);
        Task<double> GetAverageSalary();
        Task<double> GetTotalSumOfSalaries();
        Task<IEnumerable<Employee>> GetSalariesBiggerThen(double salary);
        Task<IEnumerable<Employee>> GetSalariesLessThen(double salary);
        Task<IEnumerable<Employee>> GetSalariesInRange(double startPoint, double endPoint);
        Task<IEnumerable<Employee>> GetAgeLessThen(int age);
        Task<IEnumerable<Employee>> GetAgeBiggerThen(int age);
        Task<IEnumerable<Employee>> GetAgeInRange(int startPoint, int endPoint);
        Task<IEnumerable<Employee>> SearchByDepartmentName(string departmentName);
        Task<IEnumerable<Employee>> SearchByGender(string gender);
        Task<IEnumerable<Employee>> OrderByAgeDescending();
        Task<IEnumerable<Employee>> OrderByAgeAscending();
        Task<IEnumerable<Employee>> OrderBySalaryDescending();
        Task<IEnumerable<Employee>> OrderBySalaryAscending();
    }
}
