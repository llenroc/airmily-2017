using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using airmily.Services.Models;
using airmily.Services.ModelsExample;
using Prism.Events;
using Prism.Navigation;
using Xamarin.Forms;

namespace airmily.ViewModels
{
    public class ExampleDashboardPageViewModel : BindableBase, INavigationAware
    {
        private INavigationService _navigationService;
        private IEventAggregator _eventAggregator;

        private string _title;

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private ObservableCollection<SampleCategory> _items;

        public ObservableCollection<SampleCategory> Items
        {
            get { return _items; }
            set { SetProperty(ref _items, value); }
        }

        public ExampleDashboardPageViewModel(INavigationService navigationService, IEventAggregator eventAggregator)
        {
            _navigationService = navigationService;
            _eventAggregator = eventAggregator;

            SamplesDefinition.NavigationService = _navigationService;
            SamplesDefinition.EventAggregator = _eventAggregator;

            Title = "Dashboard";
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {

        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            Items = SamplesDefinition.SamplesCategoryList;
        }
    }
}
