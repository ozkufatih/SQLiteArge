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
    public class AssetTypeService : IAssetTypeService
    {
        private readonly IAssetTypeRepository _assetTypeRepository;
        private readonly IMapper _mapper;

        public AssetTypeService(IAssetTypeRepository assetTypeRepository, IMapper mapper)
        {
            _assetTypeRepository = assetTypeRepository;
            _mapper = mapper;
        }

        public async Task<ResultItem<BaseDto>> CreateAsync(BaseDto dto)
        {
            try
            {
                var createAssetTypeDto = dto as CreateAssetTypeDto;

                if (createAssetTypeDto == null)
                {
                    return ResultItem<BaseDto>.Failure("Invalid data type.");
                }

                var assetType = _mapper.Map<AssetType>(createAssetTypeDto);
                assetType.Initialize();
                var createdAssetType = await _assetTypeRepository.CreateAsync(assetType);

                var createdDto = _mapper.Map<CreateAssetTypeDto>(createdAssetType);

                return ResultItem<BaseDto>.Success(createdDto);
            }
            catch (Exception)
            {
                return ResultItem<BaseDto>.Failure("Failed to create AssetType.");
            }
        }

        public async Task<ResultItem<BaseDto>> DeleteAsync(Guid guid)
        {
            try
            {
                var assetType = await GetAssetTypeByIdAsync(guid);
                await _assetTypeRepository.DeleteAsync(assetType);

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

        public async Task<ResultItem<List<GetAssetTypeDto>>> GetAllAsync()
        {
            try
            {
                var assetTypes = await _assetTypeRepository.GetAllAsync();
                var dtos = _mapper.Map<List<GetAssetTypeDto>>(assetTypes);

                return ResultItem<List<GetAssetTypeDto>>.Success(dtos);
            }
            catch (Exception)
            {
                return ResultItem<List<GetAssetTypeDto>>.Failure("Failed to retrieve AssetTypes.");
            }
        }

        public async Task<ResultItem<BaseDto>> GetByIdAsync(Guid guid)
        {
            try
            {
                var asset = await GetAssetTypeByIdAsync(guid);
                var dto = _mapper.Map<GetAssetTypeDto>(asset);

                return ResultItem<BaseDto>.Success(dto);
            }
            catch (NotFoundException ex)
            {
                return ResultItem<BaseDto>.Failure(ex.Message);
            }
            catch (Exception)
            {
                return ResultItem<BaseDto>.Failure("Failed to retrieve AssetType.");
            }
        }

        public async Task<ResultItem<BaseDto>> UpdateAsync(BaseDto dto)
        {
            try
            {
                var asset = await GetAssetTypeByIdAsync(dto.Guid);

                _mapper.Map(dto, asset);
                asset.RefreshUpdatedAt();

                await _assetTypeRepository.UpdateAsync(asset);

                var updatedDto = _mapper.Map<UpdateAssetTypeDto>(asset);

                return ResultItem<BaseDto>.Success(updatedDto);
            }
            catch (NotFoundException ex)
            {
                return ResultItem<BaseDto>.Failure(ex.Message);
            }
            catch (Exception)
            {
                return ResultItem<BaseDto>.Failure("Failed to update AssetType.");
            }
        }

        private async Task<AssetType> GetAssetTypeByIdAsync(Guid guid)
        {
            var assetType = await _assetTypeRepository.GetByIdAsync(guid);

            if (assetType == null)
            {
                throw new NotFoundException("AssetType not found.");
            }

            return assetType;
        }
    }
}
