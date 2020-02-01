using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using StudentRecords.API.Models;
using System.Net.Http;
using System.Web;

namespace StudentRecords.Controllers
{
    public class DefaultController : Controller
    {
        private static string service_url = System.Configuration.ConfigurationManager.AppSettings["WebServiceUrl"].ToString();

        public ActionResult Index()
        {
            ViewBag.StdMsg = null;
            return View();
        }

        public ActionResult NewStudent()
        {
            ModelState.Clear();
            return PartialView("SaveStudent", null);
        }

        public async Task<ActionResult> ShowStudent(string student_id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(service_url);

                var response = await client.GetAsync($"Student/Show/{student_id}");

                if (response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var std = await response.Content.ReadAsAsync<Student>();
                    return PartialView("SaveStudent", std);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error try again later.");
                    return PartialView("SaveStudent", null);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveStudent(Student std)
        {
            if (std.ID == 0)
                ModelState.Remove("ID");

            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(service_url);

                    if (Request.Files.Count > 0)
                    {
                        for (int i = 0; i < Request.Files.Count; i++)
                        {
                            HttpPostedFileBase upload = Request.Files[i];

                            if (upload != null && upload.ContentLength > 0)
                            {
                                string fileName = Guid.NewGuid().ToString() + "-" + System.IO.Path.GetFileName(upload.FileName);
                                string filePath = "~/Uploads/" + fileName;
                                upload.SaveAs(Server.MapPath("~/Uploads/") + fileName);

                                std.StudentFiles.Add(new StudentFile { FileName = filePath, FileSize = upload.ContentLength, StudentID = std.ID });
                            }
                        }
                    }

                    dynamic response;

                    if (std.ID != 0)
                    {
                        response = await client.PutAsJsonAsync("Student/Update", std);
                    }
                    else
                    {
                        response = await client.PostAsJsonAsync("Student/Add", std);
                    }

                    if (response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        ViewBag.StdMsg = "success";
                        ModelState.Clear();
                        return PartialView("SaveStudent", null);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Server error try again later.");
                        return PartialView("SaveStudent", null);
                    }
                }
            }
            else
            {
                return PartialView("SaveStudent", std);
            }
        }

        [HttpPost]
        public async Task<ActionResult> DeleteStudent(string student_id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(service_url);

                var response = await client.DeleteAsync($"Student/Delete/{student_id}");

                if (response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    ViewBag.StdMsg = "success";
                    ModelState.Clear();
                    return PartialView("SaveStudent", null);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error try again later.");
                    return PartialView("SaveStudent", null);
                }
            }
        }

        [HttpPost]
        public async Task<JsonResult> DeleteStudentFile(string file_id)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(service_url);

                    var response = await client.GetAsync($"Student/DeleteFile/{file_id}");

                    if (response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string img_path = await response.Content.ReadAsAsync<string>();

                        System.IO.File.Delete(Server.MapPath(img_path));

                        return Json(new { res = "success" });
                    }
                    else
                    {
                        return Json(new { res = "error" });
                    }
                }
            }
            catch (Exception)
            {
                return Json(new { res = "error" });
            }
        }



    }
}