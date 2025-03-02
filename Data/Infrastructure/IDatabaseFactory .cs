using Domain.Models;
using System;

namespace bank.Data.Infrastructure
{
    public interface IDatabaseFactory : IDisposable
    {
        SteDataBaseContext DataContext { get; }
    }

}
