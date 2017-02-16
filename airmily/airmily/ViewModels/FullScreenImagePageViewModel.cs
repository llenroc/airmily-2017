using airmily.Services.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Xamarin.Forms;

namespace airmily.ViewModels
{
    public class FullScreenImagePageViewModel : BindableBase, INavigationAware
    {
        private readonly INavigationService _navigationService;

        private AlbumItem _image;

        private DelegateCommand<ItemTappedEventArgs> _onCloseButtonClicked;

        private ImageSource _src;

        public FullScreenImagePageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public ImageSource Src
        {
            get { return _src; }
            set { SetProperty(ref _src, value); }
        }

        public AlbumItem Image
        {
            get { return _image; }
            set { SetProperty(ref _image, value); }
        }

        public DelegateCommand<ItemTappedEventArgs> OnCloseButtonClicked
        {
            get
            {
                _onCloseButtonClicked = new DelegateCommand<ItemTappedEventArgs>(async selected =>
                {
                    await _navigationService.GoBackAsync(parameters);
                    //change functionality to be a delete button!
                });
                return _onCloseButtonClicked;
            }
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            //add image as param
            if (parameters.ContainsKey("Src"))
            {
                Image = (AlbumItem) parameters["Src"];
                Src = Image.ImageSrc;
            }
        }
    }
}