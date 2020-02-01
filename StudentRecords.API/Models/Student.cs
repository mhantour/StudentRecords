using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentRecords.API.Models
{

    [Table("Student", Schema = "dbo")]
    public class Student
    {
        public Student()
        {
            this.StudentFiles = new HashSet<StudentFile>();
        }

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [Column(TypeName = "NVARCHAR")]
        [StringLength(250)]
        public string Name { get; set; }

        [Required]
        public double Grade { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [Column(TypeName = "NVARCHAR")]
        [StringLength(1000)]
        public string Address { get; set; }

        public virtual ICollection<StudentFile> StudentFiles { get; set; }

    }
}