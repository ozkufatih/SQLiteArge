using Playground.Domain.Dtos.Base;

namespace Playground.Domain.Dtos
{
    public class CreateAssetDto : BaseDto
    {
        public string Name { get; set; }

        public double Quantity { get; set; }

        public Guid AssetTypeId { get; set; }

        #region AutoMapper NotMapped
        public Guid PortfolioId { get; set; }
        #endregion
    }
}
