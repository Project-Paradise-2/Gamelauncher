using Newtonsoft.Json.Linq;
using ProjectParadise2.Core;
using ProjectParadise2.Core.Log;
using ProjectParadise2.Database.Data;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ProjectParadise2.Views
{
    /// <summary>
    /// Interaktionslogik für ServiceView.xaml
    /// </summary>
    public partial class ServiceView : UserControl
    {
        /// <summary>
        /// The username of the currently logged-in account.
        /// </summary>
        private string AccountName { get; set; }
        /// <summary>
        /// The password of the currently logged-in account.
        /// </summary>
        private string AccountPassword { get; set; }
        /// <summary>
        /// Indicates whether the user is currently logged in.
        /// </summary>
        private bool IsLoggedIn { get; set; } = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceView"/> class.
        /// Sets up the UI components and starts the initialization processes.
        /// </summary>
        public ServiceView()
        {
            InitializeComponent();
            BackgroundWorker.OnLangset += SetLang;
            SetLang();
            SetUserasnonLogged();
            UpdateProgress("", true);
            if (Database.Database.p2Database.Usersettings.BackupType == BackUptype.OnStart)
            {
                this.BackupType.SelectedIndex = 0;
                this.BackupType.SelectedItem = 0;
            }
            else
            {
                this.BackupType.SelectedIndex = 1;
                this.BackupType.SelectedItem = 1;
            }
            Thread refreshThread = new Thread(SetupInfos);
            refreshThread.Start();
        }

        /// <summary>
        /// Sets up the information and UI components for the ServiceView.
        /// </summary>
        void SetupInfos()
        {
            Thread.Sleep(50);

            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new Action(SetupInfos));
                return;
            }

            SetupLocalBackup();


            if (!string.IsNullOrEmpty(Database.Database.p2Database.Usersettings.Acountname))
            {
                AccountName = Database.Database.p2Database.Usersettings.Acountname;
                Accountname.Text = AccountName;
            }
            if (!string.IsNullOrEmpty(Database.Database.p2Database.Usersettings.Accountpassword))
            {
                AccountPassword = Database.Database.p2Database.Usersettings.Accountpassword;
                Accountpassword.Text = AccountPassword;
            }
            if (!string.IsNullOrEmpty(Database.Database.p2Database.Usersettings.Accountpassword) && !string.IsNullOrEmpty(Database.Database.p2Database.Usersettings.Acountname))
                Login();
        }

        /// <summary>
        /// Sets up the information and UI components for the ServiceView.
        /// </summary>
        private void SetupLocalBackup()
        {
            this.DiscordToggle.IsChecked = Database.Database.p2Database.Usersettings.LocalBackup;
            this.MaximalSaves.Text = Database.Database.p2Database.Usersettings.NumOfBackups.ToString();

            if (Database.Database.p2Database.Usersettings.BackupType == BackUptype.OnStart)
            {
                this.BackupType.SelectedIndex = 0;
                this.BackupType.SelectedItem = 0;
            }
            else
            {
                this.BackupType.SelectedIndex = 1;
                this.BackupType.SelectedItem = 1;
            }
            if (this.DiscordToggle.IsChecked == true)
            {
                this.BackupTypeText.Visibility = Visibility.Visible;
                this.BackupType.Visibility = Visibility.Visible;
                this.MaximalSavesText.Visibility = Visibility.Visible;
                this.MaximalSaves.Visibility = Visibility.Visible;
                this.OpenBackupFolder.Visibility = Visibility.Visible;
            }
            else
            {
                this.BackupTypeText.Visibility = Visibility.Hidden;
                this.BackupType.Visibility = Visibility.Hidden;
                this.MaximalSavesText.Visibility = Visibility.Hidden;
                this.MaximalSaves.Visibility = Visibility.Hidden;
                this.OpenBackupFolder.Visibility = Visibility.Hidden;
            }

            if (Database.Database.p2Database.Usersettings.LocalBackup)
            {
                if (!Directory.Exists(Constans.DokumentsFolder + "Local_Backups"))
                    Directory.CreateDirectory(Constans.DokumentsFolder + "Local_Backups");
            }
        }

        /// <summary>
        /// Logs out the user from the cloud account and resets relevant UI components.
        /// </summary>
        private void CloudLogout(object sender, System.Windows.RoutedEventArgs e)
        {
            IsLoggedIn = false;
            AccountPassword = "";
            AccountName = "";
            Database.Database.p2Database.Usersettings.Acountname = AccountName;
            Database.Database.p2Database.Usersettings.Accountpassword = AccountPassword;
            Accountpassword.Text = "Accountpassword";
            Accountname.Text = "Accountname";
            Database.Database.Write();
            SetUserasnonLogged();
        }

        /// <summary>
        /// Logs in the user to the cloud account using the provided credentials.
        /// </summary>
        private void CloudLogin(object sender, System.Windows.RoutedEventArgs e)
        {
            Login();
        }

        /// <summary>
        /// Refreshes the savegame information from the cloud.
        /// </summary>
        private void RefreshCloud(object sender, System.Windows.RoutedEventArgs e)
        {
            GetSavegameInfos();
        }

        /// <summary>
        /// Handles the login process by validating the credentials and updating the UI accordingly.
        /// </summary>
        private void Login()
        {
            try
            {
                if (string.IsNullOrEmpty(AccountName) || string.IsNullOrEmpty(AccountPassword))
                {
                    if (string.IsNullOrEmpty(AccountName) && string.IsNullOrEmpty(AccountPassword))
                    {
                        MessageBox.Show(Lang.GetText(75),
                                        "Project Paradise 2 - Cloud Login",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Warning);
                    }
                    else if (string.IsNullOrEmpty(AccountName))
                    {
                        MessageBox.Show(Lang.GetText(76),
                                        "Project Paradise 2 - Cloud Login",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Warning);
                    }
                    else if (string.IsNullOrEmpty(AccountPassword))
                    {
                        MessageBox.Show(Lang.GetText(77),
                                        "Project Paradise 2 - Cloud Login",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Warning);
                    }
                    return;
                }

                var client = new WebClient();
                client.Headers.Add("User-Agent", "Cloud Login");
                string url = $"{Constans.Cdn}/Requests/cloud.php?r=1&u={AccountName}&p={System.Convert.ToBase64String(Encoding.UTF8.GetBytes(AccountPassword))}";
                string content = client.DownloadString(url);
                JObject jsonResponse = JObject.Parse(content);
                string status = (string)jsonResponse["status"];

                if (status == "fail")
                {
                    MessageBox.Show(Lang.GetText(78),
                                    "Project Paradise 2 - Cloud Login",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                    return;
                }
                else if (status == "success")
                {
                    Database.Database.p2Database.Usersettings.Acountname = AccountName;
                    Database.Database.p2Database.Usersettings.Accountpassword = AccountPassword;
                    IsLoggedIn = true;
                    SetUserasLogged();
                    GetSavegameInfos();
                    return;
                }
                else
                {
                    MessageBox.Show("An unexpected response was received from the server. Please try again later.",
                                    "Project Paradise 2 - Cloud Login",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                    return;
                }
            }
            catch (WebException webEx)
            {
                MessageBox.Show(string.Format(Lang.GetText(79), webEx.Message),
                                "Project Paradise 2 - Cloud Login",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
            }
            catch (UriFormatException uriEx)
            {
                MessageBox.Show(string.Format(Lang.GetText(80), uriEx.Message),
                                "Project Paradise 2 - Cloud Login",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
            catch (InvalidOperationException invOpEx)
            {
                MessageBox.Show(string.Format(Lang.GetText(81), invOpEx.Message),
                                "Project Paradise 2 - Cloud Login",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format(Lang.GetText(82), ex.Message),
                                "Project Paradise 2 - Cloud Login",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Updates the UI to show the logged-in state.
        /// </summary>
        private void SetUserasLogged()
        {
            LoginText.Visibility = Visibility.Hidden;
            Accountname.Visibility = Visibility.Hidden;
            Accountpassword.Visibility = Visibility.Hidden;
            AccountText.Visibility = Visibility.Hidden;
            PasswordText.Visibility = Visibility.Hidden;
            LoginBtn.Visibility = Visibility.Hidden;
            LogoutBtn.Visibility = Visibility.Visible;
            Refresh.Visibility = Visibility.Visible;
            SaveInfoText.Visibility = Visibility.Visible;
            Savegamedata.Visibility = Visibility.Visible;
            CloudUpload.Visibility = Visibility.Visible;
            CloudDownload.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Updates the UI to show the non-logged-in state.
        /// </summary>
        private void SetUserasnonLogged()
        {
            LoginText.Visibility = Visibility.Visible;
            Accountname.Visibility = Visibility.Visible;
            Accountpassword.Visibility = Visibility.Visible;
            AccountText.Visibility = Visibility.Visible;
            PasswordText.Visibility = Visibility.Visible;
            LoginBtn.Visibility = Visibility.Visible;
            LogoutBtn.Visibility = Visibility.Hidden;
            Refresh.Visibility = Visibility.Hidden;
            SaveInfoText.Visibility = Visibility.Hidden;
            Savegamedata.Visibility = Visibility.Hidden;
            CloudUpload.Visibility = Visibility.Hidden;
            CloudDownload.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Retrieves savegame information from the cloud and updates the UI.
        /// </summary>
        private void GetSavegameInfos()
        {
            if (IsLoggedIn)
            {

                var client = new WebClient();
                client.Headers.Add("User-Agent", "Cloud Info");

                if (string.IsNullOrEmpty(Database.Database.p2Database.Usersettings.Acountname) || string.IsNullOrEmpty(Database.Database.p2Database.Usersettings.Accountpassword))
                {
                    return;
                }

                string url = $"{Constans.Cdn}/Requests/cloud.php?r=2&u={Database.Database.p2Database.Usersettings.Acountname}&p={System.Convert.ToBase64String(Encoding.UTF8.GetBytes(Database.Database.p2Database.Usersettings.Accountpassword))}";

                try
                {
                    string content = client.DownloadString(url);
                    JObject jsonResponse = JObject.Parse(content);
                    if (jsonResponse["exists"] != null && (bool)jsonResponse["exists"])
                    {
                        long fileSize = (long)jsonResponse["filesize"];
                        string lastModifiedString = (string)jsonResponse["lastModified"];
                        DateTime lastModifiedDate = DateTime.Parse(lastModifiedString);
                        string formattedLastModified = lastModifiedDate.ToString("HH:mm dd.MM.yyyy");
                        string remoteName = (string)jsonResponse["name"];

                        string savegameStatus = (bool)jsonResponse["exists"] ? Lang.GetText(83) : Lang.GetText(84);
                        string saveSize = FormatFileSize(fileSize);
                        string lastUploaded = formattedLastModified;
                        string remoteSavegameName = remoteName;

                        // Dann den Text wie folgt setzen

                        string data = string.Format(Lang.GetText(85), savegameStatus, saveSize, lastUploaded, remoteSavegameName, DateTime.Now.ToString("HH:mm dd.MM.yyyy"));

                        this.Savegamedata.Text = data;
                    }
                    else
                    {
                        string message = (string)jsonResponse["message"];
                        this.Savegamedata.Text = Lang.GetText(86);
                    }
                }
                catch (WebException webEx)
                {
                    MessageBox.Show(string.Format(Lang.GetText(79), webEx.Message),
                                        "Project Paradise 2 - Cloud Login",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Warning);
                }
                catch (UriFormatException uriEx)
                {
                    MessageBox.Show(string.Format(Lang.GetText(80), uriEx.Message),
                                        "Project Paradise 2 - Cloud Login",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Error);
                }
                catch (InvalidOperationException invOpEx)
                {
                    MessageBox.Show(string.Format(Lang.GetText(81), invOpEx.Message),
                                        "Project Paradise 2 - Cloud Login",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format(Lang.GetText(82), ex.Message),
                                        "Project Paradise 2 - Cloud Login",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// Formats a file size in bytes into a human-readable string.
        /// </summary>
        /// <param name="fileSize">The size of the file in bytes.</param>
        /// <returns>A string representation of the file size (e.g., "1.23 MB").</returns>
        public string FormatFileSize(long fileSize)
        {
            string[] sizeUnits = { "Bytes", "KB", "MB", "GB", "TB", "PB", "EB" };
            double size = fileSize;
            int unitIndex = 0;

            while (size >= 1024 && unitIndex < sizeUnits.Length - 1)
            {
                size /= 1024;
                unitIndex++;
            }

            return string.Format("{0:0.##} {1}", size, sizeUnits[unitIndex]);
        }

        /// <summary>
        /// Called when the user changes the username input.
        /// Updates the internal account name property.
        /// </summary>
        private void UsernameInputChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox outerTextBox)
            {
                var innerTextBox = FindChild<TextBox>(outerTextBox, "Input");
                if (innerTextBox != null)
                {
                    string inputText = innerTextBox.Text;
                    AccountName = inputText;
                }
            }
        }

        /// <summary>
        /// Called when the user changes the password input.
        /// Updates the internal account password property.
        /// </summary>
        private void PasswordInputChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox outerTextBox)
            {
                var innerTextBox = FindChild<TextBox>(outerTextBox, "Input");
                if (innerTextBox != null)
                {
                    string inputText = innerTextBox.Text;
                    AccountPassword = inputText;
                }
            }
        }

        /// <summary>
        /// Finds a child UI element of the specified type and name within a parent element.
        /// </summary>
        /// <typeparam name="T">The type of the child element to find.</typeparam>
        /// <param name="parent">The parent element.</param>
        /// <param name="childName">The name of the child element to find.</param>
        /// <returns>The found child element, or null if not found.</returns>
        private static T FindChild<T>(DependencyObject parent, string childName) where T : DependencyObject
        {
            int childCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T typedChild && ((FrameworkElement)child).Name == childName)
                {
                    return typedChild;
                }

                var result = FindChild<T>(child, childName);
                if (result != null)
                    return result;
            }
            return null;
        }

        /// <summary>
        /// Handles the savegame upload process, including preparation and progress updates.
        /// </summary>
        private async void OnCloudupload(object sender, RoutedEventArgs e)
        {
            if (IsLoaded)
            {
                this.Uploadstate.Visibility = Visibility.Visible;
                this.Uploadstate.Text = Lang.GetText(87);

                try
                {
                    await UploadSavegameAsync();
                }
                catch (Exception ex)
                {
                    this.Uploadstate.Text = $"Error: {ex.Message}";
                    Console.WriteLine(ex.StackTrace);
                }
            }
        }

        /// <summary>
        /// Uploads the savegame asynchronously with progress tracking.
        /// </summary>
        private async Task UploadSavegameAsync()
        {
            try
            {
                string backupFolder = Constans.DokumentsFolder + "Backups/";
                string zipFilePath = backupFolder + "Upload.zip";

                if (!Directory.Exists(backupFolder))
                {
                    Directory.CreateDirectory(backupFolder);
                }
                if (File.Exists(zipFilePath))
                {
                    UpdateProgress(Lang.GetText(88));
                    File.Delete(zipFilePath);
                    Log.Info("Clean Up Old Local Savegame");
                }
                UpdateProgress(Lang.GetText(89));
                ZipFile.CreateFromDirectory(Constans.SaveFolder, zipFilePath);
                await Task.Delay(250);
                UpdateProgress(Lang.GetText(90));
                Log.Info("Start Upload");
                await Task.Delay(250);
                var progressHandler = new Progress<long>(bytesUploaded =>
                {
                    long totalBytes = new FileInfo(zipFilePath).Length;
                    int progressPercentage = (int)(bytesUploaded * 100 / totalBytes);
                    UpdateProgress(string.Format(Lang.GetText(91), progressPercentage, FormatFileSize(bytesUploaded)));
                });

                await UploadFileWithProgressAsync(zipFilePath, $"{Constans.Cdn}/Requests/Fileupload.php?u={Database.Database.p2Database.Usersettings.Acountname}", progressHandler);

                UpdateProgress(Lang.GetText(92), true);
                Log.Info("Upload erfolgreich abgeschlossen");
                GetSavegameInfos();

                if (File.Exists(zipFilePath))
                {
                    File.Delete(zipFilePath);
                }

                if (Directory.Exists(backupFolder))
                {
                    Directory.Delete(backupFolder);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                Log.Error($"Error: {ex.Message}\n" + ex?.StackTrace.ToString());
            }
        }

        /// <summary>
        /// Uploads a file to a server with progress reporting.
        /// </summary>
        /// <param name="filePath">The path to the file being uploaded.</param>
        /// <param name="url">The server URL where the file will be uploaded.</param>
        /// <param name="progress">Progress reporter to track upload progress.</param>
        private async Task UploadFileWithProgressAsync(string filePath, string url, IProgress<long> progress)
        {
            using (var httpClient = new HttpClient())
            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                var totalBytes = fileStream.Length;
                var buffer = new byte[8192];

                using (var content = new MultipartFormDataContent())
                {
                    using (var streamContent = new ProgressStreamContent(fileStream, totalBytes, progress))
                    {
                        content.Add(streamContent, "file", Path.GetFileName(filePath));
                        var response = await httpClient.PostAsync(url, content);

                        if (!response.IsSuccessStatusCode)
                        {
                            Log.Error($"Upload failed with status code: {response.StatusCode}");
                            return;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Initiates the cloud savegame download process when triggered by the UI.
        /// </summary>
        private async void OnClouddownload(object sender, RoutedEventArgs e)
        {
            if (IsLoggedIn)
            {
                await DownloadSavegameAsync();
            }
        }

        /// <summary>
        /// Downloads the savegame from the cloud and unpacks it locally.
        /// </summary>
        private async Task DownloadSavegameAsync()
        {
            try
            {
                string saveFolderPath = Constans.SaveFolder;
                EnsureDirectoryExists(saveFolderPath);

                string remoteUri = $"{Constans.Cdn}/Requests/savegames/{Database.Database.p2Database.Usersettings.Acountname}-Upload.zip";
                string localZipPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), $"{Database.Database.p2Database.Usersettings.Acountname}-Upload.zip");

                UpdateProgress(Lang.GetText(93));

                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync(remoteUri, HttpCompletionOption.ResponseHeadersRead))
                    {
                        if (!response.IsSuccessStatusCode)
                        {
                            throw new HttpRequestException($"Failed to download file. Status code: {response.StatusCode}");
                        }
                        var totalBytes = response.Content.Headers.ContentLength ?? -1L;
                        if (totalBytes <= 0)
                        {
                            throw new InvalidOperationException("Unable to determine the file size from the server response.");
                        }

                        var progressHandler = new Progress<long>(bytesDownloaded =>
                        {
                            int progressPercentage = (int)((bytesDownloaded * 100) / totalBytes);
                            UpdateProgress(string.Format(Lang.GetText(94), progressPercentage, FormatFileSize(bytesDownloaded)));
                        });

                        await DownloadWithProgressAsync(httpClient, response, localZipPath, progressHandler);
                    }
                }

                UpdateProgress(Lang.GetText(95));
                await Task.Run(() => ExtractSavegame(localZipPath, saveFolderPath));
                UpdateProgress(Lang.GetText(96));
                await Task.Delay(300);
                UpdateProgress(Lang.GetText(96), true);
                if (File.Exists(localZipPath))
                    File.Delete(localZipPath);
            }
            catch (Exception ex)
            {
                Log.Error("Cloud Savegame Download Failed ", ex);
            }
            finally
            {
                GC.Collect();
            }
        }

        /// <summary>
        /// Downloads a file with progress tracking and writes it to a specified path.
        /// </summary>
        /// <param name="httpClient">The HTTP client used to fetch the file.</param>
        /// <param name="response">The HTTP response containing the file.</param>
        /// <param name="destinationPath">The local path where the file will be saved.</param>
        /// <param name="progress">Progress reporter for tracking download progress.</param>
        private async Task DownloadWithProgressAsync(HttpClient httpClient, HttpResponseMessage response, string destinationPath, IProgress<long> progress)
        {
            var buffer = new byte[8192];
            long bytesDownloaded = 0;

            using (var contentStream = await response.Content.ReadAsStreamAsync())
            using (var fileStream = new FileStream(destinationPath, FileMode.Create, FileAccess.Write, FileShare.None, buffer.Length, true))
            {
                int bytesRead;
                while ((bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    await fileStream.WriteAsync(buffer, 0, bytesRead);
                    bytesDownloaded += bytesRead;
                    progress?.Report(bytesDownloaded);
                }
            }
        }

        /// <summary>
        /// Extracts a ZIP archive to a specified folder.
        /// Handles directory creation, file backups, and extraction errors.
        /// </summary>
        /// <param name="zipFilePath">The path of the ZIP file to extract.</param>
        /// <param name="extractToPath">The destination folder for extraction.</param>
        private void ExtractSavegame(string zipFilePath, string extractToPath)
        {
            try
            {
                if (File.Exists(zipFilePath))
                {
                    using (var archive = ZipFile.OpenRead(zipFilePath))
                    {
                        foreach (var entry in archive.Entries)
                        {
                            string destinationPath = Path.Combine(extractToPath, entry.FullName.Replace('/', '\\'));
                            string directoryPath = Path.GetDirectoryName(destinationPath);
                            if (entry.FullName.EndsWith("/"))
                            {
                                if (!Directory.Exists(destinationPath))
                                {
                                    try
                                    {
                                        Directory.CreateDirectory(destinationPath);
                                        Log.Info($"Directory created: {destinationPath}");
                                    }
                                    catch (Exception dirEx)
                                    {
                                        Log.Error($"Failed to create directory: {destinationPath}, Error: {dirEx.Message}");
                                        throw;
                                    }
                                }
                            }
                            else
                            {
                                if (!Directory.Exists(directoryPath))
                                {
                                    try
                                    {
                                        Directory.CreateDirectory(directoryPath);
                                        Log.Info($"Directory created: {directoryPath}");
                                    }
                                    catch (Exception dirEx)
                                    {
                                        Log.Error($"Failed to create directory: {directoryPath}, Error: {dirEx.Message}");
                                        throw;
                                    }
                                }

                                if (File.Exists(destinationPath))
                                {
                                    string backupPath = destinationPath + ".backup";
                                    Log.Info($"Existing file backed up: {destinationPath} -> {backupPath}");
                                    if (File.Exists(backupPath))
                                    {
                                        File.Delete(backupPath);
                                    }

                                    File.Move(destinationPath, backupPath);
                                }

                                try
                                {
                                    entry.ExtractToFile(destinationPath, overwrite: true);
                                    Log.Info($"File extracted: {destinationPath}");
                                }
                                catch (Exception extractEx)
                                {
                                    Log.Error($"Failed to extract {entry.FullName} to {destinationPath}. Error: {extractEx.Message}");
                                    throw;
                                }
                            }
                        }
                    }

                    Log.Info("Cloud Savegame unpacked successfully.");
                    UpdateProgress(Lang.GetText(96));
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Cloud Unpack Failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Ensures a directory exists by creating it if it does not already exist.
        /// </summary>
        /// <param name="path">The path of the directory to check or create.</param>
        private void EnsureDirectoryExists(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        /// <summary>
        /// Updates the progress UI element with the provided message and state.
        /// </summary>
        /// <param name="input">The progress message to display.</param>
        /// <param name="isDone">Indicates whether the operation is complete.</param>
        public void UpdateProgress(string input, bool isDone = false)
        {

            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new Action<string, bool>(UpdateProgress), input, isDone);
                return;
            }

            if (isDone)
            {
                this.Uploadstate.Visibility = Visibility.Hidden;
                GetSavegameInfos();
            }
            else
            {
                this.Uploadstate.Visibility = Visibility.Visible;
            }

            this.Uploadstate.Text = input;
        }

        /// <summary>
        /// Handles the text change event for updating the number of backups in the user's settings.
        /// Retrieves input from the associated text box, parses it, and updates the database.
        /// </summary>
        /// <param name="sender">The source of the event, typically a TextBox control.</param>
        /// <param name="e">Event arguments containing details about the text change.</param>
        private void SavePointsEdit(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (sender is TextBox outerTextBox)
                {
                    var innerTextBox = FindChild<TextBox>(outerTextBox, "Input");
                    if (innerTextBox != null)
                    {
                        string inputText = innerTextBox.Text;
                        Database.Database.p2Database.Usersettings.NumOfBackups = int.Parse(inputText);
                        Database.Database.Write();
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Opens the local backup folder in Windows Explorer.
        /// Displays an error if the folder does not exist.
        /// </summary>
        private void OnOpenbackFolder(object sender, RoutedEventArgs e)
        {
            string folderPath = Path.Combine(Constans.DokumentsFolder, "Local_Backups");
            OpenDirectory(folderPath + @"\", "The savegame folder cannot be opened because folder not exist.");
        }

        /// <summary>
        /// Updates the user's backup type preference based on the selection in the UI.
        /// </summary>
        /// <param name="sender">The source of the event, typically a UI element.</param>
        /// <param name="e">Event arguments containing details about the selection change.</param>
        private void BackupTypeChanged(object sender, SelectionChangedEventArgs e)
        {
            Database.Database.p2Database.Usersettings.BackupType = (BackUptype)BackupType.SelectedIndex;
            Database.Database.Write();
        }

        /// <summary>
        /// Toggles the local backup option based on the user's selection in the UI and initializes the local backup setup.
        /// </summary>
        /// <param name="sender">The source of the event, typically a UI element.</param>
        /// <param name="e">Event arguments containing details about the toggle event.</param>
        private void OnLocalbackup(object sender, RoutedEventArgs e)
        {
            Database.Database.p2Database.Usersettings.LocalBackup = (bool)DiscordToggle.IsChecked;
            Database.Database.Write();
            SetupLocalBackup();
        }

        /// <summary>
        /// Helper method to open a directory in Windows Explorer.
        /// </summary>
        /// <param name="folderPath">The folder path to open.</param>
        /// <param name="errorMessage">The error message to display if the folder does not exist.</param>
        private void OpenDirectory(string folderPath, string errorMessage)
        {
            if (Directory.Exists(folderPath))
            {
                Process.Start(new ProcessStartInfo
                {
                    Arguments = folderPath.Replace("/", @"\"),
                    FileName = "explorer.exe"
                });
            }
            else
            {
                MessageBox.Show("Folder not exist:\n" + folderPath.Replace("/", @"\"), "Project Paradise 2", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SetLang(object sender, EventArgs e)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new Action<object, EventArgs>(SetLang), sender, e);
                return;
            }
            LoginText.Text = Lang.GetText(21);
            LoginBtn.ToolTip = Lang.GetTooltipText(0);
            LogoutBtn.ToolTip = Lang.GetTooltipText(1);
            Refresh.ToolTip = Lang.GetTooltipText(2);
            DiscordToggle.ToolTip = Lang.GetTooltipText(3);
            OpenBackupFolder.ToolTip = Lang.GetTooltipText(4);
            BackupType.ToolTip = Lang.GetTooltipText(5);
            MaximalSaves.ToolTip = Lang.GetTooltipText(6);
            xOpenGamefolder.ToolTip = Lang.GetTooltipText(7);
            xOpenSavefolder.ToolTip = Lang.GetTooltipText(8);
            CloudDownload.ToolTip = Lang.GetTooltipText(10);
            CloudUpload.ToolTip = Lang.GetTooltipText(9);
            AccountText.Text = Lang.GetText(22);
            PasswordText.Text = Lang.GetText(23);
            LoginBtn.Content = Lang.GetText(27);
            LogoutBtn.Content = Lang.GetText(24);
            Refresh.Content = Lang.GetText(29);
            CloudUpload.Content = Lang.GetText(25);
            CloudDownload.Content = Lang.GetText(26);
            OpenBackupFolder.Content = Lang.GetText(30);
            MaximalSavesText.Text = Lang.GetText(38);
            xOpenGamefolder.Content = Lang.GetText(33);
            xOpenSavefolder.Content = Lang.GetText(35);
        }

        private void SetLang()
        {
            LoginText.Text = Lang.GetText(21);
            AccountText.Text = Lang.GetText(22);
            PasswordText.Text = Lang.GetText(23);
            LoginBtn.Content = Lang.GetText(27);
            LogoutBtn.Content = Lang.GetText(24);
            Refresh.Content = Lang.GetText(29);
            CloudUpload.Content = Lang.GetText(25);
            CloudDownload.Content = Lang.GetText(26);
            OpenBackupFolder.Content = Lang.GetText(30);
            MaximalSavesText.Text = Lang.GetText(38);
            xOpenGamefolder.Content = Lang.GetText(33);
            xOpenSavefolder.Content = Lang.GetText(35);
            LoginBtn.ToolTip = Lang.GetTooltipText(0);
            LogoutBtn.ToolTip = Lang.GetTooltipText(1);
            Refresh.ToolTip = Lang.GetTooltipText(2);
            DiscordToggle.ToolTip = Lang.GetTooltipText(3);
            OpenBackupFolder.ToolTip = Lang.GetTooltipText(4);
            BackupType.ToolTip = Lang.GetTooltipText(5);
            MaximalSaves.ToolTip = Lang.GetTooltipText(6);
            xOpenGamefolder.ToolTip = Lang.GetTooltipText(7);
            xOpenSavefolder.ToolTip = Lang.GetTooltipText(8);
            CloudDownload.ToolTip = Lang.GetTooltipText(10);
            CloudUpload.ToolTip = Lang.GetTooltipText(9);
        }

        private void OpenGamefolder(object sender, RoutedEventArgs e)
        {
            OpenDirectory(Database.Database.p2Database.Usersettings.Gamedirectory, Lang.GetText(107));
        }

        private void OpenSavefolder(object sender, RoutedEventArgs e)
        {
            OpenDirectory(Constans.SaveFolder, Lang.GetText(108));
        }
    }
}