using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Azure.Mobile.Server.Config;

namespace airmily.AppService.Controllers
{
	[MobileAppController]
    public class AuthController : ApiController
    {
	    public HttpResponseMessage Post(string username, string password)
	    {
		    if (!IsPasswordValid(username, password))
			    return Request.CreateUnauthorizedResponse();

		    //JwtSecurityToken token = this.GetAuthenticationTokenForUser(username);

		    return Request.CreateBadRequestResponse();

		    return Request.CreateResponse(HttpStatusCode.OK, new
		    {
				//Token = token.RawData,
				Username = username
		    });
	    }

	    private bool IsPasswordValid(string username, string password)
	    {
		    if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
			    return false;

		    return true;
	    }
    }
}
