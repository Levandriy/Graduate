using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Builders;
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
using Graduate.Models;
using Graduate.UserControls;
using Graduate.Windows;
using Microsoft.Win32;

namespace Graduate.Forms
{
    /// <summary>
    /// Логика взаимодействия для MaterialWindow.xaml
    /// </summary>
    public partial class MaterialWindow : Window
    {
        public MaterialWindow(Guid guid, string type)
        {
            InitializeComponent();
            var query = App._context.Materials.Find(guid);
            View.Material = query ?? new Materials() { Type = type };
            GetTags(guid);
        }
        private void GetTags(Guid guid)
        {
            var query = App._context.Materials.Find(guid);
            if (query != null)
            {
                var tags = from cons in App._context.Theme_cons
                           join mat in App._context.Materials on cons.Matrial equals mat.Id
                           join theme in App._context.Themes on cons.Theme equals theme.Id
                           where mat.Id == query.Id
                           select new Tag()
                           {
                               ConsId = cons.Id,
                               ThemeName = theme.Title
                           };
                View.Tags = tags.ToList();
            }
        }
        public class MatView
        {
            public Materials Material { get; set; }
            public List<Tag> Tags { get; set; }
            public List<string> Types { get; set; } = new List<string>() { "Учебник", "Работа" };
            public List<string> Themes { get; set; } = App._context.Themes.Select(x => x.Title).ToList();
        }
        public MatView View = new MatView();
        public class Tag
        {
            public Guid ConsId { get; set; }
            public string ThemeName { get; set; }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MainPanel.DataContext = View;
        }

        private void ChooseFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog text_book = new OpenFileDialog()
            {
                Filter = "PDF (*.pdf)|*.pdf|DOCX (*.docx)|*.docx|DOC (*.doc)|*.doc|TXT (*.txt)|*.txt|All files (*.*)|*.*",
                RestoreDirectory = false,
                Multiselect = true
            };
            if (text_book.ShowDialog() == true)
            {
                try
                {
                    if (text_book.FileNames.Count() > 1)
                    {
                        foreach (string file in text_book.FileNames)
                        {
                            FileInfo fileInfo = new FileInfo(file);
                            Materials material = new Materials()
                            {
                                Title = System.IO.Path.GetFileNameWithoutExtension(fileInfo.FullName),
                                File = File.ReadAllBytes(fileInfo.FullName),
                                Ext = fileInfo.Extension,
                                Type = View.Material.Type
                            };
                            Debug.WriteLine(material.Id);
                            Debug.WriteLine(material.Title);
                            Debug.WriteLine(material.File);
                            Debug.WriteLine(material.Ext);
                            Debug.WriteLine(material.Type);
                            App._context.Materials.Add(material);
                        }
                        App._context.SaveChanges();
                        Close();
                    }
                    else
                    {
                        foreach (string file in text_book.FileNames)
                        {
                            FileInfo fileInfo = new FileInfo(file);
                            View.Material.File = File.ReadAllBytes(fileInfo.FullName);
                            View.Material.Ext = fileInfo.Extension;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                App._context.Materials.AddOrUpdate(View.Material);
                App._context.SaveChanges();
                MessageBox.Show("Сохранение успешно");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                App._context.Materials.Remove(View.Material);
                App._context.SaveChanges();
                MessageBox.Show("Удаление успешно");
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DeleteTag_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var tag = button.DataContext as Tag;
            var cons = App._context.Theme_cons.Find(tag.ConsId);
            App._context.Theme_cons.Remove(cons);
            App._context.SaveChanges();
            GetTags(View.Material.Id);
            MainPanel.DataContext = null;
            MainPanel.DataContext = View;
        }

        private void AddThemes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var box = sender as ComboBox;
            if (box?.SelectedValue != null)
            {
                var Theme = box?.SelectedValue.ToString();
                Debug.WriteLine($"Selected: {Theme}");
                var cons = new Theme_cons()
                {
                    Theme = App._context.Themes.Where(x => x.Title == Theme).Select(x => x.Id).FirstOrDefault(),
                    Matrial = View.Material.Id
                };
                App._context.Theme_cons.Add(cons);
                GetTags(View.Material.Id);
                MainPanel.DataContext = null;
                MainPanel.DataContext = View;
            }
            
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var win = Owner as TeacherWindow;
            if (win != null)
            {
                Debug.WriteLine("Нашёл Owner");
                if (View.Material.Type == "Учебник")
                {
                    win.DataContext = new TextBooks();
                    Debug.WriteLine("Поменял контекст на учебники");
                }
                else
                {
                    win.DataContext = new Labs();
                    Debug.WriteLine("Поменял контекст на работы");
                }
                win.Topmost = true;
            }
        }
    }
}
