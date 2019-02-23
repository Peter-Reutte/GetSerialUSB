using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace GetSerialUSB
{
    class Program
    {
        static void Main(string[] args)
        {
            // DriveInfo[] info = DriveInfo.GetDrives();
            foreach (ManagementObject drive in new ManagementObjectSearcher("select * from Win32_DiskDrive where InterfaceType='USB'").Get())
            {
                ManagementObject theSerialNumberObjectQuery = new ManagementObject("Win32_PhysicalMedia.Tag='" + drive["DeviceID"] + "'");
                foreach (ManagementObject partition in new ManagementObjectSearcher("ASSOCIATORS OF {Win32_DiskDrive.DeviceID='" + drive["DeviceID"] + "'} WHERE AssocClass = Win32_DiskDriveToDiskPartition").Get())
                {
                    foreach (ManagementObject disk in new ManagementObjectSearcher("ASSOCIATORS OF {Win32_DiskPartition.DeviceID='"
                          + partition["DeviceID"]
                          + "'} WHERE AssocClass = Win32_LogicalDiskToPartition").Get())
                    {
                        Console.WriteLine(disk["Name"].ToString() + "\t" + theSerialNumberObjectQuery["SerialNumber"].ToString());
                    }
                }
            }
            //Console.ReadKey(true);
            //Console.ReadKey();
            Console.ReadLine();
        }
    }
}