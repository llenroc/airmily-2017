using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using airmily.Services.Models;
using airmily.Services.ModelsExample;
using airmily.Services.TrackSeries;
using Microsoft.Practices.Unity.ObjectBuilder;
using Xamarin.Forms;

namespace airmily.ViewModels
{
    public class ExampleMainPageViewModel : BindableBase, INavigationAware
    {
        private readonly ITrackSeries _trackSeries;
        private readonly INavigationService _navigationService;

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

        public ExampleMainPageViewModel(ITrackSeries trackSeries, INavigationService navigationService)
        {
            _trackSeries = trackSeries;
            _navigationService = navigationService;

            Title = "Main Page";
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
                        var param = new NavigationParameters { { "show", selected.Item } };
                        var serie = selected.Item as SerieFollowers;
                        param.Add("id", serie.Id);
                        await _navigationService.NavigateAsync("ExampleDetailPage", param);
                    });
                }

                return _goToDetailPage;
            }
        }
    }
}
