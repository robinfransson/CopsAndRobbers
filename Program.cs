using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace CopsAndRobbers
{
    class Program
    {

        static readonly Random rand = new Random();
        static int numberOfRobbedCitizens = 0;
        static int numberOfRobbersCaught = 0;

        static void Main()
        {
            List<Person> citizens = new List<Person>();

            int numberOfRobbers = 5;
            int numberOfCops = 20;
            int numberOfCitizens = 40;
            int height = 25;
            int width = 100;
            char[,] gameField = GeneratePlayingField(height, width);

            //Generera invånare i våran stad
            citizens.AddRange(GenerateCops(numberOfCops, height, width));
            citizens.AddRange(GenerateRobbers(numberOfRobbers, height, width));
            citizens.AddRange(GenerateRegularCitizens(numberOfCitizens, height, width));

            //lite info
            Console.WriteLine("Om en rånare blir tagen av en polis blir det ett 'W'\n" +
                              "Om en invånare blir rånad blir det ett 'Q'\n" +
                              "Tryck vart som helst för att fortsätta..");
            Console.ReadKey();
            Console.Clear();
            while (true)
            {
                MoveAround(citizens, height, width);
                gameField = CheckCollision(citizens, gameField);
                ShowLocations(gameField);
                Console.WriteLine($"Number of Robbers caught: {numberOfRobbersCaught}\nNumber of Citizens robbed: {numberOfRobbedCitizens}");
                gameField = ResetField(gameField);
                Thread.Sleep(200);
                Console.Clear();

            }
        }





        static char[,] CheckCollision(List<Person> citizens, char[,] gameField)
        {
            List<Person> listOfAlreadyCheckedPersons = new List<Person>();
            foreach (Person person in citizens)
            {
                List<Cop> copsAtLocation = CopsOnSpot(citizens, person);
                List<Robber> robbersAtLocation = RobbersOnSpot(citizens, person);
                List<Citizen> citizensAtLocation = CitizensOnSpot(citizens, person);
                if (robbersAtLocation.Any() && citizensAtLocation.Any()) // om det finns en eller flera rånare/vanliga medborgare ska rånarna ta en sak av varje medborgare
                {
                    gameField = TryRobbing(robbersAtLocation, citizensAtLocation, gameField, listOfAlreadyCheckedPersons);
                }
                if (copsAtLocation.Any() && robbersAtLocation.Any())
                {
                    gameField = TryToCatchRobber(robbersAtLocation, copsAtLocation, gameField, listOfAlreadyCheckedPersons);
                }
                else
                {
                    gameField = NotCollided(person, gameField);
                }

                listOfAlreadyCheckedPersons.AddRange(copsAtLocation);
                listOfAlreadyCheckedPersons.AddRange(robbersAtLocation);
                listOfAlreadyCheckedPersons.AddRange(citizensAtLocation);


            }

            return gameField;

        }




        static void RobberCaughtPrint(Cop cop, Robber robber)
        {

            Console.Clear();
            Console.WriteLine($"A ROBBER GOT BUSTED \nBusted times: {robber.timesCaught}");
            Console.WriteLine($"The cop who busted him have busted {cop.robbersBusted}, and now has these items: ");
            ListItems(cop);
            Thread.Sleep(2000);
            Console.Clear();
        }




        static void CitizenRobbedPrint(Citizen citizen, Robber robber)
        {
            Console.Clear();
            Console.WriteLine("SOMEONE GOT ROBBED");
            Console.WriteLine($"ROBBED TIMES: {citizen.timesRobbed}");
            Console.WriteLine($"The Robber now has robbed {robber.peopleRobbed} and has this in his inventory:");
            ListItems(robber);
            Console.WriteLine("\nThe citizen now has: ");
            ListItems(citizen);
            Thread.Sleep(2000);
            Console.Clear();


        }



        static char[,] TryRobbing(List<Robber> robbers, List<Citizen> citizens, char[,] gameField, List<Person> listOfAlreadyCheckedPersons)
        {
            foreach (Citizen citizen in citizens)
            {
                if (!citizen.Belongings.Any() || listOfAlreadyCheckedPersons.Contains(citizen)) // om inte medborgaren har något på sig kan han inte bli rånad
                {
                    continue;
                }
                foreach (Robber robber in robbers) // är det flera rånare på samma ställe kan alla sno en sak av invånaren
                {
                    robber.StealFrom(citizen);
                    robber.peopleRobbed++;
                    citizen.timesRobbed++;
                    CitizenRobbedPrint(citizen, robber);
                    numberOfRobbedCitizens++;
                }
                gameField[citizen.VerticalPosition, citizen.HorizontalPosition] = 'Q';

            }
            return gameField;
        }



        static void ListItems(Person person)
        {
            if (person is Cop cop)
            {
                foreach (Item item in cop.SiezedItems)
                {
                    Console.Write(item.ItemName + "\n");
                }
            }
            else if (person is Robber robber)
            {
                foreach (Item goods in robber.StolenGoods)
                {
                    Console.Write(goods.ItemName + "\n");
                }
            }
            else
            {
                Citizen citizen = (Citizen)person;
                foreach (Item item in citizen.Belongings)
                {
                    Console.Write(item.ItemName + "\n");
                }
            }
        }



        static char[,] TryToCatchRobber(List<Robber> robbers, List<Cop> cops, char[,] gameField, List<Person> listOfAlreadyCheckedPersons)
        {
            foreach (Robber robber in robbers)
            {
                if (!robber.StolenGoods.Any()) // om rånaren inte har något i sin inventory är han ännu inte en rånare (eller har avtjänat sitt straff)
                {
                    continue;
                }
                Cop cop = cops[rand.Next(cops.Count)];
                cop.TakeGoodsFromRobber(robber); // en slumpad polis tar rånarens saker (om det är flera på samma ställe)
                robber.timesCaught++;
                cop.robbersBusted++;
                gameField[robber.VerticalPosition, robber.HorizontalPosition] = 'W';
                Console.Clear();
                RobberCaughtPrint(cop, robber);
                numberOfRobbersCaught++;
            }
            return gameField;
        }


        static List<Cop> CopsOnSpot(List<Person> persons, Person person)
        {
            List<Cop> citizens = persons.OfType<Cop>().Where(x => x.HorizontalPosition == person.HorizontalPosition && x.VerticalPosition == person.VerticalPosition).ToList();
            return citizens;
        }



        static List<Robber> RobbersOnSpot(List<Person> persons, Person person)
        {
            List<Robber> citizens = persons.OfType<Robber>().Where(x => x.HorizontalPosition == person.HorizontalPosition && x.VerticalPosition == person.VerticalPosition).ToList();
            return citizens;
        }





        static List<Citizen> CitizensOnSpot(List<Person> persons, Person person)
        {
            List<Citizen> citizens = persons.OfType<Citizen>().Where(x => x.HorizontalPosition == person.HorizontalPosition && x.VerticalPosition == person.VerticalPosition).ToList();
            return citizens;
        }



        static char[,] NotCollided(Person person, char[,] gameField)
        {
            if (person is Cop)
            {
                gameField[person.VerticalPosition, person.HorizontalPosition] = 'P';
            }
            else if (person is Robber)
            {
                gameField[person.VerticalPosition, person.HorizontalPosition] = 'R';
            }
            else
            {
                gameField[person.VerticalPosition, person.HorizontalPosition] = 'I';
            }

            return gameField;
        }



        static char[,] ResetField(char[,] gameField) // sätter alla chars till mellanslag igen
        {
            for (int i = 0; i <= gameField.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= gameField.GetUpperBound(1); j++)
                {
                    gameField[i, j] = ' ';
                }
            }
            return gameField;

        }
        static void MoveAround(List<Person> citizens, int height, int width)
        {
            foreach (Person citizen in citizens)
            {
                citizen.Move(height, width);
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

        static List<Person> GenerateCops(int ammountToGenerate, int height, int width)
        {
            List<Person> cops = new List<Person>();
            for (int i = 1; i <= ammountToGenerate; i++)
            {
                cops.Add(new Cop(rand.Next(0, height), rand.Next(0, width), rand));
            }
            return cops;
        }

        static List<Person> GenerateRobbers(int ammountToGenerate, int height, int width)
        {
            List<Person> robbers = new List<Person>();
            for (int i = 1; i <= ammountToGenerate; i++)
            {
                robbers.Add(new Robber(rand.Next(height), rand.Next(width), rand));
            }
            return robbers;
        }
        static List<Person> GenerateRegularCitizens(int ammountToGenerate, int height, int width)
        {
            List<Person> regulars = new List<Person>();
            for (int i = 1; i <= ammountToGenerate; i++)
            {
                regulars.Add(new Citizen(rand.Next(height), rand.Next(width), rand));
            }
            return regulars;
        }

        static void ShowLocations(char[,] gameField)
        {
            for (int verticalSize = 0; verticalSize <= gameField.GetUpperBound(0); verticalSize++)
            {
                for (int horizontalSize = 0; horizontalSize <= gameField.GetUpperBound(1); horizontalSize++)
                {
                    Console.Write(gameField[verticalSize, horizontalSize]);
                    if (horizontalSize == gameField.GetUpperBound(1))
                    {
                        Console.Write("\n");
                    }
                }
            }

        }
    }
}
//        static Person SameSpotAsPerson(Person person, List<Person> citizens) //kollar om det står någon på samma plats
//          {
//              List<Person> citizensOnSameSpot = citizens.FindAll(x => x.HorizontalPosition == person.HorizontalPosition && x.VerticalPosition == person.VerticalPosition);
//              return citizensOnSameSpot.Where(x => x != person).FirstOrDefault(); //returnerar den första i listan, om det finns någon annars null
//          }
//static char[,] CheckWhoCollided(Person person, Person otherPerson, List<Person> citizens, char[,] gameField, List<Person> alreadyChecked, Random rand)
//{
//
//    List<Cop> copsAtLocation = CopsOnSpot(citizens, person).Cast<Cop>().ToList();
//    List<Robber> robbersAtLocation = RobbersOnSpot(citizens, person).Cast<Robber>().ToList();
//    List<Citizen> citizensAtLocation = CitizensOnSpot(citizens, person).Cast<Citizen>().ToList(); ;
//    string items = "";
//    foreach(Citizen citizen in citizensAtLocation)
//    {
//        if(!citizen.Belongings.Any())
//        {
//            continue;
//        }
//        if(!robbersAtLocation.Any())
//        {
//            break;
//        }
//        Robber r = robbersAtLocation[rand.Next(robbersAtLocation.Count)];
//        r.StealFrom(citizen); // en slumpad rånare rånar personen på en sak
//        foreach(Item i in r.StolenGoods)
//        {
//            items += i.ItemName + "\n";
//        }
//        MessageBox.Show(items);
//        numberOfRobbedCitizens++;
//        gameField[citizen.VerticalPosition, citizen.HorizontalPosition] = 'Q';
//        
//    }
//
//    foreach(Robber robber in robbersAtLocation)
//    {
//        if(!robber.StolenGoods.Any())
//        {
//            numberOfRobbersCaught++;
//            continue;
//        }
//        if(!copsAtLocation.Any()) // om det inte finns några poliser på samma ruta ska loopen avslutas
//        {
//            break;
//        }
//        Cop c = copsAtLocation[rand.Next(copsAtLocation.Count)];
//        c.TakeGoodsFromRobber(robber); // en slumpad polis tar rånarens saker
//        gameField[robber.VerticalPosition, robber.HorizontalPosition] = 'W';
//        foreach (Item i in c.SiezedItems)
//        {
//            items += i.ItemName + "\n";
//        }
//        MessageBox.Show(items);
//        numberOfRobbersCaught++;
//    }
//    return gameField;
//
//    //bool sameAsCop = SameSpotAsPerson(person, citizens).GetType() == typeof(Cop); //om personen 
//    //bool sameAsRobber = SameSpotAsPerson(person, citizens).GetType() == typeof(Robber);
//    //
//    //
//    //if (person.GetType() == typeof(Robber) && sameAsCop && !alreadyChecked.Contains(person))
//    //{
//    //    ((Cop)otherPerson).TakeGoodsFromRobber((Robber)person);
//    //    numberOfRobbersCaught++;
//    //    string items = "";
//    //    foreach(Item item in ((Cop)otherPerson).SiezedItems)
//    //    {
//    //        items += item.ItemName;
//    //    }
//    //    MessageBox.Show(items);
//    //}
//    //else if (person.GetType() == typeof(Citizen) && sameAsRobber && !alreadyChecked.Contains(person))
//    //{
//    //    ((Robber)otherPerson).StealFrom((Citizen)person);
//    //    numberOfRobbedCitizens++; 
//    //    string items = "";
//    //    foreach (Item item in ((Robber)otherPerson).StolenGoods)
//    //    {
//    //        items += item.ItemName;
//    //    }
//    //    MessageBox.Show(items);
//    //}
//    //else
//    //{
//    //    gameField = NotCollided(person, gameField);
//    //}
//    //return gameField;
//}