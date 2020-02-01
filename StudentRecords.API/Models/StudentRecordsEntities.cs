using System.Data.Entity;

namespace StudentRecords.API.Models
{
    public class StudentRecordsEntities : DbContext
    {
        public StudentRecordsEntities() : base("name=StudentRecordsEntities")
        {
        }

        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<StudentFile> StudentFiles { get; set; }
    }
}