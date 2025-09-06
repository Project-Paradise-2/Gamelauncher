using ProjectParadise2.Core.Log;
using ProjectParadise2.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ProjectParadise2
{
    internal class Utils
    {
        private static readonly Dictionary<string, string> CountryRegionMapping = new Dictionary<string, string>
        {
            // Europe (EU)
            { "RU", "EU" },    // Russia
            { "DE", "EU" },    // Germany
            { "FR", "EU" },    // France
            { "IT", "EU" },    // Italy
            { "GB", "EU" },    // United Kingdom
            { "ES", "EU" },    // Spain
            { "PL", "EU" },    // Poland
            { "SE", "EU" },    // Sweden
            { "NL", "EU" },    // Netherlands
            { "BE", "EU" },    // Belgium
            { "FI", "EU" },    // Finland
            { "AT", "EU" },    // Austria
            { "CH", "EU" },    // Switzerland
            { "NO", "EU" },    // Norway
            { "DK", "EU" },    // Denmark
            { "PT", "EU" },    // Portugal
            { "GR", "EU" },    // Greece
            { "HU", "EU" },    // Hungary
            { "CZ", "EU" },    // Czech Republic
            { "RO", "EU" },    // Romania
            { "BG", "EU" },    // Bulgaria
            { "SK", "EU" },    // Slovakia
            { "HR", "EU" },    // Croatia
            { "SI", "EU" },    // Slovenia
            { "EE", "EU" },    // Estonia
            { "LT", "EU" },    // Lithuania
            { "LV", "EU" },    // Latvia
            { "CY", "EU" },    // Cyprus
            { "IS", "EU" },    // Iceland
        
            // North America (NA)
            { "US", "NA" },    // United States
            { "CA", "NA" },    // Canada
            { "MX", "NA" },    // Mexico
        
            // South America (SA)
            { "BR", "SA" },    // Brazil
            { "AR", "SA" },    // Argentina
            { "CL", "SA" },    // Chile
            { "CO", "SA" },    // Colombia
            { "PE", "SA" },    // Peru
            { "VE", "SA" },    // Venezuela
            { "EC", "SA" },    // Ecuador
            { "UY", "SA" },    // Uruguay
            { "PY", "SA" },    // Paraguay
            { "BO", "SA" },    // Bolivia
            { "SR", "SA" },    // Suriname
            { "GY", "SA" },    // Guyana
        
            // Asia Pacific (APAC)
            { "IN", "APAC" },  // India
            { "CN", "APAC" },  // China
            { "JP", "APAC" },  // Japan
            { "KR", "APAC" },  // South Korea
            { "ID", "APAC" },  // Indonesia
            { "PH", "APAC" },  // Philippines
            { "SG", "APAC" },  // Singapore
            { "TH", "APAC" },  // Thailand
            { "MY", "APAC" },  // Malaysia
            { "VN", "APAC" },  // Vietnam
            { "PK", "APAC" },  // Pakistan
            { "BD", "APAC" },  // Bangladesh
            { "MM", "APAC" },  // Myanmar
            { "LK", "APAC" },  // Sri Lanka
            { "KH", "APAC" },  // Cambodia
            { "NP", "APAC" },  // Nepal
            { "LA", "APAC" },  // Laos
            { "MN", "APAC" },  // Mongolia
        
            // Africa (AF)
            { "ZA", "AF" },    // South Africa
            { "NG", "AF" },    // Nigeria
            { "EG", "AF" },    // Egypt
            { "KE", "AF" },    // Kenya
            { "MA", "AF" },    // Morocco
            { "DZ", "AF" },    // Algeria
            { "ET", "AF" },    // Ethiopia
            { "GH", "AF" },    // Ghana
            { "UG", "AF" },    // Uganda
            { "CI", "AF" },    // Ivory Coast
            { "CM", "AF" },    // Cameroon
            { "SN", "AF" },    // Senegal
            { "TN", "AF" },    // Tunisia
            { "ZW", "AF" },    // Zimbabwe
            { "MZ", "AF" },    // Mozambique
            { "AO", "AF" },    // Angola
        
            // Oceania (OCE)
            { "AU", "OCE" },   // Australia
            { "NZ", "OCE" },   // New Zealand
        
            // Middle East (ME)
            { "SA", "ME" },    // Saudi Arabia
            { "AE", "ME" },    // United Arab Emirates
            { "IQ", "ME" },    // Iraq
            { "JO", "ME" },    // Jordan
            { "LB", "ME" },    // Lebanon
            { "SY", "ME" },    // Syria
            { "KW", "ME" },    // Kuwait
            { "OM", "ME" },    // Oman
            { "YE", "ME" },    // Yemen
            { "BH", "ME" },    // Bahrain
            { "QA", "ME" },    // Qatar
        
            // Special Regions (Other regions, such as EU or global groupings)
            { "EU", "EU" },    // European Union (not a country, but used for regions)
            { "APAC", "APAC" }, // Asia Pacific
            { "NA", "NA" },    // North America
            { "LATAM", "LATAM" }, // Latin America
        };

        /// <summary>
        /// Retrieves the region code associated with a given country code.
        /// The method looks up the country code in a predefined mapping and returns
        /// the corresponding region (e.g., "EU" for European countries, "NA" for North America).
        /// If the country code is not found in the mapping, it returns "Unknown".
        /// </summary>
        /// <param name="countryCode">The two-letter ISO country code (e.g., "US", "DE", "IN").</param>
        /// <returns>
        /// The region code associated with the country code (e.g., "EU", "NA", "APAC"), or "Unknown"
        /// if the country code is not found in the mapping.
        /// </returns>
        public static string GetRegionFromCountryCode(string countryCode)
        {
            if (CountryRegionMapping.TryGetValue(countryCode.ToUpper(), out string region))
            {
                return region;
            }
            return "Unknown";
        }

        /// <summary>
        /// Ensures that custom server connection entries are added to the system's hosts file.
        /// This method checks for the presence of the hosts file and verifies if the required 
        /// entries for "testdriveunlimited2.com" and related proxy domains already exist.
        /// If the entries are missing, they are appended to redirect the game's network traffic 
        /// to the custom PP2 servers without modifying game files.
        /// </summary>
        /// <remarks>
        /// - Adds entries for multiple proxy subdomains (pc-proxy1 to pc-proxy8) mapped to the IP "194.164.199.240".
        /// - Prevents redundant entries by scanning the hosts file for existing entries.
        /// - Operates only on Windows systems with an accessible hosts file.
        /// - Exits gracefully if the hosts file is missing, potentially indicating an unsupported platform.
        /// </remarks>
        public static void UpdateConnection()
        {
            try
            {
                string hostsFilePath = @"C:\Windows\System32\drivers\etc\hosts";
                string[] requiredEntries = new[]
                {
                "#",
                "# Custom Connection for PP2 Server",
                "#",
                $"{Constans.ServerIP} testdriveunlimited2.com #Server is Offline, Lets Relay it",
                "#",
                "# Server Proxys",
                "#",
                $"{Constans.ServerIP} pc-proxy1.testdriveunlimited2.com #Set Custom Connection to PP2-Server without Modify Gamefiles",
                $"{Constans.ServerIP} pc-proxy2.testdriveunlimited2.com #Set Custom Connection to PP2-Server without Modify Gamefiles",
                $"{Constans.ServerIP} pc-proxy3.testdriveunlimited2.com #Set Custom Connection to PP2-Server without Modify Gamefiles",
                $"{Constans.ServerIP} pc-proxy4.testdriveunlimited2.com #Set Custom Connection to PP2-Server without Modify Gamefiles",
                $"{Constans.ServerIP} pc-proxy5.testdriveunlimited2.com #Set Custom Connection to PP2-Server without Modify Gamefiles",
                $"{Constans.ServerIP} pc-proxy6.testdriveunlimited2.com #Set Custom Connection to PP2-Server without Modify Gamefiles",
                $"{Constans.ServerIP} pc-proxy7.testdriveunlimited2.com #Set Custom Connection to PP2-Server without Modify Gamefiles",
                $"{Constans.ServerIP} pc-proxy8.testdriveunlimited2.com #Set Custom Connection to PP2-Server without Modify Gamefiles"
            };

                if (File.Exists(hostsFilePath))
                {
                    var currentLines = new HashSet<string>(File.ReadAllLines(hostsFilePath));

                    foreach (string entry in requiredEntries)
                    {
                        if (!currentLines.Contains(entry))
                        {
                            File.AppendAllText(hostsFilePath, "\n" + entry);
                            Log.Info("Add host entry: " + entry);
                        }
                    }
                }
                else
                {
                    ///Idk is it Working on Linux with Wine/Prothon
                    MessageBox.Show("The server connection patch was unsuccessful. \nYou should manually set the connection after the game update.", "Project Paradise 2", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("The server connection patch was unsuccessful. \nYou should manually set the connection after the game update.\n error:" + ex.Message, "Project Paradise 2", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// Formats the given byte value into a human-readable string representation with appropriate units (B, KB, MB, etc.).
        /// </summary>
        /// <param name="bytes">The size in bytes to be formatted.</param>
        /// <returns>A string representing the size with the appropriate unit.</returns>
        public static string FormatBytes(long bytes)
        {
            string[] Suffix = { "B", "KB", "MB", "GB", "TB" };
            int i;
            double dblSByte = bytes;
            for (i = 0; i < Suffix.Length && bytes >= 1024; i++, bytes /= 1024)
            {
                dblSByte = bytes / 1024.0;
            }
            return String.Format("{000:0.##} {1}", dblSByte, Suffix[i]);
        }

        /// <summary>
        /// Retrieves the name of the calling class and method, useful for logging and debugging purposes.
        /// </summary>
        /// <returns>A string representing the calling class and method name.</returns>
        public static string NameOfCallingClass()
        {
            StackTrace stackTrace = new StackTrace();
            StackFrame stackFrame = stackTrace.GetFrame(1);
            MethodBase methodBase = stackFrame.GetMethod();

            string message = string.Format(
             System.Globalization.CultureInfo.InvariantCulture,
             "{0}::{1}",
             methodBase.ReflectedType,
             methodBase);
            return message;
        }

        /// <summary>
        /// Converts a boolean state into a corresponding <see cref="Visibility"/> value.
        /// </summary>
        /// <param name="state">The boolean state representing visibility.</param>
        /// <returns><see cref="Visibility.Visible"/> if state is true, otherwise <see cref="Visibility.Hidden"/>.</returns>
        public static Visibility GetVisibility(bool state)
        {
            if (state)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Hidden;
            }
        }

        /// <summary>
        /// Loads an image from a byte array and returns it as a <see cref="BitmapImage"/>.
        /// If loading the image fails, returns a default icon.
        /// </summary>
        /// <param name="imageData">The byte array containing the image data.</param>
        /// <returns>A <see cref="BitmapImage"/> containing the loaded image or a default image if loading fails.</returns>
        public static BitmapImage LoadImage(byte[] imageData)
        {
            try
            {
                if (imageData == null || imageData.Length == 0)
                {
                    return null;
                }

                var image = new BitmapImage();
                using (var mem = new MemoryStream(imageData))
                {
                    mem.Position = 0;
                    image.BeginInit();
                    image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.UriSource = null;
                    image.StreamSource = mem;
                    image.EndInit();
                }
                image.Freeze();
                return image;
            }
            catch (Exception ex)
            {
                var image = new BitmapImage();
                using (var mem = new MemoryStream(Resources.icon))
                {
                    mem.Position = 0;
                    image.BeginInit();
                    image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.UriSource = null;
                    image.StreamSource = mem;
                    image.EndInit();
                }
                image.Freeze();
                Log.Error("Failed Load Mod-Image: " + ex.Message, ex);
                return image;
            }
        }
    }
}