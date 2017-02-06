using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using Prism.Navigation;

namespace airmily.ViewModels
{
    public class PrismContentPage1ViewModel : BindableBase, INavigationAware
    {
        private string _title;

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public PrismContentPage1ViewModel()
        {
            Title = "XXX Title XXX";
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
            
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey("data"))
                Title = (string)parameters["data"];
        }
    }
}
