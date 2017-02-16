using Xamarin.Forms;

namespace airmily.Views
{
    public partial class FullScreenImagePage : ContentPage
    {
        public FullScreenImagePage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }
    }
}
