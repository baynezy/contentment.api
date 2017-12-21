using System;
using Contentment.Api.Model;

namespace Contentment.Api.Test.Helpers {
	public class ContentHelper {
		public static ContentCreate ValidContentCreate()
		{
			return new ContentCreate {
				Title = "Valid Title",
				Body = "Valid Body!"
			};
		}

		public static ContentCreate InvalidContentCreate()
		{
			return new ContentCreate();
		}

		public static Content ValidContent(string contentId)
		{
			var contentCreate = ValidContentCreate();
			var content = new Content {
				Id = contentId,
				CreatedDateTime = new DateTime(2017,12,21,9,12,59)
			};

			return content;
		}
	}
}