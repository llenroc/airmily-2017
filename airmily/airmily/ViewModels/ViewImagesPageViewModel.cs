using System.Collections.ObjectModel;
using airmily.Services.Azure;
using airmily.Services.Models;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;

namespace airmily.ViewModels
{
    public class ViewImagesPageViewModel : BindableBase, INavigationAware
    {
        private readonly IAzure _azure;
        private readonly IPageDialogService _pageDialogService;


        private ObservableCollection<AlbumItem> _imageItems;

        public ViewImagesPageViewModel(IPageDialogService pageDialogService, IAzure azure)
        {
            _pageDialogService = pageDialogService;
            _azure = azure;
        }

        public ObservableCollection<AlbumItem> ImageItems
        {
            get { return _imageItems; }
            set { SetProperty(ref _imageItems, value); }
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
        }

        public async void OnNavigatedTo(NavigationParameters parameters)
        {
//            if (parameters.ContainsKey("id"))
            {
                //Transaction current = (Transaction)parameters["id"];
                var ret = await _azure.GetImages("98C597C2-7322-4D87-A95F-974F513DBFC4"); /*current.AlbumID*/
                _imageItems = new ObservableCollection<AlbumItem>(ret);
            }
        }
    }
}