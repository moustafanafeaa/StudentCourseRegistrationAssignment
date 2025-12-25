using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentCourseRegistrationAssignment.BLL.ViewModels
{
    public class CourseVM
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;

        [Range(1, 15)]
        public int Credits { get; set; }
        [Range(1, 15)]
        public int Semester { get; set; }

        public bool IsRegistered { get; set; } 
    }
}
