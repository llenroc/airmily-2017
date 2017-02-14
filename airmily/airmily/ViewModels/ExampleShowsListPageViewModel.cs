using System.Collections.ObjectModel;
using airmily.Services.ModelsExample;
using airmily.Services.TrackSeries;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Xamarin.Forms;

namespace airmily.ViewModels
{
    public class ExampleShowsListPageViewModel : BindableBase, INavigationAware, IConfirmNavigation
    {
        private readonly INavigationService _navigationService;
        private readonly ITrackSeries _trackSeries;

        private DelegateCommand<ItemTappedEventArgs> _goToDetailPage;

        private bool _isTappingEnabled;

        private string _title;

        private ObservableCollection<SerieFollowers> _topSeries;

        public ExampleShowsListPageViewModel(ITrackSeries trackSeries, INavigationService navigationService)
        {
            _trackSeries = trackSeries;
            _navigationService = navigationService;

            IsTappingEnabled = true;
            Title = "Shows List";
        }

        public bool IsTappingEnabled
        {
            get { return _isTappingEnabled; }
            set { SetProperty(ref _isTappingEnabled, value); }
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
                        var param = new NavigationParameters();
                        var serie = selected.Item as SerieFollowers;
                        param.Add("id", serie.Id);

                        await _navigationService.NavigateAsync(
                            "/NavigationPage/ExampleMainTabbedPage/ExampleShowsListPage/ExampleDetailPage", param);

                        // await _navigationService.NavigateAsync(
                        //    "/ExampleMainTabbedPage/NavigationPage/ExampleShowsListPage/ExampleDetailPage", param);

                        //await _navigationService.NavigateAsync(
                        //   "/NavigationPage/ExampleShowsListPage/ExampleDetailPage", param);
                    }, args => IsTappingEnabled);

                return _goToDetailPage;
            }
        }

        public bool CanNavigate(NavigationParameters parameters)
        {
            return true;
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