using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proiect.Models
{
    public class Joke
    {
        public string type { get; set; } 
        public string setup { get; set; }
        public string punchline { get; set; }

        [PrimaryKey]
        public int id { get; set; }

    }
}
