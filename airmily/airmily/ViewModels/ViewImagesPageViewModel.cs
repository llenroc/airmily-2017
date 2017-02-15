using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using airmily.Services.Azure;
using airmily.Services.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Forms;
using Plugin.Media;
using Plugin.Media.Abstractions;

namespace airmily.ViewModels
{
	public class ViewImagesPageViewModel : BindableBase, INavigationAware
	{
		private readonly IAzure _azure;
		private readonly IPageDialogService _pageDialogService;
		private readonly INavigationService _navigationService;

		private Transaction _currentTransaction;

		public ViewImagesPageViewModel(IPageDialogService pageDialogService, IAzure azure, INavigationService navigation)
		{
			_pageDialogService = pageDialogService;
			_azure = azure;
			_navigationService = navigation;
		}

		//private ObservableCollection<AlbumItem> _imageItems;
		//public ObservableCollection<AlbumItem> ImageItems
		//{
		//	get { return _imageItems; }
		//	set { SetProperty(ref _imageItems, value); }
		//}

		private ObservableCollection<AlbumItem> _receipt1 = new ObservableCollection<AlbumItem>();
		public ObservableCollection<AlbumItem> Receipt1
		{
			get { return _receipt1; }
			set { SetProperty(ref _receipt1, value); }
		}

		private ObservableCollection<AlbumItem> _receipt2 = new ObservableCollection<AlbumItem>();
		public ObservableCollection<AlbumItem> Receipt2
		{
			get { return _receipt2; }
			set { SetProperty(ref _receipt2, value); }
		}

		private ObservableCollection<AlbumItem> _receipt3 = new ObservableCollection<AlbumItem>();
		public ObservableCollection<AlbumItem> Receipt3
		{
			get { return _receipt3; }
			set { SetProperty(ref _receipt3, value); }
		}

		private ObservableCollection<AlbumItem> _good1 = new ObservableCollection<AlbumItem>();
		public ObservableCollection<AlbumItem> Good1
		{
			get { return _good1; }
			set { SetProperty(ref _good1, value); }
		}

		private ObservableCollection<AlbumItem> _good2 = new ObservableCollection<AlbumItem>();
		public ObservableCollection<AlbumItem> Good2
		{
			get { return _good2; }
			set { SetProperty(ref _good2, value); }
		}

		private ObservableCollection<AlbumItem> _good3 = new ObservableCollection<AlbumItem>();

		public ObservableCollection<AlbumItem> Good3
		{
			get { return _good3; }
			set { SetProperty(ref _good3, value); }
		}

		public void OnNavigatedFrom(NavigationParameters parameters)
		{

		}

		public async void OnNavigatedTo(NavigationParameters parameters)
		{
			if (!parameters.ContainsKey("transaction")) return;

			_currentTransaction = (Transaction)parameters["transaction"];

			List<AlbumItem> receipts = await _azure.GetAllImages(_currentTransaction.AlbumID, true);
			receipts.Add(new AlbumItem { IsAddButton = true });
			foreach (AlbumItem t in receipts)
			{
				switch (receipts.IndexOf(t) % 3)
				{
					case 0:
						_receipt1.Add(t);
						break;
					case 1:
						_receipt2.Add(t);
						break;
					case 2:
						_receipt3.Add(t);
						break;
				}
			}

			List<AlbumItem> goods = await _azure.GetAllImages(_currentTransaction.AlbumID, false);
			goods.Add(new AlbumItem { IsAddButton = true });
			foreach (AlbumItem t in goods)
			{
				switch (goods.IndexOf(t) % 3)
				{
					case 0:
						_good1.Add(t);
						break;
					case 1:
						_good2.Add(t);
						break;
					case 2:
						_good3.Add(t);
						break;
				}
			}
		}

		private DelegateCommand<ItemTappedEventArgs> _onImageTapped;
		public DelegateCommand<ItemTappedEventArgs> OnImageTapped
		{
			get
			{
				if (_onImageTapped == null)
				{
					_onImageTapped = new DelegateCommand<ItemTappedEventArgs>(async selected =>
					{
						var item = selected.Item as AlbumItem;
						if (item == null) return;

						if (!item.IsAddButton)
						{
							var parameters = new NavigationParameters {["img"] = item.ImageSrc};
							await _navigationService.NavigateAsync("", parameters);
						}
						else
						{
							await CrossMedia.Current.Initialize();
							if (CrossMedia.Current.IsCameraAvailable && CrossMedia.Current.IsTakePhotoSupported)
							{
								var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
								{
									Directory = "ReceiptsAndGoods",
									Name = "test.jpg",
									SaveToAlbum = false
								});

								if (file == null) return;

								AlbumItem newItem = new AlbumItem
								{
									ImageName = new Guid().ToString(),
									IsAddButton = false,
									IsReceipt = true,
									Image = new byte[file.GetStream().Length]
								};
								file.GetStream().Read(newItem.Image, 0, newItem.Image.Length);

								if (string.IsNullOrEmpty(_currentTransaction.AlbumID))
								{
									_currentTransaction.AlbumID = new Guid().ToString();
									_azure.UpdateSingleTransaction(_currentTransaction);
								}

								newItem.Album = _currentTransaction.AlbumID;
								await _azure.UploadImage(newItem);
							}
						}
					});
				}
				return _onImageTapped;
			}
		}
	}
}