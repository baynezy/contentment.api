using System.Buffers;
using System.Text;
using System.Threading.Tasks;
using Contentment.Api.Domain;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;

namespace Contentment.Api.Formatters
{
	public class VendorContentTypeJsonOutputFormatter : JsonOutputFormatter
	{
		public VendorContentTypeJsonOutputFormatter() : base (new JsonSerializerSettings(), ArrayPool<char>.Shared)
		{
			SupportedMediaTypes.Clear();
			SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse(ContentTypes.VENDOR_MIME_TYPE));
		}
	}
}