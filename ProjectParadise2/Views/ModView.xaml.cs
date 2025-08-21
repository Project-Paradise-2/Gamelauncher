using ProjectParadise2.Core;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ProjectParadise2.Views
{
    /// <summary>
    /// Interaktionslogik für ModView.xaml
    /// </summary>
    public partial class ModView : UserControl
    {
        /// <summary>
        /// Singleton instance of the ModView class.
        /// </summary>
        public static ModView Instance;

        /// <summary>
        /// Initializes a new instance of the ModView class.
        /// Sets up the UI, initializes the instance, and loads the mod list.
        /// </summary>
        public ModView()
        {
            InitializeComponent();
            BackgroundWorker.OnLangset += SetLang;
            SetLang();
            Instance = this;
            this.Modpagelist.Text = "";
            this.Status.Text = "";
            HideAllUI();
            ModViewModel.GetList();
        }

        /// <summary>
        /// Hides all UI elements by disabling and making them invisible.
        /// </summary>
        private void HideAllUI()
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new Action(HideAllUI));
                return;
            }
            Element1.IsEnabled = false;
            Element1.Visibility = Utils.GetVisibility(Element1.IsEnabled);
            Element2.IsEnabled = false;
            Element2.Visibility = Utils.GetVisibility(Element2.IsEnabled);
            Element3.IsEnabled = false;
            Element3.Visibility = Utils.GetVisibility(Element3.IsEnabled);
            Element4.IsEnabled = false;
            Element4.Visibility = Utils.GetVisibility(Element4.IsEnabled);
            Element5.IsEnabled = false;
            Element5.Visibility = Utils.GetVisibility(Element5.IsEnabled);
            Element6.IsEnabled = false;
            Element6.Visibility = Utils.GetVisibility(Element6.IsEnabled);
            Element7.IsEnabled = false;
            Element7.Visibility = Utils.GetVisibility(Element7.IsEnabled);
            Element8.IsEnabled = false;
            Element8.Visibility = Utils.GetVisibility(Element8.IsEnabled);
            Element9.IsEnabled = false;
            Element9.Visibility = Utils.GetVisibility(Element9.IsEnabled);
            Element10.IsEnabled = false;
            Element10.Visibility = Utils.GetVisibility(Element10.IsEnabled);
            LastPageBTN.IsEnabled = false;
            LastPageBTN.Visibility = Utils.GetVisibility(LastPageBTN.IsEnabled);
            NextPageBTN.IsEnabled = false;
            NextPageBTN.Visibility = Utils.GetVisibility(NextPageBTN.IsEnabled);
            Modpagelist.Text = "";
            GC.Collect();
        }

        /// <summary>
        /// Opens the Installed Mods Manager when the corresponding button is clicked.
        /// </summary>
        private void InstalledMods(object sender, RoutedEventArgs e)
        {
            MainViewModel.OpenModManager();
        }

        /// <summary>
        /// Navigates to the next page of mods if available.
        /// </summary>
        private void NextPage(object sender, RoutedEventArgs e)
        {
            HideAllUI();
            if (ModViewModel.CurrentPage < (ModViewModel.Totalmods / 10))
            {
                ModViewModel.CurrentPage++;
            }
            ModViewModel.GetList();
        }

        /// <summary>
        /// Navigates to the previous page of mods if available.
        /// </summary>
        private void LastPage(object sender, RoutedEventArgs e)
        {
            HideAllUI();
            if (ModViewModel.CurrentPage >= 1)
            {
                ModViewModel.CurrentPage--;
            }
            else
            {
                ModViewModel.CurrentPage = 0;
            }
            ModViewModel.GetList();
        }

        /// <summary>
        /// Displays detailed information about the mod when clicked.
        /// </summary>
        private void ModInfo1(object sender, RoutedEventArgs e)
        {
            ModViewModel.SelectedMod = 0;
            MainViewModel.OpenModInfo();
        }

        /// <summary>
        /// Displays detailed information about the mod when clicked.
        /// </summary>
        private void ModInfo2(object sender, RoutedEventArgs e)
        {
            ModViewModel.SelectedMod = 1;
            MainViewModel.OpenModInfo();
        }

        /// <summary>
        /// Displays detailed information about the mod when clicked.
        /// </summary>
        private void ModInfo3(object sender, RoutedEventArgs e)
        {
            ModViewModel.SelectedMod = 2;
            MainViewModel.OpenModInfo();
        }

        /// <summary>
        /// Displays detailed information about the mod when clicked.
        /// </summary>
        private void ModInfo4(object sender, RoutedEventArgs e)
        {
            ModViewModel.SelectedMod = 3;
            MainViewModel.OpenModInfo();
        }

        /// <summary>
        /// Displays detailed information about the mod when clicked.
        /// </summary>
        private void ModInfo5(object sender, RoutedEventArgs e)
        {
            ModViewModel.SelectedMod = 4;
            MainViewModel.OpenModInfo();
        }

        /// <summary>
        /// Displays detailed information about the mod when clicked.
        /// </summary>
        private void ModInfo6(object sender, RoutedEventArgs e)
        {
            ModViewModel.SelectedMod = 5;
            MainViewModel.OpenModInfo();
        }

        /// <summary>
        /// Displays detailed information about the mod when clicked.
        /// </summary>
        private void ModInfo7(object sender, RoutedEventArgs e)
        {
            ModViewModel.SelectedMod = 6;
            MainViewModel.OpenModInfo();
        }

        /// <summary>
        /// Displays detailed information about the mod when clicked.
        /// </summary>
        private void ModInfo8(object sender, RoutedEventArgs e)
        {
            ModViewModel.SelectedMod = 7;
            MainViewModel.OpenModInfo();
        }

        /// <summary>
        /// Displays detailed information about the mod when clicked.
        /// </summary>
        private void ModInfo9(object sender, RoutedEventArgs e)
        {
            ModViewModel.SelectedMod = 8;
            MainViewModel.OpenModInfo();
        }

        /// <summary>
        /// Displays detailed information about the mod when clicked.
        /// </summary>
        private void ModInfo10(object sender, RoutedEventArgs e)
        {
            ModViewModel.SelectedMod = 9;
            MainViewModel.OpenModInfo();
        }

        /// <summary>
        /// Updates the notification message and refreshes the UI based on the event code.
        /// </summary>
        /// <param name="sender">The sender of the notification message.</param>
        /// <param name="e">The event code associated with the notification.</param>
        public void OnNotifymessage(string sender, int e)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new Action<string, int>(OnNotifymessage), sender, e);
                return;
            }

            if (e != 2 || e != 3)
            {
                Status.Text = sender;
            }

            if (e == 3)
            {
                int slot = int.Parse(sender);
                PrintModPreview(slot + 1, ModViewModel.Mods[slot].Modname, ModViewModel.Mods[slot].Modversion);
                CreateModlist();
            }

            if (e != 3)
            {
                Modpagelist.Text = (ModViewModel.CurrentPage + 1) + " | " + ((ModViewModel.Totalmods / 10) + 1);
            }
        }

        /// <summary>
        /// Generates the mod list for the current page and updates the UI.
        /// </summary>
        private void CreateModlist()
        {
            try
            {
                if (!Dispatcher.CheckAccess())
                {
                    Dispatcher.Invoke(new Action(CreateModlist));
                    return;
                }
                if (ModViewModel.CurrentPage < (ModViewModel.Totalmods / 10))
                {
                    NextPageBTN.IsEnabled = true;
                    NextPageBTN.Visibility = Utils.GetVisibility(NextPageBTN.IsEnabled);
                }
                if (ModViewModel.CurrentPage >= 1)
                {
                    LastPageBTN.IsEnabled = true;
                    LastPageBTN.Visibility = Utils.GetVisibility(LastPageBTN.IsEnabled);
                }
            }
            catch (Exception) { }
            GC.Collect();
        }

        /// <summary>
        /// Updates the UI for a specific mod preview based on its number, name, and version.
        /// </summary>
        /// <param name="ModNumber">The position of the mod on the list (1 to 10).</param>
        /// <param name="Modname">The name of the mod.</param>
        /// <param name="Version">The version of the mod.</param>
        private void PrintModPreview(int ModNumber, string Modname, string Version)
        {
            switch (ModNumber)
            {
                case 1:
                    Element1.IsEnabled = true;
                    Element1.Visibility = Utils.GetVisibility(Element1.IsEnabled);
                    Element1Text.Text = Modname;
                    if (Database.Database.isModInstalled(Modname))
                    {
                        if (!Database.Database.GetInstalledversion(Modname, Version))
                        {
                            Element1Text.Foreground = Brushes.Orange;
                        }
                        else
                        {
                            Element1Text.Foreground = Brushes.DarkOliveGreen;
                        }
                    }
                    else
                    {
                        Element1Text.Foreground = Brushes.Wheat;
                    }
                    break;
                case 2:
                    Element2.IsEnabled = true;
                    Element2.Visibility = Utils.GetVisibility(Element2.IsEnabled);
                    Element2Text.Text = Modname;
                    if (Database.Database.isModInstalled(Modname))
                    {
                        if (!Database.Database.GetInstalledversion(Modname, Version))
                        {
                            Element2Text.Foreground = Brushes.Orange;
                        }
                        else
                        {
                            Element2Text.Foreground = Brushes.DarkOliveGreen;
                        }
                    }
                    else
                    {
                        Element2Text.Foreground = Brushes.Wheat;
                    }
                    break;
                case 3:
                    Element3.IsEnabled = true;
                    Element3.Visibility = Utils.GetVisibility(Element3.IsEnabled);
                    Element3Text.Text = Modname;
                    if (Database.Database.isModInstalled(Modname))
                    {
                        if (!Database.Database.GetInstalledversion(Modname, Version))
                        {
                            Element3Text.Foreground = Brushes.Orange;
                        }
                        else
                        {
                            Element3Text.Foreground = Brushes.DarkOliveGreen;
                        }
                    }
                    else
                    {
                        Element3Text.Foreground = Brushes.Wheat;
                    }
                    break;
                case 4:
                    Element4.IsEnabled = true;
                    Element4.Visibility = Utils.GetVisibility(Element4.IsEnabled);
                    Element4Text.Text = Modname;
                    if (Database.Database.isModInstalled(Modname))
                    {
                        if (!Database.Database.GetInstalledversion(Modname, Version))
                        {
                            Element4Text.Foreground = Brushes.Orange;
                        }
                        else
                        {
                            Element4Text.Foreground = Brushes.DarkOliveGreen;
                        }
                    }
                    else
                    {
                        Element4Text.Foreground = Brushes.Wheat;
                    }
                    break;
                case 5:
                    Element5.IsEnabled = true;
                    Element5.Visibility = Utils.GetVisibility(Element5.IsEnabled);
                    Element5Text.Text = Modname;
                    if (Database.Database.isModInstalled(Modname))
                    {
                        if (!Database.Database.GetInstalledversion(Modname, Version))
                        {
                            Element5Text.Foreground = Brushes.Orange;
                        }
                        else
                        {
                            Element5Text.Foreground = Brushes.DarkOliveGreen;
                        }
                    }
                    else
                    {
                        Element5Text.Foreground = Brushes.Wheat;
                    }
                    break;
                case 6:
                    Element6.IsEnabled = true;
                    Element6.Visibility = Utils.GetVisibility(Element6.IsEnabled);
                    Element6Text.Text = Modname;
                    if (Database.Database.isModInstalled(Modname))
                    {
                        if (!Database.Database.GetInstalledversion(Modname, Version))
                        {
                            Element6Text.Foreground = Brushes.Orange;
                        }
                        else
                        {
                            Element6Text.Foreground = Brushes.DarkOliveGreen;
                        }
                    }
                    else
                    {
                        Element6Text.Foreground = Brushes.Wheat;
                    }
                    break;
                case 7:
                    Element7.IsEnabled = true;
                    Element7.Visibility = Utils.GetVisibility(Element7.IsEnabled);
                    Element7Text.Text = Modname;
                    if (Database.Database.isModInstalled(Modname))
                    {
                        if (!Database.Database.GetInstalledversion(Modname, Version))
                        {
                            Element7Text.Foreground = Brushes.Orange;
                        }
                        else
                        {
                            Element7Text.Foreground = Brushes.DarkOliveGreen;
                        }
                    }
                    else
                    {
                        Element7Text.Foreground = Brushes.Wheat;
                    }
                    break;
                case 8:
                    Element8.IsEnabled = true;
                    Element8.Visibility = Utils.GetVisibility(Element8.IsEnabled);
                    Element8Text.Text = Modname;
                    if (Database.Database.isModInstalled(Modname))
                    {
                        if (!Database.Database.GetInstalledversion(Modname, Version))
                        {
                            Element8Text.Foreground = Brushes.Orange;
                        }
                        else
                        {
                            Element8Text.Foreground = Brushes.DarkOliveGreen;
                        }
                    }
                    else
                    {
                        Element8Text.Foreground = Brushes.Wheat;
                    }
                    break;
                case 9:
                    Element9.IsEnabled = true;
                    Element9.Visibility = Utils.GetVisibility(Element9.IsEnabled);
                    Element9Text.Text = Modname;
                    if (Database.Database.isModInstalled(Modname))
                    {
                        if (!Database.Database.GetInstalledversion(Modname, Version))
                        {
                            Element9Text.Foreground = Brushes.Orange;
                        }
                        else
                        {
                            Element9Text.Foreground = Brushes.DarkOliveGreen;
                        }
                    }
                    else
                    {
                        Element9Text.Foreground = Brushes.Wheat;
                    }
                    break;
                case 10:
                    Element10.IsEnabled = true;
                    Element10.Visibility = Utils.GetVisibility(Element10.IsEnabled);
                    Element10Text.Text = Modname;
                    if (Database.Database.isModInstalled(Modname))
                    {
                        if (!Database.Database.GetInstalledversion(Modname, Version))
                        {
                            Element10Text.Foreground = Brushes.Orange;
                        }
                        else
                        {
                            Element10Text.Foreground = Brushes.DarkOliveGreen;
                        }
                    }
                    else
                    {
                        Element10Text.Foreground = Brushes.Wheat;
                    }
                    break;
            }
            ApplayImages(ModNumber);
        }

        /// <summary>
        /// Applies images for the specified mod slot.
        /// </summary>
        /// <param name="ModNumber">The slot number of the mod (1 to 10).</param>
        private void ApplayImages(int ModNumber)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new Action<int>(ApplayImages), ModNumber);
                return;
            }
            switch (ModNumber)
            {
                case 1:
                    Element1Image.Source = Utils.LoadImage(ModViewModel.Mods[0].Modimage);
                    break;
                case 2:
                    Element2Image.Source = Utils.LoadImage(ModViewModel.Mods[1].Modimage);
                    break;
                case 3:
                    Element3Image.Source = Utils.LoadImage(ModViewModel.Mods[2].Modimage);
                    break;
                case 4:
                    Element4Image.Source = Utils.LoadImage(ModViewModel.Mods[3].Modimage);
                    break;
                case 5:
                    Element5Image.Source = Utils.LoadImage(ModViewModel.Mods[4].Modimage);
                    break;
                case 6:
                    Element6Image.Source = Utils.LoadImage(ModViewModel.Mods[5].Modimage);
                    break;
                case 7:
                    Element7Image.Source = Utils.LoadImage(ModViewModel.Mods[6].Modimage);
                    break;
                case 8:
                    Element8Image.Source = Utils.LoadImage(ModViewModel.Mods[7].Modimage);
                    break;
                case 9:
                    Element9Image.Source = Utils.LoadImage(ModViewModel.Mods[8].Modimage);
                    break;
                case 10:
                    Element10Image.Source = Utils.LoadImage(ModViewModel.Mods[9].Modimage);
                    break;
            }
        }

        private void SetLang(object sender, EventArgs e)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new Action<object, EventArgs>(SetLang), sender, e);
                return;
            }
            MyMods.Content = Lang.GetText(20);
        }

        private void SetLang()
        {
            MyMods.Content = Lang.GetText(20);
        }
    }
}