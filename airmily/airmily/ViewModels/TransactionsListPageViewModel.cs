using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using airmily.Services.Azure;
using airmily.Services.Models;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Forms;

namespace airmily.ViewModels
{
	public class TransactionsListPageViewModel : BindableBase, INavigationAware
	{
        private readonly IPageDialogService _pageDialogService;
        private readonly IAzure _azure;

		private ObservableCollection<Transaction> _transactionsList;
		public ObservableCollection<Transaction> TransactionsList
		{
			get { return _transactionsList; }
			set { SetProperty(ref _transactionsList, value); }
		}

        private string _title;

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public TransactionsListPageViewModel(IPageDialogService pageDialogService, IAzure azure)
        {
            _pageDialogService = pageDialogService;
			_azure = azure;

		    Title = "Transactions";
		}

		public void OnNavigatedFrom(NavigationParameters parameters)
		{

		}

		public async void OnNavigatedTo(NavigationParameters parameters)
		{
		    if (parameters.ContainsKey("cardId"))
		    {
		        var cardId = parameters["cardId"].ToString();
                var ret = await _azure.GetTransactions(cardId);
                TransactionsList = new ObservableCollection<Transaction>(ret);
            }
        }

        private DelegateCommand<ItemTappedEventArgs> _onTransactionTapped;

        public DelegateCommand<ItemTappedEventArgs> OnTransactionTapped
        {
            get
            {
                if (_onTransactionTapped == null)
                {
                    _onTransactionTapped = new DelegateCommand<ItemTappedEventArgs>(async selected =>
                    {
                        var transaction = selected.Item as Transaction;
                        var id = new NavigationParameters { ["id"] = transaction.ID };
                        await _pageDialogService.DisplayAlertAsync(
                            "Upload",
                            String.Format("Please upload receipts/tax forms for Transaction {0}", id),
                            "OK");
                    });
                }

                return _onTransactionTapped;
            }
        }
    }
}
