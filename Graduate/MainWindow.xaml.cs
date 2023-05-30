using Graduate.Models;
using Graduate.Windows;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Graduate
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Teachers user = new Teachers();
            UserPanel.DataContext = user;
        }
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (UserPanel.DataContext is Teachers)
            {
                Teachers user = UserPanel.DataContext as Teachers;
                var query = App._context.Teachers.Where(x => x.Login == user.Login);
                if (query.Any())
                {
                    query = query.Where(x => x.Password == user.Password);
                    if (query.Any())
                    {
                        App.user = query.FirstOrDefault().Id;
                        var window = new TeacherWindow();
                        window.Show();
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("Неправильный пароль");
                    }
                }
                else
                {
                    App._context.Teachers.Add(user);
                    App._context.SaveChanges();
                }
            }
            if (UserPanel.DataContext is Students)
            {
                Students user = UserPanel.DataContext as Students;
                Debug.WriteLine("Вход студента");
                var query = App._context.Students.Where(x => x.Login == user.Login);
                if (query.Any())
                {
                    query = query.Where(x => x.Password == user.Password);
                    if (query.Any())
                    {
                        App.user = query.FirstOrDefault().Id;
                        var window = new StudentWindow();
                        window.Show();
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("Неправильный пароль");
                    }
                }
                else
                {
                    StudentCreate create = new StudentCreate(user);
                    create.Show();
                    Close();
                }
            }
            
        }

        private void StudentItem_Selected(object sender, RoutedEventArgs e)
        {
            UserPanel.DataContext = new Students();
        }

        private void TeacherItem_Selected(object sender, RoutedEventArgs e)
        {
            UserPanel.DataContext = new Teachers();
        }

        private void ProgInfoButton_Click(object sender, RoutedEventArgs e)
        {
            var win = new About();
            win.Show();
        }

        private void PassBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (UserPanel.DataContext is Teachers)
            {
                Teachers user = UserPanel.DataContext as Teachers;
                user.Password = PassBox.Password;
            }
            else if (UserPanel.DataContext is Students)
            {
                Students user = UserPanel.DataContext as Students;
                user.Password = PassBox.Password;
            }
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.F12))
            {
                var admin = new AdminWindow();
                admin.Show();
                this.Hide();
            }
        }
    }
}
