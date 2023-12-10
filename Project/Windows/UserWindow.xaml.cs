using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
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
    /// Логика взаимодействия для UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        public UserWindow()
        {
            InitializeComponent();
            Load();
            DataContext = new User();
        }

        public UserWindow(User user)
        {
            InitializeComponent();
            Load();
            DataContext = user;
        }

        private void Load()
        {
            CmbPosition.ItemsSource = Helper.context.Roles.ToList();
            cmbEnterprise.ItemsSource = Helper.context.Enterprises.ToList();
        }

        private void UserSave_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is User user && user.User_ID == 0)
            {
                Helper.context.Users.Add(user);
            }

            try
            {
                Helper.context.SaveChanges();
                MessageBox.Show("Данные сохранены!");
            }
            catch (DbEntityValidationException ex)
            {
                foreach (DbEntityValidationResult validationError in ex.EntityValidationErrors)
                {
                    MessageBox.Show("Object: " + validationError.Entry.Entity.ToString());

                    foreach (DbValidationError err in validationError.ValidationErrors)
                    {
                        MessageBox.Show(err.ErrorMessage + "");
                    }
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
