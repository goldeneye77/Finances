namespace Umbriel.Data
{
    using System;

    public interface IDbTransactionScope: IDisposable
    {
        void Commit();
    }
}
