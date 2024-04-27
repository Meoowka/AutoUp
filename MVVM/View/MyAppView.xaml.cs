using AutoUp.Entities;
using AutoUp.Interfaces;
using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;


namespace AutoUp.MVVM.View
{
    /// <summary>
    /// Логика взаимодействия для MyAppView.xaml
    /// </summary>
    public partial class MyAppView : UserControl
    {
        public string FilePathLocalFolder = ConfigurationManager.AppSettings.Get("FilePathLocalFolder");
        public string FilePathLocalFolderML = ConfigurationManager.AppSettings.Get("FilePathLocalFolderML");
        IModulesProvider localProvider;
        public MyAppView()
        {
            InitializeComponent();
            localProvider = new DirectoryModulesProvider(FilePathLocalFolderML);
        }
        public Module[] ClientFiles = Array.Empty<Module>();
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (File.Exists(FilePathLocalFolderML)) 
            { 
                ListingLocal(); 
            }
            else
            {
                MessageBox.Show("У вас пока нету приложений");
            }
               
        }
        public void ListingLocal()
        {
            ClientFiles = localProvider.ListModuleInfo();
            foreach (var item in ClientFiles)
            {
                DataGridMyApps.Items.Add(item);
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void InstallButton_Click(object sender, RoutedEventArgs e)
        {
            var files = DataGridMyApps.SelectedItem as Module;
            string files_r = files?.Name_File.ToString() + files?.Extension;
            Process.Start(FilePathLocalFolder + '\\' + files_r);
        }
    }
}
