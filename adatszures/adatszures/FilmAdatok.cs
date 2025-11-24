using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace adatszures
{
    internal class FilmAdatok
    {
        public double rating { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string[] genre { get; set; }
        public int length { get; set; }

        public FilmAdatok(string[] input)
        {
            rating = Math.Round(Convert.ToDouble(input[0].Replace(".", ",")), 1);
            name = input[1];
            type = input[2];
            genre = input[3].Split(',');
            length = int.Parse(input[4]);
        }

    }
}


