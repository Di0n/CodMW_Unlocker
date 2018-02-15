using Microsoft.Win32;
using System;
using System.IO;

namespace CodMW_Unlocker
{
    class Program
    {
        private const string KEY_NAME = "codkey";
        private const string REG_PATH = "SOFTWARE\\Activision\\Call of Duty 4";
        private const string CD_KEY = "DL2J8PY44Q22GE4888D2";
        static int Main(string[] args)
        {
            string profilePath = string.Empty;
            if (args.Length == 0)
            {
                Console.WriteLine("No arguments found.");
                Console.Write("Enter your profile path: ");
                profilePath = Console.ReadLine();
            }
            else profilePath = args[0];

            if (!Directory.Exists(profilePath) || !profilePath.Contains("profiles"))
            {
                Console.WriteLine("Invalid path.");
                Console.ReadLine();
                return 1;
            }

            Console.Clear();

            Console.Write("Start? Y/N");
            string answer = Console.ReadLine().ToUpper();

            if (answer != "Y") return 0;

            Console.WriteLine("Editing registry...");

            bool regSuccess = false;
            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(REG_PATH, true))
                    if (key != null)
                    {
                        key.SetValue(KEY_NAME, CD_KEY, RegistryValueKind.String);
                        regSuccess = true;
                    }
            }
            catch (Exception) { }

            if (regSuccess)
            {
                Console.WriteLine("Successfully edited Registry");

                DirectoryInfo dir = new DirectoryInfo(profilePath);

                Console.WriteLine("Deleting old configs...");
                foreach (FileInfo file in dir.GetFiles())
                    file.Delete();

                foreach (DirectoryInfo subDir in dir.GetDirectories())
                    subDir.Delete(true);

                int lastSlash = profilePath.LastIndexOf('\\');

                Console.WriteLine("Writing new mpdata...");
                File.WriteAllBytes(profilePath + ((profilePath.Length == lastSlash) ? "" : "\\") + nameof(Properties.Resources.mpdata), Properties.Resources.mpdata);
                Console.WriteLine("New mpdata written.");
            }
            else
            {
                Console.WriteLine("Failed to edit registry");
                Console.ReadLine();
                return 1;
            }

            Console.WriteLine("Done.");
            Console.ReadLine();
            return 0;
        }
    }
}








//try
//{
//    using (RegistryKey localMachine = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
//    using (RegistryKey key = localMachine.OpenSubKey(PATH_32, true))
//        if (key != null)
//        {
//            key.SetValue(KEY_NAME, CD_KEY, RegistryValueKind.String);
//            count++;
//        }

//    Console.WriteLine("Edited 32 Registry path");
//}
//catch (Exception)
//{
//    Console.WriteLine("Could not edit 32 Registry path");
//}

//try
//{ // Hive 32
//    using (RegistryKey key = Registry.LocalMachine.OpenSubKey(PATH_64, true))
//        if (key != null)
//        {
//            key.SetValue(KEY_NAME, CD_KEY, RegistryValueKind.String);
//            count++;
//        }
//    Console.WriteLine("Edited 64 Registry path");
//}
//catch (Exception)
//{
//    Console.WriteLine("Could not edit 64 Registry path");
//}