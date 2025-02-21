using AIIcsoftAPI.Models.RequestModels;
using AIIcsoftAPI.Services.JsonData;
using Microsoft.AspNetCore.Mvc;

namespace AIIcsoftAPI.Controllers
{

    [Route("api/[controller]/[action]")]
    public class JsonDataController : ControllerBase
    {
        private readonly IJsonDataService _jsonDataService;
        public JsonDataController(IJsonDataService jsonDataService)
        {
            _jsonDataService = jsonDataService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetJsonData([FromQuery] JsonDataGetModel jsonDataGetModel)
        {
            var result = await _jsonDataService.GetJsonDataAsync(jsonDataGetModel.DataType);
            return Ok(result);
        }

    }
}