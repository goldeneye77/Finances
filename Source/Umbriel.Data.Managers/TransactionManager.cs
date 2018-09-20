using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Umbriel.Data.Framework;
using Umbriel.Models;
using Umbriel.Models.Interfaces.Managers;

namespace Umbriel.Data.Managers
{
    public class TransactionManager : ITransactionManager
    {
        private IDataRepository DataRepo {get; set; }

        public TransactionManager(IDataRepository repo)
        {
            this.DataRepo = repo;
        } 

        public IReadOnlyList<Transaction> GetAllTransactions()
        {
            string sql =
                "SELECT" +
                   " t.TransactionId," +
                   " t.TransactionDate," +
                   " t.TransactionType," +
                   " t.Quantity," +
                   " t.Price," +
                   " a.AccountId AS Account_AccountId," +
                   " a.AccountTypeName AS Account_Name," +
                   " a.CurrencyType AS Account_CurrencyTypeName," +
                   " i.InvestorId AS Investor_InvestorId," +
                   " i.InvestorLastName AS Investor_LastName," +
                   " i.InvestorFirstName AS Investor_FirstName," +
                   " s.SecurityId AS Security_SecurityId," +
                   " s.SecurityAbbreviation AS Security_Abbreviation," +
                   " s.SecurityName AS Security_Name" +
                " FROM" +
                    " Transactions t" +
                    " INNER JOIN Investors i ON t.InvestorId = i.InvestorId" +
                    " INNER JOIN Securities s ON s.SecurityId = t.SecurityId" +
                    " INNER JOIN AccountTypes a ON a.AccountId = t.AccountId" +
                " ORDER BY" +
                    " t.TransactionDate DESC";

            using (DataGridReader grid = this.DataRepo.QueryMultiple(sql, CommandType.Text))
            {
                SqlMapper.GridReader reader = grid.Reader;
                IEnumerable<dynamic> transactions = reader.Read<dynamic>();

                IEnumerable<Transaction> data = Slapper.AutoMapper.MapDynamic<Transaction>(transactions);

                // Clear the internal cache created
                Slapper.AutoMapper.Cache.ClearInstanceCache();

                var items = data.ToList().AsReadOnly();

                return items;
            }
        }
    }
}