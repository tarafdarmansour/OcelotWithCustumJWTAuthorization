using System.Security.Claims;

namespace OcelotAPIGateWay.Services
{
    public interface IJWTHelpers
    {
        ClaimsPrincipal GetPrincipal(string token);
        string GetUserNameFromToken(string token);
    }
}