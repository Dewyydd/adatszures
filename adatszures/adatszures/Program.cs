using System.IO;

namespace adatszures 
{
    class Program 
    {
        static void Main(string[] args)
        {
            List<FilmAdatok> BemenetiAdatok = new List<FilmAdatok>();

            foreach (var line in File.ReadAllLines("data.csv").Skip(1)) 
            {
                FilmAdatok temp = new FilmAdatok(line.Split(';'));

                BemenetiAdatok.Add(temp);
            }

            List<string> SGenre = new List<string>();
            List<string> MGenre = new List<string>();

            foreach (var F in BemenetiAdatok) 
            {
                foreach (var G in F.genre) 
                {
                    if (F.type == "series" && !SGenre.Contains(G)) 
                    {
                        SGenre.Add(G);
                    }

                    else if (F.type == "movie" && !MGenre.Contains(G))
                    {
                        MGenre.Add(G);
                    }
                }
            }

            FilmAdatok BekertAdatok = new FilmAdatok(AdatokBekerese(SGenre, MGenre));

            Console.WriteLine("\nSikeres adatbekérés! Nyomj meg egy gombot a folytatáshoz.");
            Console.ReadKey();
            Console.Clear();

            Console.WriteLine(BekertAdatok.rating);
            Console.WriteLine(BekertAdatok.name);
            Console.WriteLine(BekertAdatok.type);
            Console.WriteLine(BekertAdatok.genre[0]);
            Console.WriteLine(BekertAdatok.length);
        }

        static string[] AdatokBekerese(List<string> SGenre, List<string> MGenre) 
        {
            double r = 0.0;
            string n = "BEKÉRT ADATOK";
            string t = "";
            string g = "";
            int l = 0;

            Console.WriteLine("Mi legyen a minimum IMDb értékelés? [pl. 5.5]");
            Console.Write("Értékelés: ");
            try
            {
                r = double.Parse(Console.ReadLine().Trim().Replace(".",","));
            }
            catch 
            {
                Console.WriteLine("Nem megfelelő formátum/adat. Nyomj meg egy gombot az újrakezdéshez.");
                Console.ReadKey();
                Console.Clear();
                AdatokBekerese(SGenre, MGenre);
            }

            if (r < 0.0 || r > 9.9) 
            {
                Console.WriteLine("Az értékelésnek nagyobbnak kell lenni mint [0.0] és kisebbnek mint [9.9]. Nyomj meg egy gombot az újrakezdéshez.");
                Console.ReadKey();
                Console.Clear();
                AdatokBekerese(SGenre, MGenre);
            }

            Console.WriteLine("Mit szeretnél nézni? [S = Sorozat, M = Film, D = Dokumentum film]");
            Console.Write("Típus: ");
            t = Console.ReadLine().Trim();

            if (t == "S")
            {
                t = "series";

                string[] adat = seriesBekeres(SGenre, MGenre).Split(";");

                g = adat[0];
                l = int.Parse(adat[1]);
            }

            else if (t == "M")
            {
                t = "movie";
                string[] adat = movieBekeres(SGenre, MGenre).Split(";");

                g = adat[0];
                l = int.Parse(adat[1]);
            }

            else if (t == "D")
            {
                t = "documentary";
                g = "Documentary";

                l = documentaryBekeres(SGenre, MGenre);
            }

            else 
            {
                Console.WriteLine("Nem megfelelő formátum/adat. Nyomj meg egy gombot az újrakezdéshez.");
                Console.ReadKey();
                Console.Clear();
                AdatokBekerese(SGenre, MGenre);
            }
            
            string[] bekertAdatok = new string[] { Convert.ToString(r), n, t, g, Convert.ToString(l) };

            return bekertAdatok;
        }

        static string movieBekeres(List<string> SGenre, List<string> MGenre) 
        {
            string g = "";
            int l = 0;

            Console.WriteLine("Milyen műfajú legyen a film? A következők érhetőek el: ");
            for (int i = 0; i < MGenre.Count(); i++)
            {
                Console.WriteLine($"{i + 1}. {MGenre[i]}");
            }
            Console.Write("Sorszám: ");

            int sorszam = 0;
            try
            {
                sorszam = int.Parse(Console.ReadLine().Trim()) - 1;
            }
            catch
            {
                Console.WriteLine("Nem megfelelő formátum/adat. Nyomj meg egy gombot az újrakezdéshez.");
                Console.ReadKey();
                Console.Clear();
                AdatokBekerese(SGenre, MGenre);
            }

            if (sorszam < 0 || sorszam > MGenre.Count() - 1)
            {
                Console.WriteLine("Csak a listában szereplő sorszámot adhatsz meg. Nyomj meg egy gombot az újrakezdéshez.");
                Console.ReadKey();
                Console.Clear();
                AdatokBekerese(SGenre, MGenre);
            }

            g = MGenre[sorszam];

            Console.WriteLine("Milyen hosszú filmet szeretnél nézni? [percben]");
            Console.Write("Hossz: ");

            try
            {
                l = int.Parse(Console.ReadLine().Trim());
            }
            catch
            {
                Console.WriteLine("Nem megfelelő formátum/adat. Nyomj meg egy gombot az újrakezdéshez.");
                Console.ReadKey();
                Console.Clear();
                AdatokBekerese(SGenre, MGenre);
            }

            return g+";"+Convert.ToString(l);
        }

        static string seriesBekeres(List<string> SGenre, List<string> MGenre)
        {
            string g = "";
            int l = 0;

            Console.WriteLine("Milyen műfajú legyen a sorozat? A következők érhetőek el: ");
            for (int i = 0; i < SGenre.Count(); i++)
            {
                Console.WriteLine($"{i + 1}. {SGenre[i]}");
            }
            Console.Write("Sorszám: ");

            int sorszam = 0;
            try
            {
                sorszam = int.Parse(Console.ReadLine().Trim()) - 1;
            }
            catch
            {
                Console.WriteLine("Nem megfelelő formátum/adat. Nyomj meg egy gombot az újrakezdéshez.");
                Console.ReadKey();
                Console.Clear();
                AdatokBekerese(SGenre, MGenre);
            }

            if (sorszam < 0 || sorszam > SGenre.Count() - 1)
            {
                Console.WriteLine("Csak a listában szereplő sorszámot adhatsz meg. Nyomj meg egy gombot az újrakezdéshez.");
                Console.ReadKey();
                Console.Clear();
                AdatokBekerese(SGenre, MGenre);
            }

            g = SGenre[sorszam];

            Console.WriteLine("Hány epizódos sorozatot szeretnél nézni? [darab]");
            Console.Write("Hossz: ");

            try
            {
                l = int.Parse(Console.ReadLine().Trim());
            }
            catch
            {
                Console.WriteLine("Nem megfelelő formátum/adat. Nyomj meg egy gombot az újrakezdéshez.");
                Console.ReadKey();
                Console.Clear();
                AdatokBekerese(SGenre, MGenre);
            }

            return g + ";" + Convert.ToString(l);
        }

        static int documentaryBekeres(List<string> SGenre, List<string> MGenre)
        {
            int l = 0;

            Console.WriteLine("Milyen hosszú dokumentum filmet szeretnél nézni? [percben]");
            Console.Write("Hossz: ");

            try
            {
                l = int.Parse(Console.ReadLine().Trim());
            }
            catch
            {
                Console.WriteLine("Nem megfelelő formátum/adat. Nyomj meg egy gombot az újrakezdéshez.");
                Console.ReadKey();
                Console.Clear();
                AdatokBekerese(SGenre, MGenre);
            }

            return l;
        }
    }
}