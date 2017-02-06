using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using airmily.Services.Models;
using airmily.Services.TrackSeries;
using Prism.Navigation;
using Xamarin.Forms;

namespace airmily.ViewModels
{
    public class ShowsListPageViewModel : BindableBase, INavigationAware, IConfirmNavigation
    {
        private readonly ITrackSeries _trackSeries;
        private readonly INavigationService _navigationService;

        private bool _isTappingEnabled;

        public bool IsTappingEnabled
        {
            get { return _isTappingEnabled; }
            set { SetProperty(ref _isTappingEnabled, value); }
        }

        private string _title;

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private ObservableCollection<SerieFollowers> _topSeries;

        public ObservableCollection<SerieFollowers> TopSeries
        {
            get { return _topSeries; }
            set { SetProperty(ref _topSeries, value); }
        }
        
        public ShowsListPageViewModel(ITrackSeries trackSeries, INavigationService navigationService)
        {
            _trackSeries = trackSeries;
            _navigationService = navigationService;

            IsTappingEnabled = true;
            Title = "Shows List";
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {

        }

        public async void OnNavigatedTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey("title"))
                Title = (string)parameters["title"];

            var result = await _trackSeries.GetStatsTopSeries();
            TopSeries = new ObservableCollection<SerieFollowers>(result);
        }

        private DelegateCommand<ItemTappedEventArgs> _goToDetailPage;

        public DelegateCommand<ItemTappedEventArgs> GoToDetailPage
        {
            get
            {
                if (_goToDetailPage == null)
                {
                    _goToDetailPage = new DelegateCommand<ItemTappedEventArgs>(async selected =>
                    {
                        NavigationParameters param = new NavigationParameters();
                        var serie = selected.Item as SerieFollowers;
                        param.Add("id", serie.Id);

                        await _navigationService.NavigateAsync("/NavigationPage/MainTabbedPage/ShowsListPage/DetailPage", param);

                        // await _navigationService.NavigateAsync(
                        //    "/MainTabbedPage/NavigationPage/ShowsListPage/DetailPage", param);

                        //await _navigationService.NavigateAsync(
                        //   "/NavigationPage/ShowsListPage/DetailPage", param);
                    }, args => IsTappingEnabled);
                }

                return _goToDetailPage;
            }
        }

        public bool CanNavigate(NavigationParameters parameters)
        {
            return true;
        }
    }
}
