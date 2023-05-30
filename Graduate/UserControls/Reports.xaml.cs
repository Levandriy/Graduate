using Graduate.Forms;
using Graduate.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using Graduate.Classes;
using Graduate.Windows;

namespace Graduate.UserControls
{
    /// <summary>
    /// Логика взаимодействия для Reports.xaml
    /// </summary>
    public partial class Reports : UserControl
    {
        public Reports()
        {
            InitializeComponent();
        }
        public Guid Theme;
        public static string Group = string.Empty;
        public static string Student = string.Empty;
        public static string Lab = string.Empty;
        public class ReportView
        {
            public Guid Id { get; set; }
            public string Title { get; set; }
            public string Material { get; set; }
            public DateTime Date { get; set; }
            public int[] Marks { get; set; } = { 2, 3, 4, 5 };
            public int Mark { get; set; }
        }
        public List<ReportView> ListView { get; set; }
        private void Load()
        {
            var query = from report in App._context.Journal
                        where report.Date >= App.StartPeriod
                        && report.Date <= App.EndPeriod
                        join mat in App._context.Materials on report.Lab equals mat.Id
                        where mat.Title == (string.IsNullOrEmpty(Lab) ? mat.Title : Lab)
                        join st in App._context.Students on report.Student equals st.Id
                        where st.Name == (string.IsNullOrEmpty(Student) ? st.Name : Student)
                        join gr in App._context.Groups on st.Group equals gr.Id
                        where gr.Teacher == App.user
                        && gr.Title == (string.IsNullOrEmpty(Group) ? gr.Title : Group)
                        where mat.Title.Contains(string.IsNullOrEmpty(SearchBox.Text) ? mat.Title : SearchBox.Text)
                        || gr.Title.Contains(string.IsNullOrEmpty(SearchBox.Text) ? gr.Title : SearchBox.Text)
                        || st.Name.Contains(string.IsNullOrEmpty(SearchBox.Text) ? st.Name : SearchBox.Text)
                        select new ReportView
                        {
                            Id = report.Id,
                            Date = report.Date,
                            Material = mat.Title,
                            Title = st.Name,
                            Mark = report.Mark == null ? 0 : (int)report.Mark,
                        };
            Debug.WriteLine($"Группа: {Group}");
            Debug.WriteLine($"Лабораторная: {Lab}");
            Debug.WriteLine($"Студент: {Student}");
            ListView = query.ToList();
            //foreach(var row in ListView)
            //{
            //    Debug.WriteLine(row.Id);
            //    Debug.WriteLine(row.Date);
            //    Debug.WriteLine(row.Title);
            //    Debug.WriteLine(row.Mark);
            //}
            DataPanel.ItemsSource = ListView;
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Labs.ItemsSource = App._context.Materials.Where(x => x.Type == "Работа").Select(x => x.Title).ToList();
            Labs.SelectedIndex = 0;
            Groups.ItemsSource = App._context.Groups.Select(x => x.Title).ToList();
            Groups.SelectedIndex = 0;
            Students.ItemsSource = (from st in App._context.Students
                                    join gr in App._context.Groups on st.Group equals gr.Id
                                    where gr.Title == Groups.SelectedValue.ToString()
                                    select st.Name).ToList();
            Load();
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Load();
        }

        private void UserControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var obj = e.OriginalSource as FrameworkElement;
            if (obj != null)
            {
                if (obj.DataContext != null)
                {
                    var report = obj.DataContext as ReportView;
                    Debug.WriteLine("Тыкнута карточка, запускаю этот файл");
                    var repObj = App._context.Journal.Find(report.Id);
                    var bytes = repObj.File;
                    string file_name = report.Title + repObj.Ext;
                    string file_path = $"{Environment.CurrentDirectory}\\{file_name}";
                    File.WriteAllBytes(file_path, bytes);
                    Process.Start(file_path);
                }
            }
        }
        private void ReportDetails_Click(object sender, RoutedEventArgs e)
        {
            var element = sender as FrameworkElement;
            if (element?.DataContext != null)
            {
                var report = element.DataContext as ReportView;
                Debug.WriteLine(element.DataContext);
                var page = new ReportWindowTeacher(report.Id)
                {
                    Owner = Window.GetWindow(this)
                };
                page.Show();
            }
        }

        private void DeleteReport_Click(object sender, RoutedEventArgs e)
        {
            var element = sender as FrameworkElement;
            if (element?.DataContext != null)
            {
                var report = element.DataContext as ReportView;
                var repObj = App._context.Materials.Find(report.Id);
                App._context.Materials.Remove(repObj);
                App._context.SaveChanges();
            }
            Load();
        }

        private void Mark_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var box = sender as ComboBox;
            Debug.WriteLine(box?.SelectedValue);
            var item = int.Parse(box.SelectedValue.ToString());
            Debug.WriteLine("Оценка изменена: " + item);
            var report = box.DataContext as ReportView;
            Debug.WriteLine("Нашёл: " + report.Id);
            var query = App._context.Journal.Find(report.Id);
            query.Mark = item;
            App._context.Journal.AddOrUpdate(query);
            App._context.SaveChanges();
            Load();
        }

        private void EndDP_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            App.EndPeriod = EndDP.SelectedDate;
            Load();
        }

        private void StartDP_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            App.StartPeriod = StartDP.SelectedDate;
            Load();
        }

        private void Groups_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Group = Groups.SelectedValue == null ? string.Empty : Groups.SelectedValue.ToString();
            Students.ItemsSource = (from st in App._context.Students
                                    join gr in App._context.Groups on st.Group equals gr.Id
                                    where gr.Title == Group
                                    select st.Name).ToList();
            Load();
        }

        private void Labs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Lab = Labs.SelectedValue == null ? string.Empty : Labs.SelectedValue.ToString();
            Load();
        }

        private void Students_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Student = Students.SelectedValue == null ? string.Empty : Students.SelectedValue.ToString();
            Load();
        }

        private void StatementButton_Click(object sender, RoutedEventArgs e)
        {
            var groupedList = ListView.GroupBy(x => x.Title);
            SaveFileDialog Statement = new SaveFileDialog()
            {
                Filter = "Excel Files|*.xlsx;*.xls;*.xlsm",
                RestoreDirectory = false
            };
            if (Statement.ShowDialog() == true)
            {
                //Cоздание приложения Excel
                Excel.Application app = new Excel.Application
                {
                    DisplayAlerts = false,
                    SheetsInNewWorkbook = 1
                };
                Excel.Workbook wb = app.Workbooks.Add();
                Excel.Worksheet sheet = (Excel.Worksheet)app.Worksheets.get_Item(1);
                sheet.Name = $"Ведомость оценок за{DateTime.Now: dd.MM.yyyy}";
                //Шапка
                sheet.Cells[1, 1] = $"Имя студента";
                sheet.Cells[1, 2] = $"Работа";
                sheet.Cells[1, 3] = "Дата";
                sheet.Cells[1, 4] = $"Оценка";
                //Форматирование шапки
                Excel.Range header = sheet.Range[sheet.Cells[1, 1], sheet.Cells[1, 4]];
                header.Font.Size = 14;
                header.Font.Bold = true;
                header.Borders.Weight = 2;
                header.WrapText = true;
                header.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                header.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;

                var arr = groupedList.ToArray();
                //Вспомогательные переменные
                int currentLine = 2;
                int Fives = 0;
                int Fours = 0;
                int Threes = 0;
                int Twos = 0;
                int NoMarks = 0;
                //Цикл по группированному массиву
                for (int i = 0; i < arr.Count(); i++)
                {
                    sheet.Cells[currentLine, 1] = arr[i].First().Title.ToUpper();
                    currentLine++;
                    //Разгруппировка, поиск отдельных элементов
                    for (int j = 0; j < arr[i].Count(); j++)
                    {
                        sheet.Cells[currentLine, 2] = arr[i].ToArray()[j].Material;
                        sheet.Cells[currentLine, 3] = arr[i].ToArray()[j].Date;
                        sheet.Cells[currentLine, 4] = arr[i].ToArray()[j].Mark;
                        switch (arr[i].ToArray()[j].Mark)
                        {
                            case 2: Twos++; break;
                            case 3: Threes++; break;
                            case 4: Fours++; break;
                            case 5: Fives++; break;
                            default: NoMarks++; break;
                        }
                        currentLine++;
                    }
                }
                //Итого
                sheet.Cells[currentLine, 1] = $"Итого";
                sheet.Cells[currentLine + 1, 1] = "Оценка '5'";
                sheet.Cells[currentLine + 1, 2] = Fives;
                sheet.Cells[currentLine + 2, 1] = "Оценка '4'";
                sheet.Cells[currentLine + 2, 2] = Fours;
                sheet.Cells[currentLine + 3, 1] = "Оценка '3'";
                sheet.Cells[currentLine + 3, 2] = Threes;
                sheet.Cells[currentLine + 4, 1] = "Оценка '2'";
                sheet.Cells[currentLine + 4, 2] = Twos;
                sheet.Cells[currentLine + 5, 1] = "Оценка не выставлена";
                sheet.Cells[currentLine + 5, 2] = NoMarks;
                //Форматирование значений таблицы
                Excel.Range values = sheet.Range[sheet.Cells[2, 1], sheet.Cells[currentLine, 4]];
                values.EntireColumn.AutoFit();
                values.Font.Size = 12;
                values.Borders.Weight = 1;
                //Формирование диаграммы
                Excel.Range chartRange;

                Excel.ChartObjects xlCharts = (Excel.ChartObjects)sheet.ChartObjects(Type.Missing);
                Excel.ChartObject chartObj = xlCharts.Add(468, 160, 348, 268); //позиция диаграммы
                Excel.Chart chart = chartObj.Chart;

                chartRange = sheet.Range[sheet.Cells[currentLine + 1, 1], sheet.Cells[currentLine + 5, 2]]; //значения
                chart.SetSourceData(chartRange, System.Reflection.Missing.Value);
                chart.ChartType = Excel.XlChartType.xlPieExploded; 
                chart.HasTitle = true;
                chart.ChartTitle.Text = "Ведомость оценок";
                chart.ApplyDataLabels(Excel.XlDataLabelsType.xlDataLabelsShowPercent, Excel.XlDataLabelsType.xlDataLabelsShowLabel, true, false, false, true, false, true); //видимость надписей

                wb.SaveAs(Statement.FileName);
                wb.Close();
            }
        }
    }
}
