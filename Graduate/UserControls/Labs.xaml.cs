using Graduate.Forms;
using Graduate.Models;
using System;
using System.Collections.Generic;
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
using Graduate.Classes;
using Graduate.Windows;

namespace Graduate.UserControls
{
    /// <summary>
    /// Логика взаимодействия для Labs.xaml
    /// </summary>
    public partial class Labs : UserControl
    {
        public Labs()
        {
            InitializeComponent();
        }
        public Guid Theme;
        public List<Views.MaterialView> ListView { get; set; }
        private void Load(string text)
        {
            ListView = new List<Views.MaterialView>();
            var query = from mats in App._context.Materials
                        where mats.Type == "Работа"
                        join cons in App._context.Theme_cons on mats.Id equals cons.Matrial
                        join themes in App._context.Themes on cons.Theme equals themes.Id
                        where cons.Owner == App.user
                        && (mats.Title.Contains(string.IsNullOrEmpty(text) ? mats.Title : text)
                        || themes.Title.Contains(string.IsNullOrEmpty(text) ? themes.Title : text))
                        group new
                        {
                            mats.Id,
                            mats.Title,
                        } by mats.Id;
            foreach (var book in query)
            {
                var mat = new Views.MaterialView()
                {
                    Id = book.First().Id,
                    Title = book.First().Title
                };
                ListView.Add(mat);
            }
            var Textbooks = (from mats in App._context.Materials
                             where mats.Type == "Работа"
                             && mats.Title.Contains(string.IsNullOrEmpty(text) ? mats.Title : text)
                             select new Views.MaterialView() { Id = mats.Id, Title = mats.Title }).ToList();
            //Debug.WriteLine(Textbooks.Any());
            foreach (var mat in Textbooks)
            {
                var test = ListView.Select(x => x.Id).Contains(mat.Id);
                if (!test)
                {
                    ListView.Add(mat);
                }
            }
            LabsItems.Collection = ListView;
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Load(string.Empty);
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Load(SearchBox.Text);
        }

        private void AddMaterial_Click(object sender, RoutedEventArgs e)
        {
            var page = new MaterialWindow(new Guid(), "Работа")
            {
                Owner = Window.GetWindow(this)
            };
            page.Show();
        }
        private void ChooseTheme_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var box = sender as ComboBox;
            var item = box.SelectedItem.ToString();
            var query = App._context.Themes.Where(x => x.Title == item).Select(x => x.Id).FirstOrDefault();
            var mat = box.DataContext as Views.MaterialView;
            Theme_cons cons = new Theme_cons()
            {
                Matrial = mat.Id,
                Theme = query,
                Owner = App.user
            };
            App._context.Theme_cons.Add(cons);
            App._context.SaveChanges();
            Load(SearchBox.Text);
        }

        private void UserControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var obj = e.OriginalSource as FrameworkElement;
            if (obj != null)
            {
                if (obj.DataContext != null)
                {
                    var mat = obj.DataContext as Views.MaterialView;
                    Debug.WriteLine("Тыкнута карточка, запускаю этот файл");
                    var matObj = App._context.Materials.Find(mat.Id);
                    var bytes = matObj.File;
                    string file_name = matObj.Title + matObj.Ext;
                    string file_path = $"{Environment.CurrentDirectory}\\{file_name}";
                    File.WriteAllBytes(file_path, bytes);
                    Process.Start(file_path);
                }
            }
        }
        private void EditMaterial_Click(object sender, RoutedEventArgs e)
        {
            var element = sender as FrameworkElement;
            if (element?.DataContext != null)
            {
                var mat = element.DataContext as Views.MaterialView;
                Debug.WriteLine(element.DataContext);
                var page = new MaterialWindow(mat.Id, "Работа")
                {
                    Owner = Window.GetWindow(this)
                };
                page.Show();
            }
        }

        private void DeleteMaterial_Click(object sender, RoutedEventArgs e)
        {
            var element = sender as FrameworkElement;
            if (element?.DataContext != null)
            {
                var mat = element.DataContext as Views.MaterialView;
                var matObj = App._context.Materials.Find(mat.Id);
                var consObj = App._context.Theme_cons.Where(x => x.Owner == App.user).Where(x => x.Matrial == matObj.Id).ToList();
                App._context.Theme_cons.RemoveRange(consObj);
                App._context.Materials.Remove(matObj);
                App._context.SaveChanges();
            }
            Load(SearchBox.Text);
        }
    }
}
