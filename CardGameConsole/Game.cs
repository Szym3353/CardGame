using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using CardGameConsole;


namespace ConsoleApp1
{
    internal class Game
    {
        private Deck deck;
        private List<Player> players;
        private int currentTurn;
        private bool isReversed;
        private int wildSum;
        private bool wildActive;

        public Game(List<Profile> playersProfiles)
        {
            deck = new Deck();
            players = new List<Player>();
            foreach (var profile in playersProfiles)
            {
                players.Add(new Player(profile.Name, profile.Id));
            }

            isReversed = false;

        }

        public void Start()
        {
            
            //Rozdanie kart
            foreach (Player player in players)
            {
                player.AddCards(deck.Draw(7));
            }

            currentTurn = 0;
            isReversed = false;

            while (!IsGameEnd())
            {
                PlayTurn();
            }

            Console.WriteLine("Koniec gry");
            Console.ReadKey();

        }



        private bool IsGameEnd() {
            foreach (Player player in players) {

                if(player.HandCount == 0)
                {
                    using var db = new AppDbContext();
                    foreach(Player endPlayer in players)
                    {
                        var profile = db.Profiles.FirstOrDefault(p => p.Id == endPlayer.GetId());
                        if(profile != null)
                        {

                            profile.GamesPlayed++;
                            db.SaveChanges();
                        }

                    }
                    Console.WriteLine($"{player.Name} wygrał/a.");

                    var winnerProfile = db.Profiles.FirstOrDefault(p => p.Id == player.GetId());

                    if (winnerProfile != null) {
                        winnerProfile.GamesWon++;
                    }


                    return true;
                }
            }

            return false;
            
        }

        private void PlayTurn()
        {
            Player currentPlayer = players[currentTurn];
            Card currentMiddleCard = deck.FirstCard;

            int chosenIndex = -1;
            Card chosenCard = null;
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"\nTura gracza: {currentPlayer.Name}");
                Console.Write($"\nAktualna środkowa karta: ");
                PrintColoredName(currentMiddleCard);
                Console.ResetColor();
                if (wildActive) {
                    Console.WriteLine($"\nSuma plusów: {wildSum}");
                }
                Console.WriteLine($"\nTwoje karty:");

                for (int i = 0; i < currentPlayer.HandCount; i++)
                {
                    Card handCard = currentPlayer.Hand[i];
                    Console.Write($"\n{i}. ");
                    PrintColoredName(handCard);
                }

                Console.WriteLine($"\n\n\nWybierz indeks karty, którą chcesz zagrać, lub wpisz 'd' aby pobrać kartę ze środka.");
                string playerInput = Console.ReadLine();
                if(playerInput.ToLower() == "d")
                {
                    //Dobieranie karty
                    if (wildActive) {
                        currentPlayer.AddCards(deck.Draw(wildSum));
                        wildActive = false;
                        wildSum = 0;
                        break;
                    }

                    currentPlayer.AddCards(deck.Draw(7));
                    
                    break;
                }else if(int.TryParse(playerInput, out chosenIndex))
                {
                    if (chosenIndex >= 0 && chosenIndex < currentPlayer.HandCount)
                    {
                        chosenCard = currentPlayer.Hand[chosenIndex];
                        //Sprawdź czy można zagrać
                        if(CanPlayCard(chosenCard))
                        {
                            deck.DiscardCard(chosenCard);
                            currentPlayer.RemoveCard(chosenCard);

                            //JEŚLI KARTA JEST SPECJALNA: EFEKTY
                            if (chosenCard.IsWild)
                            {
                                if(chosenCard.Value == "+2")
                                {
                                    if (wildActive == false) wildActive = true;
                                    wildSum += 2;
                                }else if(chosenCard.Value == "rev")
                                {
                                    isReversed = !isReversed;
                                }
                            }

                            break;
                        }
                        else
                        {
                            Console.WriteLine("Niepoprawna karta");
                        }
                    };
                }
            }

            NextPlayer();
        }

        private bool CanPlayCard(Card card)
        {
            Card currentMiddleCard = deck.FirstCard;

            if(wildActive == true)
            {
                return currentMiddleCard.Value == "+2" && (card.Value == "+2" || card.Value == "rev");
            }

            return card.Color == currentMiddleCard.Color || card.Value == currentMiddleCard.Value;
        }

        private void NextPlayer()
        {
            currentTurn = currentTurn + 1 * (isReversed ? 1 : -1);

            if(currentTurn == players.Count)
            {
                currentTurn = 0;
            }
            else if(currentTurn < 0)
            {
                currentTurn = players.Count - 1;
            }
            
        }

        public static void PrintColoredName(Card card)
        {
            Console.ForegroundColor = card.Color switch
            {
                "red" => ConsoleColor.Red,
                "green" => ConsoleColor.Green,
                "blue" => ConsoleColor.Blue,
                "yellow" => ConsoleColor.Yellow,
                _ => ConsoleColor.White
            };
            Console.Write($"{card.Color} - {card.Value}");
            Console.ResetColor();
        }

    }
}
