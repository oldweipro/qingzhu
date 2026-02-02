using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;

namespace qingzhu.ViewModels
{
    public partial class SettingsViewModel : ViewModelBase
    {
        [ObservableProperty]
        private ElementTheme selectedTheme = ElementTheme.Default;

        [ObservableProperty]
        private string backdrop = "Mica";

        [ObservableProperty]
        private bool autoStart = false;

        [ObservableProperty]
        private bool minimizeToTray = false;

        [ObservableProperty]
        private string language = "简体中文";

        [ObservableProperty]
        private string logLevel = "调试 (Debug)";

        [ObservableProperty]
        private bool autoSave = true;

        [ObservableProperty]
        private string dataDirectory = @"C:\Users\...\AppData\Local\qingzhu";

        [ObservableProperty]
        private string version = "1.0.0";

        public SettingsViewModel()
        {
        }

        [RelayCommand]
        private void OpenDataFolder()
        {
            // Open data folder in explorer
        }

        [RelayCommand]
        private void CheckForUpdates()
        {
            // Check for updates
        }

        [RelayCommand]
        private void OpenLicense()
        {
            // Open license info
        }

        partial void OnSelectedThemeChanged(ElementTheme value)
        {
            // Apply theme change
        }

        partial void OnBackdropChanged(string value)
        {
            // Apply backdrop change
        }
    }
}
