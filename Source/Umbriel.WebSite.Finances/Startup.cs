using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Umbriel.Data;
using Umbriel.Data.Enums;
using Umbriel.Data.Framework;
using Umbriel.Data.Managers;
using Umbriel.Models.Interfaces.Managers;

namespace Umbriel.WebSite.Finances
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            DatabaseType databaseType;

            bool useSqliteDb = Convert.ToBoolean(this.Configuration.GetSection("UseSqliteDb").Value);

            string connectionString;

            if (useSqliteDb)
            {
                databaseType = DatabaseType.Sqlite;
                string relativePath = this.Configuration.GetSection("SqliteDbPath").Value;
                string currentPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
                string absolutePath = Path.Combine(currentPath, relativePath);
                absolutePath = absolutePath.Remove(0, 6);//this code is written to remove file word from absolute path
                connectionString = string.Format("Data Source={0}", absolutePath);
            }
            else
            {
                databaseType = DatabaseType.SqlServer;
                string databaseToUse = this.Configuration.GetSection("DatabaseToUse").Value;
                connectionString = this.Configuration.GetConnectionString(databaseToUse);
            }

            // Inject IDataRepository, with implementation from DataRepository class.
            IDataRepository repository = new DataRepository(databaseType, connectionString);
            services.AddTransient(sp => repository);

            services.AddTransient<IInvestorManager>(im => new InvestorManager(repository));
            services.AddTransient<ITransactionManager>(tm => new TransactionManager(repository));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseMvc();
        }
    }
}
