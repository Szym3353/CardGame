// See https://aka.ms/new-console-template for more information
using CardGameConsole;
using ConsoleApp1;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;

InitializeDatabase();

List<Profile> profiles;
using (var db = new AppDbContext())
{
    profiles = db.Profiles.ToList();
}


    while (true)
    {
    Console.Clear();

    Console.ForegroundColor = ConsoleColor.Magenta;
    Console.WriteLine("                                                                           ");
    Console.WriteLine(",--. ,--.,--.  ,--. ,-----.      ,-----.,--.    ,-----. ,--.  ,--.,------. ");
    Console.WriteLine("|  | |  ||  ,'.|  |'  .-.  '    '  .--./|  |   '  .-.  '|  ,'.|  ||  .---'");
    Console.WriteLine("|  | |  ||  |' '  ||  | |  |    |  |    |  |   |  | |  ||  |' '  ||  `--,  ");
    Console.WriteLine("'  '-'  '|  | `   |'  '-'  '    '  '--'\\|  '--.'  '-'  '|  | `   ||  `---. ");
    Console.WriteLine(" `-----' `--'  `--' `-----'      `-----'`-----' `-----' `--'  `--'`------'");
    Console.ResetColor();

    Console.WriteLine("\n");

    Console.WriteLine("Wybierz opcję:\n");

    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("1. Rozpocznij grę");

    Console.ForegroundColor = ConsoleColor.Blue;
    Console.WriteLine("2. Profile");

    Console.ForegroundColor = ConsoleColor.Green;   
    Console.WriteLine("3. Historia gier");

    Console.ForegroundColor = ConsoleColor.Yellow;  
    Console.WriteLine("0. Wyjdź");


    Console.ResetColor();


        string input = Console.ReadLine();
        switch (input)
        {
            case "1":
            //startgame
            Console.Clear();
                Console.WriteLine("Podaj liczbę graczy (minimum 2) : ");
                
                int playerCount = int.Parse(Console.ReadLine());
                if (playerCount < 2)
            {
                Console.WriteLine("Zbyt mała liczba graczy.");
                Console.ReadKey();
                break;
            }
                List<Profile> players = new List<Profile>();
            if (playerCount < 2)
            {
                Console.WriteLine("Minimum 2 graczy.");
                break;
            }

                for (int i = 0; i < playerCount; i++)
                {
                    while (true)
                    {


                    //wybierz profil albo graj jako gość
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine($"Gracz numer {i + 1} - wybór profilu \n");
                    Console.ResetColor();
                        for (int j = 0; j < profiles.Count; j++)
                        {
                            Console.WriteLine($"{j}. {profiles[j].Name}");
                        }
                        Console.WriteLine("\nx - graj bez profilu");

                        string playerSelectionInput = Console.ReadLine();

                        if (playerSelectionInput == "x")
                        {
                            //wybieranie gościa
                            players.Add(new Profile($"Gość {i}"));
                            break;
                        }
                        else if (int.TryParse(playerSelectionInput, out int index) && index >= 0 && index < profiles.Count)
                        {
                            //Wybranie profilu
                            players.Add(profiles[index]);
                            break;
                        }
                    }
                    if (playerCount == players.Count)
                    {
                        Game game = new Game(players);
                        game.Start();
                    }
                }
                break;
            case "2":
                Profiles();
                break;
            case "3":
            //historia gier
            Console.WriteLine("Implementacja w przyszłości. Wciśnij dowolny klawisz aby wrócić do menu głównego.");
            Console.ReadKey();
                break;
            case "0":
                return;
            default:
                Console.WriteLine("Nieprawidłowy wybór.");
                break;
        }
    }

void Profiles()
{
    while (true)
    {

        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("====================================");
        Console.WriteLine("        <> LISTA PROFILI <>        ");
        Console.WriteLine("====================================\n\n");
        Console.ResetColor();

        for (int i = 0; i < profiles.Count; i++)
        {
            Console.WriteLine($"{i}. {profiles[i].Name}");
        }
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("\n> Wybierz indeks profilu, aby zobaczyć statystyki lub zarządzać nim.\n\n");
        Console.ResetColor();

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("> Naciśnij 'n' - Utwórz nowy profil");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("> Naciśnij 'b' - Wróć do menu głównego");
        Console.ResetColor();

        string input = Console.ReadLine();

        if (input == "b") return;
        else if (input == "n")
        {
            ProfileCreate();
        }
        else if (int.TryParse(input, out int index) && index >= 0 && index < profiles.Count) {
            ProfileDetails(profiles[index]);
        }
    }
}

void ProfileDetails(Profile profile)
{
    while (true)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("====================================");
        Console.WriteLine($"   INFORMACJE O PROFILU: {profile.Name}   ");
        Console.WriteLine("====================================\n\n");
        Console.ResetColor();

        Console.WriteLine($"<> Rozegrane gry: {profile.GamesPlayed}");
        Console.WriteLine($"<> Wygrane: {profile.GamesWon}\n\n");

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n====================================");
        Console.WriteLine("             OPCJE                  ");
        Console.WriteLine("====================================\n\n");
        Console.ResetColor();

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("1 -> Zmień nazwę");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("2 -> Usuń profil");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("3 -> Powrót");
        Console.ResetColor();

        string input = Console.ReadLine();

        switch (input)
        {
            case "1":
                ProfileRename(profile);
                break;
            case "2":
                ProfileRemove(profile);
                return;
            case "3":
                return;
        }
    }
}

void ProfileRename(Profile profile)
{
    Console.WriteLine("Podaj nową nazwę: ");
    string newName = Console.ReadLine();
    profile.Name = newName;
}

void ProfileRemove(Profile profile){
    using var db = new AppDbContext();
    db.Profiles.Remove(profile);
    db.SaveChanges();

    profiles = db.Profiles.ToList();
}

void ProfileCreate()
{
    while (true)
    {
        Console.Clear();
        Console.WriteLine("Podaj nazwę nowego profilu: ");

        string input = Console.ReadLine();

        Console.WriteLine($"Utworzyć nowy profil o nazwie: {input} ?");
        Console.WriteLine("y - tak, n - nie");
        string confirm = Console.ReadLine();
        if (confirm == "y")
        {
            using var db = new AppDbContext();
            var newProfile = new Profile { Name = input };
            db.Profiles.Add(newProfile);
            db.SaveChanges();
            profiles = db.Profiles.ToList();

            return;
        }
        else if (confirm == "n")
        {
            return;
        }
    }
}

void InitializeDatabase()
{
    using var db = new AppDbContext();
    db.Database.EnsureCreated();

    if (!db.Profiles.Any())
    {
        db.Profiles.Add(new Profile { Name = "Profile1", GamesPlayed = 5, GamesWon = 3 });
        db.Profiles.Add(new Profile { Name = "Profile2", GamesPlayed = 8, GamesWon = 6 });
        db.SaveChanges();
    }
}

Console.ReadLine();