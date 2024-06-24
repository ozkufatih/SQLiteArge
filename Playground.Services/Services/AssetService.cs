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
    public class AssetService : IAssetService
    {
        private readonly IAssetRepository _assetRepository;
        private readonly IPortfolioAssetRepository _portfolioAssetRepository;
        private readonly IMapper _mapper;

        public AssetService(IAssetRepository assetRepository, IMapper mapper, IPortfolioAssetRepository portfolioAssetRepository)
        {
            _assetRepository = assetRepository;
            _portfolioAssetRepository = portfolioAssetRepository;
            _mapper = mapper;
        }

        public async Task<ResultItem<BaseDto>> CreateAsync(BaseDto dto)
        {
            try
            {
                var createAssetDto = dto as CreateAssetDto;

                if (createAssetDto == null)
                {
                    return ResultItem<BaseDto>.Failure("Invalid data type");
                }

                var asset = _mapper.Map<Asset>(createAssetDto);
                asset.Initialize();
                var createdAsset = await _assetRepository.CreateAsync(asset);

                var portfolioAsset = CreatePortfolioAsset(createAssetDto.PortfolioId, createdAsset.Guid);
                await _portfolioAssetRepository.CreateAsync(portfolioAsset);

                var createdDto = _mapper.Map<CreateAssetDto>(createdAsset);

                return ResultItem<BaseDto>.Success(createdDto);
            }
            catch (Exception)
            {
                return ResultItem<BaseDto>.Failure("Failed to create asset.");
            }
        }

        public async Task<ResultItem<BaseDto>> DeleteAsync(Guid guid)
        {
            try
            {
                var asset = await GetAssetByIdAsync(guid);
                await _assetRepository.DeleteAsync(asset);

                return ResultItem<BaseDto>.Success();
            }
            catch (NotFoundException ex)
            {
                return ResultItem<BaseDto>.Failure(ex.Message);
            }
            catch (Exception)
            {
                return ResultItem<BaseDto>.Failure("Failed to delete asset.");
            }
        }

        public async Task<ResultItem<List<GetAssetDto>>> GetAllAsync()
        {
            try
            {
                var assets = await _assetRepository.GetAllAsync();
                var dtos = _mapper.Map<List<GetAssetDto>>(assets);

                return ResultItem<List<GetAssetDto>>.Success(dtos);
            }
            catch (Exception)
            {
                return ResultItem<List<GetAssetDto>>.Failure("Failed to retrieve assets.");
            }
        }

        public async Task<ResultItem<BaseDto>> GetByIdAsync(Guid guid)
        {
            try
            {
                var asset = await GetAssetByIdAsync(guid);
                var dto = _mapper.Map<GetAssetDto>(asset);

                return ResultItem<BaseDto>.Success(dto);
            }
            catch (NotFoundException ex)
            {
                return ResultItem<BaseDto>.Failure(ex.Message);
            }
            catch (Exception)
            {
                return ResultItem<BaseDto>.Failure("Failed to retrieve asset.");
            }
        }

        public async Task<ResultItem<BaseDto>> UpdateAsync(BaseDto dto)
        {
            try
            {
                var asset = await GetAssetByIdAsync(dto.Guid);

                _mapper.Map(dto, asset);
                asset.RefreshUpdatedAt();

                await _assetRepository.UpdateAsync(asset);

                var updatedDto = _mapper.Map<UpdateAssetDto>(asset);

                return ResultItem<BaseDto>.Success(updatedDto);
            }
            catch (NotFoundException ex)
            {
                return ResultItem<BaseDto>.Failure(ex.Message);
            }
            catch(Exception)
            {
                return ResultItem<BaseDto>.Failure("Failed to update asset.");
            }
        }

        private async Task<Asset> GetAssetByIdAsync(Guid guid)
        {
            var asset = await _assetRepository.GetByIdAsync(guid);

            if (asset == null)
            {
                throw new NotFoundException("Asset not found.");
            }

            return asset;
        }

        private PortfolioAsset CreatePortfolioAsset(Guid portfolioId, Guid assetId)
        {
            return new PortfolioAsset
            {
                PortfolioId = portfolioId,
                AssetId = assetId
            };
        }
    }
}
