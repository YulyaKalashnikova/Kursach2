using System;
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
using Project.Properties;


namespace Project
{
    /// <summary>
    /// Логика взаимодействия для AuthorizationWindow.xaml
    /// </summary>
    public partial class AuthorizationWindow : Window
    {
        public AuthorizationWindow()
        {
            InitializeComponent();
        }

        private void Auth_Click(object sender, RoutedEventArgs e)
        {
            if (txtlogin.Text == "" || txtpass.Password == "")
            {
                MessageBox.Show("Пустые поля!");
                return;
            }

            var user = dbData.GetContext().Users.FirstOrDefault(u => u.Login == txtlogin.Text && u.Password == txtpass.Password);
            if (user == null)
            {
                MessageBox.Show("Данного пользователя нет в системе!");
                return;
            }

            dbData.s_user = user;
            switch (user.FKRoleID)
            {
                case 1:
                    Osnova osnova = new Osnova(user);
                    osnova.Show();
                    break;

                case 2:
                    Osnova osnovaa = new Osnova(user);
                    osnovaa.Show();
                    break;
            }

            Close();
        }

        private void NewPass_Click(object sender, RoutedEventArgs e)
        {
            new NewPassWindow().Show();
        }
    }
}
