using AutoUp.Entities;
using System;
using System.Data.SqlClient;
using Dapper;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using System.Data;

namespace AutoUp
{
    /// <summary>
    /// Логика взаимодействия для RegistrForm.xaml
    /// </summary>
    public partial class RegistrForm : Window
    {
        public RegistrForm()
        {
            InitializeComponent();
        }
       
        private void btnRegistr_Click(object sender, RoutedEventArgs e)
        {
            var login = txtUser.Text;
            var pass = txtPass.Text;
            var email = txtemail.Text;


            DataBase db = new DataBase();
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable dt = new DataTable();

            string query = $"use DiplomAutoUp insert into registr(idUser,login_user,password_user,email_user) values('4','{login}','{pass}','{email}') ";

            SqlCommand command = new SqlCommand(query,db.getConnection());

            db.OpenConnection();
            if (checkInput())
            {
                MessageBox.Show("Одно или оба поля не заполнены.");
                return;
            }

            if (checkUser())
                return;


            if (command.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Аккаунт создан!");
                LoginForms newfrm = new LoginForms();
                this.Hide();
                newfrm.Show();
            }
            else
            {
                MessageBox.Show("Аккаунт не создан..");
            }


            db.CloseConnection();
        }

        private Boolean checkInput()
        {
            var login = txtUser.Text;
            var pass = txtPass.Text;

            if (login == "" || pass == "")
            {
                return true;
            }
            return false; 
        }
        private Boolean checkUser()
        {
            var login = txtUser.Text;
            var pass = txtPass.Text;
            var connString = "Data Source=meoowka\\sqlexpress;Initial Catalog=DiplomAutoUp;Integrated Security=True";
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable dt = new DataTable();
            using (var conn = new SqlConnection(connString))
            {
                string query = $"use DiplomAutoUp select IdUser,login_user,password_user from registr" +
                $" where login_user = '{login}' and password_user = '{pass}'";

                var result = new SqlCommand(query, conn);
                adapter.SelectCommand = result;
                adapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    MessageBox.Show("Пользователь уже создан");
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        private void btnLast_Click(object sender, RoutedEventArgs e)
        {
            LoginForms newfrm = new LoginForms();
            this.Hide();
            newfrm.Show();
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
