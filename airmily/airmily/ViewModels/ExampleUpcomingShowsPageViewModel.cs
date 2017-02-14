using Prism.Mvvm;
using Prism.Navigation;

namespace airmily.ViewModels
{
    public class ExampleUpcomingShowsPageViewModel : BindableBase, INavigationAware
    {
        private string _content;
        private string _title;

        public ExampleUpcomingShowsPageViewModel()
        {
            Title = "Upcoming Shows";
            Content = "Upcoming shows... Star Wars... Terminator 3... Sing... Star Trek...";
        }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public string Content
        {
            get { return _content; }
            set { SetProperty(ref _content, value); }
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
        }
    }
}