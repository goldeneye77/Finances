using System;
using System.Collections.Generic;
using System.Text;

namespace Umbriel.Models.Interfaces.Managers
{
    public interface ITransactionManager
    {
        IReadOnlyList<Transaction> GetAllTransactions();
    }
}