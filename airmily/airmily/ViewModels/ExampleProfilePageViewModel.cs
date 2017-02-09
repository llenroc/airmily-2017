using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace airmily.ViewModels
{
    public class ExampleProfilePageViewModel : BindableBase
    {
        private string _title;

        public string Title
        {
            get { return _title;}
            set { SetProperty(ref _title, value); }
        }

        public ExampleProfilePageViewModel()
        {
            Title = "Profile";
        }
    }
}
