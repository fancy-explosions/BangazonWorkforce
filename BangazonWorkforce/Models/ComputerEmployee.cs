﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonWorkforce.Models
{
    public class ComputerEmployee
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        [Required]
        public int ComputerId { get; set; }

        [Required]
        public DateTime AssignDate { get; set; }

        public DateTime UnassignDate { get; set; }
    }
}
