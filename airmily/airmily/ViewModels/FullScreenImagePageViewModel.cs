using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using airmily.Services.Models;
using Prism.Navigation;
using Xamarin.Forms;

namespace airmily.ViewModels
{
    public class FullScreenImagePageViewModel : BindableBase, INavigationAware
    {
        private readonly INavigationService _navigationService;

        private ImageSource _src;

        public ImageSource Src
        {
            get { return _src;}
            set { SetProperty(ref _src, value); }
        }

        private AlbumItem _image;
        public AlbumItem Image
        {
            get { return _image; }
            set { SetProperty(ref _image, value); }
        }

        private Transaction _trans;

        public Transaction Trans
        {
            get { return _trans; }
            set { SetProperty(ref _trans, value); }
        }
        public FullScreenImagePageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
            
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            //add image as param
            if (parameters.ContainsKey("Src"))
            {
                Image = (AlbumItem)parameters["Src"];
                Src = Image.ImageSrc;
            }
            if (parameters.ContainsKey("Trans"))
            {
                Trans = (Transaction) parameters["Trans"];
            }
        }

        private DelegateCommand<ItemTappedEventArgs> _onCloseButtonClicked;

        public DelegateCommand<ItemTappedEventArgs> OnCloseButtonClicked
        {
            get
            {
                _onCloseButtonClicked = new DelegateCommand<ItemTappedEventArgs>(async selected =>
                {
                   var parameters = new NavigationParameters {["transaction"] = Trans};
                   await _navigationService.GoBackAsync(parameters);
                    //TODO change functionality to be a delete button!

                });
                return _onCloseButtonClicked;
            }
        }
    }
}
