using System.Collections.ObjectModel;
using System.Linq;
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



        public ViewImagesPageViewModel(IPageDialogService pageDialogService, IAzure azure)
        {
            _pageDialogService = pageDialogService;
            _azure = azure;
        }


        private ObservableCollection<AlbumItem> _imageItems;
        public ObservableCollection<AlbumItem> ImageItems
        {
            get { return _imageItems; }
            set { SetProperty(ref _imageItems, value); }
        }

        private ObservableCollection<AlbumItem> _receipt1;

        public ObservableCollection<AlbumItem> Receipt1
        {
            get { return _receipt1; }
            set { SetProperty(ref _receipt1, value); }
        }


        private ObservableCollection<AlbumItem> _receipt2;
        public ObservableCollection<AlbumItem> Receipt2
        {
            get { return _receipt2; }
            set { SetProperty(ref _receipt2, value); }
        }
        private ObservableCollection<AlbumItem> _receipt3;

        public ObservableCollection<AlbumItem> Receipt3
        {
            get { return _receipt3; }
            set { SetProperty(ref _receipt3, value); }
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
                int j = 1;
                foreach (AlbumItem t in ret)
                {
                    if (j > 3)
                    {
                        j = 1;
                    }
                    switch (j)
                    {
                        case 1:
                            _receipt1.Add(t);
                            break;
                        case 2:
                            _receipt2.Add(t);
                            break;
                        case 3:
                            _receipt3.Add(t);
                            break;
                    }
                    j++;
                }
            }
        }
    }
}