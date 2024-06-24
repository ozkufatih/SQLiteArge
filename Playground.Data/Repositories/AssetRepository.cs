using Playground.Data.Contexts;
using Playground.Data.Generics;
using Playground.Domain.Entities;

namespace Playground.Data.Repositories
{
    public class AssetRepository : GenericRepository<Asset>, IAssetRepository
    {
        public AssetRepository(DataContext context) : base(context)
        {
        }
    }
}
