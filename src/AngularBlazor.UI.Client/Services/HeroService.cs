namespace AngularBlazor.UI.Client.Services
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;

    using AngularBlazor.UI.Shared;

    using Microsoft.AspNetCore.Blazor;

    public class HeroService
    {
        #region [ Fields ]

        private readonly HttpClient client;

        private readonly MessagesService messageService;

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
            this.messageService.Add("Hero Service: Fetched Heroes");
            return await this.client.GetJsonAsync<Hero[]>("api/Hero");
        }

        public void Add(Hero hero)
        {
            // heroes.Add(hero);
            this.NotifyStateChanged();
        }

        public void Change()
        {
            Console.WriteLine("Heroservice Change");
            this.OnChange?.Invoke();
        }

        #endregion

        #region [ Helpers ]

        private void NotifyStateChanged() => this.OnChange?.Invoke();

        #endregion
    }
}