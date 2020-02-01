using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudentRecords.API.Models;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace StudentRecords.Tests
{
    [TestClass]
    public class StudentUnitTest
    {
        private StudentRecordsEntities db;
        private API.Controllers.StudentController controller;

        [TestInitialize]
        public void InitializeTestParams()
        {
            db = new StudentRecordsEntities();
            controller = new API.Controllers.StudentController();
        }

        [TestMethod]
        public async Task Should_Get_All_Students()
        {
            int act_count = db.Students.Count();

            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            IHttpActionResult actionResult = await controller.Search(new DataTableRequest
            {
                Draw = 1,
                Length = 10,
                Start = 1,
                Search = new DataTableSearch { Value = "" },
                Order = new DataTableOrder[0],
            });
            var contentResult = actionResult as OkNegotiatedContentResult<DataTableResponse>;

            Assert.IsNotNull(contentResult);
            Assert.AreEqual(act_count, contentResult.Content.recordsTotal);
        }

        [TestMethod]
        public async Task Should_Not_Get_All_Students()
        {
            int act_count = db.Students.Count();

            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            IHttpActionResult actionResult = await controller.Search(new DataTableRequest
            {
                Draw = 1,
                Length = 10,
                Start = 1
            });
            var contentResult = actionResult as NegotiatedContentResult<DataTableResponse>;

            Assert.IsNull(contentResult);
            Assert.IsInstanceOfType(actionResult, typeof(StatusCodeResult));
        }

        [TestMethod]
        public async Task Get_One_Student_Should_Fail_Validation()
        {
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            IHttpActionResult actionResult = await controller.Show(0);

            Assert.IsInstanceOfType(actionResult, typeof(BadRequestErrorMessageResult));
        }

        [TestMethod]
        public async Task Should_Get_One_Student()
        {
            var std = db.Students.Find(2);

            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            IHttpActionResult actionResult = await controller.Show(2);
            var contentResult = actionResult as OkNegotiatedContentResult<Student>;

            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(std.Name, contentResult.Content.Name);
        }

        [TestMethod]
        public async Task Should_Not_Get_One_Student()
        {
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            IHttpActionResult actionResult = await controller.Show(10000);
            var contentResult = actionResult as NegotiatedContentResult<Student>;

            Assert.IsNull(contentResult);
            Assert.IsInstanceOfType(actionResult, typeof(StatusCodeResult));
        }

        [TestMethod]
        public async Task Add_Student_Should_Fail_Validation()
        {
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            IHttpActionResult actionResult = await controller.Add(new Student { Grade = 3.5, DateOfBirth = DateTime.Now.AddYears(-15), Address = "Egypt" });

            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Should_Add_Student()
        {
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            IHttpActionResult actionResult = await controller.Add(new Student { Name = "Mohammed Hantour", Grade = 3.5, DateOfBirth = DateTime.Now.AddYears(-15), Address = "Egypt" });

            Assert.IsInstanceOfType(actionResult, typeof(OkResult));
        }

        [TestMethod]
        public async Task Update_Student_Should_Fail_Validation()
        {
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            IHttpActionResult actionResult = await controller.Update(new Student { Name = "Mohammed Hantour", Grade = 3.5, DateOfBirth = DateTime.Now.AddYears(-15), Address = "Egypt" });

            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Should_Update_Student()
        {
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            IHttpActionResult actionResult = await controller.Update(new Student {ID = 2,  Name = "Mohammed Hantour", Grade = 3.5, DateOfBirth = DateTime.Now.AddYears(-30), Address = "Egypt" });

            Assert.IsInstanceOfType(actionResult, typeof(OkResult));
        }

        [TestMethod]
        public async Task Delete_Student_Should_Fail_Validation()
        {
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            IHttpActionResult actionResult = await controller.Delete(0);

            Assert.IsInstanceOfType(actionResult, typeof(BadRequestErrorMessageResult));
        }

        [TestMethod]
        public async Task Should_Delete_Student()
        {
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            IHttpActionResult actionResult = await controller.Delete(3);

            Assert.IsInstanceOfType(actionResult, typeof(OkResult));
        }

        [TestMethod]
        public async Task Should_Not_Delete_Student()
        {
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            IHttpActionResult actionResult = await controller.Delete(10000);

            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Delete_StudentFile_Should_Fail_Validation()
        {
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            IHttpActionResult actionResult = await controller.DeleteFile(0);

            Assert.IsInstanceOfType(actionResult, typeof(BadRequestErrorMessageResult));
        }

        [TestMethod]
        public async Task Should_Delete_StudentFile()
        {
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            IHttpActionResult actionResult = await controller.DeleteFile(3);
            var contentResult = actionResult as OkNegotiatedContentResult<string>;

            Assert.IsNotNull(contentResult);
            Assert.AreNotEqual("error", contentResult.Content.ToString());
        }

        [TestMethod]
        public async Task Should_Not_Delete_StudentFile()
        {
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            IHttpActionResult actionResult = await controller.DeleteFile(10000);
            var contentResult = actionResult as OkNegotiatedContentResult<string>;

            Assert.IsNotNull(contentResult);
            Assert.AreEqual("error", contentResult.Content.ToString());
        }


    }
}
