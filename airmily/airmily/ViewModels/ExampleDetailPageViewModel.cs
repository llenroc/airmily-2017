using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using airmily.Services.Models;
using airmily.Services.ModelsExample;
using airmily.Services.TrackSeries;
using Prism.Navigation;

namespace airmily.ViewModels
{
    public class ExampleDetailPageViewModel : BindableBase, INavigationAware
    {
        private readonly ITrackSeries _trackSeries;

        private SerieInfo _selectedShow;

        public SerieInfo SelectedShow
        {
            get { return _selectedShow; }
            set { SetProperty(ref _selectedShow, value); }
        }

        public ExampleDetailPageViewModel(ITrackSeries trackSeries)
        {
            _trackSeries = trackSeries;
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {

        }

        public async void OnNavigatedTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey("id"))
            {
                int id = Convert.ToInt32(parameters["id"]);
                SelectedShow = await _trackSeries.GetSerieById(id);
            }
            else if (parameters.ContainsKey("id"))
            {
                var show = parameters["show"] as SerieFollowers;
                SelectedShow = await _trackSeries.GetSerieById(show.Id);
            }
        }
    }
}
