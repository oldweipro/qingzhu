using Microsoft.UI.Xaml.Controls;
using qingzhu.ViewModels;

namespace qingzhu.Views
{
    public sealed partial class SettingsPage : Page
    {
        public SettingsViewModel ViewModel { get; }

        public SettingsPage()
        {
            InitializeComponent();
            ViewModel = (SettingsViewModel)DataContext;
        }
    }
}
