namespace ProjectParadise2.Views
{
    /// <summary>
    /// ViewModel class responsible for managing and navigating between different views in the application.
    /// </summary>
    internal class MainViewModel : ObservableObject
    {
        /// <summary>
        /// Static instance of the MainViewModel.
        /// </summary>
        public static MainViewModel Instance;

        /// <summary>
        /// Commands for navigating to different views.
        /// </summary>
        public RelayCommand HomeViewCommand { get; set; }
        public RelayCommand InfoViewCommand { get; set; }
        public RelayCommand SettingsViewCommand { get; set; }
        public RelayCommand UpdateViewCommand { get; set; }
        public RelayCommand ModViewCommand { get; set; }
        public RelayCommand ServiceViewCommand { get; set; }
        public RelayCommand ModDetailViewCommand { get; set; }
        public RelayCommand InstalledModViewCommand { get; set; }
        public RelayCommand AboutViewCommand { get; set; }
        public RelayCommand LauncherProfilesViewCommand { get; set; }

        public RelayCommand CreateProfilViewCommand { get; set; }

        public RelayCommand EditProfileCommand { get; }
        public RelayCommand DeleteProfileCommand { get; }


        /// <summary>
        /// ViewModel instances for different views.
        /// </summary>
        public HomeViewModel HomeVM { get; private set; }
        public InfoViewModel InfoVM { get; private set; }
        public SettingsViewModel SettingsVM { get; private set; }
        public UpdateViewModel UpdateVM { get; private set; }
        public ModViewModel ModVM { get; private set; }
        public ServiceViewModel ServiceViewVM { get; private set; }
        public ModDeatilModel ModDeatilVM { get; private set; }
        public InstalledModModel InstalledModVM { get; private set; }
        public LauncherUpdateViewModel LauncherVM { get; private set; }
        public AboutViewModel AboutVM { get; private set; }
        public LauncherProfilesViewModel LauncherProfilesVM { get; private set; }

        public CreateProfileViewModel CreateProfileVM { get; private set; }

        /// <summary>
        /// View instances for different views.
        /// </summary>
        public static HomeView HomeView { get; set; }
        public static InfoView InfoView { get; set; }
        public static SettingsView Settings { get; set; }
        public static UpdateView UpdateView { get; set; }
        public static ModView ModView { get; set; }
        public static ServiceView ServiceView { get; set; }
        public static ModDetail ModDetailView { get; set; }
        public static InstalledMods InstalledModsView { get; set; }
        public static LauncherUpdate LauncherUpdateVM { get; set; }
        public static About AboutView { get; set; }
        public static CreateProfileView CreateProfileView { get; set; }
        public static LauncherProfilesView LauncherProfilesView { get; set; }

        private object _currentView;

        /// <summary>
        /// The currently active view.
        /// </summary>
        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Initializes the MainViewModel and sets up the commands for view navigation.
        /// </summary>
        public MainViewModel()
        {
            Instance = this;
            /// Initialize the ViewModel instances **Main View**
            HomeVM = new HomeViewModel();
            /// Initialize the ViewModel instances **Hardware View**
            InfoVM = new InfoViewModel();
            /// Initialize the ViewModel instances **Settings View**
            SettingsVM = new SettingsViewModel();
            /// Initialize the ViewModel instances **Update Game View**
            UpdateVM = new UpdateViewModel();
            /// Initialize the ViewModel instances **Mod View**
            ModVM = new ModViewModel();
            /// Initialize the ViewModel instances **Installed Mod View**
            InstalledModVM = new InstalledModModel();
            /// Initialize the ViewModel instances **Mod Detail View**
            ModDeatilVM = new ModDeatilModel();
            /// Initialize the ViewModel instances **Service View**
            ServiceViewVM = new ServiceViewModel();
            /// Initialize the ViewModel instances **Launcher Update View**
            LauncherVM = new LauncherUpdateViewModel();
            /// Initialize the ViewModel instances **About View**
            AboutVM = new AboutViewModel();
            /// Initialize the ViewModel instances **Create Profile View**
            CreateProfileVM = new CreateProfileViewModel();
            /// Initialize the ViewModel instances **Launcher Profiles View**
            LauncherProfilesVM = new LauncherProfilesViewModel();
            CurrentView = HomeVM;

            // Initialize the commands for view navigation
            HomeViewCommand = new RelayCommand(o =>
            {
                CurrentView = HomeVM;
            });

            LauncherProfilesViewCommand = new RelayCommand(o =>
            {
                CurrentView = LauncherProfilesVM;
            });

            InfoViewCommand = new RelayCommand(o =>
            {
                CurrentView = InfoVM;
                InfoVM.Refresh();
            });

            SettingsViewCommand = new RelayCommand(o =>
            {
                CurrentView = SettingsVM;
                SettingsVM.Refresh();
            });

            UpdateViewCommand = new RelayCommand(o =>
            {
                CurrentView = UpdateVM;
            });

            ModViewCommand = new RelayCommand(o =>
            {
                CurrentView = ModVM;
            });

            AboutViewCommand = new RelayCommand(o =>
            {
                CurrentView = AboutVM;
            });

            ServiceViewCommand = new RelayCommand(o =>
            {
                CurrentView = ServiceViewVM;
            });

            ModDetailViewCommand = new RelayCommand(o =>
            {
                CurrentView = ModDeatilVM;
            });

            InstalledModViewCommand = new RelayCommand(o =>
            {
                CurrentView = InstalledModVM;
            });

            CreateProfilViewCommand = new RelayCommand(o =>
            {
                CurrentView = CreateProfileVM;
            });

            EditProfileCommand = new RelayCommand(o =>
            {
                if (o is GameProfile profile)
                {
                    EditProfile(profile);
                }
            });

            DeleteProfileCommand = new RelayCommand(o =>
            {
                if (o is GameProfile profile)
                {
                    DeleteProfile(profile);
                }
            });
        }

        public void EditProfile(GameProfile profile)
        {
        }

        private void DeleteProfile(GameProfile profile)
        {
        }

        /// <summary>
        /// Switches the current view to the Update view.
        /// </summary>
        public static void GetUpdateView()
        {
            if (Instance != null)
                Instance.CurrentView = Instance.UpdateVM;
        }

        public static void GetMainview()
        {

            if (Instance != null)
                Instance.CurrentView = Instance.HomeVM;
        }

        public static void OpenLauncherupdate()
        {

            if (Instance != null)
                Instance.CurrentView = Instance.LauncherVM;
        }

        /// <summary>
        /// Opens the Mod browser view.
        /// </summary>
        public static void OpenModbrowser()
        {
            if (MainViewModel.Instance != null)
            {
                Instance.CurrentView = MainViewModel.Instance.ModVM;
            }
        }

        /// <summary>
        /// Opens the Mod information view.
        /// </summary>
        public static void OpenModInfo()
        {
            if (MainViewModel.Instance != null)
            {
                Instance.CurrentView = MainViewModel.Instance.ModDeatilVM;
            }
        }

        /// <summary>
        /// Opens the Mod manager view.
        /// </summary>
        public static void OpenModManager()
        {
            if (MainViewModel.Instance != null)
            {
                Instance.CurrentView = MainViewModel.Instance.InstalledModVM;
            }
        }

        public static void OpenCreateProfile()
        {
            if (MainViewModel.Instance != null)
            {
                Instance.CurrentView = MainViewModel.Instance.CreateProfileVM;
            }
        }

        public static void OpenCreateProfile(CreateProfileView view)
        {
            if (MainViewModel.Instance != null)
            {
                Instance.CurrentView = view;
            }
        }

        public static void OpenLauncherProfiles()
        {
            if (MainViewModel.Instance != null)
            {
                Instance.CurrentView = MainViewModel.Instance.LauncherProfilesVM;
            }
        }
    }
}