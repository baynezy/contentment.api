using System;
using System.Net;
using Contentment.Api.Domain;
using Contentment.Api.Model;
using Contentment.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Contentment.Api.Controllers {
	public class ContentController : Controller
	{
		private readonly IContentService _contentService;

		public ContentController(IContentService contentService)
		{
			_contentService = contentService;
		}

		[HttpPost("/content")]
		public JsonResult PostContent([FromBody] ContentCreate content)
		{
			JsonResult result;
			if (ModelState.IsValid) {
				var newContent = _contentService.Create(content);
				result = Json(newContent);
				result.StatusCode = (int)HttpStatusCode.Created;
				result.ContentType = ContentTypes.VENDOR_MIME_TYPE;
			}
			else {
				var error = new ClientError(ModelState){
					Code = ErrorCodes.INVALID_CONTENT,
					Description = "Content is invalid"
				};
				result = Json(error);
				result.StatusCode = (int)HttpStatusCode.BadRequest;
				result.ContentType = ContentTypes.VENDOR_MIME_TYPE_ERROR;
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