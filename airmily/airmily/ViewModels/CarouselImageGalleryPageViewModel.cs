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
            foreach (AlbumItem t in Images)
            {
                ImagesWithComments temp = new ImagesWithComments();
                temp.image = t;
                //TODO add comments once comments are completed
                ImagesTest.Add(temp);
            }
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

        private ObservableCollection<ImagesWithComments> _imagesTest = new ObservableCollection<ImagesWithComments>();

        public ObservableCollection<ImagesWithComments> ImagesTest
        {
            get { return _imagesTest; }
            set { SetProperty(ref _imagesTest, value); }
        }
    }

    public class ImagesWithComments
    {
        public AlbumItem image { get; set; }
        //public ObservableCollection<Transaction> comment { get; set; }
        public ImagesWithComments()
        {
        }
    } 

}
