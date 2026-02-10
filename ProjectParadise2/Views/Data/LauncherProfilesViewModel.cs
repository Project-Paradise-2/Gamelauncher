using ProjectParadise2.Core;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;

namespace ProjectParadise2.Views
{
    internal class LauncherProfilesViewModel : ObservableObject
    {
        public ObservableCollection<GameProfile> GameProfiles { get; }

        public ICommand EditProfileCommand { get; }
        public ICommand DeleteProfileCommand { get; }

        public LauncherProfilesViewModel()
        {
            // List -> ObservableCollection
            GameProfiles = new ObservableCollection<GameProfile>(BackgroundWorker.GameProfiles);

            EditProfileCommand = new RelayCommand(o =>
            {
                if (o is GameProfile profile)
                    EditProfile(profile);
            });

            DeleteProfileCommand = new RelayCommand(o =>
            {
                if (o is GameProfile profile)
                    DeleteProfile(profile);
            });
        }

        private void EditProfile(GameProfile profile)
        {
            var view = new CreateProfileView();
            view.SetInfos(profile);
            MainViewModel.OpenCreateProfile(view);
        }

        private void DeleteProfile(GameProfile profile)
        {
            if (File.Exists(Constans.DokumentsFolder + "/GameProfiles/" + profile.Profilename + ".profile"))
            {
                File.Delete(Constans.DokumentsFolder + "/GameProfiles/" + profile.Profilename + ".profile");
            }

            if (GameProfiles.Contains(profile))
                GameProfiles.Remove(profile);

            BackgroundWorker.RefreshProfiles();
        }
    }
}