using System;
using System.Collections.Generic;
using System.Data;
using Dapper;

namespace Umbriel.Data.Framework
{
    public interface IDataRepository : IDisposable
    {
        void Execute(
            string commandText,
            CommandType? commandType = CommandType.StoredProcedure,
            DynamicParameters parameters = null,
            IDbTransaction transaction = null);

        IList<T> Execute<T>(
            string commandText,
            CommandType? commandType = CommandType.StoredProcedure,
            DynamicParameters parameters = null,
            IDbTransaction transaction = null);

        DataGridReader QueryMultiple(
            string commandText,
            CommandType? commandType = CommandType.StoredProcedure,
            DynamicParameters parameters = null,
            IDbTransaction transaction = null);

        IDbTransactionScope BeginTransactionScope();
    }
}