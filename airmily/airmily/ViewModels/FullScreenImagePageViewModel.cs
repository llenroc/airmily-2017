using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
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

        }

        public void OnCloseButtonClicked()
        {
            _navigationService.GoBackAsync();
        }
    }
}
