using Playground.Domain.Entities.Base;

namespace Playground.Domain.Entities
{
    public class PortfolioAsset : JoinEntity
    {
        public Guid PortfolioId { get; set; }
        public Portfolio Portfolio { get; set; }

        public Guid AssetId { get; set; }
        public Asset Asset { get; set; }
    }
}
