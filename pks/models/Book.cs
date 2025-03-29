using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pks.models;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int AuthorId { get; set; }  // Ссылка на таблицу Author
    public Author Author { get; set; }
    public int PublishYear { get; set; }  // Год издания
    public string ISBN { get; set; }  // Изменили тип на string
    public int GenreId { get; set; }  // Ссылка на таблицу Genre
    public Genre Genre { get; set; }
    public int QuantityInStock { get; set; }
}
