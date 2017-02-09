using airmily.Services;
using airmily.Services.TrackSeries;
using airmily.Services.Azure;
using Prism.Unity;
using airmily.Views;
using Microsoft.Practices.Unity;
using Prism.Navigation;
using Xamarin.Forms;

namespace airmily
{
    public partial class App : PrismApplication
    {
        public App(IPlatformInitializer initializer = null) : base(initializer) { }

        protected override void OnInitialized()
        {
            InitializeComponent();

            //var parameters = new NavigationParameters {["userId"] = "668788"};
            //NavigationService.NavigateAsync("NavigationPage/CardsListPage", parameters);

            NavigationService.NavigateAsync("NavigationPage/ExampleProfilePage");
        }

        protected override void RegisterTypes()
        {
			Container.RegisterType<IAzure, Azure>();

            Container.RegisterTypeForNavigation<NavigationPage>();
			Container.RegisterTypeForNavigation<CardsListPage>();
			Container.RegisterTypeForNavigation<TransactionsListPage>();
            Container.RegisterTypeForNavigation<ExampleProfilePage>();
        }
            Container.RegisterTypeForNavigation<ViewImagesPage>();
        }
    }
}
