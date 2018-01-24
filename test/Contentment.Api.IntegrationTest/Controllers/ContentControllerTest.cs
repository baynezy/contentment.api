using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Contentment.Api.Model;
using Newtonsoft.Json;
using NUnit.Framework;
using Contentment.Api.Test.Helpers;
using Contentment.Api.Domain;

namespace Contentment.Api.IntegrationTest.Controllers {
	[TestFixture]
	public class ContentControllerTest : BaseIntegrationController {
		[Test]
		public async Task PostContent_WhenCalledWithValidContent_ThenShouldReturnCreatedContent() {
			var validContent = ContentHelper.ValidContent();
			var content = new StringContent(JsonConvert.SerializeObject(validContent), Encoding.UTF8, ContentTypes.VENDOR_MIME_TYPE);
			var response = await GetClient().PostAsync("/content", content);
			var jsonString = await response.Content.ReadAsStringAsync();
			dynamic json = JsonConvert.DeserializeObject(jsonString);

			Assert.That(json.title.ToString(), Is.EqualTo(validContent.Title));
			Assert.That(json.body.ToString(), Is.EqualTo(validContent.Body));
			Assert.That(json.id, Is.Not.Null);
			Assert.That(json.createdDateTime, Is.Not.Null);
		}

		[Test]
		public async Task PostContent_WhenCalledWithValidContent_ThenShouldReturnVendorContentType() {
			var validContent = ContentHelper.ValidContent();
			var content = new StringContent(JsonConvert.SerializeObject(validContent), Encoding.UTF8, ContentTypes.VENDOR_MIME_TYPE);
			var response = await GetClient().PostAsync("/content", content);

			var headers = response.Content.Headers;

			Assert.That(headers.Contains("Content-Type"), Is.True);
			Assert.That(headers.ContentType.MediaType, Is.EqualTo(ContentTypes.VENDOR_MIME_TYPE));
		}

		[Test]
		public async Task PostContent_WhenCalledWithInvalidContent_ThenShouldReturnVendorContentType() {
			var validContent = ContentHelper.InvalidContent();
			var content = new StringContent(JsonConvert.SerializeObject(validContent), Encoding.UTF8, ContentTypes.VENDOR_MIME_TYPE);
			var response = await GetClient().PostAsync("/content", content);

			var headers = response.Content.Headers;

			Assert.That(headers.Contains("Content-Type"), Is.True);
			Assert.That(headers.ContentType.MediaType, Is.EqualTo(ContentTypes.VENDOR_MIME_TYPE_ERROR));
		}
	}
}