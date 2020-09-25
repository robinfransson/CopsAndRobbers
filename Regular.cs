using System.Collections.Generic;

namespace CopsAndRobbers
{
    class Regular : Citizen
    {
        public List<Item> Belongings = new List<Item>();
        public Regular(int verticalPosition, int horizontalPosition)
        {
            Belongings.Add(new Item("Clock"));
            Belongings.Add(new Item("Keys"));
            Belongings.Add(new Item("Cash"));
            Belongings.Add(new Item("Phone"));
            HorizontalPosition = horizontalPosition;
            VerticalPosition = verticalPosition;
        }
    }
}
