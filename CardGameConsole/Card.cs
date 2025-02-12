using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Card
    {
        public enum CardColor
        {
            Red,
            Blue,
            Green,
            Yellow
        }
        private string color;
        private string value;
        private bool isWild;
        private string wildType;

        public Card(string clr, string v)
        {
            this.color = clr;
            this.value = v;
            this.isWild = false;
        }

        public Card(string clr, string v, bool iw)
        {
            this.color = clr;
            this.value = v;
            this.isWild = iw;
        }

        public string Color { get { return this.color; } }
        public string Value { get { return this.value; } }

        public bool IsWild { get { return this.isWild; } }
    }
}
