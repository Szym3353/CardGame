using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Deck
    {
        private List<Card> cards;
        private List<Card> discarded;
        private Random rng = new Random();

        public Deck()
        {
            cards = new List<Card>();
            discarded = new List<Card>();
            Initialize();
            Shuffle();
            DiscardCard(cards[0]);
            cards.RemoveAt(0);
        }

        private void Initialize()
        {
            string[] colors = { "red", "green", "blue", "yellow" };
            foreach (string color in colors)
            {
                for (int i = 0; i <= 9; i++)
                {
                    cards.Add(new Card(color, i.ToString()));
                    cards.Add(new Card(color, i.ToString()));
                }

                cards.Add(new Card(color, "+2", true));
                cards.Add(new Card(color, "+2", true));
                cards.Add(new Card(color, "reverse", true));
                cards.Add(new Card(color, "reverse", true));
            }
        }

        private void Shuffle()
        {
            for (int i = cards.Count - 1; i > 0; i--)
            {
                int j = rng.Next(i + 1);
                var temp = cards[i];
                cards[i] = cards[j];
                cards[j] = temp;
            }
        }

        public Card[] Draw(int number)
        {
            if (cards.Count <= number)
            {
                ReshuffleDiscarded();

                if(cards.Count <= number)
                {
                    Console.WriteLine("Brak wystarczającej ilości kart do dobrania.");
                    return null;
                }
            }

            Card[] drawCards = new Card[number];


            for (int i = 0; i < number; i++)
            {
                Card card = cards[0];
                drawCards[i] = card;
                cards.RemoveAt(0);
            }
            return drawCards;
        }

        public void DiscardCard(Card card)
        {
            discarded.Add(card);
        }

        private void ReshuffleDiscarded()
        {
            if (discarded.Count <= 1) return; //Jedna karta musi zostać na stole

            Console.WriteLine("Brak kart do pobrania. Tasowanie odrzuconego stosu...");

            Card lastCard = discarded[discarded.Count - 1];
            discarded.RemoveAt(discarded.Count - 1);
            cards.AddRange(discarded);
            discarded.Clear();
            discarded.Add(lastCard);

            Shuffle();
        }

        public int Count => cards.Count;
        public Card FirstCard => discarded[discarded.Count - 1];
    }
}
