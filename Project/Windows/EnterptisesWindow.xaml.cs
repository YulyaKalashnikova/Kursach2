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
    /// Логика взаимодействия для EnterptisesWindow.xaml
    /// </summary>
    public partial class EnterptisesWindow : Window
    {
        public EnterptisesWindow()
        {
            InitializeComponent();
            DataContext = new Enterprise();
        }

        public EnterptisesWindow(Enterprise enterprise)
        {
            InitializeComponent();
            DataContext = enterprise;
        }

        private void EnterptisesSave_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is Enterprise enterprise && enterprise.Enterprise_ID == 0)
            {
                Helper.context.Enterprises.Add(enterprise);
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
