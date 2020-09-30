using System;
using System.Collections.Generic;

namespace CopsAndRobbers
{
    class Citizen : Person
    {
        public List<Item> Belongings = new List<Item>();
        public int TimesRobbed { get; set; }
        public Citizen(int verticalPosition, int horizontalPosition, Random rand, int id) : base(verticalPosition, horizontalPosition, id, rand)
        {
            TimesRobbed = 0;
            Belongings.Add(new Item("Clock"));
            Belongings.Add(new Item("Keys"));
            Belongings.Add(new Item("Cash"));
            Belongings.Add(new Item("Phone"));
        }
    }
}
