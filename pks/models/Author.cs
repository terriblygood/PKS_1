using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pks.models
{
    public class Author
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? BirthDate { get; set; }  // Nullable, так как в базе данных может быть NULL
        public string Country { get; set; }
        public string FullName => $"{FirstName} {LastName}";

        public List<Book> Books { get; set; }
    }
}
