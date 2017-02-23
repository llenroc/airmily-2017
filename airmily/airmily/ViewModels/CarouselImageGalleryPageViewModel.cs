using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http.Headers;
using airmily.Services.Azure;
using airmily.Services.Models;
using Microsoft.Practices.ObjectBuilder2;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Forms;

namespace airmily.ViewModels
{
    public class CarouselImageGalleryPageViewModel : BindableBase, INavigationAware
    {
        private readonly IAzure _azure;
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _pageDialogService;


        public CarouselImageGalleryPageViewModel(IPageDialogService pageDialogService, IAzure azure, INavigationService navigationService)
        {
            _pageDialogService = pageDialogService;
            _azure = azure;
            _navigationService = navigationService;
        }

        #region OnNavStuff
        public void OnNavigatedFrom(NavigationParameters parameters)
        {
            
        }

        public async void OnNavigatedTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey("Images"))
            {
                Images = (ObservableCollection<AlbumItem>) parameters["Images"];
                foreach (AlbumItem t in Images)
                {
                    ImagesWithComments temp = new ImagesWithComments();
                    temp.image = t;
                    List<Comment> Tcomments = await _azure.GetComments(t.ID);
                    foreach (Comment c in Tcomments)
                    {
                        temp.comments.Add(c);
                    }
                    temp.AddCommentText = "";
                    ImagesTest.Add(temp);
                }
            }
        }
        #endregion



        private DelegateCommand _addComment;
        public DelegateCommand AddCommentCmd
        {
            get
            {
                if (_addComment == null)
                {
                    _addComment = new DelegateCommand(AddComment);
                }
                return _addComment;
   
            }
        }
        private ImagesWithComments _selectedImage = new ImagesWithComments();
        public ImagesWithComments selectedImage
        {
            get{ return _selectedImage; }
        set{ SetProperty(ref _selectedImage, value); }
        }

        private async void AddComment()
        {
            var testselectedImage = selectedImage;
            if (string.IsNullOrEmpty(testselectedImage.AddCommentText))
            {
                return;
            }

            Comment newComment = new Comment
            {
                ImageID = testselectedImage.image.ID,
                UserID = "588842",
                Message = testselectedImage.AddCommentText
            };
            await _azure.AddComment(newComment);
            testselectedImage.AddCommentText = "";
        }



        #region Observable Collections
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
        #endregion


    }

    public class ImagesWithComments : BindableBase
    {
        public AlbumItem image { get; set; }

        private ObservableCollection<Comment> _comments = new ObservableCollection<Comment>();
        

        public ObservableCollection<Comment> comments
        {
            get {return _comments;}
            set { SetProperty(ref _comments, value); }
        }

        public string AddCommentText { get; set; }

        //public string AddCommentText { get; set; }
        //private Command _addComment;
        //public Command AddComment
        //{
        //    get
        //    {
        //        return _addComment ?? (_addComment = new Command(async () =>
        //        {
        //            if (string.IsNullOrEmpty(AddCommentText)) return;

        //            Comment newComment = new Comment
        //            {
        //                ImageID = Image.ID,
        //                UserID = "588842",
        //                Message = AddCommentText
        //            };
        //            await _azure.AddComment(newComment);
        //            AddCommentText = "";
        //            RefreshComments(Image.ID);
        //        }));
        //    }
        //}
        public ImagesWithComments()
        {

        }
    } 

}
