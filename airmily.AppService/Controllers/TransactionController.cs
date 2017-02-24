using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using airmily.AppService.DataObjects;
using airmily.AppService.Models;

namespace airmily.AppService.Controllers
{
    public class TransactionController : TableController<Transaction>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            MobileServiceContext context = new MobileServiceContext();
            DomainManager = new EntityDomainManager<Transaction>(context, Request);
        }

		// GET tables/Transaction
		public IQueryable<Transaction> GetAllTransaction()
        {
            return Query(); 
        }

		// GET tables/Transaction/48D68C86-6EA6-4C25-AA33-223FC9A27959
		public SingleResult<Transaction> GetTransaction(string id)
        {
            return Lookup(id);
        }

		// PATCH tables/Transaction/48D68C86-6EA6-4C25-AA33-223FC9A27959
		public Task<Transaction> PatchTransaction(string id, Delta<Transaction> patch)
        {
             return UpdateAsync(id, patch);
        }

		// POST tables/Transaction
		public async Task<IHttpActionResult> PostTransaction(Transaction item)
        {
            Transaction current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

		// DELETE tables/Transaction/48D68C86-6EA6-4C25-AA33-223FC9A27959
		public Task DeleteTransaction(string id)
        {
             return DeleteAsync(id);
        }
    }
}
