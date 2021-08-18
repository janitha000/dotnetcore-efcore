using System;

namespace efcore.Dtos
{
    public record EnrollmentDateGroup
    {
        public DateTime? EnrollmentDate { get; set; }
        public int StudentCount { get; set; }
    }
}