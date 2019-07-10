using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BangazonWorkforce.Models
{
    public class Department
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Department")]
        public string Name { get; set; }

        [Required]
        public int Budget { get; set; }
        public List<Employee> Employee{ get; set; } = new List<Employee>();
    }
}
