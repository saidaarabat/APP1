using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Web.Mvc;
using WebAPI_Sondage.Models;
using System.IO;
using Newtonsoft.Json;

namespace WebAPI_Sondage.Controllers
{
    class RESTAuthorizeAttribute : AuthorizeAttribute
	{
		private const string _securityToken = "token"; // Name of the url parameter.
		public override void OnAuthorization(AuthorizationContext filterContext)
		{
			if (Authorize(filterContext))
			{
				return;
			}
			HandleUnauthorizedRequest(filterContext);
		}
		protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
		{
			base.HandleUnauthorizedRequest(filterContext);
		}


		private bool Authorize(AuthorizationContext actionContext)
		{
			string token = "test";


			Boolean validationToken = false;
			List<User> users = LoadJson();
			foreach (User user in users)
			{
				if (user.token.Equals(token))
				{
					validationToken = true;
				}
			}

			//HttpRequestBase request = actionContext.RequestContext.HttpContext.Request;
			//string token = request.Params[_securityToken];
			//return SecurityManager.IsTokenValid(token, CommonManager.GetIP(request), request.UserAgent);

			return validationToken;

		}

		public List<User> LoadJson()
		{
			using (StreamReader r = new StreamReader("UserToken.json"))
			{
				string json = r.ReadToEnd();
				List<User> items = JsonConvert.DeserializeObject<List<User>>(json);
				return items;
			}

		}
	}
}