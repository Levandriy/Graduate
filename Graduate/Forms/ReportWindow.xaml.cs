using Graduate.Models;
using Graduate.UserControls;
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
using Graduate.Classes;
using Graduate.Windows;

namespace Graduate.Forms
{
    /// <summary>
    /// Логика взаимодействия для ReportWindow.xaml
    /// </summary>
    public partial class ReportWindow : Window
    {
        public ReportWindow(Guid guid)
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
                    Labs = App._context.Materials.Where(x => x.Type == "Работа").Select(x => x.Title).ToList()
                };
            }
        }
        
        public Views.ReportView View { get; set; }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MainPanel.DataContext = View;
        }

        private void ChooseFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog report = new OpenFileDialog()
            {
                Filter = "PDF (*.pdf)|*.pdf|DOCX (*.docx)|*.docx|DOC (*.doc)|*.doc|TXT (*.txt)|*.txt|All files (*.*)|*.*",
                RestoreDirectory = false,
                Multiselect = false
            };
            if (report.ShowDialog() == true)
            {
                try
                {
                    FileInfo fileInfo = new FileInfo(report.FileName);
                    View.Report.File = File.ReadAllBytes(fileInfo.FullName);
                    View.Report.Ext = fileInfo.Extension;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    MessageBox.Show(ex.Message);
                }
            }
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
                App._context.Journal.AddOrUpdate(rep);
                App._context.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var win = Owner as StudentWindow;
            if (win != null)
            {
                win.DataContext = new ReportsStudent();
            }
        }
    }
}
