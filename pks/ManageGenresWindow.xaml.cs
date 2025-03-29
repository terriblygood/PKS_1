using System.Linq;
using System.Windows;
using pks.models;

namespace pks
{
    public partial class ManageGenresWindow : Window
    {
        private ContentContext _context;

        public ManageGenresWindow(ContentContext context)
        {
            InitializeComponent();
            _context = context;
            LoadGenres();
        }

        private void LoadGenres()
        {
            // Загружаем все жанры
            GenresListBox.ItemsSource = _context.Genres.ToList();
        }

        private void EditGenreButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedGenre = GenresListBox.SelectedItem as Genre;
            if (selectedGenre != null)
            {
                // Открываем окно редактирования
                var editGenreWindow = new AddEditGenreWindow(_context, selectedGenre);
                if (editGenreWindow.ShowDialog() == true)
                {
                    _context.SaveChanges();
                    LoadGenres();
                }
            }
        }

        private void AddGenreButton_Click(object sender, RoutedEventArgs e)
        {
            var addGenreWindow = new AddEditGenreWindow(_context);
            if (addGenreWindow.ShowDialog() == true)
            {
                _context.SaveChanges();
                LoadGenres();
            }
        }

        private void DeleteGenreButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedGenre = GenresListBox.SelectedItem as Genre;
            if (selectedGenre != null)
            {
                _context.Genres.Remove(selectedGenre);
                _context.SaveChanges();
                LoadGenres();
            }
        }
    }
}
