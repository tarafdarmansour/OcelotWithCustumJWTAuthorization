using Microsoft.AspNetCore.Http;
using OcelotAPIGateWay.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace OcelotAPIGateWay.DelegateHandler
{
    /// <summary>
    /// THis handler remove Authorization from header and add username to header for methods
    /// </summary>
    public class HeaderDelegatingHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJWTHelpers _jWTHelpers;

        public HeaderDelegatingHandler(IHttpContextAccessor httpContextAccessor, IJWTHelpers jWTHelpers)
        {
            _httpContextAccessor = httpContextAccessor;
            _jWTHelpers = jWTHelpers;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {

            var _authorizationHeader = request.Headers.Authorization;
            if (_authorizationHeader != null && !string.IsNullOrEmpty(_authorizationHeader.Parameter))
            {
                string username;
                if (_jWTHelpers.TokenIsValid(_authorizationHeader.Parameter, out username))
                {
                    request.Headers.Add("username", username);
                    request.Headers.Authorization = null;
                }
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
