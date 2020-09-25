using System;

namespace CopsAndRobbers
{
    class Citizen
    {
        enum Direction
        {
            N,
            E,
            W,
            S,
            SE,
            SW,
            NE,
            NW
        }
        public int HorizontalPosition { get; set; }
        public int VerticalPosition { get; set; }
        public void Move(int Height, int Width, Random rand)
        {
            bool isAtSouthWall = VerticalPosition == Height-1;
            bool isAtNorthWall = VerticalPosition == 0;
            bool isAtWestWall = HorizontalPosition == 0;
            bool isAtEastWall = HorizontalPosition == Width-1;

            // if (this is Robber)
            // {
            //     MessageBox.Show($"at Vertical position {VerticalPosition}, Horizonstal position {HorizontalPosition} \nisAtSouthWall: {isAtSouthWall}, isAtNorthWall: {isAtNorthWall} isAtWestWall: {isAtWestWall} isAtEastWall {isAtEastWall}");
            // }
            Direction moveDirection = (Direction)rand.Next(0, Enum.GetValues(typeof(Direction)).Length);
            while (true)
            {
                if (moveDirection == Direction.NE && !(isAtNorthWall || isAtEastWall))
                {
                    HorizontalPosition++;
                    VerticalPosition--;
                    break;
                }
                else if (moveDirection == Direction.SE && !(isAtEastWall || isAtSouthWall))
                {
                    HorizontalPosition++;
                    VerticalPosition++; 
                    break;
                }
                else if (moveDirection == Direction.SW && !(isAtWestWall || isAtSouthWall))
                {
                    HorizontalPosition--;
                    VerticalPosition++;
                    break;
                }
                else if (moveDirection == Direction.NW && !(isAtWestWall || isAtNorthWall))
                {
                    HorizontalPosition--;
                    VerticalPosition--;
                    break;
                }
                else if (moveDirection == Direction.S && !isAtSouthWall)
                {
                    VerticalPosition++;
                    break;
                }
                else if (moveDirection == Direction.W && !isAtWestWall)
                {
                    HorizontalPosition--;
                    break;
                }
                else if (moveDirection == Direction.N && !isAtNorthWall)
                {
                    VerticalPosition--;
                    break;
                }
                else if (moveDirection == Direction.E && !isAtEastWall)
                {
                    HorizontalPosition++;
                    break;
                }else
                {

                    moveDirection = (Direction)rand.Next(0, 8);
                }
            }
        }
    }
}
