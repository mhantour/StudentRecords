using System;
using System.Data.Entity.Migrations;

namespace StudentRecords.API.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<Models.StudentRecordsEntities>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(Models.StudentRecordsEntities context)
        {
            context.Students.AddOrUpdate(x => x.ID,
                new Models.Student() { ID = 1, Name = "Mohammed Hantour", Grade = 3.5, DateOfBirth = DateTime.Now.AddYears(-30), Address = "Egypt" },
                new Models.Student() { ID = 2, Name = "Mohammed", Grade = 2.1, DateOfBirth = DateTime.Now.AddYears(-25), Address = "Saudi Arabia" },
                new Models.Student() { ID = 3, Name = "Ahmed", Grade = 4, DateOfBirth = DateTime.Now.AddYears(-37), Address = "Kuwait" });

            base.Seed(context);
        }
    }
}
