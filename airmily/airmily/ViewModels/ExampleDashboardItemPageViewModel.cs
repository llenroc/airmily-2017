using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using airmily.Services.Models;
using Prism.Events;
using Prism.Navigation;
using Xamarin.Forms;

namespace airmily.ViewModels
{
    public class ExampleDashboardItemPageViewModel : BindableBase
    {
        private readonly INavigationService _navigationService;
        private readonly IEventAggregator _eventAggregator;

        public ExampleDashboardItemPageViewModel(INavigationService navigationService, IEventAggregator eventAggregator)
        {
            _navigationService = navigationService;
            _eventAggregator = eventAggregator;
        }

        private DelegateCommand<EventArgs> _onNavigatingFrom;

        public DelegateCommand<EventArgs> OnNavigatingFrom
        {
            get
            {
                if (_onNavigatingFrom == null)
                {
                    _onNavigatingFrom = new DelegateCommand<EventArgs>(async e =>
                    {
                        await _navigationService.NavigateAsync("CardsListPage");
                    });
                }

                return _onNavigatingFrom;
            }
        }
    }
}
