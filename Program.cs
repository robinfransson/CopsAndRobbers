using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CopsAndRobbers
{
    class Program
    {
        static Random rand = new Random();
        static int NumberOfRobbedCitizens = 0;
        static int NumberOfRobbersCaught = 0;
        static void Main()
        {
            List<Citizen> citizens = new List<Citizen>();

            int NumberOfRobbers = 5;
            int NumberOfCops = 5;
            int NumberOfCitizens = 30;
            int Width = 60;
            int Height = 25;
            char[,] GameField = GeneratePlayingField(Height, Width);


            citizens.AddRange(GenerateCops(NumberOfCops, Height, Width));
            citizens.AddRange(GenerateRobbers(NumberOfRobbers, Height, Width));
            citizens.AddRange(GenerateRegularCitizens(NumberOfCitizens, Height, Width));

            Console.WriteLine("Om en rånare blir tagen av en polis blir det ett 'W'\n" +
                "Om en invånare blir rånad blir det ett 'Q'\n" +
                "Tryck vart som helst för att fortsätta..");
            Console.ReadKey();
            Console.Clear();
            while (true)
            {
                MoveAround(citizens, Height, Width);
                GameField = CheckCollision(citizens, GameField);
                ShowLocations(GameField);
                Console.WriteLine($"Number of Robbers caught: {NumberOfRobbersCaught}\nNumber of Citizens robbed: {NumberOfRobbedCitizens}");
                Thread.Sleep(2000);
                Console.Clear();

                GameField = ResetField(GameField);

            }
        }
        static Citizen SameSpotAsPerson(Citizen c, List<Citizen> citizens) //kollar om det står någon på samma plats
        {
            List<Citizen> citizensOnSameSpot = citizens.FindAll(x => x.HorizontalPosition == c.HorizontalPosition && x.VerticalPosition == c.VerticalPosition);
            return citizensOnSameSpot.Where(x => x != c).FirstOrDefault(); //
        }



        static char[,] CheckCollision(List<Citizen> citizens, char[,] GameField)
        {
            foreach (Citizen c in citizens)
            {
                Citizen otherPerson = SameSpotAsPerson(c, citizens);

                if (otherPerson != null)
                {
                    GameField = CheckWhoCollided(c, otherPerson, citizens, GameField);
                }
                else
                {
                    GameField = NotCollided(c, GameField);
                }

            }

            return GameField;

        }

        static char[,] CheckWhoCollided(Citizen c, Citizen otherPerson, List<Citizen> citizens, char[,] GameField)
        {
            List<Citizen> alreadyChecked = new List<Citizen>();

            bool sameAsCop = SameSpotAsPerson(c, citizens).GetType() == typeof(Cop);
            bool sameAsRobber = SameSpotAsPerson(c, citizens).GetType() == typeof(Robber);


            if (c.GetType() == typeof(Robber) && sameAsCop && !alreadyChecked.Contains(c))
            {
                ((Cop)otherPerson).TakeGoodsFromRobber((Robber)c);
                alreadyChecked.Add(c);
                alreadyChecked.Add(otherPerson);
                GameField[c.VerticalPosition, c.HorizontalPosition] = 'W';
                NumberOfRobbersCaught++;
            }
            else if (c.GetType() == typeof(Regular) && sameAsRobber && !alreadyChecked.Contains(c))
            {
                ((Robber)otherPerson).StealFrom((Regular)c);
                alreadyChecked.Add(c);
                alreadyChecked.Add(otherPerson);
                GameField[c.VerticalPosition, c.HorizontalPosition] = 'Q';
                NumberOfRobbedCitizens++;
            }
            else
            {
                GameField = NotCollided(c, GameField);
            }
            return GameField;
        }

        static char[,] NotCollided(Citizen c, char[,] GameField)
        {
            if (c is Cop)
            {
                GameField[c.VerticalPosition, c.HorizontalPosition] = 'P';
            }
            else if (c is Robber)
            {
                GameField[c.VerticalPosition, c.HorizontalPosition] = 'R';
            }
            else
            {
                GameField[c.VerticalPosition, c.HorizontalPosition] = 'I';
            }

            return GameField;
        }

        

        static char[,] ResetField(char[,] field) // sätter alla chars till mellanslag igen
        {
            for (int i = 0; i <= field.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= field.GetUpperBound(1); j++)
                {
                    field[i, j] = ' ';
                }
            }
            return field;

        }
        static void MoveAround(List<Citizen> citizens, int Height, int Width)
        {
            foreach (Citizen citizen in citizens)
            {
                citizen.Move(Height, Width, rand);
            }
        }
        static char[,] GeneratePlayingField(int height, int width)
        {
            char[,] field = new char[height, width];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    field[i, j] = ' ';
                }
            }
            return field;
        }

        static List<Citizen> GenerateCops(int Ammount, int height, int width)
        {
            List<Citizen> cops = new List<Citizen>();
            for (int i = 1; i <= Ammount; i++)
            {
                cops.Add(new Cop(rand.Next(0, height), rand.Next(0, width)));
            }
            return cops;
        }

        static List<Citizen> GenerateRobbers(int Ammount, int height, int width)
        {
            List<Citizen> robbers = new List<Citizen>();
            for (int i = 1; i <= Ammount; i++)
            {
                robbers.Add(new Robber(rand.Next(height), rand.Next(width)));
            }
            return robbers;
        }
        static List<Citizen> GenerateRegularCitizens(int Ammount, int height, int width)
        {
            List<Citizen> regulars = new List<Citizen>();
            for (int i = 1; i <= Ammount; i++)
            {
                regulars.Add(new Regular(rand.Next(height), rand.Next(width)));
            }
            return regulars;
        }

        static void ShowLocations(char[,] field)
        {
            for (int vertical = 0; vertical <= field.GetUpperBound(0); vertical++)
            {
                for (int horizontal = 0; horizontal <= field.GetUpperBound(1); horizontal++)
                {
                    Console.Write(field[vertical, horizontal]);
                    if (horizontal == field.GetUpperBound(1))
                    {
                        Console.Write(".\n");
                    }
                }
            }

        }
    }
}
