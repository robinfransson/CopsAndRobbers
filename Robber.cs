using System;
using System.Collections.Generic;

namespace CopsAndRobbers
{
    class Robber : Person
    {
        public int timesCaught { get; set; }
        public int peopleRobbed { get; set; }
        public List<Item> StolenGoods = new List<Item>();
        public Robber(int verticalPosition, int horizontalPosition, Random rand)
        {
            peopleRobbed = 0;
            timesCaught = 0;
            HorizontalPosition = horizontalPosition;
            VerticalPosition = verticalPosition;
            MoveDirection = (Direction)rand.Next(0, Enum.GetValues(typeof(Direction)).Length);
        }
        public void StealFrom(Citizen regular)
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
