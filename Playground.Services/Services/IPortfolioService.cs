using Playground.Common.ResultItems;
using Playground.Domain.Dtos;
using Playground.Domain.Dtos.Base;
using Playground.Services.Services.Base;

namespace Playground.Services.Services
{
    public interface IPortfolioService : IBaseService<BaseDto, ResultItem<BaseDto>>, IBatchOperationsService<ResultItem<List<GetPortfolioDto>>>
    {
        Task<ResultItem<List<GetAssetDto>>> GetAssetsByPortfolioIdAsync(Guid portfolioId);
    }
}
