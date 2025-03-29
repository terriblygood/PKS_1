using System;
using System.Linq;
using System.Windows;
using pks.models;

namespace pks
{
    public partial class AddEditBookWindow : Window
    {
        private ContentContext _context;
        private Book _currentBook;

        public AddEditBookWindow(ContentContext context)
        {
            InitializeComponent();
            _context = context;

            AuthorComboBox.ItemsSource = _context.Authors.ToList();
            GenreComboBox.ItemsSource = _context.Genres.ToList();
        }

        public AddEditBookWindow(ContentContext context, Book book) : this(context)
        {
            _currentBook = book;

            TitleTextBox.Text = book.Title;
            PublishYearTextBox.Text = book.PublishYear.ToString();
            AuthorComboBox.SelectedItem = book.Author;
            GenreComboBox.SelectedItem = book.Genre;
            QuantityInStockTextBox.Text = book.QuantityInStock.ToString(); // Привязка для количества в наличии
            ISBNTextBox.Text = book.ISBN; // Привязка для ISBN
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Проверяем на пустые поля
                if (string.IsNullOrWhiteSpace(TitleTextBox.Text) || string.IsNullOrWhiteSpace(PublishYearTextBox.Text) ||
                    AuthorComboBox.SelectedItem == null || GenreComboBox.SelectedItem == null ||
                    string.IsNullOrWhiteSpace(QuantityInStockTextBox.Text) || string.IsNullOrWhiteSpace(ISBNTextBox.Text))
                {
                    MessageBox.Show("Пожалуйста, заполните все поля.");
                    return;
                }

                // Преобразуем количество в наличии в число
                int quantityInStock;
                if (!int.TryParse(QuantityInStockTextBox.Text, out quantityInStock))
                {
                    MessageBox.Show("Количество в наличии должно быть числом.");
                    return;
                }

                // Преобразуем год публикации в число
                int publishYear;
                if (!int.TryParse(PublishYearTextBox.Text, out publishYear))
                {
                    MessageBox.Show("Год издания должен быть числом.");
                    return;
                }

                // Если это редактирование существующей книги
                if (_currentBook != null)
                {
                    _currentBook.Title = TitleTextBox.Text;
                    _currentBook.PublishYear = publishYear;  // Используем преобразованное число
                    _currentBook.AuthorId = ((Author)AuthorComboBox.SelectedItem).Id;
                    _currentBook.GenreId = ((Genre)GenreComboBox.SelectedItem).Id;
                    _currentBook.QuantityInStock = quantityInStock;  // Обновляем количество
                    _currentBook.ISBN = ISBNTextBox.Text;  // Обновляем ISBN
                }
                else
                {
                    // Создание новой книги
                    var newBook = new Book
                    {
                        Title = TitleTextBox.Text,
                        PublishYear = publishYear,  // Используем преобразованное число
                        AuthorId = ((Author)AuthorComboBox.SelectedItem).Id,
                        GenreId = ((Genre)GenreComboBox.SelectedItem).Id,
                        QuantityInStock = quantityInStock,  // Устанавливаем количество
                        ISBN = ISBNTextBox.Text  // Устанавливаем ISBN (теперь это строка)
                    };

                    _context.Books.Add(newBook);
                }

                _context.SaveChanges();  // Сохраняем изменения в базе данных

                // Устанавливаем DialogResult после того, как данные были сохранены
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}");
            }
        }

    }
}
