using System;
using airmily.Services.ModelsExample;
using airmily.Services.TrackSeries;
using Prism.Mvvm;
using Prism.Navigation;

namespace airmily.ViewModels
{
    public class ExampleDetailPageViewModel : BindableBase, INavigationAware
    {
        private readonly ITrackSeries _trackSeries;

        private SerieInfo _selectedShow;

        public ExampleDetailPageViewModel(ITrackSeries trackSeries)
        {
            _trackSeries = trackSeries;
        }

        public SerieInfo SelectedShow
        {
            get { return _selectedShow; }
            set { SetProperty(ref _selectedShow, value); }
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
        }

        public async void OnNavigatedTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey("id"))
            {
                var id = Convert.ToInt32(parameters["id"]);
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