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
	public class TransactionController : TableController<Transaction>
	{
		protected override void Initialize(HttpControllerContext controllerContext)
		{
			base.Initialize(controllerContext);
			MobileServiceContext context = new MobileServiceContext();
			DomainManager = new EntityDomainManager<Transaction>(context, Request, true);
		}

		// GET tables/Transaction
		public IQueryable<Transaction> GetAllTodoItems()
		{
			return Query();
		}

		// GET tables/Transaction/48D68C86-6EA6-4C25-AA33-223FC9A27959
		public SingleResult<Transaction> GetTodoItem(string id)
		{
			return Lookup(id);
		}

		// PATCH tables/Transaction/48D68C86-6EA6-4C25-AA33-223FC9A27959
		public Task<Transaction> PatchTodoItem(string id, Delta<Transaction> patch)
		{
			return UpdateAsync(id, patch);
		}

		// POST tables/Transaction
		public async Task<IHttpActionResult> PostTodoItem(Transaction item)
		{
			Transaction current = await InsertAsync(item);
			return CreatedAtRoute("Tables", new { id = current.Id }, current);
		}

		// DELETE tables/Transaction/48D68C86-6EA6-4C25-AA33-223FC9A27959
		public Task DeleteTodoItem(string id)
		{
			return DeleteAsync(id);
		}
	}
}