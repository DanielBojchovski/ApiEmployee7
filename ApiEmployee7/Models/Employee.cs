using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiEmployee7.Models
{
    public class Employee
    {
        [Key]
        [Display(Name = "Employee ID")]
        [Range(1, int.MaxValue)]
        public int EmployeeId { get; set; }


        [Required(ErrorMessage = "First Name is required./ Името е задолжително")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "FirstName must be between 2 and 50 characters./ Името треба да биде помеѓу 2 и 50 букви.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }


        [Required(ErrorMessage = "Last Name is required./ Презимето е задолжително")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "LastName must be between 2 and 50 characters./ Презимето треба да биде помеѓу 2 и 50 букви.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }


        [Required(ErrorMessage = "Employee's salary is required./ Платата на вработениот е задолжителна")]
        [Range(18000, 617500, ErrorMessage = "Salary must be in range between 18000 and 617500 denars./ Платата мора да биде во опсег помеѓу 18000 и 617500 денари.")]
        public double Salary { get; set; }


        [Required(ErrorMessage = "Employee's email address is required./ Email адресата на вработениот е задолжителна")]
        [StringLength(50, ErrorMessage = "Email's length cannot exceed 50 characters./ Email адресата не смее да надмине 50 знаци")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Employee's password address is required./ Лозинката на вработениот е задолжителна")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Password must be between 2 and 50 characters./ Лозинката треба да биде помеѓу 2 и 50 букви.")]
        [Compare("ConfirmPassword", ErrorMessage = "Password mismatch./ Неусогласеност помеѓѕ двете лозинки.")]
        public string Password { get; set; }


        [Required]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Employee's date of birth is required./ Датата на раѓање на вработениот е задолжителна")]
        [Display(Name = "Date Of Birth")]
        public DateTime DateOfBrith { get; set; }


        public Gender? Gender { get; set; }


        [Display(Name = "Photo Path")]
        public string? PhotoPath { get; set; }


        [Required(ErrorMessage = "Department id is required./ Идентификациониот број на секторот е задолжителен")]
        [ForeignKey("Department")]
        [Display(Name = "Department ID")]
        public int DepartmentId { get; set; }


        [Required(ErrorMessage = "Employee's department is required./ Секторот на вработениот е задолжителен")]
        public Department Department { get; set; }


        public string? Skills { get; set; }

        public DateTime StartingDayInCompany => DateTime.Now;
        public DateTime ExpirationOfContract => DateTime.Now.AddMonths(18);
    }
}
