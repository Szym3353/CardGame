using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CardGameConsole
{
    public class Profile
    {
        [Key]
        public int Id { get; set; }

        private string name;
        private int gamesPlayed;
        private int gamesWon;
        public string Name { get { return this.name; } set { name = value; } }
        public int GamesPlayed { get { return gamesPlayed; } set { gamesPlayed = value; } }
        public int GamesWon {  get { return gamesWon; } set { gamesWon = value; } }

        public Profile() { }

        public Profile(string name)
        {
            this.name = name;
            this.gamesPlayed = 5;
            this.gamesWon = 3;
        }
       
    }
}
