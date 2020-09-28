using System;
using System.Collections.Generic;

namespace CopsAndRobbers
{
    class Citizen : Person
    {
        public List<Item> Belongings = new List<Item>();
        public int timesRobbed { get; set; }
        public Citizen(int verticalPosition, int horizontalPosition, Random rand)
        {
            timesRobbed = 0;
            Belongings.Add(new Item("Clock"));
            Belongings.Add(new Item("Keys"));
            Belongings.Add(new Item("Cash"));
            Belongings.Add(new Item("Phone"));
            HorizontalPosition = horizontalPosition;
            VerticalPosition = verticalPosition;
            MoveDirection = (Direction)rand.Next(0, Enum.GetValues(typeof(Direction)).Length);
        }
    }
}
