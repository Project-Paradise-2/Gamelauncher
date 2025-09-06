using Newtonsoft.Json;
using ProjectParadise2.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;

namespace ProjectParadise2.Views
{
    public class Stuff
    {
        public List<string> Team { get; set; }
        public List<string> Patreons { get; set; }
        public List<string> Booster { get; set; }
    }

    /// <summary>
    /// Interaktionslogik für About.xaml
    /// </summary>
    public partial class About : UserControl
    {
        public About()
        {
            InitializeComponent();
            Refresh();
        }

        private void Refresh()
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new Action(Refresh));
                return;
            }
            using (WebConnection wc = new WebConnection())
            {
                wc.Timeout = 10;
                string json = System.Text.Encoding.UTF8.GetString(
                    wc.DownloadData($"http://{Constans.ServerIP}:9024/Getstuff")
                );

                Stuff data = JsonConvert.DeserializeObject<Stuff>(json);
                Teamnames.Text = "";
                Teamnames.Text += Environment.NewLine + "Team Members:" + Environment.NewLine;

                foreach (var member in data.Team)
                    Teamnames.Text += "•" + FilterBadwords(member) + Environment.NewLine;

                Patreonnames.Text = "";
                Patreonnames.Text += Environment.NewLine + "Patreons:" + Environment.NewLine;
                foreach (var supporter in data.Patreons)
                    Patreonnames.Text += "•" + FilterBadwords(supporter) + Environment.NewLine;

                Boosternames.Text = "";
                Boosternames.Text += Environment.NewLine + "Discord Booster:" + Environment.NewLine;
                foreach (var supporter in data.Booster)
                    Boosternames.Text += "•" + FilterBadwords(supporter) + Environment.NewLine;
            }
        }

        /// <summary>
        /// Filters bad words from the input string by replacing them with "***".
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private string FilterBadwords(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;

            string filtered = input;
            foreach (var bad in Constans.Badwords)
            {
                filtered = System.Text.RegularExpressions.Regex.Replace(
                    filtered,
                    "\\b" + System.Text.RegularExpressions.Regex.Escape(bad) + "\\b",
                    "***",
                    System.Text.RegularExpressions.RegexOptions.IgnoreCase
                );
            }
            return filtered;
        }

        /// <summary>
        /// Event handler to open the project's GitHub page.
        /// </summary>
        private void OpenGit(object sender, RoutedEventArgs e)
        {
            OpenWebsite("https://github.com/Project-Paradise-2");
        }

        /// <summary>
        /// Event handler to open the project's official website.
        /// </summary>
        private void OpenWebsite(object sender, RoutedEventArgs e)
        {
            OpenWebsite("https://project-paradise2.de/");
        }

        /// <summary>
        /// Event handler to open the project's Facebook page.
        /// </summary>
        private void OpenFacebook(object sender, RoutedEventArgs e)
        {
            OpenWebsite("https://www.facebook.com/tdupp2");
        }

        /// <summary>
        /// Event handler to open the project's YouTube channel.
        /// </summary>
        private void OpenYoutube(object sender, RoutedEventArgs e)
        {
            OpenWebsite("https://www.youtube.com/@ProjectParadise2");
        }

        /// <summary>
        /// Event handler to open the project's Twitter page.
        /// </summary>
        private void OpenTwitter(object sender, RoutedEventArgs e)
        {
            OpenWebsite("https://x.com/ProjParadise2");
        }

        private void OpenHelp(object sender, RoutedEventArgs e)
        {
            OpenWebsite("https://nova-appendix-de2.notion.site/TDU-2-Guides-77024b8a3b3b4f518301d9d2ff354773");
        }

        /// <summary>
        /// Opens a website URL in the default browser, after user confirmation.
        /// </summary>
        /// <param name="url">The URL to open.</param>
        private void OpenWebsite(string url)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Process.Start("xdg-open", url);
            }
            else
            {
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            }
        }
    }
}