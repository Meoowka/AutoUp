using AutoUp;
using AutoUp.Interfaces;
using System;
using System.Configuration;
using System.Windows;
using System.Windows.Input;
using AutoUp.MVVM.View;
using AutoUp.Entities;
using System.Linq;
namespace AutoUp
{
    /// <summary>
    /// Логика взаимодействия для MainForms.xaml
    /// </summary>
    public partial class MainForms : Window
    {

        public MainForms()
        {
            InitializeComponent();        
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
     
        }


        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
