namespace AngularBlazor.UI.Server.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AngularBlazor.UI.Shared;

    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    public class HeroController : Controller
    {
        #region [ Fields ]

        private Random rand = new Random((int)(DateTime.Now.Ticks % int.MaxValue));

        #endregion

        #region [ Endpoints ]

        [HttpGet]
        public async Task<IEnumerable<Hero>> Get()
        {
            // Thread.Sleep(5000);
            return await Task.FromResult(Data.Heroes.ToList());
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Hero hero)
        {
            var toUpdate = Data.Heroes.SingleOrDefault(h => h.Id == hero.Id);
            if (toUpdate == null || this.ShouldFail())
                return new BadRequestResult();

            toUpdate.Name = hero.Name;
            return new OkObjectResult(toUpdate);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] string name)
        {
            var id = Data.Heroes.Select(h => h.Id).Max() + 1;
            if (this.ShouldFail())
                return new BadRequestResult();

            Data.Heroes.Add(new Hero { Id = id, Name = name });
            return new OkObjectResult(id);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = Data.Heroes.SingleOrDefault(h => h.Id == id);
            if (deleted == null)
                return new NotFoundResult();

            if (this.ShouldFail())
                return new BadRequestResult();

            Data.Heroes.Remove(deleted);
            return new OkResult();
        }

        #endregion

        #region [ Private Helpers ]

        public bool ShouldFail()
        {
            return this.rand.Next(0, 10) >= 9;
        }

        #endregion
    }
} 