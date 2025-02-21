using AIIcsoftAPI.Dto;

namespace AIIcsoftAPI.Services
{
    public interface IAuthenticationService
    {

        Task<ServiceResponse<string>> AuthenticateClient(LoginDto obj);
        
        Task<ServiceResponse<string>> AuthenticateUsingAPItoken(string token);
        Task<ServiceResponse<string>> CreateOurTokenfromZvolvToken(string token);

        Task<ServiceResponse<string>> sAuthenticateClient(string token);
        
        Task<ServiceResponse<string>> ValidateToken(string token);
        bool ValidateTokenClaims(int papicallerempid, string papicalleremailid);

        bool GetEmpIdEmailIdFromToken(string token, ref int empid, ref string emailid);
        Task<ServiceResponse<EmployeeDTO>> GetEmployeeByEmailId(string emailid);
    }
}
