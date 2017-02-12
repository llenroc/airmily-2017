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
        private readonly INavigationService _navigationService;

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

        public ExampleDashboardPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

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
