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
    /// Логика взаимодействия для ProductWindow.xaml
    /// </summary>
    public partial class ThingWindow : Window
    {


        public ThingWindow()
        {
            InitializeComponent();
            Load();
            DataContext = new Thing();
        }

        public ThingWindow(Thing thing)
        {
            InitializeComponent();
            Load();
            DataContext = thing;
        }

        private void Load()
        {
            CmbType.ItemsSource = Helper.context.TypeOfThings.ToList();
        }

        private void ThingSave_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is Thing thing && thing.Thing_ID == 0)
            {
                Helper.context.Things.Add(thing);
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

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
