using System;
using Contentment.Api.Services;
using Contentment.Api.Test.Helpers;
using Moq;
using NUnit.Framework;

namespace Contentment.Api.Test.Services {
	[TestFixture]
	public class ContentServiceTest {
		[Test]
		public void ContentServiceTest_ImplementsIContentService() {
			var service = CreateService();

			Assert.That(service, Is.InstanceOf<IContentService>());
		}

		[Test]
		public void Create_WhenPassedValidContent_ThenShouldGetNewIdFromIdGenerator() {
			const string expectedId = "egesdf8w";
			var idGenerator = new Mock<IIdGenerator>();
			idGenerator.Setup(m => m.Generate()).Returns(expectedId);
			var service = CreateService(idGenerator.Object);
			var validContent = ContentHelper.ValidContentCreate();

			var newContent = service.Create(validContent);

			idGenerator.Verify(f => f.Generate(), Times.Once());
			Assert.That(newContent.Id, Is.EqualTo(expectedId));
		}

		private ContentService CreateService(IIdGenerator generator = null)
		{
			return new ContentService(generator ?? new Mock<IIdGenerator>().Object);
		}
	}
}