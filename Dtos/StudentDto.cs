using System;
using System.ComponentModel.DataAnnotations;

namespace efcore.Dtos
{
    public record StudentDto
    {
        [Required]
        public string FirstName { get; init; }
        [Required]
        public string LastName { get; init; }
        [Required]
        public DateTime EnrollmentDate { get; init; }
    }
}