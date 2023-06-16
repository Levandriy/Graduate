using Graduate.Classes;
using Graduate.UserControls;
using Graduate.Windows;
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
using System.Windows.Shapes;

namespace Graduate.Forms
{
    /// <summary>
    /// Логика взаимодействия для ReportWindowTeacher.xaml
    /// </summary>
    public partial class ReportWindowTeacher : Window
    {
        public ReportWindowTeacher(Guid guid)
        {
            InitializeComponent();
            var report = App._context.Journal.Find(guid);
            if (report != null)
            {
                var lab = App._context.Materials.Find(report.Lab).Title;
                var student = App._context.Students.Find(report.Student);
                var group = App._context.Groups.Find(student.Group).Title;
                View = new Views.ReportView()
                {
                    Report = report,
                    Lab = lab,
                    Student = student.Name,
                    Group = group,
                    Groups = App._context.Groups.Where(x => x.Teacher == App.user).Select(x => x.Title).ToList()
                };
            }
        }

        public Views.ReportView View { get; set; }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MainPanel.DataContext = View;
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            var rep = MainPanel.DataContext as Views.ReportView;
            string file_name = $"{rep.Lab} {rep.Student}{rep.Report.Ext}";
            string file_path = $"{Environment.CurrentDirectory}\\{file_name}";
            File.WriteAllBytes(file_path, rep.Report.File);
            Process.Start(file_path);
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var rep = App._context.Journal.Find(View.Report.Id);
                App._context.Journal.Remove(rep);
                App._context.SaveChanges();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var rep = App._context.Journal.Find(View.Report.Id);
                var groupid = from gr in App._context.Groups
                              where gr.Title == View.Group
                              select gr.Id;
                var student = App._context.Students.Find(View.Report.Student);
                student.Group = groupid.FirstOrDefault();
                App._context.Students.AddOrUpdate(student);
                App._context.Journal.AddOrUpdate(rep);
                App._context.SaveChanges();
                MessageBox.Show("Сохранение успешно");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var win = Owner as TeacherWindow;
            if (win != null)
            {
                win.DataContext = new Reports();
            }
        }
    }
}
