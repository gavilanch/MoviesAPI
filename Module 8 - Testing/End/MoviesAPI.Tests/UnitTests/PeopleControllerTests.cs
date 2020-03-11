using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MoviesAPI.Controllers;
using MoviesAPI.DTOs;
using MoviesAPI.Entities;
using MoviesAPI.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MoviesAPI.Tests.UnitTests
{
    [TestClass]
    public class PeopleControllerTests : BaseTests
    {
        [TestMethod]
        public async Task GetPeoplePaginated()
        {
            var databaseName = Guid.NewGuid().ToString();
            var context = BuildContext(databaseName);
            var mapper = BuildMap();

            context.People.Add(new Person() { Name = "Person 1" });
            context.People.Add(new Person() { Name = "Person 2" });
            context.People.Add(new Person() { Name = "Person 3" });
            context.SaveChanges();

            var context2 = BuildContext(databaseName);

            var controller = new PeopleController(context2, mapper, null);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            var responsePage1 = await controller.Get(new PaginationDTO() { Page = 1, RecordsPerPage = 2 });
            var peoplePage1 = responsePage1.Value;
            Assert.AreEqual(2, peoplePage1.Count);

            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            var responsePage2 = await controller.Get(new PaginationDTO() { Page = 2, RecordsPerPage = 2 });
            var peoplePage2 = responsePage2.Value;
            Assert.AreEqual(1, peoplePage2.Count);

            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            var responsePage3 = await controller.Get(new PaginationDTO() { Page = 3, RecordsPerPage = 2 });
            var peoplePage3 = responsePage3.Value;
            Assert.AreEqual(0, peoplePage3.Count);
        }

        [TestMethod]
        public async Task CreatePersonWithoutImage()
        {
            var databaseName = Guid.NewGuid().ToString();
            var context = BuildContext(databaseName);
            var mapper = BuildMap();

            var newPerson = new PersonCreationDTO() { Name = "New Genre", Biography = "abc", DateOfBirth = DateTime.Now };

            var mock = new Mock<IFileStorageService>();
            mock.Setup(x => x.SaveFile(null, null, null, null))
                .Returns(Task.FromResult("url"));

            var controller = new PeopleController(context, mapper, mock.Object);
            var response = await controller.Post(newPerson);
            var result = response as CreatedAtRouteResult;
            Assert.AreEqual(201, result.StatusCode);

            var context2 = BuildContext(databaseName);
            var list = await context2.People.ToListAsync();
            Assert.AreEqual(1, list.Count);
            Assert.IsNull(list[0].Picture);

            Assert.AreEqual(0, mock.Invocations.Count);
        }

        [TestMethod]
        public async Task CreatePersonWithImage()
        {
            var databaseName = Guid.NewGuid().ToString();
            var context = BuildContext(databaseName);
            var mapper = BuildMap();

            var content = Encoding.UTF8.GetBytes("This is a  dummy image");
            var file = new FormFile(new MemoryStream(content), 0, content.Length, "Data", "dummy.jpg");
            file.Headers = new HeaderDictionary();
            file.ContentType = "image/jpg";

            var newPerson = new PersonCreationDTO()
            {
                Name = "New Person",
                Biography = "abc",
                DateOfBirth = DateTime.Now,
                Picture = file
            };

            var mock = new Mock<IFileStorageService>();
            mock.Setup(x => x.SaveFile(content, ".jpg", "people", file.ContentType))
                .Returns(Task.FromResult("url"));

            var controller = new PeopleController(context, mapper, mock.Object);
            var response = await controller.Post(newPerson);
            var result = response as CreatedAtRouteResult;
            Assert.AreEqual(201, result.StatusCode);

            var context2 = BuildContext(databaseName);
            var list = await context2.People.ToListAsync();
            Assert.AreEqual(1, list.Count);
            Assert.AreEqual("url", list[0].Picture);
            Assert.AreEqual(1, mock.Invocations.Count);
        }

        [TestMethod]
        public async Task PatchReturns404IfPersonNotFound()
        {
            var databaseName = Guid.NewGuid().ToString();
            var context = BuildContext(databaseName);
            var mapper = BuildMap();

            var controller = new PeopleController(context, mapper, null);
            var patchDoc = new JsonPatchDocument<PersonPatchDTO>();
            var response = await controller.Patch(1, patchDoc);
            var result = response as StatusCodeResult;
            Assert.AreEqual(404, result.StatusCode);
        }

        [TestMethod]
        public async Task PatchUpdatesSingleField()
        {
            var databaseName = Guid.NewGuid().ToString();
            var context = BuildContext(databaseName);
            var mapper = BuildMap();

            var dateOfBirth = DateTime.Now;
            var person = new Person() { Name = "whatever", Biography = "abc", DateOfBirth = dateOfBirth };
            context.Add(person);
            await context.SaveChangesAsync();

            var context2 = BuildContext(databaseName);

            var controller = new PeopleController(context2, mapper, null);

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<object>()));

            controller.ObjectValidator = objectValidator.Object;

            var patchDoc = new JsonPatchDocument<PersonPatchDTO>();
            patchDoc.Operations.Add(new Operation<PersonPatchDTO>("replace", "/name", null, "new value"));
            var response = await controller.Patch(1, patchDoc);
            var result = response as StatusCodeResult;
            Assert.AreEqual(204, result.StatusCode);

            var context3 = BuildContext(databaseName);
            var personFromDB = await context3.People.FirstAsync();
            Assert.AreEqual("new value", personFromDB.Name);
            Assert.AreEqual("abc", personFromDB.Biography);
            Assert.AreEqual(dateOfBirth, personFromDB.DateOfBirth);

        }
    }
}
