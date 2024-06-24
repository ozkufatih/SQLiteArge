using Playground.Data.Generics;
using Playground.Domain.Entities;

namespace Playground.Data.Repositories
{
    public interface IPortfolioAssetRepository : IGenericRepository<PortfolioAsset>
    {
        Task<List<PortfolioAsset>> GetByPortfolioIdAsync(Guid portfolioId);
    }
}
