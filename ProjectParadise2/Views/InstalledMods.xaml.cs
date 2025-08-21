using ProjectParadise2.Core;
using ProjectParadise2.Core.Log;
using ProjectParadise2.Database.Data;
using ProjectParadise2.Objects;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace ProjectParadise2.Views
{
    /// <summary>
    /// Interaction logic for InstalledMods.xaml
    /// </summary>
    public partial class InstalledMods : UserControl
    {
        /// <summary>
        /// The current page of displayed mods.
        /// </summary>
        private int CurrentPage = 0;

        /// <summary>
        /// Initializes the Installed Mods view.
        /// </summary>
        public InstalledMods()
        {
            InitializeComponent();
            BackgroundWorker.OnLangset += SetLang;
            SetLang();
            ReadInstalledMods();
        }

        /// <summary>
        /// Reads installed mods from the database and displays them in the UI.
        /// </summary>
        private void ReadInstalledMods()
        {
            try
            {
                Hideall();
                int itemsPerPage = 7;
                int totalMods = Database.Database.p2Database.Usermods.Count;
                int maxPage = Math.Max((totalMods - 1) / itemsPerPage, 0);

                if (CurrentPage > maxPage)
                {
                    CurrentPage = maxPage;
                }

                int startIndex = CurrentPage * itemsPerPage;
                int endIndex = Math.Min(startIndex + itemsPerPage, totalMods);

                if (startIndex >= totalMods && CurrentPage > 0)
                {
                    CurrentPage--;
                    ReadInstalledMods();
                    return;
                }

                for (int i = startIndex; i < endIndex; i++)
                {
                    ShowMod(i, i - startIndex);
                }
            }
            catch (Exception ex)
            {
                Log.Print("An error occurred while reading installed mods: ", ex);
            }
        }

        /// <summary>
        /// Displays the details of a mod in the UI.
        /// </summary>
        /// <param name="ModId">The ID of the mod in the database.</param>
        /// <param name="rowId">The position in the current page view.</param>
        private void ShowMod(int ModId, int rowId)
        {
            var mod = Database.Database.p2Database.Usermods[ModId];

            switch (rowId)
            {
                case 0:
                    this.InstalledMod1.Text = mod.Name + " [" + mod.Version + "] Installed: " + mod.Installed;
                    this.InstalledMod1.Visibility = Visibility.Visible;
                    this.InstalledMod1Delete.Visibility = Visibility.Visible;
                    break;
                case 1:
                    this.InstalledMod2.Text = mod.Name + " [" + mod.Version + "] Installed: " + mod.Installed;
                    this.InstalledMod2.Visibility = Visibility.Visible;
                    this.InstalledMod2Delete.Visibility = Visibility.Visible;
                    break;
                case 2:
                    this.InstalledMod3.Text = mod.Name + " [" + mod.Version + "] Installed: " + mod.Installed;
                    this.InstalledMod3.Visibility = Visibility.Visible;
                    this.InstalledMod3Delete.Visibility = Visibility.Visible;
                    break;
                case 3:
                    this.InstalledMod4.Text = mod.Name + " [" + mod.Version + "] Installed: " + mod.Installed;
                    this.InstalledMod4.Visibility = Visibility.Visible;
                    this.InstalledMod4Delete.Visibility = Visibility.Visible;
                    break;
                case 4:
                    this.InstalledMod5.Text = mod.Name + " [" + mod.Version + "] Installed: " + mod.Installed;
                    this.InstalledMod5.Visibility = Visibility.Visible;
                    this.InstalledMod5Delete.Visibility = Visibility.Visible;
                    break;
                case 5:
                    this.InstalledMod6.Text = mod.Name + " [" + mod.Version + "] Installed: " + mod.Installed;
                    this.InstalledMod6.Visibility = Visibility.Visible;
                    this.InstalledMod6Delete.Visibility = Visibility.Visible;
                    break;
                case 6:
                    this.InstalledMod7.Text = mod.Name + " [" + mod.Version + "] Installed: " + mod.Installed;
                    this.InstalledMod7.Visibility = Visibility.Visible;
                    this.InstalledMod7Delete.Visibility = Visibility.Visible;
                    break;
                case 7:
                    this.InstalledMod8.Text = mod.Name + " [" + mod.Version + "] Installed: " + mod.Installed;
                    this.InstalledMod8.Visibility = Visibility.Visible;
                    this.InstalledMod8Delete.Visibility = Visibility.Visible;
                    break;
            }

            int totalPages = (int)Math.Ceiling(Database.Database.p2Database.Usermods.Count / 7.0);
            this.PageList.Text = " - " + (CurrentPage + 1) + " / " + totalPages + " - ";

            this.NextPageBTN.Visibility = (CurrentPage + 1 >= totalPages) ? Visibility.Hidden : Visibility.Visible;
            this.LastPageBTN.Visibility = (CurrentPage > 0) ? Visibility.Visible : Visibility.Hidden;
        }

        /// <summary>
        /// Navigates back to the Mods browser.
        /// </summary>
        private void BackToMods(object sender, RoutedEventArgs e)
        {
            MainViewModel.OpenModbrowser();
        }

        /// <summary>
        /// Moves to the next page of mods.
        /// </summary>
        private void PageNext(object sender, RoutedEventArgs e)
        {
            CurrentPage++;
            ReadInstalledMods();
        }

        /// <summary>
        /// Moves to the previous page of mods.
        /// </summary>
        private void PageLast(object sender, RoutedEventArgs e)
        {
            CurrentPage--;
            ReadInstalledMods();
        }

        /// <summary>
        /// Deletes a mod based on the clicked button's associated row.
        /// </summary>
        private void DeleteMod_Click(object sender, RoutedEventArgs e)
        {
            if (sender is FA_BTN button && int.TryParse(button.Tag?.ToString(), out int rowId))
            {
                int modId = CurrentPage * 7 + rowId;
                if (modId >= 0 && modId < Database.Database.p2Database.Usermods.Count)
                {
                    var mod = Database.Database.p2Database.Usermods[modId];
                    RemoveInstalledMod(mod, modId);
                }
                else
                {
                    MessageBox.Show("Invalid Mod ID. Please try again.",
                        "Project Paradise 2 - Mod Manager",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Invalid element. Unable to determine the mod.",
                    "Project Paradise 2 - Mod Manager",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Removes an installed mod by deleting its files and updating the database.
        /// </summary>
        /// <param name="mod">The mod object to remove.</param>
        /// <param name="modId">The mod's ID in the database.</param>
        private void RemoveInstalledMod(Gamemod mod, int modId)
        {
            try
            {
                if (Database.Database.p2Database.Usersettings.Packedgame)
                {
                    foreach (var item in mod.Files)
                    {
                        string filePath = Path.Combine(Database.Database.p2Database.Usersettings.Gamedirectory, item.FileName);

                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                            Log.PrintMod($"[MOD] Removed Mod File: {item.FileName}", mod.Name);
                        }
                        else
                        {
                            Log.PrintMod($"[MOD] File not found for deletion: {item.FileName}", mod.Name);
                        }
                    }
                    Database.Database.p2Database.Usermods.RemoveAt(modId);
                    Database.Database.Write();
                    ReadInstalledMods();

                    MessageBox.Show($"Mod \"{mod.Name}\" has been successfully uninstalled.",
                                    "Project Paradise 2 - Mod Manager",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Information);
                }
                else
                {
                    Log.Print("Unpacked game mods cannot be uninstalled from the launcher.");
                    Database.Database.p2Database.Usermods.RemoveAt(modId);
                    ReadInstalledMods();

                    MessageBox.Show($"Unpacked game mods cannot be uninstalled from the launcher.",
                                    "Project Paradise 2 - Mod Manager",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                HandleError($"An error occurred: {ex.Message}", ex, "Error");
            }
        }

        /// <summary>
        /// Handles errors by logging the exception and displaying an error message to the user.
        /// </summary>
        /// <param name="userMessage">The message to display to the user in a message box.</param>
        /// <param name="ex">The exception that was caught, containing error details.</param>
        /// <param name="errorTitle">The title of the error message box.</param>
        private void HandleError(string userMessage, Exception ex, string errorTitle)
        {
            // Log the error details, including the message and stack trace.
            Log.Print($"Mod-Deinstall: {errorTitle}: \n", ex);

            // Display an error message box to inform the user of the problem.
            MessageBox.Show(userMessage,
                            "Project Paradise 2 - Mod Manager",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
        }

        /// <summary>
        /// Hides all mod-related UI elements on the page.
        /// </summary>
        private void Hideall()
        {
            this.InstalledMod1Delete.Visibility = Visibility.Hidden;
            this.InstalledMod1.Visibility = Visibility.Hidden;

            this.InstalledMod2Delete.Visibility = Visibility.Hidden;
            this.InstalledMod2.Visibility = Visibility.Hidden;

            this.InstalledMod3Delete.Visibility = Visibility.Hidden;
            this.InstalledMod3.Visibility = Visibility.Hidden;

            this.InstalledMod4Delete.Visibility = Visibility.Hidden;
            this.InstalledMod4.Visibility = Visibility.Hidden;

            this.InstalledMod5Delete.Visibility = Visibility.Hidden;
            this.InstalledMod5.Visibility = Visibility.Hidden;

            this.InstalledMod6Delete.Visibility = Visibility.Hidden;
            this.InstalledMod6.Visibility = Visibility.Hidden;

            this.InstalledMod7Delete.Visibility = Visibility.Hidden;
            this.InstalledMod7.Visibility = Visibility.Hidden;

            this.InstalledMod8Delete.Visibility = Visibility.Hidden;
            this.InstalledMod8.Visibility = Visibility.Hidden;
        }

        private void SetLang(object sender, EventArgs e)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new Action<object, EventArgs>(SetLang), sender, e);
                return;
            }
            BackBtn.Content = Lang.GetText(9);
        }

        private void SetLang()
        {
            BackBtn.Content = Lang.GetText(9);
        }
    }
}
