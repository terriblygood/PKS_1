using pks.models;
using System.Windows;

namespace pks
{
    public partial class AddEditGenreWindow : Window
    {
        private ContentContext _context;
        private Genre _currentGenre;

        public AddEditGenreWindow(ContentContext context)
        {
            InitializeComponent();
            _context = context;
        }

        public AddEditGenreWindow(ContentContext context, Genre genre) : this(context)
        {
            _currentGenre = genre;
            NameTextBox.Text = genre.Name;
            DescriptionTextBox.Text = genre.Description;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameTextBox.Text) || string.IsNullOrWhiteSpace(DescriptionTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return;
            }

            if (_currentGenre != null)
            {
                _currentGenre.Name = NameTextBox.Text;
                _currentGenre.Description = DescriptionTextBox.Text;
            }
            else
            {
                var newGenre = new Genre
                {
                    Name = NameTextBox.Text,
                    Description = DescriptionTextBox.Text
                };

                _context.Genres.Add(newGenre);
            }

            _context.SaveChanges();
            MessageBox.Show("Жанр сохранен.");
            this.DialogResult = true;
            this.Close();
        }
    }
}