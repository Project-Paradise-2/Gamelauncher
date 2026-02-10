using ProjectParadise2.Gameupdate;
using System;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace ProjectParadise2.Views
{
    /// <summary>
    /// Interaction logic for UpdateView.xaml.
    /// This view is responsible for displaying the update process, 
    /// including the progress bars and status messages for file verification, download, and installation.
    /// </summary>
    public partial class UpdateView : UserControl
    {
        public static UpdateView Instance { get; private set; }

        /// <summary>
        /// Constructor for the UpdateView. Initializes components, sets up the initial progress, 
        /// and starts a background thread to begin the file check process.
        /// </summary>
        public UpdateView()
        {
            InitializeComponent();
            Instance = this;
            this.TotalProgress.Value = 0;
            this.DownloadProgress.Value = 0;
            this.DownloadProgressText.Text = "";
            this.StatusText.Text = "Starting game file check. This might take a while. Grab a coffee and relax while the launcher works.";
            // Start a new thread to perform the file check.
            var check = new Thread(Filecheck.StartCheck);
            Thread.Sleep(200);
            check.Start();
        }

        /// <summary>
        /// Updates the status text displayed in the view.
        /// This method can be called from any thread and ensures the UI is updated on the UI thread.
        /// </summary>
        /// <param name="status">The status message to be displayed.</param>
        public void Updatestatus(string status)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new Action<string>(Updatestatus), status);
                return;
            }
            this.StatusText.Text = status;
        }

        /// <summary>
        /// Updates the total progress bar and the message text. 
        /// This method can be called from any thread and ensures the UI is updated on the UI thread.
        /// </summary>
        /// <param name="totalSteps">The total number of steps in the process.</param>
        /// <param name="currentStep">The current step being processed.</param>
        /// <param name="message">The message to display alongside the progress.</param>
        public void UpdateProgress(int totalSteps, int currentStep, string message)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new Action<int, int, string>(UpdateProgress), totalSteps, currentStep, message);
                return;
            }
            int progress = 0;
            // Ensure that totalSteps is valid to avoid division by zero.
            if (totalSteps > 0)
            {
                progress = Math.Min((currentStep * 100) / totalSteps, 100);
            }
            // Update the total progress bar and display the corresponding message.
            TotalProgress.Value = progress;
            DownloadProgressText.Text = message;
        }

        /// <summary>
        /// Updates the download progress bar and related text (current state, speed, and details).
        /// This method can be called from any thread and ensures the UI is updated on the UI thread.
        /// </summary>
        /// <param name="value">The current download progress.</param>
        /// <param name="maximal">The maximum value of the download progress (total size).</param>
        /// <param name="state">The state message to display about the download.</param>
        /// <param name="state2">Additional state information (e.g., download details).</param>
        /// <param name="speed">The download speed in a readable format.</param>
        public void WriteCurrentDownload(double value, double maximal, string state, string state2, string speed)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new Action<double, double, string, string, string>(WriteCurrentDownload), value, maximal, state, state2, speed);
                return;
            }
            // Update the download progress bar and text.
            DownloadProgress.Value = value;
            DownloadProgress.Maximum = maximal;
            DownloadProgressText.Text = state;
            StatusText.Text = "Status: " + state2 + " at ↓" + speed;
        }

        /// <summary>
        /// Appends a message to the console textbox, with optional color formatting based on the message type.
        /// This method can be called from any thread and ensures the UI is updated on the UI thread.
        /// </summary>
        /// <param name="message">The message to display.</param>
        /// <param name="messagetype">The type of the message (e.g., neutral, OK, error, etc.).</param>
        public void PrintMessage(string message, int messagetype = 5)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new Action<string, int>(PrintMessage), message, messagetype);
                return;
            }

            TextBlock tb = new TextBlock();
            switch (messagetype)
            {
                case 0: // Neutral
                    tb.Inlines.Add(new Run(message) { Foreground = Brushes.Orange });
                    break;
                case 1: // OK
                    tb.Inlines.Add(new Run(message) { Foreground = Brushes.Green });
                    break;
                case 2: // Error
                    tb.Inlines.Add(new Run(message) { Foreground = Brushes.Red });
                    break;
                case 3: // Dark Green
                    tb.Inlines.Add(new Run(message) { Foreground = Brushes.DarkGreen });
                    break;
                case 4: // Dark Red
                    tb.Inlines.Add(new Run(message) { Foreground = Brushes.DarkRed });
                    break;
                case 5: // White (default)
                    tb.Inlines.Add(new Run(message) { Foreground = Brushes.Wheat });
                    break;
            }

            // Add the message to the console and scroll to the bottom.
            ConsoleTextbox.Inlines.Add(tb);
            ConsoleTextbox.Inlines.Add("\n");
            ScrollView.ScrollToEnd();
        }

        public void HideStatus(int state)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new Action<int>(HideStatus), state);
                return;
            }
            if (state == 0)
            {
                StatusText.Visibility = System.Windows.Visibility.Hidden;
            }
            else
            {
                StatusText.Visibility = System.Windows.Visibility.Visible;
            }
        }
    }
}