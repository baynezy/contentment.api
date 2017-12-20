using Contentment.Api.Model;

namespace Contentment.Api.Services {
	public interface IContentService
	{
		Content Create(ContentCreate content);
		Content FindById(string contentId);
	}
}