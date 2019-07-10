using System.ComponentModel.DataAnnotations;

namespace BangazonWorkforce.Models
{
    public class Employee
    {
        public int Id { get; set; }

        //[Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        //[Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        //[Required]
        [Display(Name = "Department ID")]
        public int DepartmentId { get; set; }

        //[Required]
        [Display(Name = "Is A Supervisor")]

        public bool IsSuperVisor { get; set; }
        //[Required]
        [Display(Name = "Department Name")]
        public Department department { get; set; }
    }
}
