using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using airmily.Interfaces;
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
        private readonly IAuth _auth;

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
        private DelegateCommand _refreshCommand;
        private bool _isRefreshing;
        private Transaction _selectedTransaction;
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set { SetProperty(ref _isRefreshing, value); }
        }
        public TransactionsListPageViewModel(IPageDialogService pageDialogService, IAzure azure,
            INavigationService navigationService, IAuth auth)
        {
            _pageDialogService = pageDialogService;
            _azure = azure;
            _navigationService = navigationService;
            _auth = auth;
            Title = "Transactions";
        }
        public ObservableCollection<Transaction> TransactionsList
        {
            get { return _transactionsList; }
            set { SetProperty(ref _transactionsList, value); }
        }
        public DelegateCommand RefreshCommand
        {
            get
            {
                if(_refreshCommand == null)
                {
                    _refreshCommand = new DelegateCommand(RefreshList);
                }
                return _refreshCommand;
            }
        }
        public Transaction SelectedTransaction
        {
            get { return _selectedTransaction; }
            set { SetProperty(ref _selectedTransaction, value); }
        }
        public async void RefreshList()
        {
            IsRefreshing = true;

            HockeyApp.MetricsManager.TrackEvent("Transaction List Refreshed");

            await _azure.UpdateAllTransactions(_auth.getCurrentUser(), CurrentCard.CardID);
            var ret = await _azure.GetAllTransactions(CurrentCard.CardID);
            TransactionsList = null;
            TransactionsList = new ObservableCollection<Transaction>(ret);
            IsRefreshing = false;
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
                        var parameters = new NavigationParameters {["transaction"] = trans};
                        SelectedTransaction = null;
                        await _navigationService.NavigateAsync("ViewImagesPage", parameters);
                    });
                return _onTransactionTapped;
            }
        }
        public void OnNavigatedFrom(NavigationParameters parameters)
        {
        }
        public void OnNavigatedTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey("card"))
            {
                // CurrentUser = parameters.ContainsKey("ffx") ? (User) parameters["ffx"] : new User {Active = false};
                CurrentCard = (Card) parameters["card"];
                RefreshList();
                HockeyApp.MetricsManager.TrackEvent("Transaction Page Loaded");
            }
        }
    }
}