using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OcelotAPIGateWay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OcelotAPIGateWay.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IJWTHelpers _jWTHelpers;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _client;

        public AuthorizationService(IJWTHelpers jWTHelpers,IConfiguration configuration,HttpClient client)
        {
            _jWTHelpers = jWTHelpers;
            _configuration = configuration;
            _client = client;
        }

        public async Task<bool> IsValid(HttpContext context)
        {
            try
            {
                var urlstoignore = _configuration.GetSection("IgnoreValidateUpstreamURLs").Get<List<string>>();
                var url = context.Request.Path;
                if (urlstoignore.Contains(url))
                    return true;
                var token = context.Request.Headers["Authorization"].ToString().Split(' ').Last();

                if (string.IsNullOrEmpty(token))
                    return false;

                string username = null;

                if(!_jWTHelpers.TokenIsValid(token,out username))
                    return false;

                string AuthorizeServicePath = _configuration.GetValue(typeof(string), "AuthorizationService").ToString();


                var authorizereq = new AuthorizeRequest { Resource = url, UserName = username };
                string strPayload = JsonConvert.SerializeObject(authorizereq);
                HttpContent content = new StringContent(strPayload, Encoding.UTF8, "application/json");

                using (HttpResponseMessage response = await _client.PostAsync(AuthorizeServicePath, content))
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        //List<SampleData> users = await response.Content.ReadAsAsync<List<SampleData>>();
                        var sampleData =
                            JsonConvert.DeserializeObject<bool>(await response.Content.ReadAsStringAsync());
                        return sampleData;
                    }

                    return false;
                }


                return true;
            }
            catch (Exception exp)
            {
                return false;
            }

        }

        private bool IsAuthorized(AuthorizeRequest request)
        {

            return false;
        }
    }
}
