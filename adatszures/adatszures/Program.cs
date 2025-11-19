using System.Collections.Generic;
using System.IO;
using System.Linq;

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
            List<string> DGenre = new List<string>();

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

                    else if (F.type == "documentary" && !DGenre.Contains(G))
                    {
                        DGenre.Add(G);
                    }
                }
            }

            bool valaszt = true;
            int currentIndex = 0;
            List<string> Opciok = new List<string>() {"Értékelés", "Típus", "Műfaj", "Hossz", "Maximum sorok"};
            List<bool> ValasztottOpciok = [false, false, false, false, false];

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

                if (keyInfo.Key == ConsoleKey.DownArrow && currentIndex != ValasztottOpciok.Count() - 1)
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

            List<string> SzurtAdatok = Bekeres(ValasztottOpciok, SGenre, MGenre, DGenre, BemenetiAdatok);
            Console.Clear();

            Szures(SzurtAdatok, BemenetiAdatok);
        }

        static List<string> Bekeres(List<bool> ValasztottOpciok, List<string> SGenre, List<string> MGenre, List<string> DGenre, List<FilmAdatok> BemenetiAdatok)
            
        {
            List<string> SzuresAdatok = new List<string>();
            string tipusMufaj = "NINCS";

            if (ValasztottOpciok[0] == true) 
            {
                SzuresAdatok.Add(Ertekeles());
                Console.Clear();
            }

            if (ValasztottOpciok[1] == true)
            {
                if (ValasztottOpciok[3] == true)
                {
                    string a = Tipus(true, BemenetiAdatok);
                    SzuresAdatok.Add(a.Split(';')[0]);
                    SzuresAdatok.Add(a.Split(';')[1]);

                    tipusMufaj = a.Split(';')[0].Substring(1);
                }

                else 
                {
                    string a = Tipus(false, BemenetiAdatok);
                    SzuresAdatok.Add(a);
                    tipusMufaj = a.Substring(1);
                }

                Console.Clear();
            }

            if (ValasztottOpciok[2] == true)
            {
                SzuresAdatok.Add(Mufaj(tipusMufaj, SGenre, MGenre, DGenre, BemenetiAdatok));
                Console.Clear();
            }

            if (ValasztottOpciok[4] == true) 
            {
                SzuresAdatok.Add(Darabszam());
                Console.Clear();
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Sikeres adat bekérés.\nNyomj meg egy gombot a folytatáshoz!");
            Console.ForegroundColor = ConsoleColor.White;
            Console.ReadKey();

            return SzuresAdatok;
        }

        static void Szures(List<string> SzurtAdatok, List<FilmAdatok> BemenetiAdatok) 
        {
            double minErtekeles = 0.0;
            string tipus = "";
            string mufaj = "";
            int minHossz = 0;
            int maxSorok = 0;

            foreach (var a in SzurtAdatok) 
            {
                if (a[0] == 'E') 
                {
                    minErtekeles = double.Parse(a.Substring(1));
                }

                if (a[0] == 'T')
                {
                    tipus = a.Substring(1);
                }

                if (a[0] == 'H')
                {
                    minHossz = int.Parse(a.Substring(1));
                }

                if (a[0] == 'M')
                {
                    mufaj = a.Substring(1);
                }

                if (a[0] == 'D')
                {
                    maxSorok = int.Parse(a.Substring(1));
                }
            }

            Console.WriteLine("Az adatok a következők alapján lettek szűrve: ");

            if (minErtekeles != 0.0) 
            {
                Console.WriteLine("\tMinimum Értékelés: " + minErtekeles);
            }
            if (tipus != "") 
            {
                Console.WriteLine("\tTípus: " + tipus);
            }
            if (mufaj != "") 
            {
                Console.WriteLine("\tMűfaj: " + mufaj);
            }
            if (minHossz != 0) 
            {
                Console.WriteLine("\tMinimum Hossz: " + minHossz);
            }
            if (maxSorok != 0) 
            {
                Console.WriteLine("\tMaximum Sorok: " + maxSorok);
            }

            Console.WriteLine("\n\n\n");
            Console.WriteLine("Szűrt adatok: ");
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
                        Console.WriteLine("Az értékelésnek [0.0] és [10.0] között kell lennie.");
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
        static string Tipus(bool kellHossz, List<FilmAdatok> BemenetiAdatok)
        {
            List<string> Tipusok = new List<string>();

            foreach (var t in BemenetiAdatok) 
            {
                if (!Tipusok.Contains(t.type)) 
                {
                    Tipusok.Add(t.type);
                }
            }

            int sorszam = 0;
            bool helyes = false;

            Console.WriteLine("Milyen típsú műsort akarsz nézni? A következők érhetőek el:");
            for (int i = 0; i < Tipusok.Count; i++) 
            {
                Console.WriteLine($"  {i+1}. {Tipusok[i].ToUpper()[0]}{Tipusok[i].Substring(1)}");
            }


            while (helyes != true) 
            {
                try
                {
                    Console.Write("Sorszám: ");
                    sorszam = int.Parse(Console.ReadLine().Trim());

                    if ((sorszam - 1) < 0 || (sorszam - 1) > Tipusok.Count)
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

            string output = Tipusok[sorszam-1];

            if (kellHossz)
            {
                Console.Clear();
                return "T" + output + ";H" + Hossz(output);
            }

            else 
            {
                return "T" + output;
            }      
        }
        static string Mufaj(string Tipus, List<string> SGenre, List<string> MGenre, List<string> DGenre, List<FilmAdatok> BemenetiAdatok)
        {
            bool helyes = false;
            string output = "";
            int sorszam = 0;
            List<string> OsszesGenre = new List<string>();

            foreach (var adat in BemenetiAdatok) 
            {
                foreach (var genre in adat.genre) 
                {
                    if (!OsszesGenre.Contains(genre))
                    {
                        OsszesGenre.Add(genre);
                    }
                }
            }

            if (Tipus == "NINCS")
            {
                Console.WriteLine("Milyen műfajú műsort szeretnél nézni? A következő műfajok érhetőek el:");

                for (int i = 0; i < OsszesGenre.Count; i++)
                {
                    Console.WriteLine($"  {i + 1}. {OsszesGenre[i]}");
                }

                while (helyes != true) 
                {
                    try
                    {
                        Console.Write("Sorszám: ");
                        sorszam = int.Parse(Console.ReadLine().Trim());

                        if (sorszam - 1 < 0 || sorszam > OsszesGenre.Count)
                        {
                            Console.WriteLine("Csak a listában szereplő sorszámot adhatsz meg.");
                        }
                        else 
                        {
                            helyes = true;
                            output = OsszesGenre[sorszam - 1];
                        }
                    }

                    catch 
                    {
                        Console.WriteLine("Nem megfelelő adat");
                    }
                }
            }

            else if (Tipus == "series") 
            {
                Console.WriteLine("Milyen műfajú sorozatot szeretnél nézni? A következő műfajok érhetőek el:");

                for (int i = 0; i < SGenre.Count; i++)
                {
                    Console.WriteLine($"  {i + 1}. {SGenre[i]}");
                }

                while (helyes != true)
                {
                    try
                    {
                        Console.Write("Sorszám: ");
                        sorszam = int.Parse(Console.ReadLine().Trim());

                        if (sorszam - 1 < 0 || sorszam > SGenre.Count)
                        {
                            Console.WriteLine("Csak a listában szereplő sorszámot adhatsz meg.");
                        }
                        else
                        {
                            helyes = true;
                            output = SGenre[sorszam - 1];
                        }
                    }

                    catch
                    {
                        Console.WriteLine("Nem megfelelő adat");
                    }
                }
            }

            else if (Tipus == "movie")
            {
                Console.WriteLine("Milyen műfajú filmet szeretnél nézni? A következő műfajok érhetőek el:");

                for (int i = 0; i < MGenre.Count; i++)
                {
                    Console.WriteLine($"  {i + 1}. {MGenre[i]}");
                }

                while (helyes != true)
                {
                    try
                    {
                        Console.Write("Sorszám: ");
                        sorszam = int.Parse(Console.ReadLine().Trim());

                        if (sorszam - 1 < 0 || sorszam > MGenre.Count)
                        {
                            Console.WriteLine("Csak a listában szereplő sorszámot adhatsz meg.");
                        }
                        else
                        {
                            helyes = true;
                            output = MGenre[sorszam - 1];
                        }
                    }

                    catch
                    {
                        Console.WriteLine("Nem megfelelő adat");
                    }
                }
            }

            else if (Tipus == "documentary")
            {
                Console.WriteLine("Milyen műfajú dokumentum filmet szeretnél nézni? A következő műfajok érhetőek el:");

                for (int i = 0; i < DGenre.Count; i++)
                {
                    Console.WriteLine($"  {i + 1}. {DGenre[i]}");
                }

                while (helyes != true)
                {
                    try
                    {
                        Console.Write("Sorszám: ");
                        sorszam = int.Parse(Console.ReadLine().Trim());

                        if (sorszam - 1 < 0 || sorszam > DGenre.Count)
                        {
                            Console.WriteLine("Csak a listában szereplő sorszámot adhatsz meg.");
                        }
                        else
                        {
                            helyes = true;
                            output = DGenre[sorszam - 1];
                        }
                    }

                    catch
                    {
                        Console.WriteLine("Nem megfelelő adat");
                    }
                }
            }

            return "M" + output;
        }
        static string Hossz(string Tipus)
        {
            bool helyes = false;
            int lenght = 0;
            string output = "";

            if (Tipus == "movie" || Tipus == "documentary")
            {
                Console.WriteLine("Hány perces filmet szeretnél nézni?");

                while (helyes != true) 
                {
                    Console.Write("Hossz: ");

                    try
                    {
                        lenght = int.Parse(Console.ReadLine().Trim());

                        if (lenght < 0)
                        {
                            Console.WriteLine("Pozitív számot kell megadnod.");
                        }
                        else 
                        {
                            helyes = true;
                            output = Convert.ToString(lenght);
                        }
                    }

                    catch 
                    {
                        Console.WriteLine("Nem megfelelő adat.");
                    }
                }
            }

            else 
            {
                Console.WriteLine("Hány részes sorozatot szeretnél nézni?");

                while (helyes != true)
                {
                    Console.Write("Hossz: ");

                    try
                    {
                        lenght = int.Parse(Console.ReadLine().Trim());

                        if (lenght < 0)
                        {
                            Console.WriteLine("Pozitív számot kell megadnod.");
                        }
                        else
                        {
                            helyes = true;
                            output = Convert.ToString(lenght);
                        }
                    }

                    catch
                    {
                        Console.WriteLine("Nem megfelelő adat.");
                    }
                }
            }

            return output;
        }
        static string Darabszam() 
        {
            bool helyes = false;
            int db = 0;
            string output = "";

            Console.WriteLine("Maximum hány sor adatot szeretnél megjeleníteni?");

            while (helyes != true)
            {
                Console.Write("Darabszám: ");

                try
                {
                    db = int.Parse(Console.ReadLine().Trim());

                    if (db < 0)
                    {
                        Console.WriteLine("Pozitív számot kell megadnod.");
                    }
                    else
                    {
                        helyes = true;
                        output = Convert.ToString(db);
                    }
                }

                catch
                {
                    Console.WriteLine("Nem megfelelő adat.");
                }
            }
                return "D" + db;
        }
    }
}