using Graduate.Classes;
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

namespace Graduate.UserControls
{
    /// <summary>
    /// Логика взаимодействия для ReportsStudent.xaml
    /// </summary>
    public partial class ReportsStudent : UserControl
    {
        public ReportsStudent()
        {
            InitializeComponent();
        }
        public List<Views.ReportView> ListView { get; set; }
        public static string Lab { get; set; } = string.Empty;
        private void Load()
        {
            ListView = new List<Views.ReportView>();
            var query = from report in App._context.Journal
                        where report.Date <= App.EndPeriod
                        && report.Date >= App.StartPeriod
                        join mat in App._context.Materials on report.Lab equals mat.Id
                        join st in App._context.Students on report.Student equals st.Id
                        where st.Id == App.user
                        join gr in App._context.Groups on st.Group equals gr.Id
                        where mat.Title == (string.IsNullOrEmpty(Lab) ? mat.Title : Lab)
                        select new Views.ReportView
                        {
                            Report = report,
                            Lab = mat.Title
                        };
            ListView = query.ToList();
            foreach (var row in query)
            {
                Debug.WriteLine(row.Report);
                Debug.WriteLine(row.Lab);
            }
            ReportsItems.Collection = ListView;
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var labs = from lab in App._context.Materials
                       where lab.Type == "Работа"
                       select lab.Title;
            LabsComboBox.ItemsSource = labs.ToList();
            LabsComboBox.SelectedIndex = 0;
            Load();
        }
        private void UserControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var obj = e.OriginalSource as FrameworkElement;
            if (obj != null)
            {
                if (obj.DataContext != null)
                {
                    var report = obj.DataContext as Views.ReportView;
                    Debug.WriteLine("Тыкнута карточка, запускаю этот файл");
                    var repObj = App._context.Journal.Find(report.Report.Id);
                    var bytes = repObj.File;
                    var student = App._context.Students.Find(App.user);
                    string file_name = report.Lab + student.Name + repObj.Ext;
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
                var report = element.DataContext as Views.ReportView;
                Debug.WriteLine(element.DataContext);
                var page = new ReportWindow(report.Report.Id)
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
                var report = element.DataContext as Views.ReportView;
                var repObj = App._context.Journal.Find(report.Report.Id);
                App._context.Journal.Remove(repObj);
                App._context.SaveChanges();
            }
            Load();
        }

        private void AddReport_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ReportFile = new OpenFileDialog()
            {
                Filter = "DOCX (*.docx)|*.docx|PDF (*.pdf)|*.pdf|DOC (*.doc)|*.doc|TXT (*.txt)|*.txt|All files (*.*)|*.*",
                RestoreDirectory = false,
                Multiselect = false
            };
            if (ReportFile.ShowDialog() == true)
            {
                FileInfo file = new FileInfo(ReportFile.FileName);
                var query = App._context.Materials.Where(x => x.Title == LabsComboBox.Text).FirstOrDefault();
                if (query != null)
                {
                    var report = new Journal()
                    {
                        File = File.ReadAllBytes(ReportFile.FileName),
                        Ext = file.Extension,
                        Lab = query.Id
                    };
                    Debug.WriteLine(report.Id);
                    Debug.WriteLine(report.Date);
                    Debug.WriteLine(report.Student);
                    Debug.WriteLine(report.File);
                    Debug.WriteLine(report.Ext);
                    Debug.WriteLine(report.Lab);
                    App._context.Journal.Add(report);
                    App._context.SaveChanges();
                    Load();
                }
                else
                {
                    MessageBox.Show("Выберете лабораторную работу");
                }
            }
        }

        private void StartDP_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            App.StartPeriod = StartDP.SelectedDate;
            Load();
        }

        private void EndDP_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            App.EndPeriod = EndDP.SelectedDate;
            Load();
        }

        private void LabsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Lab = LabsComboBox.SelectedValue == null ? string.Empty : LabsComboBox.SelectedValue.ToString();
            Load();
        }
    }
}
