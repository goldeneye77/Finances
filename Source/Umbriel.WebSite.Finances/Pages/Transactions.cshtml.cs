using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Umbriel.Data;
using Umbriel.Data.Framework;
using Umbriel.Data.Managers;
using Umbriel.Models;
using Umbriel.Models.Interfaces.Managers;

namespace Umbriel.WebSite.Finances.Pages
{
    public class TransactionModel : PageModel
    {
        public IReadOnlyList<Transaction> TransactionList { get; private set; }

        private ITransactionManager TransactionManager { get; set; }

        public TransactionModel(ITransactionManager transactionManager, IDataRepository repository)
        {
            this.TransactionManager = transactionManager;
        }

        public void OnGet()
        {
            IReadOnlyList<Transaction> transactions = this.TransactionManager.GetAllTransactions();
            this.TransactionList = transactions;
        }
    }
}