using Microsoft.UI.Xaml.Controls;
using qingzhu.ViewModels;

namespace qingzhu.Views
{
    public sealed partial class HttpPage : Page
    {
        public HttpViewModel ViewModel { get; }

        public HttpPage()
        {
            InitializeComponent();
            ViewModel = (HttpViewModel)DataContext;
        }
    }
}
