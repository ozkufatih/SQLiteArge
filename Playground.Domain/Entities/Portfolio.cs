using Playground.Domain.Entities.Base;

namespace Playground.Domain.Entities
{
    public class Portfolio : BaseEntity
    {
        public string Name { get; set; }

        public ICollection<PortfolioAsset> PortfolioAssets { get; set; }
    }
}
