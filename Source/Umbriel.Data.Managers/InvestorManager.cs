using System;
using System.Collections.Generic;
using System.Linq;
using Umbriel.Data.Framework;
using Umbriel.Models;
using Umbriel.Models.Interfaces.Managers;

namespace Umbriel.Data.Managers
{
    public class InvestorManager : IInvestorManager
    {
        private IDataRepository DataRepo {get; set; }

        public InvestorManager(IDataRepository repo)
        {
            this.DataRepo = repo;
        } 

        public IReadOnlyList<Investor> GetAllInvestors()
        {
            string sql = "SELECT InvestorId, InvestorFirstName AS FirstName, InvestorLastName AS LastName FROM Investors";
            return this.DataRepo.Execute<Investor>(sql, System.Data.CommandType.Text).ToList().AsReadOnly();
        }
    }

    public class StoredProcs
    {
        public const string GetAllInvestors = "Investors_GetAll";
    }
}
