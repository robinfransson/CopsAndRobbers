using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopsAndRobbers
{
    class Cop : Person
    {
        public int RobbersBusted { get; set; }
        public List<Item> SiezedItems = new List<Item>();
        //public Cop(int x, int y)
        public Cop(int verticalPosition, int horizontalPosition, Random rand, int id) : base(verticalPosition, horizontalPosition, id, rand)
        {
            RobbersBusted = 0;
        }
        public void TakeGoodsFromRobber(Robber robber)
        {
            SiezedItems.AddRange(robber.StolenGoods);
            robber.StolenGoods.Clear();
        }
    }
}
