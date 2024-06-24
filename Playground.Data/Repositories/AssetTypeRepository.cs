using Playground.Data.Contexts;
using Playground.Data.Generics;
using Playground.Domain.Entities;

namespace Playground.Data.Repositories
{
    public class AssetTypeRepository : GenericRepository<AssetType>, IAssetTypeRepository
    {
        public AssetTypeRepository(DataContext context) : base(context)
        {
        }
    }
}
