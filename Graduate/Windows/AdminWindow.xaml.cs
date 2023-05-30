using Graduate.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Diagnostics;
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
using Excel = Microsoft.Office.Interop.Excel;

namespace Graduate.Windows
{
    /// <summary>
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();
        }
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GroupCB.ItemsSource = App._context.Groups.Select(x => x.Title).ToList();
            TeacherCB.ItemsSource = App._context.Teachers.Select(x => x.Name).ToList();
        }
        private void TeachersAdd_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog()
            {
                Multiselect = false,
                RestoreDirectory = false
            };
            if(fileDialog.ShowDialog() == true)
            {
                Excel.Application xlApp = new Excel.Application();
                Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(fileDialog.FileName);
                Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
                Excel.Range xlRange = xlWorksheet.UsedRange;
                var error = false;
                var err_line = 0;
                for (int i = 1; i <= xlRange.Rows.Count; i++)
                {
                    var teacher = new Teachers()
                    {
                        Name = xlRange.Cells[i, 1].Value2.ToString(),
                        Login = xlRange.Cells[i, 2].Value2.ToString(),
                        Password = xlRange.Cells[i, 3].Value2.ToString()
                    };
                    try
                    {
                        App._context.Teachers.Add(teacher);
                        App._context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        error = true;
                        err_line = i;
                        Debug.WriteLine(ex.Message);
                    }
                    for (int j = 1; j <= xlRange.Columns.Count; j++)
                    {
                        if (xlRange.Cells[i, j] != null && xlRange.Cells[i, j].Value2 != null)
                        {
                            string got = xlRange.Cells[i, j].Value2.ToString();
                            Debug.WriteLine(got);
                        }
                    }
                }
                if (error)
                {
                    MessageBox.Show($"Строка {err_line} дала ошибку, похоже, неправильная форма данных. Первый столбец - ФИО, Второй - Логин, Третий - Пароль.");
                }
                xlWorkbook.Close();
            }
            
        }

        private void TeachersDelete_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog()
            {
                Multiselect = false,
                RestoreDirectory = false
            };
            if (fileDialog.ShowDialog() == true)
            {
                Excel.Application xlApp = new Excel.Application();
                Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(fileDialog.FileName);
                Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
                Excel.Range xlRange = xlWorksheet.UsedRange;
                var error = false;
                var err_line = 0;
                for (int i = 1; i <= xlRange.Rows.Count; i++)
                {
                    string name = xlRange.Cells[i, 1].Value2.ToString();
                    var teacher = App._context.Teachers.Where(x => x.Name == name).FirstOrDefault();
                    try
                    {
                        App._context.Teachers.Remove(teacher);
                        App._context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        error = true;
                        err_line = i;
                        Debug.WriteLine(ex.Message);
                    }
                    for (int j = 1; j <= xlRange.Columns.Count; j++)
                    {
                        if (xlRange.Cells[i, j] != null && xlRange.Cells[i, j].Value2 != null)
                        {
                            string got = xlRange.Cells[i, j].Value2.ToString();
                            Debug.WriteLine(got);
                        }
                    }
                }
                if (error)
                {
                    MessageBox.Show($"Строка {err_line} дала ошибку, похоже, неправильная форма данных. Первый столбец - ФИО");
                }
                xlWorkbook.Close();
            }
        }

        private void StudentsAdd_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog()
            {
                Multiselect = false,
                RestoreDirectory = false
            };
            if (fileDialog.ShowDialog() == true)
            {
                Excel.Application xlApp = new Excel.Application();
                Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(fileDialog.FileName);
                Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
                Excel.Range xlRange = xlWorksheet.UsedRange;
                var error = false;
                var err_line = 0;
                for (int i = 1; i <= xlRange.Rows.Count; i++)
                {
                    var gr = App._context.Groups.Where(x => x.Title == GroupCB.SelectedValue.ToString()).FirstOrDefault();
                    var student = new Students
                    {
                        Name = xlRange.Cells[i, 1].Value2.ToString(),
                        Login = xlRange.Cells[i, 2].Value2.ToString(),
                        Password = xlRange.Cells[i, 3].Value2.ToString(),
                        Group = gr.Id
                    };
                    try
                    {
                        App._context.Students.Add(student);
                        App._context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        if (!error)
                        {
                            error = true;
                            err_line = i;
                        }
                        Debug.WriteLine(ex.Message);
                    }
                    for (int j = 1; j <= xlRange.Columns.Count; j++)
                    {
                        if (xlRange.Cells[i, j] != null && xlRange.Cells[i, j].Value2 != null)
                        {
                            string got = xlRange.Cells[i, j].Value2.ToString();
                            Debug.WriteLine(got);
                        }
                    }
                }
                if (error)
                {
                    MessageBox.Show($"Строка {err_line} дала ошибку, похоже, неправильная форма данных. Первый столбец - ФИО, Второй - Логин, Третий - Пароль.");
                }
                xlWorkbook.Close();
            }
        }

        private void StudentsDelete_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog()
            {
                Multiselect = false,
                RestoreDirectory = false
            };
            if (fileDialog.ShowDialog() == true)
            {
                Excel.Application xlApp = new Excel.Application();
                Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(fileDialog.FileName);
                Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
                Excel.Range xlRange = xlWorksheet.UsedRange;
                var error = false;
                var err_line = 0;
                for (int i = 1; i <= xlRange.Rows.Count; i++)
                {
                    string name = xlRange.Cells[i, 1].Value2.ToString();
                    var student = App._context.Students.Where(x => x.Name == name).FirstOrDefault();
                    try
                    {
                        App._context.Students.Remove(student);
                        App._context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        if (!error)
                        {
                            error = true;
                            err_line = i;
                        }
                        Debug.WriteLine(ex.Message);
                    }
                    for (int j = 1; j <= xlRange.Columns.Count; j++)
                    {
                        if (xlRange.Cells[i, j] != null && xlRange.Cells[i, j].Value2 != null)
                        {
                            string got = xlRange.Cells[i, j].Value2.ToString();
                            Debug.WriteLine(got);
                        }
                    }
                }
                if (error)
                {
                    MessageBox.Show($"Строка {err_line} дала ошибку, похоже, неправильная форма данных. Первый столбец - ФИО, Второй - Логин, Третий - Пароль.");
                }
                xlWorkbook.Close();
            }
        }

        private void GroupAdd_Click(object sender, RoutedEventArgs e)
        {
            var teacher = App._context.Teachers.Where(x => x.Name == TeacherCB.SelectedValue.ToString()).FirstOrDefault();
            var gr = new Groups()
            {
                Title = GroupTitleBox.Text,
                Specialty = GroupSpecBox.Text,
                Teacher = teacher.Id
            };
            App._context.Groups.Add(gr);
            App._context.SaveChanges();
        }

        private void GroupDelete_Click(object sender, RoutedEventArgs e)
        {
            var gr = App._context.Groups.Where(x => x.Title == GroupTitleBox.Text).FirstOrDefault();
            var check = App._context.Students.Where(x => x.Group == gr.Id);
            if (check.Any())
            {
                MessageBox.Show("Ещё есть студенты, привязанные к этой группе. Вначале удалите этих студентов");
            }
            else
            {
                App._context.Groups.Remove(gr);
                App._context.SaveChanges();
            }
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            GroupCB.ItemsSource = App._context.Groups.Select(x => x.Title).ToList();
            TeacherCB.ItemsSource = App._context.Teachers.Select(x => x.Name).ToList();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var mainwin = new MainWindow();
            mainwin.Show();
        }
    }
}
