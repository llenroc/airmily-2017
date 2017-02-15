using System.Collections.ObjectModel;
using airmily.Services.Azure;
using airmily.Services.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Forms;

namespace airmily.ViewModels
{
    public class TransactionsListPageViewModel : BindableBase, INavigationAware
    {
        private readonly IAzure _azure;
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _pageDialogService;

        private Card _currentCard = new Card
        {
            CardHolder = "John Smith",
            Number = "0000********0000",
            Currency = "£",
            Balance = "0.00"
        };

        private DelegateCommand<ItemTappedEventArgs> _onTransactionTapped;

        private string _title;

        private ObservableCollection<Transaction> _transactionsList;

        public TransactionsListPageViewModel(IPageDialogService pageDialogService, IAzure azure, INavigationService navigationService)
        {
            _pageDialogService = pageDialogService;
            _azure = azure;
            _navigationService = navigationService;

            Title = "Transactions";
        }

        public ObservableCollection<Transaction> TransactionsList
        {
            get { return _transactionsList; }
            set { SetProperty(ref _transactionsList, value); }
        }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public Card CurrentCard
        {
            get { return _currentCard; }
            set { SetProperty(ref _currentCard, value); }
        }

        public DelegateCommand<ItemTappedEventArgs> OnTransactionTapped
        {
            get
            {
                if (_onTransactionTapped == null)
                    _onTransactionTapped = new DelegateCommand<ItemTappedEventArgs>(async selected =>
                    {
                        var trans = selected.Item as Transaction;
                        var parameters = new NavigationParameters{["transaction"] = trans};
                        await _navigationService.NavigateAsync("ViewImagesPage", parameters);
                    });

                return _onTransactionTapped;
            }
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
        }

        public async void OnNavigatedTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey("card"))
            {
                var credentials = parameters.ContainsKey("ffx") ? (User) parameters["ffx"] : new User {Active = false};

                CurrentCard = (Card) parameters["card"];

                await _azure.UpdateAllTransactions(credentials, CurrentCard.CardID);
                var ret = await _azure.GetAllTransactions(CurrentCard.CardID);
                TransactionsList = new ObservableCollection<Transaction>(ret);
            }
        }
    }
}