using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopsAndRobbers
{
    class Cop : Citizen
    {
        public List<Item> SiezedItems = new List<Item>();
        //public Cop(int x, int y)
        public Cop(int verticalPosition, int horizontalPosition)
        {
            HorizontalPosition = horizontalPosition;
            VerticalPosition = verticalPosition;
        }
        public void TakeGoodsFromRobber(Robber robber)
        {
            SiezedItems.AddRange(robber.StolenGoods);
            robber.StolenGoods.Clear();
        }
    }
}
