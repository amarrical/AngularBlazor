namespace AngularBlazor.UI.Client.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;

    using AngularBlazor.UI.Shared;

    using Microsoft.AspNetCore.Blazor;

    public class HeroService
    {
        #region [ Fields ]

        private readonly HttpClient client;

        private readonly MessagesService messageService;

        private List<Hero> heroes;

        #endregion

        public HeroService(HttpClient client, MessagesService messageService)
        {
            this.client = client;
            this.messageService = messageService;
        }

        #region [ Events ]

        public event Action OnChange;

        #endregion

        #region [ Methods ]

        public async Task<IReadOnlyList<Hero>> Get()
        {
            if (this.heroes == null)
                await this.RefreshHeroes();

            return this.heroes;
        }

        public async Task<Hero> Get(int id)
        {
            return (await this.Get()).SingleOrDefault(h => h.Id == id);
        }

        public async Task Save(Hero hero)
        {
            await this.client.PutJsonAsync("api/Hero", hero);
            this.messageService.Add($"Hero {hero.Name} saved");
            await this.RefreshHeroes();
        }

        public async Task Add(string heroName)
        {
            var id = await this.client.PostJsonAsync<int>("api/Hero", heroName);
            this.messageService.Add($"Created hero: {heroName} with Id: {id}");
            await this.RefreshHeroes();
        }

        public async Task Delete(Hero hero)
        {
            await this.client.DeleteAsync($"api/hero/{hero.Id}");
            this.messageService.Add($"Hero Service: Deleted hero {hero.Name}");
            await this.RefreshHeroes();
        }

        public void Change()
        {
            this.NotifyStateChanged();
        }

        #endregion

        #region [ Helpers ]

        private void NotifyStateChanged() => this.OnChange?.Invoke();

        private async Task RefreshHeroes()
        {
            this.messageService.Add("Hero Service: Fetched Heroes");
            this.heroes = (await this.client.GetJsonAsync<Hero[]>("api/Hero")).ToList();
            this.NotifyStateChanged();
        }

        #endregion
    }
}