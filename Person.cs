using System;

namespace CopsAndRobbers
{
    class Person
    {
        public enum Direction
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
        public int ID { get; set; }
        public Direction MoveDirection { get; set; }
        public int HorizontalPosition { get; set; }
        public int VerticalPosition { get; set; }

        public Person(int verticalPosition, int horizontalPosition, int id, Random rand)
        {

            MoveDirection = (Direction)rand.Next(0, Enum.GetValues(typeof(Direction)).Length);
            HorizontalPosition = horizontalPosition;
            VerticalPosition = verticalPosition;
            ID = id;
        }
        public void MoveToNewLocation(Direction MoveDirection)
        {
            switch(MoveDirection)
            {
                
                case Direction.N:
                    VerticalPosition--;
                    break;
                case Direction.S:
                    VerticalPosition++;
                    break;
                case Direction.W:
                    HorizontalPosition--;
                    break;
                case Direction.E:
                    HorizontalPosition++;
                    break;
                case Direction.SE:
                    HorizontalPosition++;
                    VerticalPosition++;
                    break;
                case Direction.NW:
                    HorizontalPosition--;
                    VerticalPosition--;
                    break;
                case Direction.SW:
                    HorizontalPosition--;
                    VerticalPosition++;
                    break;
                case Direction.NE:
                    HorizontalPosition++;
                    VerticalPosition--;
                    break;
            }
        }
        public void Move(int height, int width)
          {
            Direction md = MoveDirection;
            int maxHorizontalPos = width - 1;
            int maxVerticalPos = height - 1;
            bool isAtSouthWall = VerticalPosition == maxVerticalPos;
            bool isAtNorthWall = VerticalPosition == 0;
            bool isAtWestWall = HorizontalPosition == 0;
            bool isAtEastWall = HorizontalPosition == maxHorizontalPos;
            bool isAtNorthWestWall = isAtNorthWall && isAtWestWall;
            bool isAtNorthEastWall = isAtNorthWall && isAtEastWall;
            bool isAtSouthWestWall = isAtSouthWall && isAtEastWall;
            bool isAtSouthEastWall = isAtSouthWall && isAtEastWall;

            if (md == Direction.NE && isAtNorthEastWall)
            {
                HorizontalPosition = 0;
                VerticalPosition = maxVerticalPos;
            }
            else if (md == Direction.SE && isAtSouthEastWall)
            {
                HorizontalPosition = 0;
                VerticalPosition = 0;
            }
            else if (md == Direction.NW && isAtNorthWestWall)
            {
                HorizontalPosition = maxHorizontalPos;
                VerticalPosition = maxVerticalPos;
            }
            else if (md == Direction.SW && isAtSouthWestWall)
            {
                HorizontalPosition = maxHorizontalPos;
                VerticalPosition = 0;
            }
            //south wall
            else if ((md == Direction.S || md == Direction.SW || md == Direction.SE) && isAtSouthWall)
            {
                VerticalPosition = 0;
            }
            //west wall
            else if ((md == Direction.W || md == Direction.SW || md == Direction.NW) && isAtWestWall)
            {
                HorizontalPosition = maxHorizontalPos;
            }
            //north wall
            else if ((md == Direction.N || md == Direction.NE || md == Direction.NW) && isAtNorthWall)
            {
                VerticalPosition = maxVerticalPos;
            }
            //east wall
            else if ((md == Direction.E || md == Direction.SE || md == Direction.NE) && isAtEastWall)
            {
                HorizontalPosition = 0;
            }
            else
            {
                MoveToNewLocation(md);
            }
        }






        //nedanför är min gamla kod, hade missuppfattat hur dom skulle röra sig, koden nedanför gör att dom får en slumpad riktning varje gång




        //public void Move(int Height, int Width, Random rand)
        //{
        //    bool isAtSouthWall = VerticalPosition == Height-1;
        //    bool isAtNorthWall = VerticalPosition == 0;
        //    bool isAtWestWall = HorizontalPosition == 0;
        //    bool isAtEastWall = HorizontalPosition == Width-1;
        //
        //    // if (this is Robber)
        //    // {
        //    //     MessageBox.Show($"at Vertical position {VerticalPosition}, Horizonstal position {HorizontalPosition} \nisAtSouthWall: {isAtSouthWall}, isAtNorthWall: {isAtNorthWall} isAtWestWall: {isAtWestWall} isAtEastWall {isAtEastWall}");
        //    // }
        //    Direction moveDirection = (Direction)rand.Next(0, Enum.GetValues(typeof(Direction)).Length);
        //    while (true)
        //    {
        //        if (moveDirection == Direction.NE && !(isAtNorthWall || isAtEastWall))
        //        {
        //            HorizontalPosition++;
        //            VerticalPosition--;
        //            break;
        //        }
        //        else if (moveDirection == Direction.SE && !(isAtEastWall || isAtSouthWall))
        //        {
        //            HorizontalPosition++;
        //            VerticalPosition++; 
        //            break;
        //        }
        //        else if (moveDirection == Direction.SW && !(isAtWestWall || isAtSouthWall))
        //        {
        //            HorizontalPosition--;
        //            VerticalPosition++;
        //            break;
        //        }
        //        else if (moveDirection == Direction.NW && !(isAtWestWall || isAtNorthWall))
        //        {
        //            HorizontalPosition--;
        //            VerticalPosition--;
        //            break;
        //        }
        //        else if (moveDirection == Direction.S && !isAtSouthWall)
        //        {
        //            VerticalPosition++;
        //            break;
        //        }
        //        else if (moveDirection == Direction.W && !isAtWestWall)
        //        {
        //            HorizontalPosition--;
        //            break;
        //        }
        //        else if (moveDirection == Direction.N && !isAtNorthWall)
        //        {
        //            VerticalPosition--;
        //            break;
        //        }
        //        else if (moveDirection == Direction.E && !isAtEastWall)
        //        {
        //            HorizontalPosition++;
        //            break;
        //        }else
        //        {
        //
        //            moveDirection = (Direction)rand.Next(0, 8);
        //        }
        //    }
        //}
    }
}
