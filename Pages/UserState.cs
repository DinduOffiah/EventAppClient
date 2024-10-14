namespace EventAppClient.Pages
{
    public class UserState
    {
        public event Action OnChange;
        private string _username;
        private string _role;

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                NotifyStateChanged();
            }
        }

        public string Role
        {
            get => _role;
            set
            {
                _role = value;
                NotifyStateChanged();
            }
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
