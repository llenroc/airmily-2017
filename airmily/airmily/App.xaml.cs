using airmily.Services;
using airmily.Services.TrackSeries;
using airmily.Services.Azure;
using airmily.Services.Models;
using Prism.Unity;
using airmily.Views;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
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

	        JsonConvert.DefaultSettings = () => new JsonSerializerSettings()
	        {
		        DateParseHandling = DateParseHandling.None
	        };

	        User current = new User()
	        {
				//UserName = "Ashley",
				//UserID = "668788",
				//Active = true,
				//UnionID = "ovrpnuAIYBHme2uJCbT-FrQZAvVs",
				//OpenID = "oDDFqw26hj8e22e1waqCRvmx_s8U",
				//FairFX = "YXNobGV5LnN3YW5zb25AYmVpZXIzNjAuY29tQFN3YW5zb24xOTk2"
				UserName = "Suzy",
				UserID = "588842",
				Active = true,
				UnionID = "",
				OpenID = "",
				FairFX = "c3V6eS5waWVyY2VAYmVpZXIzNjAuY29tQEp1TGkyMjM="
			};
            var parameters = new NavigationParameters {["user"] = current};
            NavigationService.NavigateAsync("NavigationPage/CardsListPage", parameters);
		}

        protected override void RegisterTypes()
        {
			Container.RegisterType<IAzure, Azure>();

            Container.RegisterTypeForNavigation<NavigationPage>();
			Container.RegisterTypeForNavigation<CardsListPage>();
			Container.RegisterTypeForNavigation<TransactionsListPage>();
		}
    }
}
