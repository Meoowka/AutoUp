using AutoUp.Entities;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
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

namespace AutoUp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LoginForms : Window
    {
        public LoginForms()
        {
            InitializeComponent();
        }
        public bool Authenticate(string login, string password)
        {
            var connString = "Data Source=meoowka\\sqlexpress;Initial Catalog=DiplomDatabase;Integrated Security=True";
            using (var conn = new SqlConnection(connString))
            {
                conn.Open();
                string query = "Use DiplomDatabase SELECT * FROM registr where login_user = @Login and password_user = @Password";
                var parameters = new DynamicParameters();
                parameters.Add("Login", login);
                parameters.Add("Password", password);

                var result = conn.Query<Users>(query, parameters).FirstOrDefault();

                return (result != null) ? true : false;
            }
        }
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (Authenticate(txtUser.Text, txtPass.Password.ToString()))
            {
                Hide();
                MainForms newfrm = new MainForms();
                newfrm.Show();
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль");
            }
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

        private void RegistrClick(object sender, RoutedEventArgs e)
        {
            Hide();
            RegistrForm newfrmreg = new RegistrForm();
            newfrmreg.Show();
        }
    }
   
}

