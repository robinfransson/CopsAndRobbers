using System;
using System.Collections.Generic;

namespace CopsAndRobbers
{
    class Robber : Person
    {
        public int TimesCaught { get; set; }
        public int PeopleRobbed { get; set; }
        public int TimeInPrison { get; set; }
        public List<Item> StolenGoods = new List<Item>();
        public Robber(int verticalPosition, int horizontalPosition, Random rand, int id) : base(verticalPosition, horizontalPosition, id,rand)
        {
            TimeInPrison = 0;
            PeopleRobbed = 0;
            TimesCaught = 0;
        }
        public void StealFrom(Citizen citizen)
        {
            if (citizen.Belongings.Count != 0) // om invånaren har något ska tjuven stjäla något av det
            {
                Random rand = new Random();
                int randomItem = rand.Next(0, citizen.Belongings.Count);
                StolenGoods.Add(citizen.Belongings[randomItem]);
                citizen.Belongings.RemoveAt(randomItem);
            }
        }
    }
}
