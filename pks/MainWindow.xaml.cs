using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.EntityFrameworkCore;
using pks.models;

namespace pks
{
    public partial class MainWindow : Window
    {
        private ContentContext _context;

        public MainWindow()
        {
            InitializeComponent();
            _context = new ContentContext();
            LoadBooks();
            LoadFilters();
        }

        private void LoadBooks()
        {
            var query = _context.Books
                .Include(b => b.Author)
                .Include(b => b.Genre)
                .AsQueryable();

            // Применение фильтров по названию, автору и жанру
            var queryText = SearchTextBox.Text.ToLower();
            var authorFilter = AuthorFilterComboBox.SelectedItem as Author;
            var genreFilter = GenreFilterComboBox.SelectedItem as Genre;

            if (!string.IsNullOrEmpty(queryText))
            {
                query = query.Where(b => b.Title.ToLower().Contains(queryText));
            }
            if (authorFilter != null)
            {
                query = query.Where(b => b.AuthorId == authorFilter.Id);
            }
            if (genreFilter != null)
            {
                query = query.Where(b => b.GenreId == genreFilter.Id);
            }

            // Обновляем источник данных в DataGrid
            BooksGrid.ItemsSource = query.ToList();

            // Подсчитываем общее количество книг
            int totalBooks = query.Sum(b => b.QuantityInStock);
            TotalBooksTextBlock.Text = $"Общее количество книг в наличии: {totalBooks}";
        }




        public void UpdateAuthorAndGenreFilters()
        {
            // Обновляем список авторов и жанров в ComboBox
            AuthorFilterComboBox.ItemsSource = _context.Authors.ToList();
            GenreFilterComboBox.ItemsSource = _context.Genres.ToList();

            // Применяем фильтры заново
            ApplyFilters();
        }





        private void LoadFilters()
        {
            AuthorFilterComboBox.ItemsSource = _context.Authors.ToList();
            GenreFilterComboBox.ItemsSource = _context.Genres.ToList();
        }


        private void ApplyFilters()
        {
            // Получаем выбранные фильтры
            var authorFilter = AuthorFilterComboBox.SelectedItem as Author;
            var genreFilter = GenreFilterComboBox.SelectedItem as Genre;
            string query = SearchTextBox.Text.ToLower(); // Получаем текст из поиска по названию

            // Начинаем с всех книг
            var queryableBooks = _context.Books
                .Include(b => b.Author)
                .Include(b => b.Genre)
                .AsQueryable();

            // Фильтрация по названию
            if (!string.IsNullOrEmpty(query))
            {
                queryableBooks = queryableBooks.Where(b => b.Title.ToLower().Contains(query));
            }

            // Фильтрация по автору
            if (authorFilter != null)
            {
                queryableBooks = queryableBooks.Where(b => b.AuthorId == authorFilter.Id);
            }

            // Фильтрация по жанру
            if (genreFilter != null)
            {
                queryableBooks = queryableBooks.Where(b => b.GenreId == genreFilter.Id);
            }

            // Обновляем источник данных в DataGrid
            BooksGrid.ItemsSource = queryableBooks.ToList();
        }



        private void AuthorFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters(); // Применяем фильтрацию при изменении авторов
        }

        private void GenreFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters(); // Применяем фильтрацию при изменении жанров
        }






        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters(); // Применяем фильтрацию при изменении текста в поле поиска
        }




        private void SearchTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (SearchTextBox.Text == "Поиск по названию")
            {
                SearchTextBox.Text = "";
                WatermarkText.Visibility = Visibility.Collapsed;  // Скрыть Watermark
            }
        }

        private void SearchTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchTextBox.Text))
            {
                WatermarkText.Visibility = Visibility.Visible;  // Показать Watermark
                SearchTextBox.Text = "Поиск по названию";  // Установить текст по умолчанию
            }
        }

        private void AddBookButton_Click(object sender, RoutedEventArgs e)
        {
            // Создаем окно для добавления книги
            var addBookWindow = new AddEditBookWindow(_context);

            // Отображаем окно как диалоговое
            bool? result = addBookWindow.ShowDialog();  // Используем ShowDialog() для блокировки до закрытия окна

            // После закрытия окна проверяем результат
            if (result == true)
            {
                LoadBooks(); // Перезагружаем книги после добавления
            }
        }


        private void EditBookButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedBook = BooksGrid.SelectedItem as Book;
            if (selectedBook != null)
            {
                // Передаем выбранную книгу в окно редактирования
                var editBookWindow = new AddEditBookWindow(_context, selectedBook);

                // Открываем окно как диалог
                bool? result = editBookWindow.ShowDialog();

                // После того, как диалог завершился, перезагружаем данные
                if (result == true)
                {
                    LoadBooks();  // Перезагружаем книги после редактирования
                }
            }
            else
            {
                MessageBox.Show("Выберите книгу для редактирования.");
            }
        }


        private void DeleteBookButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedBook = BooksGrid.SelectedItem as Book;
            if (selectedBook != null)
            {
                var result = MessageBox.Show("Вы уверены, что хотите удалить эту книгу?",
                                             "Подтверждение",
                                             MessageBoxButton.YesNo,
                                             MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    _context.Books.Remove(selectedBook);
                    _context.SaveChanges();
                    LoadBooks();
                }
            }
            else
            {
                MessageBox.Show("Выберите книгу для удаления.");
            }
        }

        private void ManageGenresButton_Click(object sender, RoutedEventArgs e)
        {
            var manageGenresWindow = new ManageGenresWindow(_context);
            manageGenresWindow.ShowDialog(); // Ожидаем, пока окно не закроется

            // После того как окно закрыто, обновляем фильтры
            UpdateAuthorAndGenreFilters();
        }

        private void ManageAuthorsButton_Click(object sender, RoutedEventArgs e)
        {
            var manageAuthorsWindow = new ManageAuthorsWindow(_context);
            manageAuthorsWindow.ShowDialog(); // Ожидаем, пока окно не закроется

            // После того как окно закрыто, обновляем фильтры
            UpdateAuthorAndGenreFilters();
        }




        private void ResetFiltersButton_Click(object sender, RoutedEventArgs e)
        {
            // Сбрасываем выбранные фильтры
            AuthorFilterComboBox.SelectedItem = null;
            GenreFilterComboBox.SelectedItem = null;

            // Очищаем поле поиска
            SearchTextBox.Text = string.Empty;

            // Загружаем все книги
            LoadBooks();
        }



        private void SearchTextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }
    }
}
