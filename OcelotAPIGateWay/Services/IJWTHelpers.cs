using System.Security.Claims;

namespace OcelotAPIGateWay.Services
{
    public interface IJWTHelpers
    {
        ClaimsPrincipal GetPrincipal(string token);
        bool TokenIsValid(string token, out string username);
    }
}