using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OcelotAPIGateWay.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IJWTHelpers _jWTHelpers;

        public AuthorizationService(IJWTHelpers jWTHelpers)
        {
            _jWTHelpers = jWTHelpers;
        }

        public bool Validate(HttpContext context)
        {
            try
            {
                var url = context.Request.Path;

                var token = context.Request.Headers["Authorization"].ToString().Split(' ').Last();

                if (string.IsNullOrEmpty(token))
                    return false;

                //  ارسال به سرویس


                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}
