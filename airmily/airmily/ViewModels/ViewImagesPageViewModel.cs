using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using airmily.Services.Azure;
using airmily.Services.Models;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Forms;

namespace airmily.ViewModels
{
	public class ViewImagesPageViewModel : BindableBase, INavigationAware
	{
		private readonly IAzure _azure;
		private readonly IPageDialogService _pageDialogService;

		public ViewImagesPageViewModel(IPageDialogService pageDialogService, IAzure azure)
		{
			_pageDialogService = pageDialogService;
			_azure = azure;
		}

        //private ObservableCollection<AlbumItem> _imageItems;
        //public ObservableCollection<AlbumItem> ImageItems
        //{
        //	get { return _imageItems; }
        //	set { SetProperty(ref _imageItems, value); }
        //}

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


	    private Transaction _currentTransaction = new Transaction();

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
			if (parameters.ContainsKey("trans"))
                _currentTransaction = (Transaction)parameters["trans"];

			List<AlbumItem> receipts = await _azure.GetImages("98C597C2-7322-4D87-A95F-974F513DBFC4", true); //replace with CurrentTransaction.ID
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

			List<AlbumItem> goods = await _azure.GetImages("98C597C2-7322-4D87-A95F-974F513DBFC4", false);
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
			//}
		}
	}
}