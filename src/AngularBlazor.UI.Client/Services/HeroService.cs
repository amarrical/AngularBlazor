namespace AngularBlazor.UI.Client.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Runtime.Serialization;
    using System.Threading.Tasks;

    using AngularBlazor.UI.Shared;

    using Microsoft.AspNetCore.Blazor;
    using Microsoft.JSInterop;

    public class HeroService
    {
        #region [ Fields ]

        private string requestUri = "api/Hero";

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
            try
            {
                var updated = await this.client.PutJsonAsync<Hero>(this.requestUri, hero);
                this.messageService.Add($"Hero {hero.Name} saved");
                hero = updated;
            }
            catch (SerializationException)
            {
                this.messageService.Add($"Error Updating hero: {hero.Name}");
                await this.RefreshHeroes();
            }
        }

        public async Task Add(string heroName)
        {
            try
            {
                var id = await this.client.PostJsonAsync<int>(this.requestUri, heroName);
                this.heroes.Add(new Hero { Id = id, Name = heroName });
                this.messageService.Add($"Created hero: {heroName} with Id: {id}");
            }
            catch (FormatException)
            {
                this.messageService.Add($"Error Creating hero: {heroName}");
                await this.RefreshHeroes();
            }
        }

        public async Task Delete(Hero hero)
        {
            var result = await this.client.DeleteAsync($"{this.requestUri}/{hero.Id}");
            if (!result.IsSuccessStatusCode)
            {
                this.messageService.Add($"Error deleting hero.");
                await this.RefreshHeroes();
            }
            else
            {
                this.heroes.Remove(hero);
                this.messageService.Add($"Hero Service: Deleted hero {hero.Name}");
            }
        }

        public void Change()
        {
            this.NotifyStateChanged();
        }

        #endregion

        #region [ Helpers ]

        private void NotifyStateChanged() => this.OnChange?.Invoke();

        private async Task HandleError(string message)
        {
            this.messageService.Add(message);
            await this.RefreshHeroes();
        }

        private async Task RefreshHeroes()
        {
            this.messageService.Add("Hero Service: Fetched Heroes");
            this.heroes = (await this.client.GetJsonAsync<Hero[]>(this.requestUri)).ToList();
            this.NotifyStateChanged();
        }

        #endregion
    }
}