using KuxiiSoft.Utils.Crashreport;
using ProjectParadise2.Core;
using ProjectParadise2.Core.Log;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace ProjectParadise2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Gets the instance of the MainWindow.
        /// </summary>
        public static MainWindow Instance { get; private set; }

        /// <summary>
        /// Indicates whether the window is currently being dragged.
        /// </summary>
        private bool isDragging = false;

        /// <summary>
        /// Stores the initial mouse position when dragging starts.
        /// </summary>
        private Point mouseStartPosition;

        /// <summary>
        /// Initializes a new instance of the MainWindow class and starts a background thread.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Instance = this;
            BackgroundWorker.OnLangset += SetLang;
            SetLang();
            Log.Initlog();
            Thread worker = new Thread(BackgroundWorker.ReadArgs);
            worker.IsBackground = true;
            worker.Start();
            this.Playername.Visibility = Visibility.Hidden;
            this.UserAvatar.Visibility = Visibility.Hidden;
            DoWork();
        }

        private void SetLang(object sender, EventArgs e)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new Action<object, EventArgs>(SetLang), sender, e);
                return;
            }
            Home.Content = Lang.GetText(1);
            Service.Content = Lang.GetText(2);
            Discovery.Content = Lang.GetText(3);
            Settings.Content = Lang.GetText(4);
            Info.Content = Lang.GetText(5);
            Exit.Content = Lang.GetText(6);
        }

        private void SetLang()
        {
            Home.Content = Lang.GetText(1);
            Service.Content = Lang.GetText(2);
            Discovery.Content = Lang.GetText(3);
            Settings.Content = Lang.GetText(4);
            Info.Content = Lang.GetText(5);
            Exit.Content = Lang.GetText(6);
        }

        /// <summary>
        /// Handles the event when the mouse is pressed down on the window, allowing for drag-and-drop.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The mouse button event data.</param>
        [Obsolete]
        private void OnMouseGetDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

        /// <summary>
        /// Handles the event to close the launcher.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void OnCloseLauncher(object sender, RoutedEventArgs e)
        {
            BackgroundWorker.CloseLauncher();
        }

        /// <summary>
        /// Starts the dragging process when the mouse button is pressed.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The mouse button event data.</param>
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                isDragging = true;
                mouseStartPosition = e.GetPosition(this);
                this.CaptureMouse();
            }
        }

        /// <summary>
        /// Updates the position of the window while dragging.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The mouse event data.</param>
        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                var currentPosition = e.GetPosition(null);
                var offset = new Vector(currentPosition.X - mouseStartPosition.X, currentPosition.Y - mouseStartPosition.Y);
                this.Left += offset.X;
                this.Top += offset.Y;
            }
        }

        /// <summary>
        /// Ends the dragging process when the mouse button is released.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The mouse button event data.</param>
        private void Window_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (isDragging && e.ChangedButton == MouseButton.Left)
            {
                isDragging = false;
                this.ReleaseMouseCapture();
            }
        }

        /// <summary>
        /// Executes a background task and updates the UI accordingly.
        /// </summary>
        /// <param name="state">Indicates the state for the task.</param>
        public static void DoWork(bool state = false)
        {
            try
            {
                if (!Instance.Dispatcher.CheckAccess())
                {
                    Instance.Dispatcher.Invoke(new Action<bool>(DoWork), state);
                    return;
                }
                Instance.LoadImageFromUrl(DiscordIntegration.UserAvatar);
                Instance.Working.Visibility = Utils.GetVisibility(state);
                Instance.SetLang();
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Loads an image from a URL and updates the UI with user information.
        /// </summary>
        /// <param name="url">The URL of the image to load.</param>
        public void LoadImageFromUrl(string url)
        {
            try
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(url, UriKind.Absolute);
                bitmap.EndInit();
                this.UserAvatar.Source = bitmap;
                this.Playername.Text = DiscordIntegration.DisplayName;
                this.Playername.Visibility = Visibility.Visible;
                this.UserAvatar.Visibility = Visibility.Visible;
            }
            catch (Exception)
            {
                this.Playername.Visibility = Visibility.Hidden;
                this.UserAvatar.Visibility = Visibility.Hidden;
            }
        }

    }
}