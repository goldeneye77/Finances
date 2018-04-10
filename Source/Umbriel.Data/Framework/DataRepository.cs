namespace Umbriel.Data.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Transactions;

    using Enums;
    using Dapper;
    using Microsoft.Data.Sqlite;

    public class DataRepository : IDataRepository
    {
        private DatabaseType DatabaseType { get; set; }

        private string DbConnectionString { get; set; }

        private IDbConnection DbConnection
        {
            get
            {
                IDbConnection dbConnection = null;

                switch (this.DatabaseType)
                {
                    case DatabaseType.SqlServer:
                        dbConnection = new SqlConnection(this.DbConnectionString);
                        break;
                    case DatabaseType.Sqlite:
                        dbConnection = new SqliteConnection(this.DbConnectionString);
                        break;
                    case DatabaseType.Unknown:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                if (dbConnection != null && dbConnection.State != ConnectionState.Open)
                {
                    dbConnection.Open();
                }

                if (Transaction.Current != null)
                {
                    // enlist with the existing transaction
                    ((DbConnection)dbConnection)?.EnlistTransaction(Transaction.Current);
                }

                return dbConnection;
            }
        }

        public DataRepository(DatabaseType databaseType, string dbConnectionString)
        {
            this.DatabaseType = databaseType;
            this.DbConnectionString = dbConnectionString;
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