using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using airmily.Services.Azure;
using airmily.Services.Models;
using Prism.Navigation;

namespace airmily.ViewModels
{
	public class CardsListPageViewModel : BindableBase, INavigationAware
	{
	    private readonly IAzure _azure;

	    private ObservableCollection<Card> _cardsList;

		public ObservableCollection<Card> CardsList
		{
			get { return _cardsList; }
			set { SetProperty(ref _cardsList, value); }
		}

		public CardsListPageViewModel(IAzure azure)
		{
		    _azure = azure;
        }

		public void OnNavigatedFrom(NavigationParameters parameters)
		{

		}
		public async void OnNavigatedTo(NavigationParameters parameters)
		{

            var ret = await _azure.GetCards("668788");
		    CardsList = new ObservableCollection<Card>(ret);

		}
    }
}
