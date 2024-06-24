using AutoMapper;
using Playground.Domain.Dtos;
using Playground.Domain.Dtos.Base;
using Playground.Domain.Entities;

namespace Playground.Services.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CreatePortfolioDto, Portfolio>().ReverseMap();
            CreateMap<BaseDto, Portfolio>();
            CreateMap<Portfolio, UpdatePortfolioDto>().ReverseMap().ForMember(upd => upd.CreatedAt, opt => opt.Ignore());
            CreateMap<Portfolio, GetPortfolioDto>();

            CreateMap<CreateAssetDto, Asset>().ReverseMap().ForMember(cad => cad.PortfolioId, opt=>opt.Ignore());
            CreateMap<BaseDto, Asset>().ReverseMap().ForMember(uad => uad.CreatedAt, opt => opt.Ignore());
            CreateMap<Asset, UpdateAssetDto>();
            CreateMap<Asset, GetAssetDto>();
            CreateMap<Asset, GetAssetDto>()
                .ForMember(dest => dest.AssetTypeName, opt => opt.MapFrom(src => src.AssetType.Name))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity));

            CreateMap<CreateAssetTypeDto, AssetType>().ReverseMap();
            CreateMap<BaseDto, AssetType>().ReverseMap().ForMember(uatd => uatd.CreatedAt, opt => opt.Ignore());
            CreateMap<AssetType, UpdateAssetTypeDto>();
            CreateMap<AssetType, GetAssetTypeDto>();
        }
    }
}
