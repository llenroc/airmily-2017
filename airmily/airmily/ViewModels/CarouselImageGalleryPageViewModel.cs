using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
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

		private DelegateCommand _addCommentCmd;
		public DelegateCommand AddCommentCmd
		{
			get { return _addCommentCmd ?? (_addCommentCmd = new DelegateCommand(AddComment)); }
		}
		private DelegateCommand<ViewCell> _deleteCmd;
		public DelegateCommand<ViewCell> DeleteCmd { get { return _deleteCmd ?? (_deleteCmd = new DelegateCommand<ViewCell>(DeleteComment)); } }

		private DelegateCommand _refreshCmd;
		public DelegateCommand RefreshCmd
		{
			get { return _refreshCmd ?? (_refreshCmd = new DelegateCommand(async () => await Refresh())); }
		}

		private ImagesWithComments _selectedImage = new ImagesWithComments();
		public ImagesWithComments SelectedImage
		{
			get { return _selectedImage; }
			set { SetProperty(ref _selectedImage, value); }
		}

		private ObservableCollection<ImagesWithComments> _images = new ObservableCollection<ImagesWithComments>();
		public ObservableCollection<ImagesWithComments> Images
		{
			get { return _images; }
			set { SetProperty(ref _images, value); }
		}

		public CarouselImageGalleryPageViewModel(IPageDialogService pageDialogService, IAzure azure, INavigationService navigationService)
		{
			_pageDialogService = pageDialogService;
			_azure = azure;
			_navigationService = navigationService;
		}
		public void OnNavigatedFrom(NavigationParameters parameters)
		{
		}
		public async void OnNavigatedTo(NavigationParameters parameters)
		{
			if (parameters.ContainsKey("Images"))
			{
				var albumItems = (ObservableCollection<AlbumItem>)parameters["Images"];
				foreach (AlbumItem t in albumItems)
				{
					ImagesWithComments temp = new ImagesWithComments();
					temp.Items.Add(new Comment { CurrentType = GalleryType.Image, Image = t });
					temp.Items.AddRange(await _azure.GetComments(t.ID));
					temp.Items.Add(new Comment { CurrentType = GalleryType.AddComment });					
					temp.AddCommentText = "";
					Images.Add(temp);
				}
			}
		}
		private async void AddComment()
		{
			if (string.IsNullOrEmpty(SelectedImage.AddCommentText))
				return;
			Comment newComment = new Comment
			{
				ImageID = SelectedImage.Items.First().Image.ID,
				UserID = "588842",
				Message = SelectedImage.AddCommentText,
				Date = DateTime.Now
			};
			await _azure.AddComment(newComment);
			await Refresh();
            HockeyApp.MetricsManager.TrackEvent("Comment Added");
        }
        private bool _deleting = false;
		private async void DeleteComment(ViewCell cell)
		{
			Comment c = cell.BindingContext as Comment;
			if (c == null)
				return;
			if (!_deleting)
			{
				_deleting = true;
				await _azure.DeleteComment(c);
				await Refresh();
				_deleting = false;
			}
		}
		private async Task Refresh()
		{
			/*
			SelectedImage.Items.RemoveRange(1, SelectedImage.Items.Count - 1);

			Comment image = SelectedImage.Items.First();
			List<Comment> c = await _azure.GetComments(image.Image.ID);

			SelectedImage.Items.AddRange(c);
			SelectedImage.Items.Add(new Comment { CurrentType = GalleryType.AddComment });

			SelectedImage.AddCommentText = "";
			*/
		}
	}
	public class ImagesWithComments : BindableBase
	{
		private List<Comment> _items = new List<Comment>();
		public List<Comment> Items { get { return _items; } set { SetProperty(ref _items, value); } }
		
		public string AddCommentText { get; set; }
    }
	public class GalleryDataTemplateSelector : DataTemplateSelector
	{
		public DataTemplate ImageTemplate { get; set; }
		public DataTemplate CommentTemplate { get; set; }
		public DataTemplate AddCommentTemplate { get; set; }
		protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
		{
			Comment c = (Comment)item;
			if (c == null) return AddCommentTemplate;

			switch (c.CurrentType)
			{
				default:
					return CommentTemplate;
				case GalleryType.Image:
					return ImageTemplate;
				case GalleryType.AddComment:
					return AddCommentTemplate;
			}
		}
	}
}
