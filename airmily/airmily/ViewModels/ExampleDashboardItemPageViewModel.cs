using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using airmily.Services.Models;
using Prism.Navigation;
using Xamarin.Forms;

namespace airmily.ViewModels
{
    public class ExampleDashboardItemPageViewModel : BindableBase
    {
        private readonly INavigationService _navigationService;

        public ExampleDashboardItemPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        private DelegateCommand _onNavigatingFrom;
        
        public DelegateCommand OnNavigatingFrom
        {
            get
            {
                if (_onNavigatingFrom == null)
                {
                    _onNavigatingFrom = new DelegateCommand(async () =>
                    {
                        await _navigationService.NavigateAsync("CardsListPage");
                    });
                }

                return _onNavigatingFrom;
            }
        }
    }
}
