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
    public class CardController : TableController<Card>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            MobileServiceContext context = new MobileServiceContext();
            DomainManager = new EntityDomainManager<Card>(context, Request);
        }

		// GET tables/Card
		public IQueryable<Card> GetAllCard()
        {
            return Query(); 
        }

		// GET tables/Card/48D68C86-6EA6-4C25-AA33-223FC9A27959
		public SingleResult<Card> GetCard(string id)
        {
            return Lookup(id);
        }

		// PATCH tables/Card/48D68C86-6EA6-4C25-AA33-223FC9A27959
		public Task<Card> PatchCard(string id, Delta<Card> patch)
        {
             return UpdateAsync(id, patch);
        }

		// POST tables/Card
		public async Task<IHttpActionResult> PostCard(Card item)
        {
            Card current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

		// DELETE tables/Card/48D68C86-6EA6-4C25-AA33-223FC9A27959
		public Task DeleteCard(string id)
        {
             return DeleteAsync(id);
        }
    }
}
