using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using Contentment.Api.Controllers;
using Contentment.Api.Domain;
using Contentment.Api.Model;
using Contentment.Api.Services;
using Contentment.Api.Test.Helpers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace Contentment.Api.Test.Controllers {
	[TestFixture]
	public class ContentControllerTest {

		#region Controller

		[Test]
		public void ContentController_ImplementsController() {
			var controller = CreateController();

			Assert.That(controller, Is.InstanceOf<Controller>());
		}

		#endregion

		#region PostContent

		#region ValidContent

		[Test]
		public void PostContent_WhenCalledWithValidContent_ThenReturnJsonCreatedResult() {
			var controller = CreateController();
			var content = ContentHelper.ValidContentCreate();

			var result = controller.PostContent(content);

			Assert.That(result, Is.InstanceOf<JsonCreatedResult>());
		}

		[Test]
		public void PostContent_WhenCalledWithValidContent_ThenReturn201Response() {
			var controller = CreateController();
			var content = ContentHelper.ValidContentCreate();

			var result = controller.PostContent(content) as JsonCreatedResult;

			Assert.That(result, Is.Not.Null);
			Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.Created));
		}

		[Test]
		public void PostContent_WhenCalledWithValidContent_ThenCallIContentService() {
			var mockService = new Mock<IContentService>();
			var controller = CreateController(mockService.Object);
			var content = ContentHelper.ValidContentCreate();

			controller.PostContent(content);

			mockService.Verify(f => f.Create(content), Times.Once);
		}

		[Test]
		public void PostContent_WhenCalledWithValidContent_ThenReturnContentFromIContentService() {
			var content = ContentHelper.ValidContentCreate();
			const string expectedId = "qwerty";
			var expectedCreatedDateTime = new DateTime(2017,12,19,6,49,0);
			var mockService = new Mock<IContentService>();
			mockService.Setup(m => m.Create(content)).Returns(
				new Content {
					Id = expectedId,
					Title = content.Title,
					Body = content.Body,
					CreatedDateTime = expectedCreatedDateTime
				}
			);
			var controller = CreateController(mockService.Object);

			var result = controller.PostContent(content) as JsonCreatedResult;

			var newContent = result.Value as Content;

			Assert.That(newContent, Is.Not.Null);
			Assert.That(newContent.Id, Is.EqualTo(expectedId));
			Assert.That(newContent.CreatedDateTime, Is.EqualTo(expectedCreatedDateTime));
			Assert.That(newContent.Title, Is.EqualTo(content.Title));
			Assert.That(newContent.Body, Is.EqualTo(content.Body));
		}

		#endregion

		#region InvalidContent

		[Test]
		public void PostContent_WhenCalledWithoutRequiredFields_ThenReturn400Error() {
			var invalidContent = ContentHelper.InvalidContentCreate();
			var controller = CreateController();

			ValidateModel(invalidContent, controller);

			var result = controller.PostContent(invalidContent) as JsonResult;

			Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
		}

		[Test]
		public void PostContent_WhenCalledWithoutRequiredFields_ThenReturnClientErrorJson() {
			var invalidContent = ContentHelper.InvalidContentCreate();
			var controller = CreateController();

			ValidateModel(invalidContent, controller);

			var result = controller.PostContent(invalidContent) as JsonResult;
			var error = result.Value as ClientError;

			Assert.That(error, Is.Not.Null);
		}

		[Test]
		public void PostContent_WhenCalledWithoutRequiredFields_ThenReturnPopulatedClientErrorJson() {
			const int expectedErrorCount = 2;
			var invalidContent = ContentHelper.InvalidContentCreate();
			var controller = CreateController();

			ValidateModel(invalidContent, controller);

			var result = controller.PostContent(invalidContent) as JsonResult;
			var error = result.Value as ClientError;

			Assert.That(error.Code, Is.EqualTo(ErrorCodes.INVALID_CONTENT));
			Assert.That(error.Description, Is.EqualTo("Content is invalid"));

			var details = error.Detailed;

			Assert.That(details.Count, Is.EqualTo(expectedErrorCount));
			Assert.That(details["Title"][0].Message, Is.EqualTo("The Title field is required."));
			Assert.That(details["Body"][0].Message, Is.EqualTo("The Body field is required."));
		}

		#endregion

		#endregion

		#region GetContent

		[Test]
		public void GetContent_WhenCalled_ThenReturnJsonResult() {
			var controller = CreateController();
			const string contentId = "qwerty";

			var result = controller.GetContent(contentId);

			Assert.That(result, Is.InstanceOf<JsonResult>());
		}

		[Test]
		public void GetContent_WhenCalled_ThenCallIContentService() {
			var contentService = new Mock<IContentService>();
			var controller = CreateController(contentService.Object);
			const string contentId = "qwerty";

			controller.GetContent(contentId);

			contentService.Verify(f => f.FindById(contentId), Times.Once);
		}

		[Test]
		public void GetContent_WhenCalled_ThenReturnContentFromIContentService() {
			var contentService = new Mock<IContentService>();
			const string contentId = "qwerty";
			var content = ContentHelper.ValidContent(contentId);
			contentService.Setup(m => m.FindById(contentId)).Returns(content);
			var controller = CreateController(contentService.Object);

			var result = controller.GetContent(contentId);
			var viewModel = result.Value as Content;

			Assert.That(viewModel, Is.Not.Null);
			Assert.That(viewModel.Id, Is.EqualTo(content.Id));
			Assert.That(viewModel.Body, Is.EqualTo(content.Body));
			Assert.That(viewModel.CreatedDateTime, Is.EqualTo(content.CreatedDateTime));
			Assert.That(viewModel.Title, Is.EqualTo(content.Title));
		}

		#endregion

		private ContentController CreateController(IContentService contentService = null)
		{
			return new ContentController(contentService ?? new Mock<IContentService>().Object);
		}

		private static void ValidateModel(object model, Controller controller)
		{
			var validationContext = new ValidationContext(model, null, null);
			var validationResults = new List<ValidationResult>();
			Validator.TryValidateObject(model, validationContext, validationResults);

			foreach (var validationResult in validationResults)
			{
				controller.ModelState.AddModelError(validationResult.MemberNames.First(), validationResult.ErrorMessage);
			}
		}
	}
}