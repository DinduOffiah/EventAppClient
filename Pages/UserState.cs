namespace EventAppClient.Pages
{
    public class UserState
    {
        public event Action OnChange;
        private string _username;

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                NotifyStateChanged();
            }
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
