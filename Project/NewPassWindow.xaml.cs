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

namespace Project
{
    /// <summary>
    /// Логика взаимодействия для NewPassWindow.xaml
    /// </summary>
    public partial class NewPassWindow : Window
    {
        dbData db = new dbData();

        public NewPassWindow()
        {
            InitializeComponent();
        }

        private void NewPassSave_Click(object sender, RoutedEventArgs e)
        {
            if (txtlogin.Text == "" || txtnewpass.Password == "")
            {
                MessageBox.Show("Введите данные в пустые поля!");
                return;
            }

            string login = txtlogin.Text.Trim();

            var user = db.Users.Where(x => x.Login == login).FirstOrDefault();

            if (user != null)
            {
                if (txtlogin.Text == login)
                {
                    var newPass = db.Users.Where(x => x.Login == login).FirstOrDefault();
                    newPass.Password = txtnewpass.Password;
                    if (txtrepeatnewpass.Password == txtnewpass.Password)
                    {
                        db.SaveChanges();
                        MessageBox.Show("Вы успешно сменили пароль!");
                    }
                    else if (txtrepeatnewpass.Password != txtnewpass.Password)
                    {
                        MessageBox.Show("Пароли не совпадают!");
                        return;
                    }
                }
                else 
                {
                    MessageBox.Show("Пользователь не найден!");
                    return;
                }
            }
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
