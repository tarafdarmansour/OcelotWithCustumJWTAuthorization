using Microsoft.AspNetCore.Http;

namespace OcelotAPIGateWay.Services
{
    public interface IAuthorizationService
    {
        bool Validate(HttpContext context);
    }
}