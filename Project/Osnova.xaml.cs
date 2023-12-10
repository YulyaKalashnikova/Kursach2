using Microsoft.Win32;
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
using Word = Microsoft.Office.Interop.Word;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data;


namespace Project
{
    /// <summary>
    /// Логика взаимодействия для Osnova.xaml
    /// </summary>
    public partial class Osnova : Window
    {
        private List<ListOfThing> ListOfThings;
        private List<Thing> Things;
        private List<User> Users;
        private List<Role> Roles;
        private List<Enterprise> Enterprises;

        public Osnova(User users)
        {
            InitializeComponent();
            Load();
            LoadData();
            DataContext = users;

            Title = $"{dbData.s_user.FullName}, {dbData.s_user.Role.RoleTitle} - Деятельность материально-ответственного лица на предприятии \"{dbData.s_user.Enterprise.EnterpriseTitle}\"";
        }

        private void LoadData()
        {
            // Поиск компании.
            Enterprises = Helper.context.Enterprises.ToList();

            if (TbSearchEnt.Text != string.Empty)
            {
                Enterprises = Enterprises.Where(x => x.EnterpriseTitle.ToLower().Contains(TbSearchEnt.Text.ToLower().Trim())
                || x.Address.ToLower().Contains(TbSearchEnt.Text.ToLower().Trim())).ToList();
            }
            tableEnterprises.ItemsSource = Enterprises;

            // Поиск и фильтрация пользоватей.
            Users = Helper.context.Users.ToList();
            
            CmdFilterUsers.ItemsSource = Roles;

            if (CmdFilterUsers.SelectedIndex != 0)
            {
                Users = Users.Where(x => x.Role == (CmdFilterUsers.SelectedItem as Role)).ToList();
            }

            if (TbSearchUsers.Text != string.Empty)
            {
                Users = Users.Where(x => x.LastName.ToLower().Contains(TbSearchUsers.Text.ToLower().Trim())
                || x.FirstName.ToLower().Contains(TbSearchUsers.Text.ToLower().Trim())
                || x.MiddleName.ToLower().Contains(TbSearchUsers.Text.ToLower().Trim())
                || x.Phone.ToLower().Contains(TbSearchUsers.Text.ToLower().Trim())
                || x.Login.ToLower().Contains(TbSearchUsers.Text.ToLower().Trim())
                || x.Role.RoleTitle.ToLower().Contains(TbSearchUsers.Text.ToLower().Trim())).ToList();
            }
            tableUsers.ItemsSource = Users;

            // Поиск оборудования.
            Things = Helper.context.Things.ToList();

            if (TbSearchThing.Text != string.Empty)
            {
                Things = Things.Where(x => x.ThingTitle.ToLower().Contains(TbSearchThing.Text.ToLower().Trim())
                || x.InventoryNumber.ToLower().Contains(TbSearchThing.Text.ToLower().Trim())
                || x.TypeOfThing.TypeOfThingTitle.ToLower().Contains(TbSearchThing.Text.ToLower().Trim())).ToList();
            }
            tableThing.ItemsSource = Things;

            // Поиск и фильтрация по инвентарному листу..
            ListOfThings = Helper.context.ListOfThings.ToList();
            var datePickerFilter = DatePickerFilter1.SelectedDate;
            var datePickerFilter2 = DatePickerFilter2.SelectedDate;

            if (datePickerFilter.HasValue)
            {
                ListOfThings = ListOfThings.Where(x => x.Date >= datePickerFilter.Value).ToList();
            }

            if (datePickerFilter2.HasValue)
            {
                ListOfThings = ListOfThings.Where(x => x.Date <= datePickerFilter2.Value).ToList();
            }

            if (TbSearchListOfThing.Text != string.Empty)
            {
                ListOfThings = ListOfThings.Where(x => x.Thing.ThingTitle.ToLower().Contains(TbSearchListOfThing.Text.ToLower().Trim())
                || x.Condition.ConditionTitle.ToLower().Contains(TbSearchListOfThing.Text.ToLower().Trim())
                || x.User.LastName.ToLower().Contains(TbSearchListOfThing.Text.ToLower().Trim())
                || x.User.FirstName.ToLower().Contains(TbSearchListOfThing.Text.ToLower().Trim())
                || x.User.MiddleName.ToLower().Contains(TbSearchListOfThing.Text.ToLower().Trim())
                || Convert.ToString(x.Date).Contains(TbSearchListOfThing.Text.ToLower().Trim())
                || x.Thing.InventoryNumber.Contains(TbSearchListOfThing.Text.Trim())).ToList();
            }
            tableListOfThing.ItemsSource = ListOfThings;
        }

        private void Load()
        {
            Roles = Helper.context.Roles.ToList();
            Roles.Insert(0, new Role { RoleTitle = " Все должности" });

            tableEnterprises.ItemsSource = Helper.context.Enterprises.ToList();
            tableThing.ItemsSource = Helper.context.Things.ToList();
            tableListOfThing.ItemsSource = Helper.context.ListOfThings.ToList();
            tableUsers.ItemsSource = Helper.context.Users.ToList();
        }

        // Компании.
        private void EnterprisesAdd_Click(object sender, RoutedEventArgs e)
        {
            new EnterptisesWindow().ShowDialog();
            Load();
        }

        private void EnterprisesEdit_Click(object sender, RoutedEventArgs e)
        {
            if (tableEnterprises.SelectedItem is Enterprise enterprise)
            {
                new EnterptisesWindow(enterprise).ShowDialog();
                Load();
            }
        }

        private void EnterprisesRemove_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show($"Вы точно хотите удалить данные?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                if (tableEnterprises.SelectedItem is Enterprise enterprise)
                {
                    Helper.context.Enterprises.Remove(enterprise);
                    Helper.context.SaveChanges();
                    Load();
                }
            }
        }

        // Сотрудники.
        private void UsersAdd_Click(object sender, RoutedEventArgs e)
        {
            new UserWindow().ShowDialog();
            Load();
        }

        private void UsersEdit_Click(object sender, RoutedEventArgs e)
        {
            if (tableUsers.SelectedItem is User user)
            {
                new UserWindow(user).ShowDialog();
                Load();
            }
        }

        private void UsersRemove_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show($"Вы точно хотите удалить данные?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                if (tableUsers.SelectedItem is User user)
                {
                    Helper.context.Users.Remove(user);
                    Helper.context.SaveChanges();
                    Load();
                }
            }
        }

        // Оборудование.
        private void ThingAdd_Click(object sender, RoutedEventArgs e)
        {
            new ThingWindow().ShowDialog();
            Load();
        }

        private void ThingEdit_Click(object sender, RoutedEventArgs e)
        {
            if (tableThing.SelectedItem is Thing thing)
            {
                new ThingWindow(thing).ShowDialog();
                Load();
            }
        }

        private void ThingRemove_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show($"Вы точно хотите удалить данные?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                if (tableThing.SelectedItem is Thing thing)
                {
                    Helper.context.Things.Remove(thing);
                    Helper.context.SaveChanges();
                    Load();
                }
            }
        }

        // Инвентарный лист.
        private void ListOfThingAdd_Click(object sender, RoutedEventArgs e)
        {
            new ListOfThingWindow().ShowDialog();
            Load();
        }

        private void ListOfThingEdit_Click(object sender, RoutedEventArgs e)
        {
            if (tableListOfThing.SelectedItem is ListOfThing listOfThing)
            {
                new ListOfThingWindow(listOfThing).ShowDialog();
            }
            Load();
        }

        private void ListOfThingRemove_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show($"Вы точно хотите удалить данные?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                if (tableListOfThing.SelectedItem is ListOfThing listOfThing)
                {
                    Helper.context.ListOfThings.Remove(listOfThing);
                    Helper.context.SaveChanges();
                    Load();
                }
            }
        }

        // Списать.
        private void WriteOff_Click(object sender, RoutedEventArgs e)
        {
            if (tableListOfThing.SelectedItem is ListOfThing listOfThing)
            {
                
            }
        }

        // Итог.
        private void Itog_Click(object sender, RoutedEventArgs e)
        {
            int sumCount = 0;
            for (int i = 0; i < tableListOfThing.Items.Count - 1; i++)
            {
                sumCount += (int.Parse((tableListOfThing.Columns[5].GetCellContent(tableListOfThing.Items[i]) as TextBlock).Text));
            }

            decimal sumPrice = 0m;
            for (int i = 0; i < tableListOfThing.Items.Count - 1; i++)
            {
                sumPrice += decimal.Parse((tableListOfThing.Columns[4].GetCellContent(tableListOfThing.Items[i]) as TextBlock).Text);
            }

            decimal sumSum = 0;
            for (int i = 0; i < tableListOfThing.Items.Count - 1; i++)
            {
                sumSum += (decimal.Parse((tableListOfThing.Columns[7].GetCellContent(tableListOfThing.Items[i]) as TextBlock).Text));
            }

            itogCount.Content = sumCount + " шт.";
            itogPrice.Content = sumPrice + " руб.";
            itogSum.Content = sumSum + " руб.";
        }

        // Отчёты.
        private void ReportUsersPdf_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.FileName = "ReportUsers";
            saveFile.Filter = "Pdf files | *.pdf";
            if (saveFile.ShowDialog() == true)
            {
                Word.Application app = new Word.Application();
                Word.Document document = app.Documents.Add();
                List<User> users = Helper.context.Users.ToList();
                Word.Paragraph paragraph = document.Paragraphs.Add();
                Word.Range range = paragraph.Range;
                Word.Table table = document.Tables.Add(range, users.Count + 1, 5);

                Word.Range cellRange;
                cellRange = table.Cell(1, 1).Range;
                cellRange.Text = "Фамилия";

                cellRange = table.Cell(1, 2).Range;
                cellRange.Text = "Имя";

                cellRange = table.Cell(1, 3).Range;
                cellRange.Text = "Должность";

                cellRange = table.Cell(1, 4).Range;
                cellRange.Text = "Номер телефона";

                cellRange = table.Cell(1, 5).Range;
                cellRange.Text = "Подпись";

                Users = Helper.context.Users.ToList();
                CmdFilterUsers.ItemsSource = Roles;

                if (CmdFilterUsers.SelectedIndex == 0)
                {
                    tableUsers.ItemsSource = Users;

                    for (int i = 0; i < Users.Count; i++)
                    {
                        cellRange = table.Cell(i + 2, 1).Range;
                        cellRange.Text = Users[i].LastName;
                        cellRange = table.Cell(i + 2, 2).Range;
                        cellRange.Text = Users[i].FirstName;
                        cellRange = table.Cell(i + 2, 3).Range;
                        cellRange.Text = Users[i].Role.RoleTitle;
                        cellRange = table.Cell(i + 2, 4).Range;
                        cellRange.Text = Users[i].Phone;
                    }
                }

                if (CmdFilterUsers.SelectedIndex != 0)
                {
                    Users = Users.Where(x => x.Role == (CmdFilterUsers.SelectedItem as Role)).ToList();

                    tableUsers.ItemsSource = Users;

                    for (int i = 0; i < Users.Count; i++)
                    {
                        cellRange = table.Cell(i + 2, 1).Range;
                        cellRange.Text = Users[i].LastName;
                        cellRange = table.Cell(i + 2, 2).Range;
                        cellRange.Text = Users[i].FirstName;
                        cellRange = table.Cell(i + 2, 3).Range;
                        cellRange.Text = Users[i].Role.RoleTitle;
                        cellRange = table.Cell(i + 2, 4).Range;
                        cellRange.Text = Users[i].Phone;
                    }
                }
                document.SaveAs2(saveFile.FileName, Word.WdSaveFormat.wdFormatPDF);
                document.Close();
                app.Quit();
            }
        }

        private void ReportAccountingExcel_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.FileName = "report";
            saveFile.Filter = "Excel files |*.xlsx";
            if (saveFile.ShowDialog() == true)
            {
                Excel.Application app = new Excel.Application();
                Excel.Workbook workbook = app.Workbooks.Add();
                Excel.Worksheet worksheet = app.Worksheets[1];
                worksheet.Name = "Пользователи";
                worksheet.Range["A1"].Value = "Дата";
                worksheet.Range["B1"].Value = "Оборудование";
                worksheet.Range["C1"].Value = "Номер";
                worksheet.Range["D1"].Value = "Статус списания";
                worksheet.Range["E1"].Value = "Состояние";
                worksheet.Range["F1"].Value = "Цена";
                worksheet.Range["G1"].Value = "Количество";
                worksheet.Range["H1"].Value = "Ед. измерения";
                worksheet.Range["I1"].Value = "Сумма";
                worksheet.Range["J1"].Value = "Работник";

                var datePickerFilter = DatePickerFilter1.SelectedDate;
                var datePickerFilter2 = DatePickerFilter2.SelectedDate;

                if (datePickerFilter.HasValue)
                {
                    ListOfThings = ListOfThings.Where(x => x.Date >= datePickerFilter.Value).ToList();
                }

                if (datePickerFilter2.HasValue)
                {
                    ListOfThings = ListOfThings.Where(x => x.Date <= datePickerFilter2.Value).ToList();
                }

                if (TbSearchListOfThing.Text != string.Empty)
                {
                    ListOfThings = ListOfThings.Where(x => x.Thing.ThingTitle.Contains(TbSearchListOfThing.Text.ToLower().Trim())
                    || x.Condition.ConditionTitle.ToLower().Contains(TbSearchListOfThing.Text.ToLower().Trim())
                    || x.User.LastName.ToLower().Contains(TbSearchListOfThing.Text.ToLower().Trim())
                    || x.User.FirstName.ToLower().Contains(TbSearchListOfThing.Text.ToLower().Trim())
                    || x.User.MiddleName.ToLower().Contains(TbSearchListOfThing.Text.ToLower().Trim())
                    || Convert.ToString(x.Date).Contains(TbSearchListOfThing.Text.ToLower().Trim())
                    || x.Thing.InventoryNumber.Contains(TbSearchListOfThing.Text.Trim())).ToList();

                    tableListOfThing.ItemsSource = ListOfThings;

                    for (int i = 0; i < ListOfThings.Count; i++)
                    {
                        if (ListOfThings[i] != null)
                        {
                            worksheet.Range[$"A{i + 2}"].Value = Convert.ToString(ListOfThings[i].Date);
                            worksheet.Range[$"B{i + 2}"].Value = ListOfThings[i].Thing.ThingTitle;
                            worksheet.Range[$"C{i + 2}"].Value = ListOfThings[i].Thing.InventoryNumber;
                            worksheet.Range[$"E{i + 2}"].Value = ListOfThings[i].Condition.ConditionTitle;
                            worksheet.Range[$"F{i + 2}"].Value = ListOfThings[i].Price;
                            worksheet.Range[$"G{i + 2}"].Value = ListOfThings[i].Count;
                            worksheet.Range[$"H{i + 2}"].Value = ListOfThings[i].Unit.UnitTitle;
                            worksheet.Range[$"I{i + 2}"].Value = ListOfThings[i].Sum;
                            worksheet.Range[$"J{i + 2}"].Value = ListOfThings[i].User.FullName;
                        }
                    }
                }


                if (TbSearchListOfThing.Text == string.Empty)
                {
                    for (int i = 0; i < ListOfThings.Count; i++)
                    {
                        if (ListOfThings[i] != null)
                        {
                            worksheet.Range["A1"].Value = Convert.ToString(ListOfThings[1].Date);
                            worksheet.Range[$"B{i + 2}"].Value = ListOfThings[i].Thing.ThingTitle;
                            worksheet.Range[$"B{i + 2}"].Value = ListOfThings[i].Thing.InventoryNumber;
                            worksheet.Range[$"D{i + 2}"].Value = ListOfThings[i].Condition.ConditionTitle;
                            worksheet.Range[$"E{i + 2}"].Value = ListOfThings[i].User.FullName;
                        }
                    }
                }
                workbook.SaveAs(saveFile.FileName);
                workbook.Close();
                app.Quit();
            }
        }

        // Личный кабинет.
        private void SaveProfile_Click(object sender, RoutedEventArgs e)
        {
            try 
            {
                dbData.GetContext().SaveChanges();
                MessageBox.Show("Данные сохранены");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void PictureAdd_Click(object sender, RoutedEventArgs e)
        {

        }

        // Поиск.
        private void TextBox_TextChangedEnt(object sender, TextChangedEventArgs e)
        {
            if (TbSearchEnt.Text != string.Empty)
            {
                LoadData();
            }

            if (TbSearchEnt.Text == string.Empty)
            {
                tableEnterprises.ItemsSource = Helper.context.Enterprises.ToList();
            }
        }

        private void TextBox_TextChangedUsers(object sender, TextChangedEventArgs e)
        {
            if (TbSearchUsers.Text != string.Empty)
            {
                LoadData();
            }

            if (TbSearchUsers.Text == string.Empty)
            {
                tableUsers.ItemsSource = Helper.context.Users.ToList();
            }
        }

        private void TextBox_TextChangedListOfThing(object sender, TextChangedEventArgs e)
        {
            if (TbSearchListOfThing.Text != string.Empty)
            {
                LoadData();
            }

            if (TbSearchListOfThing.Text == string.Empty)
            {
                tableListOfThing.ItemsSource = Helper.context.ListOfThings.ToList();
            }
        }

        private void TextBox_TextChangedThing(object sender, TextChangedEventArgs e)
        {
            if (TbSearchThing.Text != string.Empty)
            {
                LoadData();
            }

            if (TbSearchThing.Text == string.Empty)
            {
                tableThing.ItemsSource = Helper.context.Things.ToList();
            }
        }

        // Фильтрация.
        private void CmbFilterUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CmdFilterUsers.SelectedItem != null)
            {
                LoadData();
            }
        }

        private void DatePickerListOfThing_DateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DatePickerFilter1.SelectedDate != null && DatePickerFilter1.SelectedDate != null)
            {
                LoadData();
            }
        }

        private void Osnova_IsVisibleChange(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (dbData.s_user.FKRoleID == 2)
            {
                EntTabItem.Visibility = Visibility.Collapsed;
                UserTableItem.Visibility = Visibility.Collapsed;
                RemoveListOfThing.Visibility = Visibility.Collapsed;
                RemoveThing.Visibility = Visibility.Collapsed;
            }
        }
    }
}