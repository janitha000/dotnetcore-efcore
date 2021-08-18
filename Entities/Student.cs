using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace efcore.Entities
{
    public class Student 
    {
        public int ID { get; set; }
        
        [StringLength(50)]
        [Column("StartingName")]
        [Required]
        public string FirstName { get; set; }
        
        [StringLength(50)]
        [Required]
        public string LastName { get; set; }
        
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString="{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EnrollmentDate { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }

        public string FullName 
        { 
            get
            {
                return LastName + ", " + FirstName;
            }
        }
        
    } 
}