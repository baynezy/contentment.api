using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Contentment.Api.Domain
{
	public class JsonCreatedResult : JsonResult {

		public JsonCreatedResult(object value, string contentType, string location) : base(value)
		{
			ContentType = contentType;
			Location = location;
			StatusCode = (int)HttpStatusCode.Created;
		}

		public string Location { get; private set; }

		public override void ExecuteResult(ActionContext context)
		{
			var response = context.HttpContext.Response;
			response.Headers.Add("Location", Location);

			base.ExecuteResult(context);
		}
	}
}