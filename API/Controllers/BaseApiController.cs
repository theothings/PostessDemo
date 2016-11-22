using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace API.Controllers
{
    /// <summary>
    /// Base API controller to get the users account which they control
    /// </summary>
    public class BaseApiController : ApiController
    {
        protected int GetAccountId()
        {
            return Convert.ToInt32(((ClaimsIdentity)User.Identity).Claims.FirstOrDefault(x => x.Type == "AccountId").Value);
        }

        protected string GetUserClaim(string claimType)
        {
            return ((ClaimsIdentity)User.Identity).Claims.FirstOrDefault(x => x.Type == claimType).Value;
        }
    }
}
