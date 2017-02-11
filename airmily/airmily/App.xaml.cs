using airmily.Services;
using airmily.Services.TrackSeries;
using airmily.Services.Azure;
using Prism.Unity;
using airmily.Views;
using Microsoft.Practices.Unity;
using Prism.Common;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using Xamarin.Forms;

namespace airmily
{
    public partial class App : PrismApplication
    {
        public App(IPlatformInitializer initializer = null) : base(initializer)
        {

        }

        protected override void OnInitialized()
        {
            InitializeComponent();

            //var parameters = new NavigationParameters { ["userId"] = "668788" };
            //NavigationService.NavigateAsync("NavigationPage/CardsListPage", parameters);

            NavigationService.NavigateAsync("/NavigationPage/ExampleDashboardPage");
        }

        protected override void RegisterTypes()
        {
			Container.RegisterType<IAzure, Azure>();

            Container.RegisterTypeForNavigation<NavigationPage>();
			Container.RegisterTypeForNavigation<CardsListPage>();
			Container.RegisterTypeForNavigation<TransactionsListPage>();
            Container.RegisterTypeForNavigation<ViewImagesPage>();

            Container.RegisterTypeForNavigation<ExampleProfilePage>();
            Container.RegisterTypeForNavigation<ExampleDashboardPage>();
        }

        protected override void ConfigureViewModelLocator()
        {
            ViewModelLocationProvider.SetDefaultViewModelFactory((view, type) =>
            {
                ParameterOverrides overrides = null;
                
                var page = view as Page;
                if (page != null)
                {
                    var navService = CreateNavigationService();
                    ((IPageAware)navService).Page = page;

                    overrides = new ParameterOverrides
                    {
                        { "navigationService", navService }
                    };
                }

                var contentView = view as ContentView;
                if (contentView != null)
                {
                    var navService = CreateNavigationService();
                    var eventAggregator = Container.Resolve<IEventAggregator>();

                    overrides = new ParameterOverrides
                    {
                        { "navigationService", navService },
                        { "eventAggregator", eventAggregator }
                    };
                }

                return Container.Resolve(type, overrides);
            });
        }
    }
}
