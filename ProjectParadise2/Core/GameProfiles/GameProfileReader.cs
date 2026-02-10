using System;
using System.Collections.Generic;
using System.IO;

namespace ProjectParadise2.Core.GameProfiles
{
    class GameProfileReader
    {
        public static List<GameProfile> ReadGameProfiles()
        {
            List<GameProfile> gameProfiles = new List<GameProfile>();

            string folderPath = Constans.DokumentsFolder + "/GameProfiles/";

            if (!Directory.Exists(folderPath))
            {
                try
                {
                    Directory.CreateDirectory(folderPath);
                    Log.Log.Info("Created directory: " + folderPath);
                }
                catch (Exception ex)
                {
                    Log.Log.Error("Failed to create directory: " + folderPath, ex);
                    return gameProfiles; // Return empty list if creation fails
                }
            }
            else
            {
                var files = Directory.GetFiles(folderPath, "*.profile");
                foreach (var item in files)
                {
                    try
                    {
                        string json = File.ReadAllText(item);
                        GameProfile profile = Newtonsoft.Json.JsonConvert.DeserializeObject<GameProfile>(json);
                        gameProfiles.Add(profile);
                    }
                    catch (Newtonsoft.Json.JsonException ex)
                    {
                        Log.Log.Error("JSON deserialization failed for file: " + item, ex);
                    }
                    catch (Exception ex)
                    {
                        Log.Log.Error("Unexpected error reading file: " + item, ex);
                    }
                }
            }

            return gameProfiles;
        }
    }
}
