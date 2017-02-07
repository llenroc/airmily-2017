using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using Prism.Navigation;
using Xamarin.Forms;

namespace airmily.ViewModels
{
    public class ExampleMainTabbedPageViewModel : BindableBase
    {
        private string _title;

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public ExampleMainTabbedPageViewModel()
        {
            Title = "airmily";
        }
    }
}
