using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using airmily.Interfaces;
using airmily.Services.AppService;
using airmily.Services.Azure;
using airmily.Services.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Xamarin.Forms;
using HockeyApp;
namespace airmily.ViewModels
{
    public class CardsListPageViewModel : BindableBase, INavigationAware
    {
        private readonly IAzure _azure;
        private readonly INavigationService _navigationService;
        private readonly IAuth _auth;

        private ObservableCollection<Card> _cardsList;
        private DelegateCommand<ItemTappedEventArgs> _goToTransactionsListPage;
        private DelegateCommand _refreshCommand;
        private string _title;
        private bool _isRefreshing;
        public CardsListPageViewModel(INavigationService navigationService, IAzure azure, IAuth auth)
        {
            _navigationService = navigationService;
            _azure = azure;
            _auth = auth;
            Title = "Cards";
        }
        public ObservableCollection<Card> CardsList
        {
            get { return _cardsList; }
            set { SetProperty(ref _cardsList, value); }
        }
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set { SetProperty(ref _isRefreshing, value); }
        }
        public DelegateCommand<ItemTappedEventArgs> GoToTransactionsListPage
        {
            get
            {
                HockeyApp.MetricsManager.TrackEvent("TransactionClicked");
                if (_goToTransactionsListPage == null)
                    _goToTransactionsListPage = new DelegateCommand<ItemTappedEventArgs>(async selected =>
                    {
                        var card = selected.Item as Card;
                        var parameters = new NavigationParameters {["card"] = card};
                        await _navigationService.NavigateAsync("TransactionsListPage", parameters);
                    });
                return _goToTransactionsListPage;
            }
        }
        public DelegateCommand RefreshCommand
        {
            get
            {
                if (_refreshCommand == null)
                {
                    _refreshCommand = new DelegateCommand(RefreshList);
                }
                return _refreshCommand;
            }
        }
        public async void RefreshList()
        {
            IsRefreshing = true;
            HockeyApp.MetricsManager.TrackEvent("Cards List Refreshed");
            await _azure.UpdateAllCards(_auth.getCurrentUser());
            var ret = await _azure.GetAllCards(_auth.getCurrentUser().UserID);
            CardsList = null;
            CardsList = new ObservableCollection<Card>(ret);
            IsRefreshing = false;
        }
        public void OnNavigatedFrom(NavigationParameters parameters)
        {
        }
        public void OnNavigatedTo(NavigationParameters parameters)
        {
            if (!parameters.ContainsKey("user"))
                return;
            _auth.setCurrentUser((User)parameters["user"]);
            //_currentUser = (User) parameters["user"];  //OLD WAY

            RefreshList();
        }
    }
}