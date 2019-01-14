namespace AngularBlazor.UI.Server.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using AngularBlazor.UI.Shared;

    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    public class HeroController : Controller
    {
        [HttpGet]
        public async Task<IEnumerable<Hero>> Get()
        {
            // Thread.Sleep(5000);
            return await Task.FromResult(Data.Heroes.ToList());
        }
    }
}