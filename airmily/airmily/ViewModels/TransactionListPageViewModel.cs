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
	public class TransactionListPageViewModel : BindableBase, INavigationAware
	{
		private readonly IAzure _azure;

		private ObservableCollection<Transaction> _transactionList;
		public ObservableCollection<Transaction> TransactionList
		{
			get { return _transactionList; }
			set { SetProperty(ref _transactionList, value); }
		}

		public TransactionListPageViewModel(IAzure azure)
		{
			_azure = azure;
		}

		public void OnNavigatedFrom(NavigationParameters parameters)
		{

		}

		public async void OnNavigatedTo(NavigationParameters parameters)
		{
			if (!parameters.ContainsKey("card"))
				return;	//Initialise TransactionList?

			var ret = await _azure.GetTransactions(parameters["card"].ToString());
			TransactionList = new ObservableCollection<Transaction>(ret);
		}
	}
}
