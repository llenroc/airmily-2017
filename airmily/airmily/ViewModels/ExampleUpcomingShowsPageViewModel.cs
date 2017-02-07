using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using airmily.Services.Models;
using airmily.Services.TrackSeries;
using Prism.Navigation;

namespace airmily.ViewModels
{
    public class ExampleUpcomingShowsPageViewModel : BindableBase, INavigationAware
    {
        private string _title;

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private string _content;
        public string Content
        {
            get { return _content; }
            set { SetProperty(ref _content, value); }
        }

        public ExampleUpcomingShowsPageViewModel()
        {
            Title = "Upcoming Shows";
            Content = "Upcoming shows... Star Wars... Terminator 3... Sing... Star Trek...";
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {

        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {

        }
    }
}
