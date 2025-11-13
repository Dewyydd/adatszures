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

            Bekeres(ValasztottOpciok, SGenre, MGenre);
        }

        static void Bekeres(List<bool> KivalasztottKategoriak, List<string> SGenre, List<string> MGenre) 
        {
            Console.WriteLine("Szia");
        }
    }
}