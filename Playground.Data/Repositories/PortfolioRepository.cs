using Playground.Data.Contexts;
using Playground.Data.Generics;
using Playground.Domain.Entities;

namespace Playground.Data.Repositories
{
    public class PortfolioRepository : GenericRepository<Portfolio>, IPortfolioRepository
    {
        public PortfolioRepository(DataContext context) : base(context)
        {
        }
    }
}
