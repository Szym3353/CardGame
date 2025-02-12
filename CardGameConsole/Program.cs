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
        Console.WriteLine("Testowa wersja gry");
        Console.WriteLine("Wybierz opcje: ");
        Console.WriteLine("1. Rozpocznij grę");
        Console.WriteLine("2. Profile");
        Console.WriteLine("3. Historia gier");
        Console.WriteLine("0. Wyjdź");


        string input = Console.ReadLine();
        switch (input)
        {
            case "1":
            //startgame
            Console.Clear();
                Console.WriteLine("Podaj liczbę graczy (minimum 2) : ");
                
                int playerCount = int.Parse(Console.ReadLine());
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
                        Console.WriteLine($"Gracz {i + 1} - wybierz profil");
                        for (int j = 0; j < profiles.Count; j++)
                        {
                            Console.WriteLine($"{j}. {profiles[j].Name}");
                        }
                        Console.WriteLine("x - graj bez profilu");

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
        Console.WriteLine("Lista profili:");

        for (int i = 0; i < profiles.Count; i++)
        {
            Console.WriteLine($"{i}. {profiles[i].Name}");
        }
        Console.WriteLine("\n\nWybierz indeks profilu aby zobaczyć statystyki lub nim zarządzać");
        Console.WriteLine("n - Utwórz nowy profil");
        Console.WriteLine("b - Wróć");

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
        Console.WriteLine($"Informacje o profilu: {profile.Name}");
        Console.WriteLine($"🎮 Rozegrane gry: {profile.GamesPlayed}");
        Console.WriteLine($"🏆 Wygrane: {profile.GamesWon}");

        Console.WriteLine("\nOpcje:");
        Console.WriteLine("1. 🔧 Zmień nazwę");
        Console.WriteLine("2. 🗑 Usuń profil");
        Console.WriteLine("3. Powrót");

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