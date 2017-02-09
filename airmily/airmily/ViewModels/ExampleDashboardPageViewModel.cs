using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using airmily.Services.Models;
using airmily.Services.ModelsExample;
using Xamarin.Forms;

namespace airmily.ViewModels
{
    public class ExampleDashboardPageViewModel : BindableBase
    {
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

        public ExampleDashboardPageViewModel()
        {
            Title = "Dashboard";

            Items = SamplesDefinition.SamplesCategoryList;
        }
    }
}
