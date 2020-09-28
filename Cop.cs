using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopsAndRobbers
{
    class Cop : Person
    {
        public int robbersBusted { get; set; }
        public List<Item> SiezedItems = new List<Item>();
        //public Cop(int x, int y)
        public Cop(int verticalPosition, int horizontalPosition, Random rand)
        {
            robbersBusted = 0;
            HorizontalPosition = horizontalPosition;
            VerticalPosition = verticalPosition;
            MoveDirection = (Direction)rand.Next(0, Enum.GetValues(typeof(Direction)).Length);
        }
        public void TakeGoodsFromRobber(Robber robber)
        {
            SiezedItems.AddRange(robber.StolenGoods);
            robber.StolenGoods.Clear();
        }
    }
}
