namespace airmily.Services.IModels
{
	public interface IAlbumItem
	{
		string ImageName { get; set; }

		string Album { get; set; }

		bool IsReceipt { get; set; }

		string Address { get; set; }

		byte[] Image { get; set; }

		bool IsAddButton { get; set; }

		Xamarin.Forms.ImageSource ImageSrc { get; set; }
	}
}