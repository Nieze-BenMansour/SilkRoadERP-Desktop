using Domain.Models;
using System.Data.Entity.Core.Objects;

namespace bank.Data.Infrastructure
{
    public class DatabaseFactory : Disposable, IDatabaseFactory
    {
        private SteDataBaseContext dataContext;
        public SteDataBaseContext DataContext { get { return dataContext; } }

        public DatabaseFactory()
        {
            dataContext = new SteDataBaseContext(); 
        }
        protected override void DisposeCore()
        {
            if (DataContext != null)
                DataContext.Dispose();
        }

    }

}
