using Microsoft.EntityFrameworkCore;
using Playground.Data.Contexts;
using Playground.Data.Generics;
using Playground.Domain.Entities;

namespace Playground.Data.Repositories
{
    public class PortfolioAssetRepository : GenericRepository<PortfolioAsset>, IPortfolioAssetRepository
    {
        public PortfolioAssetRepository(DataContext context) : base(context)
        {
        }

        public async Task<List<PortfolioAsset>> GetByPortfolioIdAsync(Guid portfolioId)
        {
            return await _context.PortfolioAssets
                                 .Include(pa => pa.Asset)
                                 .ThenInclude(a => a.AssetType)
                                 .Where(pa => pa.PortfolioId == portfolioId)
                                 .ToListAsync();
        }
    }
}
