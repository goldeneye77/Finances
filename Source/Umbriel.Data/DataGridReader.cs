namespace Umbriel.Data
{
    using System;
    using System.Data;

    using Dapper;

    public class DataGridReader : IDisposable
    {
        internal DataGridReader(IDbConnection connection, SqlMapper.GridReader gridReader)
        {
            this.Reader = gridReader;
            this.Connection = connection;
        }

        public SqlMapper.GridReader Reader { get; }

        private IDbConnection Connection { get; }


        public void Dispose()
        {
            Reader?.Dispose();

            Connection?.Dispose();
        }
    }
}
