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
using pks.models;

namespace pks
{
    public partial class ManageAuthorsWindow : Window
    {
        private ContentContext _context;

        public ManageAuthorsWindow(ContentContext context)
        {
            InitializeComponent();
            _context = context;
            LoadAuthors();
        }

        private void LoadAuthors()
        {
            // Загружаем всех авторов
            AuthorsListBox.ItemsSource = _context.Authors.ToList();
        }

        private void EditAuthorButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedAuthor = AuthorsListBox.SelectedItem as Author;
            if (selectedAuthor != null)
            {
                // Открываем окно редактирования
                var editAuthorWindow = new AddEditAuthorWindow(_context, selectedAuthor);
                if (editAuthorWindow.ShowDialog() == true)
                {
                    _context.SaveChanges();
                    LoadAuthors(); // Перезагружаем авторов после редактирования
                }
            }
        }

        private void AddAuthorButton_Click(object sender, RoutedEventArgs e)
        {
            var addAuthorWindow = new AddEditAuthorWindow(_context);
            if (addAuthorWindow.ShowDialog() == true)
            {
                _context.SaveChanges();
                LoadAuthors(); // Перезагружаем авторов после добавления
            }
        }

        private void DeleteAuthorButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedAuthor = AuthorsListBox.SelectedItem as Author;
            if (selectedAuthor != null)
            {
                _context.Authors.Remove(selectedAuthor);
                _context.SaveChanges();
                LoadAuthors(); // Перезагружаем авторов после удаления
            }
        }
    }
}
