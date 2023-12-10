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
    /// Логика взаимодействия для TreatiesWindow.xaml
    /// </summary>
    public partial class ListOfThingWindow : Window
    {
        public List<Unit> Units = new List<Unit>();
        public List<Condition> Conditions = new List<Condition>();
        public List<User> Users = new List<User>();
        public List<Thing> Things = new List<Thing>();
        public List<ListOfThing> ListOfThings = new List<ListOfThing>();

        public ListOfThingWindow()
        {
            InitializeComponent();
            LoadData();
            Data();
            DataContext = new ListOfThing();
            CmbUnit.ItemsSource = Helper.context.Units.ToList();
            CmbUser.ItemsSource = Helper.context.Users.ToList();
            CmbThing.ItemsSource = Helper.context.Things.ToList();
            CmbCondition.ItemsSource = Helper.context.Conditions.ToList();
        }

        public ListOfThingWindow(ListOfThing listOfThing)
        {
            InitializeComponent();
            LoadData();
            Data();
            DataContext = listOfThing;
            CmbUnit.ItemsSource = Helper.context.Units.ToList();
            CmbUser.ItemsSource = Helper.context.Users.ToList();
            CmbThing.ItemsSource = Helper.context.Things.ToList();
            CmbCondition.ItemsSource = Helper.context.Conditions.ToList();
            DateTime? selectedDate = datePicker.SelectedDate;
            //datePicker = listOfThing;
            //datePicker.SelectedDate = Helper.context.ListOfThings.ToList();
            //datePicker.DateOfIssue = IssueDateBox.SelectedDate.Value;
        }

        private void LoadData()
        {
            Units = Helper.context.Units.ToList();
            Conditions = Helper.context.Conditions.ToList();
            Users = Helper.context.Users.ToList();
            Things = Helper.context.Things.ToList();
            //ListOfThings = Helper.context.ListOfThings.ToList();
        }

        private void Data()
        {
            CmbUnit.ItemsSource = Units;
            CmbCondition.ItemsSource = Conditions;
            CmbUser.ItemsSource = Users;
            CmbThing.ItemsSource = Things;
        }

        private void SaveListOfThing_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is ListOfThing listOfThing && listOfThing.ListOfThing_ID == 0)
            {
                var things = CmbThing.SelectedItem as Thing;
                var unit = CmbUnit.SelectedItem as Unit;
                var condition = CmbCondition.SelectedItem as Condition;
                var user = CmbUser.SelectedItem as User;

                if (things.Thing_ID == 0 || unit.Unit_ID == 0 
                    || condition.Condition_ID == 0 || user.User_ID == 0)
                {
                    MessageBox.Show("Выберите данные из выпадающего списка!");
                    return;
                }

                if (txtCount.Text == "" || txtPrice.Text == "" || datePicker.Text == "")
                {
                    MessageBox.Show("Заполните пустые поля!");
                    return;
                }

                listOfThing.Date = Convert.ToDateTime(datePicker.Text);
                listOfThing.FKThingID = things.Thing_ID;
                listOfThing.FKUnitID = unit.Unit_ID;
                listOfThing.FKConditionID = condition.Condition_ID;
                listOfThing.FKUserID = user.User_ID;
                listOfThing.Price = int.Parse(txtPrice.Text);
                listOfThing.Count = int.Parse(txtCount.Text);
                listOfThing.Sum = Convert.ToInt32(txtPrice.Text) * Convert.ToInt32(txtCount.Text);
                Helper.context.ListOfThings.Add(listOfThing);
            }
            try
            {
                Helper.context.SaveChanges();
                MessageBox.Show("Данные сохранены!");
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }

            Close();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
