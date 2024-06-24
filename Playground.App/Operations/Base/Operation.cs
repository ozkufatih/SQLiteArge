using Microsoft.Extensions.DependencyInjection;
using Playground.Services.Services;

namespace Playground.App.Operations.Base
{
    public abstract class Operation
    {
        protected readonly IPortfolioService _portfolioService;
        protected readonly IAssetService _assetService;
        protected readonly IAssetTypeService _assetTypeService;

        public Operation()
        {
            _portfolioService = Program._serviceProvider.GetService<IPortfolioService>();
            _assetService = Program._serviceProvider.GetService<IAssetService>();
            _assetTypeService = Program._serviceProvider.GetService<IAssetTypeService>();
        }
    }
}
