using Newtonsoft.Json;
using ProjectParadise2.Core;
using ProjectParadise2.Core.JsonClasses;
using System;

namespace ProjectParadise2.Views
{
    internal class HomeViewModel : ObservableObject
    {
        public static State CheckState()
        {
            try
            {
                using (WebConnection wc = new WebConnection())
                {
                    wc.Timeout = 6;
                    var data = JsonConvert.DeserializeObject<State>(System.Text.Encoding.UTF8.GetString(wc.DownloadData("http://194.164.199.240:8890/getwebsite")));
                    return data;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
