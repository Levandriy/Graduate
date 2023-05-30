using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Graduate.Models;
using Graduate.UserControls;
using Microsoft.Win32;

namespace Graduate.Windows
{
    /// <summary>
    /// Логика взаимодействия для TeacherWindow.xaml
    /// </summary>
    public partial class TeacherWindow : Window
    {
        public TeacherWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
        private void MenuButton_Checked(object sender, RoutedEventArgs e)
        {
            foreach (var b in SidePanel.Children)
            {
                if (b is ToggleButton)
                {
                    var local = b as ToggleButton;
                    local.IsChecked = local == sender as ToggleButton;
                }
            }
        }

        private void TextBooks_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new TextBooks();
        }

        private void Practice_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new Labs();
        }

        private void Reports_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new Reports();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var window = new MainWindow();
            window.Show();
        }

        private void Window_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ConCon.Content = DataContext;
        }
    }
}
