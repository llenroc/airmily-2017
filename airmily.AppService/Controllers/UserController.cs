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
	public class UserController : TableController<User>
	{
		protected override void Initialize(HttpControllerContext controllerContext)
		{
			base.Initialize(controllerContext);
			MobileServiceContext context = new MobileServiceContext();
			DomainManager = new EntityDomainManager<User>(context, Request, true);
		}

		// GET tables/User
		public IQueryable<User> GetAllTodoItems()
		{
			return Query();
		}

		// GET tables/User/48D68C86-6EA6-4C25-AA33-223FC9A27959
		public SingleResult<User> GetTodoItem(string id)
		{
			return Lookup(id);
		}

		// PATCH tables/User/48D68C86-6EA6-4C25-AA33-223FC9A27959
		public Task<User> PatchTodoItem(string id, Delta<User> patch)
		{
			return UpdateAsync(id, patch);
		}

		// POST tables/User
		public async Task<IHttpActionResult> PostTodoItem(User item)
		{
			User current = await InsertAsync(item);
			return CreatedAtRoute("Tables", new { id = current.Id }, current);
		}

		// DELETE tables/User/48D68C86-6EA6-4C25-AA33-223FC9A27959
		public Task DeleteTodoItem(string id)
		{
			return DeleteAsync(id);
		}
	}
}