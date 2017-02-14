using Prism.Mvvm;

namespace airmily.ViewModels
{
    public class ExampleMainTabbedPageViewModel : BindableBase
    {
        private string _title;

        public ExampleMainTabbedPageViewModel()
        {
            Title = "airmily";
        }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
    }
}