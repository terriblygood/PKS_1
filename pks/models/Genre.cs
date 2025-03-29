using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pks.models
{
    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; } 
        public override string ToString()
        {
            return Name;
        }

        public string Description { get; set; }
        public ICollection<Book> Books { get; set; }
    }
}
