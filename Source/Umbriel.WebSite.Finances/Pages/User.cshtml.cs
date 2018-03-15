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
    public class UserModel : PageModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        private IInvestorManager InvestorManager { get; set; }

        public UserModel(IInvestorManager investorManager, IDataRepository repository)
        {
            this.InvestorManager = investorManager;
        }

        public void OnGet()
        {
            IReadOnlyList<Investor> investors = this.InvestorManager.GetAllInvestors();
        }
    }
}