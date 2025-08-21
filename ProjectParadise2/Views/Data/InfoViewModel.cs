using System.Threading.Tasks;

namespace ProjectParadise2.Views
{
    class InfoViewModel : ObservableObject
    {
        public void Refresh()
        {
            if (InfoView.Instance != null)
            {
                Task.Run(() =>
                {
                    InfoView.Instance.Refresh();
                });
            }
        }
    }
}
