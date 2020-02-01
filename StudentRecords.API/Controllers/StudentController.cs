using StudentRecords.API.EntityManager;
using StudentRecords.API.Models;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace StudentRecords.API.Controllers
{
    [RoutePrefix("api/Student")]
    public class StudentController : ApiController
    {
        [HttpPost]
        [Route("Search")]
        public async Task<IHttpActionResult> Search([FromBody]DataTableRequest request)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var stds = await new StudentManager().Search(request);

                    if (stds != null)
                        return Ok(stds);
                    else
                        return new System.Web.Http.Results.StatusCodeResult(HttpStatusCode.NoContent, this.Request);
                }
                else
                    return BadRequest("error while validating your request");
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("Show/{studentId}")]
        public async Task<IHttpActionResult> Show(int studentId)
        {
            try
            {
                if (studentId > 0)
                {
                    var std = await new StudentManager().Show(studentId);

                    if (std != null)
                        return Ok(std);
                    else
                        return new System.Web.Http.Results.StatusCodeResult(HttpStatusCode.NoContent, this.Request);
                }
                else
                    return BadRequest("error while validating your request");
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IHttpActionResult> Add([FromBody]Student std)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int status = await new StudentManager().Add(std);

                    if (status > 0)
                        return Ok();
                    else
                        return NotFound();
                }
                else
                    return BadRequest("error while validating your request");
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IHttpActionResult> Update([FromBody]Student std)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int status = await new StudentManager().Update(std);

                    if (status > 0)
                        return Ok();
                    else
                        return NotFound();
                }
                else
                    return BadRequest("error while validating your request");
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        [HttpDelete]
        [Route("Delete/{studentId}")]
        public async Task<IHttpActionResult> Delete(int studentId)
        {
            try
            {
                if (studentId > 0)
                {
                    int status = await new StudentManager().Delete(studentId);

                    if (status > 0)
                        return Ok();
                    else
                        return NotFound();
                }
                else
                    return BadRequest("error while validating your request");
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("DeleteFile/{fileId}")]
        public async Task<IHttpActionResult> DeleteFile(int fileId)
        {
            try
            {
                if (fileId > 0)
                {
                    string status = await new StudentManager().DeleteFile(fileId);

                    if (!string.IsNullOrWhiteSpace(status))
                        return Ok(status);
                    else
                        return Ok("error");
                }
                else
                    return BadRequest("error while validating your request");
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }


    }
}
