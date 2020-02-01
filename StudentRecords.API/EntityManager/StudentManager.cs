using StudentRecords.API.Models;
using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace StudentRecords.API.EntityManager
{
    public class StudentManager
    {
        public async Task<DataTableResponse> Search(DataTableRequest request)
        {
            try
            {
                int recordsTotal = 0, recordsFiltered = 0;

                using (StudentRecordsEntities dc = new StudentRecordsEntities())
                {
                    dc.Configuration.LazyLoadingEnabled = false;

                    if (string.IsNullOrEmpty(request.Search.Value))
                    {
                        var v = (from a in dc.Students.AsNoTracking()
                                 orderby a.ID
                                 select new { a.ID, a.Name, a.Grade });

                        if (request.Order.Any())
                        {
                            v = v.OrderBy(request.Order[0].Column.ToString() + " " + request.Order[0].Dir);
                        }

                        recordsTotal = v.Count();

                        var data = await v.Skip(() => request.Start).Take(() => request.Length).ToListAsync();

                        return new DataTableResponse { draw = request.Draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                    }
                    else
                    {
                        var v = (from a in dc.Students.AsNoTracking()
                                 orderby a.ID
                                 select new { a.ID, a.Name, a.Grade });

                        recordsTotal = v.Count();

                        v = v.Where(x => x.Name.StartsWith(request.Search.Value));

                        if (request.Order.Any())
                        {
                            v = v.OrderBy(request.Order[0].Column.ToString() + " " + request.Order[0].Dir);
                        }

                        recordsFiltered = v.Count();

                        var data = await v.Skip(() => request.Start).Take(() => request.Length).ToListAsync();

                        return new DataTableResponse { draw = request.Draw, recordsFiltered = recordsFiltered, recordsTotal = recordsTotal, data = data };
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<Student> Show(int student_id)
        {
            using (StudentRecordsEntities db = new StudentRecordsEntities())
            {
                var std = await db.Students.AsNoTracking().Where(x => x.ID == student_id).FirstOrDefaultAsync();

                if (std != null)
                    return new Student { ID = std.ID, Name = std.Name, Grade = std.Grade, DateOfBirth = std.DateOfBirth, Address = std.Address, StudentFiles = std.StudentFiles };
                else
                    return null;
            }
        }

        public async Task<int> Add(Student std)
        {
            try
            {
                using (StudentRecordsEntities db = new StudentRecordsEntities())
                {
                    db.Students.Add(std);

                    return await db.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                return await Task.FromResult(0);
            }
        }

        public async Task<int> Update(Student std)
        {
            try
            {
                using (StudentRecordsEntities db = new StudentRecordsEntities())
                {
                    Student new_std = await db.Students.FindAsync(std.ID);
                    new_std.Name = std.Name;
                    new_std.Grade = std.Grade;
                    new_std.DateOfBirth = std.DateOfBirth;
                    new_std.Address = std.Address;

                    db.Entry(new_std).State = EntityState.Modified;

                    if (std.StudentFiles != null && std.StudentFiles.Count > 0)
                    {
                        foreach (var item in std.StudentFiles)
                        {
                            db.StudentFiles.Add(new StudentFile { FileSize = item.FileSize, FileName = item.FileName, StudentID = std.ID });
                        }
                    }

                    return await db.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                return await Task.FromResult(0);
            }
        }

        public async Task<int> Delete(int student_id)
        {
            try
            {
                using (StudentRecordsEntities db = new StudentRecordsEntities())
                {
                    Student std = await db.Students.FindAsync(student_id);

                    if (std != null)
                    {
                        if (db.StudentFiles.Any(x => x.StudentID == std.ID))
                        {
                            db.StudentFiles.RemoveRange(db.StudentFiles.Where(x => x.StudentID == std.ID));
                        }

                        db.Students.Remove(std);

                        return await db.SaveChangesAsync();
                    }
                    else
                        return await Task.FromResult(0);
                }
            }
            catch (Exception)
            {
                return await Task.FromResult(0);
            }
        }

        public async Task<string> DeleteFile(int file_id)
        {
            try
            {
                using (StudentRecordsEntities db = new StudentRecordsEntities())
                {
                    StudentFile file = await db.StudentFiles.FindAsync(file_id);

                    string x = file.FileName;

                    db.StudentFiles.Remove(file);

                    await db.SaveChangesAsync();

                    return await Task.FromResult(x);
                }
            }
            catch (Exception)
            {
                return await Task.FromResult("");
            }
        }


    }
}