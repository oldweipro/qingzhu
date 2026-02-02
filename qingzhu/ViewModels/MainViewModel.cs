using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;

namespace qingzhu.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string title = "工业协议模拟器";

        [ObservableProperty]
        private object? selectedMenuItem;

        public MainViewModel()
        {
        }

        [RelayCommand]
        private void NavigateToPage(string tag)
        {
            // Navigation will be handled by the view
            // This is just for command binding
        }
    }
}
