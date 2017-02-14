using Prism.Mvvm;

namespace airmily.ViewModels
{
    public class ExampleProfilePageViewModel : BindableBase
    {
        private string _title;

        public ExampleProfilePageViewModel()
        {
            Title = "Profile";
        }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
    }
}