using System;
using System.Collections.Generic;

namespace CopsAndRobbers
{
    class Robber : Citizen
    {
        public List<Item> StolenGoods = new List<Item>();
        public Robber(int verticalPosition, int horizontalPosition)
        {
            HorizontalPosition = horizontalPosition;
            VerticalPosition = verticalPosition;
        }
        public void StealFrom(Regular regular)
        {
            if (regular.Belongings.Count != 0) // om invånaren har något ska tjuven stjäla något av det
            {
                Random rand = new Random();
                int randomItem = rand.Next(0, regular.Belongings.Count);
                StolenGoods.Add(regular.Belongings[randomItem]);
                regular.Belongings.RemoveAt(randomItem);
            }
        }
    }
}
