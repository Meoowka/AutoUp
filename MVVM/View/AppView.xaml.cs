using AutoUp.Entities;
using AutoUp;
using AutoUp.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Windows;
using FluentFTP;
using Microsoft.Win32;
using System.Data;
using System.ComponentModel;
using System.Windows.Data;

namespace AutoUp.MVVM.View
{
    /// <summary>
    /// Логика взаимодействия для AppView.xaml
    /// </summary>
    public partial class AppView : UserControl
    {
        public string FilePathLocalFolder = ConfigurationManager.AppSettings.Get("FilePathLocalFolder");
        public string FilePathLocalFolderMS = ConfigurationManager.AppSettings.Get("FilePathLocalFolderMS");
        public string ModuleServer = ConfigurationManager.AppSettings.Get("ModuleServer");
        public string FilePathServer = ConfigurationManager.AppSettings.Get("FilePathServer");
        public string FilePathLocalFolderML = ConfigurationManager.AppSettings.Get("FilePathLocalFolderML");
        public string host = ConfigurationManager.AppSettings.Get("Host");
        public string login = ConfigurationManager.AppSettings.Get("Login");
        public string pass = ConfigurationManager.AppSettings.Get("Pass");
        IModulesProvider remoteProvider;
        IModulesProvider localProvider;
        public AppView()
        {
            InitializeComponent();
            remoteProvider = new FtpModulesProvider(FilePathLocalFolder, FilePathServer, FilePathLocalFolderMS, host, login, pass);
            localProvider = new DirectoryModulesProvider(FilePathLocalFolderML);
        }
        public Module[] ServerFiles = Array.Empty<Module>();
        public Module[] ClientFiles = Array.Empty<Module>();
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            remoteProvider.Connect();
            remoteProvider.Download(ModuleServer);
            ListingServer();
        }


        private void ScanUpdate_Click(object sender, RoutedEventArgs e)
        {

            DataGridApps.Items.Clear();
            remoteProvider.Download(ModuleServer);
            ShowUpdate();
            ListingServer();
        }


        private void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            List<Module> Local = new();

            var files = DataGridApps.SelectedItem as Module;
            string files_r = files?.Name_File.ToString() + files?.Extension;
            if (files != null)
            {
                var result = (FtpStatus)remoteProvider.Download(files_r);
                if (result == FluentFTP.FtpStatus.Success)
                {
                    if (MessageBox.Show("Приложение скачено", "хотите установить", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        Process.Start(FilePathLocalFolder + '\\' + files_r);
                    }
                    else
                    {
                        MessageBox.Show("Чтобы уставновить перейдите во вкладку Мои приложения");
                    }
                }

                Local.Add(files);

                WriteYmlAdd(FilePathLocalFolderML, Local);

                //IsSoftwareInstalled(files.Name_File);
                //var resultReg = IsSoftwareInstalled(files.Name_File);
                //MessageBox.Show(resultReg.ToString());

            }
        }
        public void ListingServer()
        {
            var ListServer = remoteProvider.ListModuleInfo();
            foreach (var item in ListServer)
            {
                DataGridApps.Items.Add(item);
            }
            
        }
        public void WriteYmlAdd(string filePathr, List<Module> ClientFiles)
        {
            bool lineFound = false;
            string tempFile = Path.GetTempFileName();

            using (StreamReader reader = new StreamReader(filePathr))
            using (StreamWriter writer = new StreamWriter(tempFile))
            {
                string line;
                foreach (var item in ClientFiles) {
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line == item.Name_File)
                        {
                            writer.WriteLine(item);
                            lineFound = true;
                        }
                        else
                        {
                            writer.WriteLine(line);
                        }
                    }
                }
            }

            if (!lineFound)
            {
                foreach (var item in ClientFiles)
                {
                    using (StreamWriter writer = new StreamWriter(filePathr, append: true))
                    {
                        writer.WriteLine(item);
                    }
                }
            }
          
            //using (FileStream fs = new FileStream(filePathr, FileMode.Create, FileAccess.Write))
            //using (StreamWriter sw = new StreamWriter(fs))
            //{
            //    foreach (var item in ClientFiles)
            //    {
            //        sw.WriteLine(item);
            //    }
            //}
        }
        public void ShowUpdate()
        {

            var ClientFiles = localProvider.ListModuleInfo();
            var ServerFiles = remoteProvider.ListModuleInfo();

            foreach (var items in ServerFiles)
            {
                if (items != null)
                {
                    var verName = ClientFiles.Where(x => x.Name_File!.Equals(items.Name_File)).Any();
                    if (verName)
                    {
                        var verClient = ClientFiles.ToList().Find(x => x.Name_File.Equals(items.Name_File)).Version;
                        var verServer = items.Version;
                        verClient = String.Join("", verClient.Split("."));
                        verServer = String.Join("", verServer.Split("."));
                        if (Convert.ToInt64(verClient) > Convert.ToInt64(verServer))
                        {
                            MessageBox.Show($"Версия {items.Name_File} актуальная, на клиенте версия новее");
                        }
                        else if (Convert.ToInt64(verClient) < Convert.ToInt64(verServer))
                        {
                            MessageBox.Show($"Версия {items.Name_File} не актуальная, на сервере версия новее");
                        }
                        else
                        {
                            MessageBox.Show($"Версия {items.Name_File} актуальна");
                        }
                    }
                    else
                    {
                        MessageBox.Show($"У Вас нету {items.Name_File}, советуем скачать");
                    }
                }
            }
        }
        public bool IsSoftwareInstalled(string softwareName)
        {
            var key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion") ??
                              Registry.CurrentUser.OpenSubKey(
                                  @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion");

            if (key == null)
                return false;

            return key.GetSubKeyNames()
                .Select(keyName => key.OpenSubKey(keyName))
                .Select(subkey => subkey.GetValue("DisplayName") as string)
                .Any(displayName => displayName != null && displayName.Contains(softwareName));

        }

    }
}
