using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using airmily.AppService.DataObjects;
using airmily.AppService.Models;
using Microsoft.Azure.Mobile.Server;

namespace airmily.AppService.Controllers
{
	public class CommentController : TableController<Comment>
	{
		protected override void Initialize(HttpControllerContext controllerContext)
		{
			base.Initialize(controllerContext);
			MobileServiceContext context = new MobileServiceContext();
			DomainManager = new EntityDomainManager<Comment>(context, Request, true);
		}

		// GET tables/Comment
		public IQueryable<Comment> GetAllTodoItems()
		{
			return Query();
		}

		// GET tables/Comment/48D68C86-6EA6-4C25-AA33-223FC9A27959
		public SingleResult<Comment> GetTodoItem(string id)
		{
			return Lookup(id);
		}

		// PATCH tables/Comment/48D68C86-6EA6-4C25-AA33-223FC9A27959
		public Task<Comment> PatchTodoItem(string id, Delta<Comment> patch)
		{
			return UpdateAsync(id, patch);
		}

		// POST tables/Comment
		public async Task<IHttpActionResult> PostTodoItem(Comment item)
		{
			Comment current = await InsertAsync(item);
			return CreatedAtRoute("Tables", new { id = current.Id }, current);
		}

		// DELETE tables/Comment/48D68C86-6EA6-4C25-AA33-223FC9A27959
		public Task DeleteTodoItem(string id)
		{
			return DeleteAsync(id);
		}
	}
}