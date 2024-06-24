using Playground.Domain.Entities.Base;

namespace Playground.Domain.Entities
{
    public class Asset : BaseEntity
    {
        public string Name { get; set; }

        public double Quantity { get; set; }

        public Guid AssetTypeId { get; set; }

        public AssetType AssetType { get; set; }

        public ICollection<PortfolioAsset> PortfolioAssets { get; set; }
    }
}