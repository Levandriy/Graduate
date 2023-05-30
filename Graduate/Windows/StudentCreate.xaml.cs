using Graduate.Models;
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
using Graduate.Classes;

namespace Graduate.Windows
{
    /// <summary>
    /// Логика взаимодействия для StudentCreate.xaml
    /// </summary>
    public partial class StudentCreate : Window
    {
        public StudentCreate(Students student)
        {
            InitializeComponent();
            View = new Views.StudentView()
            {
                Student = student
            };
            MainPanel.DataContext = View;
        }
        
        public Views.StudentView View { get; set; }

        private void Enter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var view = MainPanel.DataContext as Views.StudentView;
                var gr = App._context.Groups.Where(x => x.Title == view.Gr).Select(x => x.Id).FirstOrDefault();
                if (gr != null)
                {
                    view.Student.Group = gr;
                    App._context.Students.Add(view.Student);
                    App._context.SaveChanges();
                    App.user = view.Student.Id;
                    var window = new StudentWindow();
                    window.Show();
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var win = new MainWindow();
            win.Show();
        }
    }
}
