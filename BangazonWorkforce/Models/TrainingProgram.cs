// Author: Billy Mitchell
// This is a Model which is intended to represent the primary properties for each training program.

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BangazonWorkforce.Models
{
    public class TrainingProgram
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public int MaxAttendees { get; set; }
        public List<Employee> employees { get; set; } = new List<Employee>();
        //public string IsComplete()
        //{
        //    DateTime now = DateTime.Now;
        //    if(StartDate >= now)
        //    {
        //        return "false";
        //    } else
        //    {
        //        return "true";
        //    }
        //}
    }
}
