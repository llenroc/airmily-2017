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
	    private readonly INavigationService _navigationService;
        private readonly IPageDialogService _pageDialogService;

		public ViewImagesPageViewModel(IPageDialogService pageDialogService,IAzure azure, INavigationService navigationService)//, IPageDialogService pageDialogService)
		{
			_pageDialogService = pageDialogService;
			_azure = azure;
		    _navigationService = navigationService;
		}
	    #region ObservableCollections
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
	    #endregion

		private Transaction _currentTransaction = new Transaction
		{
			Currency = "GBP",
			NegativeAmount = true,
			InternalDifference = "0.00",
			Amount = "0.00",
			Description = "Description"
		};
	    public Transaction CurrentTransaction
	    {
	        get { return _currentTransaction; }
            set { SetProperty(ref _currentTransaction, value); }
	    }

		public void OnNavigatedFrom(NavigationParameters parameters)
		{

		}

		public async void OnNavigatedTo(NavigationParameters parameters)
		{
			if (!parameters.ContainsKey("transaction"))
				return;

			CurrentTransaction = (Transaction)parameters["transaction"];
			RefreshImages();
		}

		public async void RefreshImages()
		{
			_receipt1.Clear();
			_receipt2.Clear();
			_receipt3.Clear();
			_good1.Clear();
			_good2.Clear();
			_good3.Clear();

			List<AlbumItem> receipts = await _azure.GetAllImages(CurrentTransaction.ID, true);
			receipts.Add(new AlbumItem { IsAddButton = true, IsReceipt = true });
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

			List<AlbumItem> goods = await _azure.GetAllImages(CurrentTransaction.ID, false);
			goods.Add(new AlbumItem { IsAddButton = true, IsReceipt = false });
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
							var parameters = new NavigationParameters {["Src"] = item};
							await _navigationService.NavigateAsync("FullScreenImagePage", parameters);
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
									SaveToAlbum = false,
									CompressionQuality = 50
								});

								if (file == null) return;

								AlbumItem newItem = new AlbumItem
								{
									IsAddButton = false,
									IsReceipt = item.IsReceipt,
									Image = new byte[file.GetStream().Length],
									ImageName = Guid.NewGuid().ToString()
								};
								file.GetStream().Read(newItem.Image, 0, newItem.Image.Length);

								newItem.Album = CurrentTransaction.ID;
								await _azure.UploadImage(newItem);

								if (newItem.IsReceipt)
								{
									if (_receipt1.Contains(item))
									{
										_receipt1.Remove(item);
										_receipt1.Add(newItem);
										_receipt2.Add(new AlbumItem { IsAddButton = true, IsReceipt = false });
									}
									else if (_receipt2.Contains(item))
									{
										_receipt2.Remove(item);
										_receipt2.Add(newItem);
										_receipt3.Add(new AlbumItem { IsAddButton = true, IsReceipt = false });
									}
									else if (_receipt3.Contains(item))
									{
										_receipt3.Remove(item);
										_receipt3.Add(newItem);
										_receipt1.Add(new AlbumItem { IsAddButton = true, IsReceipt = false });
									}
								}
								else
								{
									if (_good1.Contains(item))
									{
										_good1.Remove(item);
										_good1.Add(newItem);
										_good2.Add(new AlbumItem { IsAddButton = true, IsReceipt = false });
									}
									else if (_good2.Contains(item))
									{
										_good2.Remove(item);
										_good2.Add(newItem);
										_good3.Add(new AlbumItem { IsAddButton = true, IsReceipt = false });
									}
									else if (_good3.Contains(item))
									{
										_good3.Remove(item);
										_good3.Add(newItem);
										_good1.Add(new AlbumItem { IsAddButton = true, IsReceipt = false });
									}
								}
							}
						}
					});
				}
				return _onImageTapped;
			}
		}
	}
}