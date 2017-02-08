using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using airmily.Services.Azure;
using airmily.Services.Models;
using Prism.Navigation;
using Xamarin.Forms;

namespace airmily.ViewModels
{
	public class CardsListPageViewModel : BindableBase, INavigationAware
	{
	    private readonly INavigationService _navigationService;
	    private readonly IAzure _azure;

	    private string _title;

	    public string Title
	    {
	        get { return _title; }
            set { SetProperty(ref _title, value); }
	    }

	    private ObservableCollection<Card> _cardsList;

		public ObservableCollection<Card> CardsList
		{
			get { return _cardsList; }
			set { SetProperty(ref _cardsList, value); }
		}

		public CardsListPageViewModel(INavigationService navigationService, IAzure azure)
		{
		    _navigationService = navigationService;
            _azure = azure;

		    Title = "Airmily";
		}

		public void OnNavigatedFrom(NavigationParameters parameters)
		{

		}
		public async void OnNavigatedTo(NavigationParameters parameters)
		{
		    if (!parameters.ContainsKey("userId"))
                return;

            var userId = parameters["userId"].ToString();
		    var ret = await _azure.GetCards(userId);
		    CardsList = new ObservableCollection<Card>(ret);
		}

        private DelegateCommand<ItemTappedEventArgs> _goToTransactionsListPage;

        public DelegateCommand<ItemTappedEventArgs> GoToTransactionsListPage
        {
            get
            {
                if (_goToTransactionsListPage == null)
                {
                    _goToTransactionsListPage = new DelegateCommand<ItemTappedEventArgs>(async selected =>
                    {
                        var card = selected.Item as Card;
                        var parameters = new NavigationParameters {["card"] = card};
                        await _navigationService.NavigateAsync("TransactionsListPage", parameters);
                    });
                }

                return _goToTransactionsListPage;
            }
        }
    }
}
