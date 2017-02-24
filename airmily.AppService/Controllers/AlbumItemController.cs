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
    public class AlbumItemController : TableController<AlbumItem>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            MobileServiceContext context = new MobileServiceContext();
            DomainManager = new EntityDomainManager<AlbumItem>(context, Request);
        }

		// GET tables/AlbumItem
		public IQueryable<AlbumItem> GetAllAlbumItem()
        {
            return Query(); 
        }

		// GET tables/AlbumItem/48D68C86-6EA6-4C25-AA33-223FC9A27959
		public SingleResult<AlbumItem> GetAlbumItem(string id)
        {
            return Lookup(id);
        }

		// PATCH tables/AlbumItem/48D68C86-6EA6-4C25-AA33-223FC9A27959
		public Task<AlbumItem> PatchAlbumItem(string id, Delta<AlbumItem> patch)
        {
             return UpdateAsync(id, patch);
        }

		// POST tables/AlbumItem
		public async Task<IHttpActionResult> PostAlbumItem(AlbumItem item)
        {
            AlbumItem current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

		// DELETE tables/AlbumItem/48D68C86-6EA6-4C25-AA33-223FC9A27959
		public Task DeleteAlbumItem(string id)
        {
             return DeleteAsync(id);
        }
    }
}
