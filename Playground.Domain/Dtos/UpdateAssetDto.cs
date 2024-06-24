using Playground.Domain.Dtos.Base;

namespace Playground.Domain.Dtos
{
    public class UpdateAssetDto : BaseDto
    {
        public string Name { get; set; }

        public double Quantity { get; set; }
    }
}
