using airmily.Services;
using airmily.Services.TrackSeries;
using airmily.Services.Azure;
using Prism.Unity;
using airmily.Views;
using Microsoft.Practices.Unity;
using Xamarin.Forms;

namespace airmily
{
    public partial class App : PrismApplication
    {
        public App(IPlatformInitializer initializer = null) : base(initializer) { }

        protected override void OnInitialized()
        {
            InitializeComponent();

			// NavigationService.NavigateAsync("NavigationPage/MainPage");

			// NavigationService.NavigateAsync("NavigationPage/MainPage?title=XXX Main Page XXX");

            NavigationService.NavigateAsync("NavigationPage/CardsListPage");

			// NavigationService.NavigateAsync("/PrismContentPage1?data=Monday");
		}

        protected override void RegisterTypes()
        {
            Container.RegisterType<ITrackSeries, TrackSeries>();
			Container.RegisterType<IAzure, Azure>();

            Container.RegisterTypeForNavigation<NavigationPage>();
            Container.RegisterTypeForNavigation<MainPage>();
            Container.RegisterTypeForNavigation<DetailPage>();
            Container.RegisterTypeForNavigation<MainTabbedPage>();
            Container.RegisterTypeForNavigation<UpcomingShowsPage>();
            Container.RegisterTypeForNavigation<ShowsListPage>();
            Container.RegisterTypeForNavigation<PrismContentPage1>();
			Container.RegisterTypeForNavigation<CardsListPage>();
			Container.RegisterTypeForNavigation<TransactionListPage>();
		}
    }
}
