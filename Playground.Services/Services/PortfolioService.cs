using AutoMapper;
using Playground.Common.Exceptions;
using Playground.Common.ResultItems;
using Playground.Data.Repositories;
using Playground.Domain.Dtos;
using Playground.Domain.Dtos.Base;
using Playground.Domain.Entities;
using Playground.Domain.Extensions;

namespace Playground.Services.Services
{
    public class PortfolioService : IPortfolioService
    {
        private readonly IPortfolioRepository _portfolioRepository;
        private readonly IPortfolioAssetRepository _portfolioAssetRepository;
        private readonly IMapper _mapper;

        public PortfolioService(IPortfolioRepository portfolioRepository, IMapper mapper, IPortfolioAssetRepository portfolioAssetRepository)
        {
            _portfolioRepository = portfolioRepository;
            _portfolioAssetRepository = portfolioAssetRepository;
            _mapper = mapper;
        }

        public async Task<ResultItem<BaseDto>> CreateAsync(BaseDto dto)
        {
            try
            {
                var createPortfolioDto = dto as CreatePortfolioDto;

                if (createPortfolioDto == null)
                {
                    return ResultItem<BaseDto>.Failure("Invalid data type.");
                }

                var portfolio = _mapper.Map<Portfolio>(createPortfolioDto);
                portfolio.Initialize();
                var createPortfolio = await _portfolioRepository.CreateAsync(portfolio);

                var createdPortfolioDto = _mapper.Map<CreatePortfolioDto>(createPortfolio);

                return ResultItem<BaseDto>.Success(createdPortfolioDto);
            }
            catch (Exception)
            {
                return ResultItem<BaseDto>.Failure("Failed to create Portfolio.");
            }
        }

        public async Task<ResultItem<BaseDto>> DeleteAsync(Guid guid)
        {
            try
            {
                var portfolio = await GetPortfolioByIdAsync(guid);
                await _portfolioRepository.DeleteAsync(portfolio);

                return ResultItem<BaseDto>.Success();
            }
            catch (NotFoundException ex)
            {
                return ResultItem<BaseDto>.Failure(ex.Message);
            }
            catch (Exception)
            {
                return ResultItem<BaseDto>.Failure("Failed to delete AssetType.");
            }

        }

        public async Task<ResultItem<List<GetPortfolioDto>>> GetAllAsync()
        {
            try
            {
                var portfolios = await _portfolioRepository.GetAllAsync();
                var portfolioDtos = _mapper.Map<List<GetPortfolioDto>>(portfolios);

                return ResultItem<List<GetPortfolioDto>>.Success(portfolioDtos);
            }
            catch (Exception)
            {
                return ResultItem<List<GetPortfolioDto>>.Failure("Failed to get Portfolios.");
            }
        }

        public async Task<ResultItem<List<GetAssetDto>>> GetAssetsByPortfolioIdAsync(Guid portfolioId)
        {
            try
            {
                var portfolioAssets = await _portfolioAssetRepository.GetByPortfolioIdAsync(portfolioId);
                var assets = portfolioAssets.Select(pa => pa.Asset).ToList();
                var assetDtos = assets.Select(asset => new GetAssetDto
                {
                    Guid = asset.Guid,
                    CreatedAt = asset.CreatedAt,
                    UpdatedAt = asset.UpdatedAt,
                    Name = asset.Name,
                    Quantity = asset.Quantity,
                    AssetTypeName = asset.AssetType?.Name // Map AssetTypeName
                }).ToList();

                return ResultItem<List<GetAssetDto>>.Success(assetDtos);
            }
            catch (Exception)
            {
                return ResultItem<List<GetAssetDto>>.Failure("Failed to retrieve assets for the portfolio.");
            }
        }

        public async Task<ResultItem<BaseDto>> GetByIdAsync(Guid guid)
        {
            try
            {
                var portfolio = await GetPortfolioByIdAsync(guid);
                var portfolioDto = _mapper.Map<GetPortfolioDto>(portfolio);

                return ResultItem<BaseDto>.Success(portfolioDto);
            }
            catch (NotFoundException ex)
            {
                return ResultItem<BaseDto>.Failure(ex.Message);
            }
        }

        public async Task<ResultItem<BaseDto>> UpdateAsync(BaseDto dto)
        {
            try
            {
                var portfolio = await GetPortfolioByIdAsync(dto.Guid);

                _mapper.Map(dto, portfolio);
                portfolio.RefreshUpdatedAt();

                var updatedPortfolio = await _portfolioRepository.UpdateAsync(portfolio);

                var updatedPortfolioDto = _mapper.Map<UpdatePortfolioDto>(updatedPortfolio);

                return ResultItem<BaseDto>.Success(updatedPortfolioDto);
            }
            catch (NotFoundException ex)
            {
                return ResultItem<BaseDto>.Failure(ex.Message);
            }
            catch (Exception)
            {
                return ResultItem<BaseDto>.Failure("Failed to update Portfolio.");
            }
        }

        private async Task<Portfolio> GetPortfolioByIdAsync(Guid guid)
        {
            var portfolio = await _portfolioRepository.GetByIdAsync(guid);

            if (portfolio == null)
            {
                throw new NotFoundException("Portfolio not found.");
            }

            return portfolio;
        }
    }
}
