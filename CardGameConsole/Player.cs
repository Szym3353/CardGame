using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Player
    {
        private string name;
        private List<Card> hand;
        private int id;

        public Player(string name, int id)
        {
            this.name = name;
            this.hand = new List<Card>();
            this.id = id;

            Console.WriteLine($"PLAYER CONSTRUCTOR - {this.id} - {this.name}");
        }

        public List<Card> Hand { get { return hand; } }

        public int GetId()
        { return id; }
        public string Name { get { return name; } }

        public void AddCards(Card[] cards)
        {
            if (cards == null)
            {
                throw new ArgumentNullException(nameof(cards), "Lista kart nie może być null.");
            }
            hand.AddRange(cards);
        }

        public void RemoveCard(Card card)
        {
            hand.Remove(card);
        }

        //
        //public void RemoveCards()

        public int HandCount => hand.Count;
    }
}
