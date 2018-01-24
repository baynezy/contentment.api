using System;
using System.Net;
using Contentment.Api.Domain;
using Contentment.Api.Model;
using Contentment.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Contentment.Api.Controllers {
	public class ContentController : Controller
	{
		private IContentService _contentService;

		public ContentController(IContentService contentService)
		{
			_contentService = contentService;
		}

		[HttpPost("/content")]
		public ActionResult PostContent([FromBody] ContentCreate content)
		{
			ActionResult result;
			if (ModelState.IsValid) {
				var newContent = _contentService.Create(content);
				result = new JsonCreatedResult(newContent, ContentTypes.VENDOR_MIME_TYPE, "/something");
			}
			else {
				var error = new ClientError(ModelState){
					Code = ErrorCodes.INVALID_CONTENT,
					Description = "Content is invalid"
				};
				var json = Json(error);
				json.StatusCode = (int)HttpStatusCode.BadRequest;
				json.ContentType = ContentTypes.VENDOR_MIME_TYPE_ERROR;
				result = json;
			}

			return result;
		}

		[HttpGet("/content/{contentId}")]
		public JsonResult GetContent(string contentId)
		{
			var content = _contentService.FindById(contentId);
			return Json(content);
		}
	}
}