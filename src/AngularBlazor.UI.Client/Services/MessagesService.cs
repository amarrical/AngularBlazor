namespace AngularBlazor.UI.Client.Services
{
    using System;
    using System.Collections.Generic;

    public class MessagesService
    {
        #region [ Fields ]

        private List<string> messages = new List<string>();

        #endregion

        #region [ Events ]

        public event Action OnChange;

        #endregion

        #region [ Properties ]

        public IReadOnlyList<string> Messages => messages;

        #endregion

        #region [ Methods ]

        public void Add(string message)
        {
            messages.Add(message);
            this.NotifyStateChanged();
        }

        public void Clear()
        {
            messages.Clear();
            this.NotifyStateChanged();
        }

        #endregion

        #region [ Helpers ]

        private void NotifyStateChanged() => this.OnChange?.Invoke();

        #endregion
    }
}
