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
using static AutoUp.LoginForms;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Media;

namespace AutoUp.MVVM.View
{
    /// <summary>
    /// Логика взаимодействия для AppView.xaml
    /// </summary>
    public partial class AppView : UserControl
    {
        public string FilePathLocalFolder = ConfigurationManager.AppSettings.Get("FilePathLocalFolder");
        public string FilePathLocalFolderMS = ConfigurationManager.AppSettings.Get("FilePathLocalFolderMS");
        public string FilePathLocalFolderMR = ConfigurationManager.AppSettings.Get("FilePathLocalFolderMR");
        public string ModuleServer = ConfigurationManager.AppSettings.Get("ModuleServer");
        public string FilePathServer = ConfigurationManager.AppSettings.Get("FilePathServer");
        public string FilePathLocalFolderML = ConfigurationManager.AppSettings.Get("FilePathLocalFolderML");
        public string host = ConfigurationManager.AppSettings.Get("Host");
        public string login = ConfigurationManager.AppSettings.Get("Login");
        public string pass = ConfigurationManager.AppSettings.Get("Pass");
        IModulesProvider remoteProvider;
        bool WindowOpened = false;
        IModulesProvider localProvider;
        IModulesProvider ignorModule;
        public AppView()
        {
            InitializeComponent();
            remoteProvider = new FtpModulesProvider(FilePathLocalFolder, FilePathServer, FilePathLocalFolderMS, host, login, pass);
            localProvider = new DirectoryModulesProvider(FilePathLocalFolderML);
            ignorModule = new DirectoryModulesProvider(FilePathLocalFolderMR);
        }   
        public Module[] ServerFiles = Array.Empty<Module>();
        public Module[] ClientFiles = Array.Empty<Module>();
       
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            remoteProvider.Connect();

            if (WindowOpened)
            {
                return;
                
            }
            else
            {
                WindowOpened = true;
                remoteProvider.Download(ModuleServer);
                ListingServer();
            }
        }

        private void ScanUpdate_Click(object sender, RoutedEventArgs e)
        {
            remoteProvider.Download(ModuleServer);
           // DataGridApps.Items.Clear();
            ShowUpdate();
           // ListingServer();
        }
        private void IgnorButton_Click(object sender, RoutedEventArgs e)
        {
            List<Module> ignor = new();
            var files = DataGridApps.SelectedItem as Module;
            if (files != null)
            {
                ignor.Add(files);
            }
            WriteYmlAddIgnor(FilePathLocalFolderMR, ignor);
        }
        private void AllUpdate_Click(object sender, RoutedEventArgs e)
        {
            bool f = false;
            if(f){ MessageBox.Show("Все доступные обновления скачены."); }
    
        }
        private void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            var files = DataGridApps.SelectedItem as Module;
            string files_r = files?.Name_File.ToString() + files?.Extension;
            object id = id_users;
           
            List<Module> Local = new();
            DataBase db = new DataBase();
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable dt = new DataTable();

            if (files != null)
            {
                var result = (FtpStatus)remoteProvider.Download(files_r);
                if (result == FluentFTP.FtpStatus.Success)
                {
                    if (MessageBox.Show("Приложение скачено", "хотите установить", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        Process.Start(FilePathLocalFolder + '\\' + files_r);

                        SqlCommand check_User_Name = new SqlCommand("SELECT COUNT(*) FROM programm WHERE (NameProgramm = @NameProgramm)", db.getConnection());
                        check_User_Name.Parameters.AddWithValue("@NameProgramm", files.Name_File);
                        db.OpenConnection();
                        int UserExist = (int)check_User_Name.ExecuteScalar();

                        if (UserExist > 0){}
                        else
                        {
                            string query = $"use DiplomAutoUp insert programm(idUsers,NameProgramm,NameVersion," +
                                       $"DataСhanges,SizeProgramm,StatusProgramm) values('{id}','{files.Name_File}','{files.Version}'," +
                                       $"'{File.GetCreationTime(FilePathLocalFolder + "\\" + files_r)}','{files_r.Length}','{result}') ";
                            SqlCommand command = new SqlCommand(query, db.getConnection());
                            db.OpenConnection();   
                            command.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Чтобы уставновить перейдите во вкладку Мои приложения");

                        SqlCommand check_User_Name = new SqlCommand("SELECT COUNT(*) FROM programm WHERE (NameProgramm = @NameProgramm)", db.getConnection());
                        check_User_Name.Parameters.AddWithValue("@NameProgramm", files.Name_File);
                        db.OpenConnection();
                        int UserExist = (int)check_User_Name.ExecuteScalar();

                        if (UserExist > 0) { }
                        else
                        {
                            string query = $"use DiplomAutoUp insert programm(idUsers,NameProgramm,NameVersion," +
                                       $"DataСhanges,SizeProgramm,StatusProgramm) values('{id}','{files.Name_File}','{files.Version}'," +
                                       $"'{File.GetCreationTime(FilePathLocalFolder + "\\" + files_r)}','{files_r.Length}','{result}') ";
                            SqlCommand command = new SqlCommand(query, db.getConnection());
                            db.OpenConnection();
                            command.ExecuteNonQuery();
                        }
                    }
                    
                }
                db.CloseConnection();
                
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
        //Запись в YAML
        public void WriteYmlAdd(string filePathr, List<Module> ClientFiles)
        {
            var clientFiles = localProvider.ListModuleInfo();
            using (FileStream fs = new FileStream(filePathr, FileMode.Append, FileAccess.Write))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                foreach (var item in ClientFiles)
                {
                    var verName = clientFiles.Where(x => x.Name_File!.Equals(item.Name_File)).Any();
                    if (verName)
                    {
                        continue;
                    }
                    sw.WriteLine(item);
                }
            }
        }
        public void WriteYmlAddIgnor(string filePathr, List<Module> ClientFiles)
        {
            var clientFiles = ignorModule.ListModuleInfo();
            using (FileStream fs = new FileStream(filePathr, FileMode.Append, FileAccess.Write))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                foreach (var item in ClientFiles)
                {
                    var verName = clientFiles.Where(x => x.Name_File!.Equals(item.Name_File)).Any();
                    if (verName)
                    {
                        continue;
                    }
                    sw.WriteLine(item);
                }
            }
        }
        public void ShowUpdate()
        {

            var ClientFiles = localProvider.ListModuleInfo();
            var ServerFiles = remoteProvider.ListModuleInfo();
            int rowIndex = 0;
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

                        DataGridRow row = (DataGridRow)DataGridApps.ItemContainerGenerator.ContainerFromIndex(rowIndex);
                      //  MessageBox.Show(DataGridApps.Items.Count.ToString());
                        if (row != null)
                        {
                            if (Convert.ToInt64(verClient) > Convert.ToInt64(verServer))
                            {
                                // MessageBox.Show($"Версия {items.Name_File} актуальная, на клиенте версия новее");
                                row.Background = new SolidColorBrush(Colors.LightSkyBlue);

                            }
                            else if (Convert.ToInt64(verClient) < Convert.ToInt64(verServer))
                            {
                                //MessageBox.Show($"Версия {items.Name_File} не актуальная, на сервере версия новее");
                                row.Background = new SolidColorBrush(Colors.Yellow);
                            }
                            else
                            {
                                // MessageBox.Show($"Версия {items.Name_File} актуальна");
                                row.Background = new SolidColorBrush(Colors.LightGreen);
                            }
                        }
                        
                        
                    }
                    else
                    {
                        MessageBox.Show($"У Вас нету {items.Name_File}, советуем скачать");
                    }
                    rowIndex++;
                }
            }
        }






        //public bool IsSoftwareInstalled(string softwareName)
        //{
        //    var key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion") ??
        //                      Registry.CurrentUser.OpenSubKey(
        //                          @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion");

        //    if (key == null)
        //        return false;

        //    return key.GetSubKeyNames()
        //        .Select(keyName => key.OpenSubKey(keyName))
        //        .Select(subkey => subkey.GetValue("DisplayName") as string)
        //        .Any(displayName => displayName != null && displayName.Contains(softwareName));

        //}

    }
}
