using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PowerStudio.Interfaces;
using PowerStudio.Models.Azure;
using PowerStudio.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PowerStudio.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        private readonly AzureService _azureService;
        private readonly IAuthenticationService _authenticationService;
        public CustomComboBoxViewModel<Subscription> SubscriptionComboBox { get; } = new CustomComboBoxViewModel<Subscription>();
        public CustomComboBoxViewModel<AppService> AppServiceComboBox { get; } = new CustomComboBoxViewModel<AppService>();

        public MainWindowViewModel(AzureService azureService, IAuthenticationService authenticationService)
        {
            _azureService = azureService;
            _authenticationService = authenticationService;
            AuthButtonClickedCommand = new AsyncRelayCommand(AuthButtonClickedAsync);
            SubscriptionComboBox.ItemSelectedCommand = new AsyncRelayCommand<Subscription>(SelectSubscriptionAsync);
            AppServiceComboBox.ItemSelectedCommand = new AsyncRelayCommand<AppService>(SelectAppServiceAsync);
            SaveCommand = new RelayCommand(Save);
            AppSettings = [];
        }

        private ObservableCollection<AppSettingsModel> _appSettings;
        private string _appSettingsJson;
        public AsyncRelayCommand AuthButtonClickedCommand { get; }
        public bool IsAuthenticated => _authenticationService.IsAuthenticated;
        public string AuthButtonText => IsAuthenticated ? "Sign Out" : "Sign In";

        public ObservableCollection<AppSettingsModel> AppSettings
        {
            get => _appSettings;
            set => SetProperty(ref _appSettings, value);
        }
        public string AppSettingsJson
        {
            get => _appSettingsJson;
            set
            {
                if (SetProperty(ref _appSettingsJson, value))
                {
                    IsWarningVisible = true;
                    WarningText = "Зміни не збережено. Натисніть Ctrl + S для збереження.";
                }
            }
        }

        private async Task AuthButtonClickedAsync()
        {
            if (!_authenticationService.IsAuthenticated)
            {
                var subscriptions = await _azureService.GetSubscriptionsAsync();
                SubscriptionComboBox.AddItems(subscriptions);
            }
            else
            {
                await _authenticationService.LogoutUserAsync();
                SubscriptionComboBox.Clear();
                AppServiceComboBox.Clear();
                AppSettings.Clear();
            }
            OnPropertyChanged(nameof(IsAuthenticated));
            OnPropertyChanged(nameof(AuthButtonText));
        }

        private async Task SelectSubscriptionAsync(Subscription selectedSubscription)
        {
            if (selectedSubscription != null)
            {
                var appServices = await _azureService.GetAppServicesBySubscriptionAsync(selectedSubscription.SubscriptionId);
                AppServiceComboBox.AddItems(appServices);
            }
        }

        private async Task SelectAppServiceAsync(AppService selectedAppService)
        {
            if (selectedAppService != null)
            {
                var properties = await _azureService.GetAppServiceSettingsAsync(selectedAppService);
                AppSettings.Clear();
                foreach (var property in properties)
                {
                    AppSettings.Add(property);
                }
                AppSettingsJson = JsonProcessingService.Serialize(properties);
            }
        }

        private bool isTableViewVisible = true;
        public bool IsTableViewVisible
        {
            get => isTableViewVisible;
            set { isTableViewVisible = value; OnPropertyChanged(); }
        }

        private bool isJsonEditorVisible = false;
        public bool IsJsonEditorVisible
        {
            get => isJsonEditorVisible;
            set { isJsonEditorVisible = value; OnPropertyChanged(); }
        }

        private bool _isWarningVisible = false;
        public bool IsWarningVisible
        {
            get => _isWarningVisible;
            set => SetProperty(ref _isWarningVisible, value);
        }

        private string _warningText = string.Empty;
        public string WarningText
        {
            get => _warningText;
            set => SetProperty(ref _warningText, value);
        }

        public ICommand SaveCommand { get; }
        private void Save()
        {
            var appSettings = JsonProcessingService.Deserialize(AppSettingsJson, AppSettings);
            AppSettings.Clear();
            foreach (var setting in appSettings)
            {
                AppSettings.Add(setting);
            }
            IsWarningVisible = false;
            WarningText = string.Empty;
        }

        public ICommand SwitchModeCommand => new RelayCommand(SwitchMode);

        private void SwitchMode()
        {
            IsTableViewVisible = !IsTableViewVisible;
            IsJsonEditorVisible = !IsJsonEditorVisible;
        }
    }
}
