using System.Diagnostics;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;

namespace Hjerpbakk.PoorMansServiceDiscovery.Controllers
{
    [Route("/")]
    public class VersionController : Controller
    {
        static bool debugging;

        static VersionController() => CheckIfDEBUG();

        [HttpGet]
        public string Get() => $"{Assembly.GetExecutingAssembly().GetName().Version} {(debugging ? "DEBUG" : "RELEASE")}";

        [Conditional("DEBUG")]
        static void CheckIfDEBUG() => debugging = true;
    }
}
