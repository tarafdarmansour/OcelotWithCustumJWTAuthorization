using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace OcelotAPIGateWay.Services
{
    public interface IAuthorizationService
    {
        Task<bool> IsValid(HttpContext context);
    }
}