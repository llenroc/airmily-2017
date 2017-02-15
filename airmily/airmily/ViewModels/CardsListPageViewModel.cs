using System.Collections.ObjectModel;
using airmily.Services.Azure;
using airmily.Services.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Xamarin.Forms;

namespace airmily.ViewModels
{
    public class CardsListPageViewModel : BindableBase, INavigationAware
    {
        private readonly IAzure _azure;
        private readonly INavigationService _navigationService;

        private ObservableCollection<Card> _cardsList;

        public ObservableCollection<Card> CardsList
        {
            get { return _cardsList; }
            set { SetProperty(ref _cardsList, value); }
        }

        private User _currentUser;

        private DelegateCommand<ItemTappedEventArgs> _goToTransactionsListPage;

        private string _title;

        public CardsListPageViewModel(INavigationService navigationService, IAzure azure)
        {
            _navigationService = navigationService;
            _azure = azure;

            Title = "Cards";
        }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
		
        public DelegateCommand<ItemTappedEventArgs> GoToTransactionsListPage
        {
            get
            {
                if (_goToTransactionsListPage == null)
                    _goToTransactionsListPage = new DelegateCommand<ItemTappedEventArgs>(async selected =>
                    {
                        var card = selected.Item as Card;
                        var parameters = new NavigationParameters {["card"] = card, ["ffx"] = _currentUser};
                        await _navigationService.NavigateAsync("TransactionsListPage", parameters);
                    });

                return _goToTransactionsListPage;
            }
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {

        }

        public async void OnNavigatedTo(NavigationParameters parameters)
        {
            if (!parameters.ContainsKey("user"))
                return;

            _currentUser = (User) parameters["user"];
            var ret = await _azure.GetAllCards(_currentUser.UserID);
            CardsList = new ObservableCollection<Card>(ret);
        }
    }
}