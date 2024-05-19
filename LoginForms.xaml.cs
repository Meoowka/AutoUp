using AutoUp.Entities;
using Dapper;

using System;
using System.Data.SqlClient;
using System.Data;
using System.Windows;
using System.Windows.Input;

namespace AutoUp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LoginForms
    {
        public LoginForms()
        {
            InitializeComponent();
        }
        
        public static object id_users { get; set; }
        
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            String loginUser = txtUser.Text;
            String passwordUser = txtPass.Password.ToString();
           
            DataBase db = new DataBase();
            DataTable dataTable = new DataTable();
            db.OpenConnection();
            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlCommand command = new SqlCommand("select * from registr where login_user = @uL and password_user = @uP", db.getConnection());
            command.Parameters.Add("@uL", SqlDbType.VarChar).Value = loginUser;
            command.Parameters.Add("@uP", SqlDbType.VarChar).Value = passwordUser;

            adapter.SelectCommand = command;
            adapter.Fill(dataTable);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows) // если есть данные
                {
                    while (reader.Read()) // построчно считываем данные
                    {
                        id_users = reader.GetValue(0);
                       // MessageBox.Show($"{id_users}");
                    }
                }
            }
            if (dataTable.Rows.Count > 0)
            {
                
                Hide();
                MainForms newfrm = new MainForms();

                newfrm.Show();
            }
            else
            {
                MessageBox.Show("Пользователь не найден");
            }
            db.CloseConnection();
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

