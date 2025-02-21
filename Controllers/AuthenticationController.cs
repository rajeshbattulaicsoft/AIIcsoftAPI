using AIIcsoftAPI.Dto;
using AIIcsoftAPI.Services;
using Microsoft.AspNetCore.Mvc;
using AIIcsoftAPI.Models.SMIcsoftDataModels;
using AIIcsoftAPI.HelperClasses;

namespace AIIcsoftAPI.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly SMDBIcsoftMainContext _icsoftmainContext;
        public AuthenticationController(IAuthenticationService authencationService, SMDBIcsoftMainContext SMDBContext)
        {
            _authenticationService = authencationService;
            _icsoftmainContext = SMDBContext;
            //Secret#2023!$
            //Secret#2023!$ = encrypted sha512 = e22819c3c11a91ce87c9e94ef9f81a5d60df86b851a0646dfd87b7c5f458c3069e0312d58fcb04a2d96e6a2d3678c3eec09893de237009907e858b0c9b32ec2c
        }



        [HttpPost]
        [AllowAnonymous]
        [Route("zlogin")]
        public async Task<ActionResult<ServiceResponse<string>>> AuthenticateClient([FromBody] LoginDto obj)
        {
            var response = await _authenticationService.AuthenticateClient(obj);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            var empid = _icsoftmainContext.Employees.Where(c=>c.EmailId.ToLower() == obj.Email.ToLower()).Select(c=>c.EmpId).FirstOrDefault();
            HttpContext.Session.SetInt32("empid", Convert.ToInt32(empid));
            HttpContext.Session.SetString("empemail", obj.Email);
            HttpContext.Session.SetString("empusername", obj.Username);
            return Ok(response);
        }



        [HttpGet]
        [AllowAnonymous]
        [Route("auth")]
        private async Task<ActionResult<ServiceResponse<string>>> AuthenticateClientToken(string token)
        {
            return Ok(_authenticationService.sAuthenticateClient(token));
        }



        [HttpGet]
        [AllowAnonymous]
        [Route("encryptpassword")]
        private async Task<ActionResult<ServiceResponse<string>>> GetEncryptedPassword(string p)
        {
            return Ok(SHA.GenerateSHA512String(p));
        }

    }

}
