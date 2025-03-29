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
    public partial class AddEditAuthorWindow : Window
    {
        private ContentContext _context;
        private Author _currentAuthor;

        public AddEditAuthorWindow(ContentContext context)
        {
            InitializeComponent();
            _context = context;
        }

        public AddEditAuthorWindow(ContentContext context, Author author) : this(context)
        {
            _currentAuthor = author;
            FirstNameTextBox.Text = author.FirstName;
            LastNameTextBox.Text = author.LastName;
            CountryTextBox.Text = author.Country;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FirstNameTextBox.Text) || string.IsNullOrWhiteSpace(LastNameTextBox.Text) || string.IsNullOrWhiteSpace(CountryTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return;
            }

            if (_currentAuthor != null)
            {
                _currentAuthor.FirstName = FirstNameTextBox.Text;
                _currentAuthor.LastName = LastNameTextBox.Text;
                _currentAuthor.Country = CountryTextBox.Text;
            }
            else
            {
                var newAuthor = new Author
                {
                    FirstName = FirstNameTextBox.Text,
                    LastName = LastNameTextBox.Text,
                    Country = CountryTextBox.Text
                };

                _context.Authors.Add(newAuthor);
            }

            _context.SaveChanges();
            MessageBox.Show("Автор сохранен.");
            this.DialogResult = true;
            this.Close();
        }
    }
}
