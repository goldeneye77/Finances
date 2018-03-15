using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Umbriel.Data.Framework
{
    //using Microsoft.Data.Sqlite;

    public class DataRepository : IDataRepository
    {
        public IConfiguration Configuration { get; set; }

        private string DbConnectionString { get; set; }

        private IDbConnection DbConnection
        {
            get
            {
                IDbConnection dbConnection = new SqlConnection(this.DbConnectionString);

                if (dbConnection.State != ConnectionState.Open)
                {
                    dbConnection.Open();
                }

                if (Transaction.Current != null)
                {
                    // join the existing transaction
                    ((DbConnection)dbConnection).EnlistTransaction(Transaction.Current);
                }

                return dbConnection;
            }
        }

        public DataRepository(IConfiguration configuration)
        {
            this.Configuration = configuration;

            this.DbConnectionString = this.Configuration.GetConnectionString("FinancesDbContext");
        }

        /// <inheritdoc />
        public void Execute(
            string commandText,
            CommandType? commandType = CommandType.StoredProcedure,
            DynamicParameters parameters = null,
            IDbTransaction transaction = null)
        {
            using (var dbConnection = this.DbConnection)
            {
                dbConnection.Execute(commandText, parameters, transaction, null, commandType);
            }
        }

        /// <inheritdoc />
        public IList<T> Execute<T>(
            string commandText,
            CommandType? commandType = CommandType.StoredProcedure,
            DynamicParameters parameters = null,
            IDbTransaction transaction = null)
        {
            using (var dbConnection = this.DbConnection)
            {
                return dbConnection.Query<T>(commandText, parameters, transaction, commandType: commandType).ToList();
            }
        }

        /// <inheritdoc />
        public IDbTransactionScope BeginTransactionScope()
        {
            return new DbTransactionScope();
        }
        
        public void Dispose()
        {
        }
    }
}