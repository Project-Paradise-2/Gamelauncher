using System.Windows;
using System.Windows.Controls;

namespace ProjectParadise2.Views
{
    /// <summary>
    /// Interaktionslogik für LauncherProfilesView.xaml
    /// </summary>
    public partial class LauncherProfilesView : UserControl
    {
        public LauncherProfilesView()
        {
            InitializeComponent();
            DataContext = new LauncherProfilesViewModel();
            AddProfile.Content = Lang.GetText(18);
            AddProfile.ToolTip = Lang.GetTooltipText(5);
        }

        private void CreateProfile(object sender, RoutedEventArgs e)
        {
            MainViewModel.OpenCreateProfile();
        }
    }
}