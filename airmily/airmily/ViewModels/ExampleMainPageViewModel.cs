using System.Collections.ObjectModel;
using airmily.Services.ModelsExample;
using airmily.Services.TrackSeries;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Xamarin.Forms;

namespace airmily.ViewModels
{
    public class ExampleMainPageViewModel : BindableBase, INavigationAware
    {
        private readonly INavigationService _navigationService;
        private readonly ITrackSeries _trackSeries;

        private DelegateCommand<ItemTappedEventArgs> _goToDetailPage;

        private string _title;

        private ObservableCollection<SerieFollowers> _topSeries;

        public ExampleMainPageViewModel(ITrackSeries trackSeries, INavigationService navigationService)
        {
            _trackSeries = trackSeries;
            _navigationService = navigationService;

            Title = "Main Page";
        }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public ObservableCollection<SerieFollowers> TopSeries
        {
            get { return _topSeries; }
            set { SetProperty(ref _topSeries, value); }
        }

        public DelegateCommand<ItemTappedEventArgs> GoToDetailPage
        {
            get
            {
                if (_goToDetailPage == null)
                    _goToDetailPage = new DelegateCommand<ItemTappedEventArgs>(async selected =>
                    {
                        var param = new NavigationParameters {{"show", selected.Item}};
                        var serie = selected.Item as SerieFollowers;
                        param.Add("id", serie.Id);
                        await _navigationService.NavigateAsync("ExampleDetailPage", param);
                    });

                return _goToDetailPage;
            }
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
        }

        public async void OnNavigatedTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey("title"))
                Title = (string) parameters["title"];

            var result = await _trackSeries.GetStatsTopSeries();
            TopSeries = new ObservableCollection<SerieFollowers>(result);
        }
    }
}