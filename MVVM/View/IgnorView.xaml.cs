using AutoUp.Entities;
using AutoUp.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AutoUp.MVVM.View
{
    /// <summary>
    /// Логика взаимодействия для IgnorView.xaml
    /// </summary>
    public partial class IgnorView : UserControl
    {
        public string FilePathLocalFolderMR = ConfigurationManager.AppSettings.Get("FilePathLocalFolderMR");
        IModulesProvider ignorModule;
        public IgnorView()
        {
            InitializeComponent();
            ignorModule = new IgnorModule(FilePathLocalFolderMR);
        }
        public Module[] IgnorFiles = Array.Empty<Module>();
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (File.Exists(FilePathLocalFolderMR))
            {
                ListingIgnor();
            }
           
        }
         public void ListingIgnor()
        {
            IgnorFiles = ignorModule.ListModuleInfo();
            foreach (var item in IgnorFiles)
            {
                DataGridIgnorApps.Items.Add(item);
            }
        }
    }
}
