using System.Transactions;

namespace Umbriel.Data.Framework
{
    public class DbTransactionScope : IDbTransactionScope
	{
        private TransactionScope transactionScope;

        public DbTransactionScope()
        {
            this.transactionScope 
                    = new TransactionScope(
                             TransactionScopeOption.Required,
                             new TransactionOptions
                             {
                                 IsolationLevel = IsolationLevel.ReadCommitted,
                                 // TODO: read timeout from config
                                 Timeout = TransactionManager.MaximumTimeout
                             },
                             TransactionScopeAsyncFlowOption.Enabled);
        }

        /// <inheritdoc />
        public void Commit()
        {
            this.transactionScope.Complete();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            this.transactionScope.Dispose();
        }
    }
}
