using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using Hjerpbakk.PoorMansServiceDiscovery.Configuration;
using Microsoft.AspNetCore.Http;

namespace Hjerpbakk.PoorMansServiceDiscovery.Authentication
{
    public class CertificateAuthenticationMiddleware
    {
        readonly RequestDelegate _next;
        readonly IClientConfiguration clientConfiguration;

        public CertificateAuthenticationMiddleware(RequestDelegate next, IClientConfiguration clientConfiguration)
        {
            _next = next;
            this.clientConfiguration = clientConfiguration;
        }

        public async Task Invoke(HttpContext context)
        {
            if (IsApiKeyValid(context)) {
                await _next(context);    
            } else {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            }
        }

        bool IsApiKeyValid(HttpContext context)
        {
            string apikey = HttpUtility.ParseQueryString(context.Request.QueryString.Value).Get("apikey");
            return clientConfiguration.ApiKeys.Contains(apikey);
        }
    }
}
