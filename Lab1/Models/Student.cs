using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FormIntro.Models
{
    public class Student
    {
        [Required(ErrorMessage = "Please provide a name")]
        [StringLength(10, ErrorMessage = "Name cannot exceed 10 characters")]
        [RegularExpression(@"^[a-zA-Z.-]+$", ErrorMessage = "Name can only contain alphabets, '.' and '-'")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please provide a valid ID")]
        [RegularExpression(@"^\d{2}-\d{5}-[1-3]$", ErrorMessage = "ID must be in the format 'xx-xxxxx-x', where xx is 00-99, xxxxx is 0-9, and the last digit is between 1-3.")]
        public string Id { get; set; }

        [Required(ErrorMessage = "Date of Birth is required")]
        [MinimumAge(20, ErrorMessage = "Age must be at least 20 years.")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
    }
}