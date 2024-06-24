using Playground.Domain.Dtos.Base;

namespace Playground.Domain.Dtos
{
    public class GetAssetDto : BaseDto
    {
        public string AssetTypeName { get; set; }

        public string Name { get; set; }

        public double Quantity { get; set; }
    }
}
