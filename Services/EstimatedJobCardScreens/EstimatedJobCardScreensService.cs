using AutoMapper;
using Newtonsoft.Json;
using AIIcsoftAPI.DAL;
using AIIcsoftAPI.Dto.EstimatedJobCardScreens;
using AIIcsoftAPI.Models.ResponseModels;
using AIIcsoftAPI.Models.SMIcsoftDataModels;

namespace AIIcsoftAPI.Services.EstimatedJobCardScreens
{
    public class EstimatedJobCardScreensService : IEstimatedJobCardScreensService
    {
        private readonly IGenericRepository _genericRepository;

        private readonly IRepository<AccessLevel> _storeIssuesRepository;

        private readonly IMapper _mapper;
        public EstimatedJobCardScreensService(IGenericRepository genericRepository, IRepository<AccessLevel> storeIssuesRepository, IMapper mapper)
        {
            _genericRepository = genericRepository;
            _storeIssuesRepository = storeIssuesRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetStoreIssueResponseModel>> GetEstimatedJobCardScreenAsync()
        {
            var entity = await _storeIssuesRepository.GetAllAsync();
            IEnumerable<GetStoreIssueResponseModel> response = _mapper.Map<IEnumerable<GetStoreIssueResponseModel>>(entity);
            return response;
        }

        public async Task<ServiceResponse<string>> SaveEstimatedJobCardScreenAsync(List<EstimatedJobCardScreensPostModel> estimatedJobCardScreensPostModel, string trMode)
        {
            var inputParams = new Dictionary<string, object>
            {
                                    { "HeaderData", JsonConvert.SerializeObject(estimatedJobCardScreensPostModel) },{ "TrMode", trMode }
                                };

            var result = await _genericRepository.ExecuteStoredProcedureAsync<string>("SP_EstimatedJobCardScreen_Save", inputParams, "perrormsg");
            return await _genericRepository.GetResponseAsync(result.Success, "Saved", "Estimated Job Card Screens", JsonConvert.SerializeObject(estimatedJobCardScreensPostModel));
        }

        public async Task<ServiceResponse<string>> UpdateEstimatedJobCardScreenAsync(List<EstimatedJobCardScreensPutModel> estimatedJobCardScreensPutModel, string trMode)
        {
            var inputParams = new Dictionary<string, object>
                               {
                                    { "HeaderData", JsonConvert.SerializeObject(estimatedJobCardScreensPutModel) },{ "TrMode", trMode }
                                };

            var result = await _genericRepository.ExecuteStoredProcedureAsync<string>("SP_EstimatedJobCardScreen_Save", inputParams, "perrormsg");
            return await _genericRepository.GetResponseAsync(result.Success, "Updated", "Estimated Job Card Screens", JsonConvert.SerializeObject(estimatedJobCardScreensPutModel));
        }
    }
}
