using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using airmily.Services.Models;
using Prism.Navigation;
using Xamarin.Forms;

namespace airmily.ViewModels
{
    public class CarouselImageGalleryPageViewModel : BindableBase, INavigationAware
    {
        public CarouselImageGalleryPageViewModel()
        {
            
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
            
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey("Images"))
            Images = (ObservableCollection<AlbumItem>)parameters["Images"];
        }

        private ObservableCollection<AlbumItem> _images;
        public ObservableCollection<AlbumItem> Images
        {
            get { return _images; }
            set { SetProperty(ref _images, value); }
        }

        private ObservableCollection<AlbumItem> _receipts;
        public ObservableCollection<AlbumItem> Receipts
        {
            get { return _receipts; }
            set { SetProperty(ref _receipts, value); }
        }
    }
}
