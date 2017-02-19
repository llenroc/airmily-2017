using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using airmily.Services.Azure;
using airmily.Services.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Forms;

namespace airmily.ViewModels
{
	public class FullScreenImagePageViewModel : BindableBase, INavigationAware
	{
		private readonly IPageDialogService _pageDialogService;
		private readonly INavigationService _navigationService;
		private readonly IAzure _azure;

		private ImageSource _src;
		public ImageSource Src
		{
			get { return _src; }
			set { SetProperty(ref _src, value); }
		}

		private AlbumItem _image;
		public AlbumItem Image
		{
			get { return _image; }
			set { SetProperty(ref _image, value); }
		}

		private ObservableCollection<Comment> _comments = new ObservableCollection<Comment>();
		public ObservableCollection<Comment> Comments
		{
			get { return _comments; }
			set { SetProperty(ref _comments, value); }
		}

		private DelegateCommand<ItemTappedEventArgs> _onCloseButtonClicked;
		public DelegateCommand<ItemTappedEventArgs> OnCloseButtonClicked
		{
			get
			{
				_onCloseButtonClicked = new DelegateCommand<ItemTappedEventArgs>(async selected =>
				{
					await _navigationService.GoBackAsync();
					//change functionality to be a delete button!
				});
				return _onCloseButtonClicked;
			}
		}

		private Command _deleteImage;
		public Command DeleteImage
		{
			get
			{
				return _deleteImage ?? (_deleteImage = new Command(async () =>
				{
					if (!await _pageDialogService.DisplayAlertAsync("Warning", "Are you sure you want to delete this image?", "Yes", "No")) return;

					await _azure.DeleteImage(Image);
					await _navigationService.GoBackAsync(new NavigationParameters { ["transaction"] = null, ["refreshing"] = true });
				}));
			}
		}

        // NOTE: Removed to fix build problem.
		//private Command _addComment;
		//public Command AddComment
		//{
		//	get
		//	{
		//		return _addComment ?? (_addComment = new Command(async () =>
		//		{
		//			if (string.IsNullOrEmpty(AddCommentText)) return;

		//			Comment newComment = new Comment
		//			{
		//				ImageID = Image.ID,
		//				UserID = "588842",
		//				Message = AddCommentText
		//			};
		//			await _azure.AddComment(newComment);
		//			AddCommentText = "";
		//			RefreshComments(Image.ID);
		//		}));
		//	}
		//}

	

		public FullScreenImagePageViewModel(IPageDialogService pageDialogService, IAzure azure, INavigationService navigationService)
		{
			_azure = azure;
			_navigationService = navigationService;
			_pageDialogService = pageDialogService;
		}

		public void OnNavigatedFrom(NavigationParameters parameters)
		{
		}

		public void OnNavigatedTo(NavigationParameters parameters)
		{
			//add image as param
			if (parameters.ContainsKey("image"))
			{
				Image = (AlbumItem)parameters["image"];

				Src = Image.ImageSrc;
				RefreshComments(Image.ID);
			}
		}

		public async void RefreshComments(string id)
		{
			Comments.Clear();
			var result = await _azure.GetComments(id);
			foreach (Comment c in result)
				Comments.Add(c);
		}
	}
}