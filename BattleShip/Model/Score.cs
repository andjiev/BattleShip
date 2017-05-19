﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShip.Model
{
    class Score
    {
       String Name { get; set; }
       int Hiscore { get; set; }
        public Score(String name, int hiscore)
        {
            Name = name;
            Hiscore = hiscore;
        }
        public override string ToString()
        {
            return String.Format("{0} : {1} pts", Name, Hiscore);
        }
    }
}