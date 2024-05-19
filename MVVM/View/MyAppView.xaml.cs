using AutoUp.Entities;
using AutoUp.Interfaces;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
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
            DataGridMyApps.Items.Clear();
            if (File.Exists(FilePathLocalFolderML)) 
            { 
                ListingLocal(); 
            }
            else
            {
               // MessageBox.Show("У вас пока нету приложений");
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

        private void InstallButton_Click(object sender, RoutedEventArgs e)
        {
            var files = DataGridMyApps.SelectedItem as Module;
            string files_r = files?.Name_File.ToString() + files?.Extension;
            Process.Start(FilePathLocalFolder + '\\' + files_r);
            MessageBox.Show("Приложение установленно.");
        }
        public void DeleteLinesInYamlFile(string filePath)
        {
            List<Module> Local = new();
            var files = DataGridMyApps.SelectedItem as Module;
            var ClientFiles = localProvider.ListModuleInfo();
            foreach (var items in ClientFiles)
            {
                var verName = items.Name_File.Equals(files.Name_File);
                var verDiscription = items.Discription.Equals(files.Discription);
                var verVersion = items.Version!.Equals(files.Version!);
                var verExtension = items.Extension.Equals(files.Extension);

                if (verName && verDiscription && verVersion && verExtension){ continue; }
                else
                {
                    Local.Add(items);
                }  
            }
            if (Local.Count > 0)
            {
                WriteYmlAdds(filePath, Local);
            }
            else
            {
                File.Delete(filePath);
            }
        }
        public void WriteYmlAdds(string filePathr, List<Module> ClientFiles)
        {
            using (FileStream fs = new FileStream(filePathr, FileMode.Create))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                foreach (var item in ClientFiles)
                {
                    sw.WriteLine(item);
                }
            }
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {

            var files = DataGridMyApps.SelectedItem as Module;
            var filesF = files?.Name_File.ToString() + files?.Extension;

            string path = FilePathLocalFolder + "\\" + filesF;
            FileInfo fileInf = new FileInfo(path);
            if (fileInf.Exists) { fileInf.Delete(); }
            DeleteLinesInYamlFile(FilePathLocalFolderML);
            MessageBox.Show("Приложение удалено.");
            DeleteFiles(files.Name_File.ToString());
            DataGridMyApps.Items.Clear();
            ListingLocal();

        }
       
        public void DeleteFiles(string deletefile)
        {
            RegistryKey myKey = Registry.LocalMachine;

            // Для удаления тоже нужно иметь права редактирования.
            RegistryKey wKey = myKey.OpenSubKey("Software", true);

            // Вывод на экран всего содержимого ключа поименно.
            string[] keyNameArray = wKey.GetSubKeyNames();

            foreach (string name in keyNameArray)
            {
                
                if (name.Contains(deletefile))
                {
                    Debug.WriteLine("-------");
                }
                Debug.WriteLine(new string(' ', 5) + name);
            }

            // Теперь пытаемся удалить ключ.
            try
            {
                Debug.WriteLine(deletefile);
                Debug.WriteLine("Всего записей в {0}: {1}.", wKey.Name, wKey.SubKeyCount);
                if (wKey.OpenSubKey(deletefile) != null)
                {
                    wKey.DeleteSubKeyTree(deletefile);
                }
                Debug.WriteLine($"Запись \'{deletefile}\' успешно удалена из реестра!");
                Debug.WriteLine("Теперь записей стало: {0}.", wKey.SubKeyCount);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            finally
            {
                wKey.Close();
            }
           
        }
      
    }
}
