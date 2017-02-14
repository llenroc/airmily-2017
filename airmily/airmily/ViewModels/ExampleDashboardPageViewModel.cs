using System;
using System.Collections.ObjectModel;
using airmily.Services.ModelsExample;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;

namespace airmily.ViewModels
{
    public class ExampleDashboardPageViewModel : BindableBase, INavigationAware
    {
        private readonly INavigationService _navigationService;

        private ObservableCollection<SampleCategory> _items;

        private DelegateCommand<EventArgs> _navigating;

        private string _title;

        public ExampleDashboardPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            Title = "Dashboard";
        }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public ObservableCollection<SampleCategory> Items
        {
            get { return _items; }
            set { SetProperty(ref _items, value); }
        }

        public DelegateCommand<EventArgs> Navigating
        {
            get
            {
                if (_navigating == null)
                    _navigating =
                        new DelegateCommand<EventArgs>(
                            async e => { await _navigationService.NavigateAsync("CardsListPage"); });

                return _navigating;
            }
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