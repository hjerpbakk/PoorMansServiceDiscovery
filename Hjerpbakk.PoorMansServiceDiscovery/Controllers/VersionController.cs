using System.Diagnostics;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;

namespace Hjerpbakk.PoorMansServiceDiscovery.Controllers
{
    [Route("/")]
    public class VersionController : Controller
    {
        bool debugging;

        [HttpGet]
        public string Get() => $"{Assembly.GetExecutingAssembly().GetName().Version} {(debugging ? "DEBUG" : "RELEASE")}";

        [Conditional("DEBUG")]
        void CheckIfDEBUG() => debugging = true;
    }
}
