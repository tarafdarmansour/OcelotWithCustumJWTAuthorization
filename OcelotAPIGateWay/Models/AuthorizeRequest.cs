using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OcelotAPIGateWay.Models
{
    public class AuthorizeRequest
    {
        public string UserName { get; set; }
        public string Resource { get; set; }
    }
}
