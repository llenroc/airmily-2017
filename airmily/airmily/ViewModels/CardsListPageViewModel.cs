using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

using airmily.Services.Models;
using Prism.Navigation;

namespace airmily.ViewModels
{
	public class CardsListPageViewModel : BindableBase, INavigationAware
	{
		private Card _card = new Card();

		public Card Card
		{
			get { return _card; }
			set { SetProperty(ref _card, value); }
		}
		private List<Card> _cardsList = new List<Card>();

		public List<Card> CardsList
		{
			get { return _cardsList; }
			set { SetProperty(ref _cardsList, value); }
		}

		public CardsListPageViewModel()
		{
			Card.CardHolder = "Test";
			Card.Currency = "£";
			Card.Balance = "194.32";
			Card.Number = "5114*****1234";
			CardsList.Add(Card);
		}

		public void OnNavigatedFrom(NavigationParameters parameters)
		{

		}
		public void OnNavigatedTo(NavigationParameters parameters)
		{

		}
	}
}
