using System;

namespace Umbriel.Data.Framework
{
    public interface IDbTransactionScope: IDisposable
    {
        void Commit();
    }
}
