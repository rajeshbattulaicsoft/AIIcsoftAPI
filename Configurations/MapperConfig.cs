using AutoMapper;
using AIIcsoftAPI.Models.ResponseModels;
using AIIcsoftAPI.Models.SMIcsoftDataModels;

namespace AIIcsoftAPI.Configurations
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            MapForStoreIssues();
        }

        private void MapForStoreIssues()
        {
            CreateMap<AccessLevel, GetStoreIssueResponseModel>()
                .ForMember(d => d.StoreIssueId, opt => opt.MapFrom(s => s.AccessLevelId));
                //.ForMember(d => d.StoreIssueName, opt => opt.MapFrom(s => s.EntryComputer));
        }
    }
}
