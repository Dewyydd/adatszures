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

            bool valaszt = true;
            int currentIndex = 0;
            List<string> Opciok = new List<string>() {"Értékelés", "Típus", "Műfaj", "Hossz"};
            List<bool> ValasztottOpciok = [false, false, false, false];

            Console.WriteLine("Mi alapján szeretnél szűrni? [ENTER = Kiválaszt | SPACE = BEFEJEZÉS]");
            for (int i = 0; i < Opciok.Count(); i++)
            {
                if (i == currentIndex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("> ");
                    Console.WriteLine(Opciok[i]);
                    Console.ForegroundColor = ConsoleColor.White;
                }

                else
                {
                    if (ValasztottOpciok[i] == true)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("  " + Opciok[i]);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        if (ValasztottOpciok[1] == false && i == 3)
                        {
                            Console.ForegroundColor = ConsoleColor.Gray;
                            Console.WriteLine("  " + Opciok[i]);
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        else
                        {
                            Console.WriteLine("  " + Opciok[i]);
                        }
                    }
                }
            }

            while (valaszt) 
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey();
                bool hibaIras = false;

                if (keyInfo.Key == ConsoleKey.Spacebar) 
                {
                    break;
                }

                if (keyInfo.Key == ConsoleKey.DownArrow && currentIndex != 3)
                {
                    currentIndex++;
                }

                else if (keyInfo.Key == ConsoleKey.UpArrow && currentIndex != 0) 
                {
                    currentIndex--;
                }

                if (ValasztottOpciok[1] == false && currentIndex == 3)
                {
                    hibaIras = true;
                }

                if (keyInfo.Key == ConsoleKey.Enter) 
                {
                    if (ValasztottOpciok[1] == false && currentIndex == 3)
                    {
                        continue;
                    }

                    else 
                    {
                        if (currentIndex == 1 && ValasztottOpciok[1] == true)
                        {
                            ValasztottOpciok[1] = false;
                            ValasztottOpciok[3] = false;
                        }

                        else 
                        {
                            if (ValasztottOpciok[currentIndex] == false)
                            {
                                ValasztottOpciok[currentIndex] = true;
                            }
                            else
                            {
                                ValasztottOpciok[currentIndex] = false;
                            }
                        }
                    }
                }

                Console.Clear();
                Console.WriteLine("Mi alapján szeretnél szűrni? [ENTER = Kiválaszt | SPACE = BEFEJEZÉS]");
                for (int i = 0; i < Opciok.Count(); i++) 
                {
                    if (i == currentIndex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("> ");
                        Console.WriteLine(Opciok[i]);
                        Console.ForegroundColor = ConsoleColor.White;
                    }

                    else 
                    {
                        if (ValasztottOpciok[i] == true)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("  " + Opciok[i]);
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        else 
                        {
                            if (ValasztottOpciok[1] == false && i == 3)
                            {
                                Console.ForegroundColor = ConsoleColor.Gray;
                                Console.WriteLine("  " + Opciok[i]);
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                            else 
                            {
                                Console.WriteLine("  " + Opciok[i]);
                            }
                        }
                    }
                }
                if (hibaIras)
                {
                    Console.WriteLine("Ehhez az opcióhoz ki kell választanod a típust alapú szűrést is.");
                }
            }
            Console.Clear();

            List<string> SzurtAdatok = Bekeres(ValasztottOpciok, SGenre, MGenre);

            foreach (var a in SzurtAdatok) 
            {
                Console.WriteLine(a);
            }
        }

        static List<string> Bekeres(List<bool> ValasztottOpciok, List<string> SGenre, List<string> MGenre) 
        {
            List<string> SzuresAdatok = new List<string>();

            if (ValasztottOpciok[0] == true) 
            {
                SzuresAdatok.Add(Ertekeles());
                Console.Clear();
            }

            if (ValasztottOpciok[1] == true)
            {
                SzuresAdatok.Add(Tipus(false));

                if (ValasztottOpciok[3] == true)
                {
                    SzuresAdatok.Add(Tipus(true));
                }

                Console.Clear();
            }

            if (ValasztottOpciok[2] == true)
            {
                SzuresAdatok.Add(Mufaj());
                Console.Clear();
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Sikeres adat bekérés.\nNyomj meg egy gombot a folytatáshoz!");
            Console.ForegroundColor = ConsoleColor.White;
            Console.ReadKey();

            return SzuresAdatok;
        }

        static string Ertekeles() 
        {
            bool helyes = false;
            double ertekeles = 0.0;

            while (helyes != true) 
            {            
                Console.WriteLine("Mi legyen a minimum értékelés? [pl. 5.5/5,5]");
                try
                {
                    Console.Write("Értékelés: ");
                    ertekeles = double.Parse(Console.ReadLine().Trim().Replace(".",","));

                    if (ertekeles < 0.0 || ertekeles > 10.0)
                    {
                        Console.WriteLine("Az értékelésnem [0.0] és [10.0] között kell lennie.");
                    }

                    else 
                    {
                        helyes = true;
                    } 
                }
                catch 
                {
                    Console.WriteLine("Nem megfelelő adat.");
                }
            }

            return "E" + Convert.ToString(ertekeles);
        }

        static string Tipus(bool kellHossz)
        {
            string[] Tipusok = { "Dokumentum Film", "Film", "Sorozat" };
            int sorszam = 0;
            bool helyes = false;

            Console.WriteLine("Milyen típsú műsort akarsz nézni? A következők érhetőek el:");
            for (int i = 0; i < Tipusok.Length; i++) 
            {
                Console.WriteLine($"  {i+1}. {Tipusok[i]}");
            }


            while (helyes != true) 
            {
                try
                {
                    Console.Write("Sorszám: ");
                    sorszam = int.Parse(Console.ReadLine().Trim());

                    if ((sorszam - 1) < 0 || (sorszam - 1) > Tipusok.Length)
                    {
                        Console.WriteLine("Csak a listában szereplő sorszámot adhatsz meg.");
                    }

                    else 
                    {
                        helyes = true;
                    }
                }

                catch 
                {
                    Console.WriteLine("Nem megfelelő adat.");
                }

            }

            string output = "";

            if (sorszam - 1 == 0) 
            {
                output = "documentary";
            }

            if (sorszam - 1 == 1)
            {
                output = "movie";
            }

            if (sorszam - 1 == 2)
            {
                output = "series";
            }

            if (kellHossz) 
            {
                Hossz(output);
            }

            return "T" + output;
        }

        static string Mufaj()
        {
            return "X";
        }

        static string Hossz(string Tipus)
        {
            if (Tipus == "movie" || Tipus == "documentary")
            {

            }

            else 
            {

            }

            return "X";
        }
    }
}