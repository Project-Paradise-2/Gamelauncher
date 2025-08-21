using HardwareInformation;
using HardwareInformation.Information;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ProjectParadise2.Core.JsonClasses
{
    [Serializable]
    public enum OsType
    {
        Win10orWin11,
        Win7,
        Win8orWin8point1,
        Unknown,
    }

    [Serializable]
    public class GPUInfo
    {
        /// <summary>
        /// AMD / Intel
        /// </summary>
        public string Vendor { get; set; }
        /// <summary>
        /// GPU Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// By AMD GPU Name
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Driver Date
        /// </summary>
        public string DriverDate { get; set; }
        /// <summary>
        /// Driver Version
        /// </summary>
        public string DriverVersion { get; set; }
        public string Type { get; set; }
        public string Memory { get; set; }

        public GPUInfo(GPU gPU)
        {
            Vendor = gPU.Vendor;
            Name = gPU.Name;
            Description = gPU.Description;
            DriverDate = gPU.DriverDate;
            DriverVersion = gPU.DriverVersion;
            Type = gPU.Type.ToString();
            Memory = Utils.FormatBytes((long)gPU.AvailableVideoMemory);
        }
    }

    [Serializable]
    public class DeviceInfo
    {
        public string TotalMemory => Utils.FormatBytes((long)Memory);
        private ulong Memory { get; set; } = 0;
        public uint MemorySpeed { get; set; }
        public int RamSticks { get; set; }

        public string CPUVendor { get; set; }
        public string CPUName { get; set; }
        public string Cores { get; set; }
        public string Caption { get; set; }
        public List<GPUInfo> GPUInfos { get; set; } = new List<GPUInfo>();

        public string Platform { get; set; }
        public string OsType { get; set; }
        public string osType { get; set; }

        public string GetGPU(int Index)
        {
            try
            {
                return $"Gpu [{Index + 1}]({GPUInfos[Index].Type}) {GPUInfos[Index].Vendor} {GPUInfos[Index].Name} Ram:{GPUInfos[Index].Memory} Driver:{GPUInfos[Index].DriverVersion} from: {GPUInfos[Index].DriverDate}";
            }
            catch (Exception e)
            {
                Log.Log.Print("Failed get Hardware infos from DLL: ", e);
                return "Unable read GPU infos";
            }
        }

        public string GetGpu()
        {
            string data = "";
            try
            {
                for (int i = 0; i < GPUInfos.Count; i++)
                {
                    data += $"{GetGPU(i)}\n";
                }
            }
            catch (Exception e)
            {
                Log.Log.Print("Failed get Hardware infos from DLL: ", e);
                return "Unable read GPU infos";
            }
            return data;
        }



        public DeviceInfo()
        {
            try
            {
                MachineInformation info = MachineInformationGatherer.GatherInformation(true);

                try
                {
                    foreach (var item in info.RAMSticks)
                    {
                        Memory += item.Capacity;
                        RamSticks++;
                        MemorySpeed = item.Speed;
                    }
                }
                catch (Exception e)
                {
                    Log.Log.Print("Failed get RAM infos from DLL: ", e);
                }

                CPUVendor = info.Cpu.Vendor;
                CPUName = info.Cpu.Name.TrimEnd();
                Cores = info.Cpu.PhysicalCores + "c/" + info.Cpu.LogicalCores + "t";
                Caption = info.Cpu.Caption;

                try
                {
                    foreach (var item in info.Gpus)
                    {
                        GPUInfos.Add(new GPUInfo(item));
                    }
                }
                catch (Exception e)
                {
                    Log.Log.Print("Failed get GPU infos from DLL: ", e);
                }
                Platform = info.Platform.ToString();
                OsType = info.OperatingSystem.Version.ToString();
            }
            catch (Exception e)
            {
                Log.Log.Print("Failed get Hardware infos from DLL: ", e);
            }

            try
            {

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    string osDescription = RuntimeInformation.OSDescription;

                    if (osDescription.Contains("10.0."))
                    {
                        osType = JsonClasses.OsType.Win10orWin11.ToString().Replace("or", " | ");
                    }
                    else if (osDescription.Contains("6.1"))
                    {
                        osType = JsonClasses.OsType.Win7.ToString();
                    }
                    else if (osDescription.Contains("6.2") || osDescription.Contains("6.3"))
                    {
                        osType = JsonClasses.OsType.Win8orWin8point1.ToString().Replace("or", " | ").Replace("point", ".");
                    }
                    else
                    {
                        osType = JsonClasses.OsType.Unknown.ToString();
                    }
                }
            }
            catch (Exception e)
            {
                Log.Log.Print("Failed get Os infos from Framework: ", e);
            }
        }

        public override string ToString()
        {
            return $"---- System Info Report ----\n" +
                $"Cpu: {CPUVendor} {CPUName} {Cores} {Caption}\n" +
                $"Ram: ({RamSticks}) {TotalMemory} @{MemorySpeed}Mhz\n" +
                $"Gpu: {GPUInfos.Count}\n" +
                $"{GetGpu()}\n" +
                $"Platform: {Platform} {System.Runtime.InteropServices.RuntimeInformation.OSArchitecture}\n" +
                $"OS: {System.Runtime.InteropServices.RuntimeInformation.OSDescription} {OsType}\n" +
                $"{osType}";
        }
    }
}
