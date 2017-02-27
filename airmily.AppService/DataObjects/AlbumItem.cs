using airmily.Services.Interfaces;
using Microsoft.Azure.Mobile.Server;

namespace airmily.AppService.DataObjects
{
	public class AlbumItem : EntityData, IAlbumItem
	{
		public string ImageName { get; set; }

		public string Album { get; set; }

		public bool IsReceipt { get; set; }

		public byte[] Image { get; set; }
	}
}
