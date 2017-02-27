using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using airmily.AppService.Models;
using Microsoft.Azure.Mobile.Server;
using Microsoft.Azure.Mobile.Server.Tables;

namespace airmily.AppService.Controllers
{
    public class ControllerBase<TItem> : TableController<TItem> where TItem : class, ITableData
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            var context = new MobileServiceContext();
            DomainManager = new EntityDomainManager<TItem>(context, Request, true);
        }

        // GET tables/User
        public IQueryable<TItem> GetAll()
        {
            return Query();
        }

        // GET tables/User/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<TItem> GetItem(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/User/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public async Task<TItem> PatchItemAsync(string id, Delta<TItem> patch)
        {
            return await UpdateAsync(id, patch);
        }

        // POST tables/User
        public async Task<IHttpActionResult> PostItemAsync(TItem item)
        {
            var current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/User/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public async Task DeleteItemAsync(string id)
        {
            await DeleteAsync(id);
        }
    }
}