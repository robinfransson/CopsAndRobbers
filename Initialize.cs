using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CopsAndRobbers
{
    class Initialize
    {
        public static readonly Random rand = new Random();
        static void AskUserAboutPrisoners(out int howManyTurns)
        {
            Console.Write("Hur många händelser ska det vara innan rånarna släpps fria från fängelset? ");
            howManyTurns = VerifyIfInt(Console.ReadLine()) * 5; // plussar på 5 i tid på varje fånge när det händer något, därav gånger 5
        }

        public static void Start()
        {
            AskUserGameSize(out int height, out int width);
            AskUserHowManyPeople(out int numberOfRobbers, out int numberOfCops, out int numberOfCitizens);
            AskUserAboutPrisoners(out int whenToReleaseFromPrison);
            GameField.GeneratePlayingField(height, width);



            GameField.GameHeight = height -1;
            GameField.GameWidth = width - 1;
            Console.Clear();
            People.TownPeople = CreateListOfPeople(numberOfCops, numberOfRobbers, numberOfCitizens, GameField.GameHeight, GameField.GameWidth);
            Prison.PrisonTimer = whenToReleaseFromPrison;


        }

        static void AskUserHowManyPeople(out int numberOfRobbers, out int numberOfCops, out int numberOfCitizens)
        {
            Console.Write("Hur många rånare vill du ha med i spelet? ");
            numberOfRobbers = VerifyIfInt(Console.ReadLine());
            while (numberOfRobbers < 1)
            {
                Console.WriteLine("Tyvärr, minst 1..");
                Console.Write("Hur många rånare vill du ha med i spelet? ");
                numberOfRobbers = VerifyIfInt(Console.ReadLine());
            }

            Console.Write("Hur många poliser vill du ha med i spelet? ");
            numberOfCops = VerifyIfInt(Console.ReadLine());
            while (numberOfCops < 1)
            {
                Console.WriteLine("Tyvärr, minst 1..");
                Console.Write("Hur många poliser vill du ha med i spelet? ");
                numberOfCops = VerifyIfInt(Console.ReadLine());
            }

            Console.Write("Hur många medborgare vill du ha med i spelet? ");
            numberOfCitizens = VerifyIfInt(Console.ReadLine());
            while (numberOfCitizens < 1)
            {
                Console.WriteLine("Tyvärr, minst 1..");
                Console.Write("Hur många invånare vill du ha med i spelet? ");
                numberOfCitizens = VerifyIfInt(Console.ReadLine());
            }
        }

        static int VerifyIfInt(string numberToConvert)
        {
            bool convertedComplete = Int32.TryParse(numberToConvert, out int result);
            while (!convertedComplete)//om det inte är giltigt ska användaren ange ett nytt tal tills det är giltigt
            {
                Console.Write("Ogiltiga tecken upptäckta, ange talet igen: ");
                numberToConvert = Console.ReadLine();
                convertedComplete = Int32.TryParse(numberToConvert, out result);

            }
            return result;
        }
        static List<Person> CreateListOfPeople(int numberOfCops, int numberOfRobbers, int numberOfCitizens, int height, int width)
        {

            List<Person> citizens = new List<Person>();


            //Generera invånare i våran stad
            citizens.AddRange(GenerateCops(numberOfCops, height, width)); // genererar ett antal poliser med slumpad position
            citizens.AddRange(GenerateRobbers(numberOfRobbers, height, width));// genererar ett antal rånare med slumpad position
            citizens.AddRange(GenerateRegularCitizens(numberOfCitizens, height, width));// genererar ett antal medborgare med slumpad position

            return citizens;
        }
        static void AskUserGameSize(out int gameFieldHeight, out int gameFieldWidth)
        {
            //gör det faktiskta fönstret lite större så att allt får plats
            int widthOffset = 25;
            int heightOffset = 4;

            //det största fönstret som tillåts, beroende på skärmstorlek
            int maxHeight = Console.LargestWindowHeight - heightOffset;
            int maxWidth = Console.LargestWindowWidth - widthOffset;

            //användaren får välja hur stort spelfältet får vara
            Console.Write($"Hur högt ska spelfältet vara? (min 25, max {maxHeight}): ");
            int height = VerifyIfInt(Console.ReadLine());
            while (height < 25 || height > maxHeight)
            {
                Console.WriteLine($"Minst 25, max {maxHeight}.. försök igen");
                height = VerifyIfInt(Console.ReadLine());
            }

            Console.Write($"Hur brett ska spelfältet vara? (min 50, max {maxWidth}): ");
            int width = VerifyIfInt(Console.ReadLine());
            while (width < 50 || width > maxWidth)
            {
                Console.WriteLine($"Minst 50, max {maxWidth}.. försök igen");
                width = VerifyIfInt(Console.ReadLine());
            }
            Console.SetWindowSize(width + widthOffset, height + heightOffset);

            Console.SetBufferSize(width + widthOffset + 1, height + heightOffset + 5);
            gameFieldWidth = width;
            gameFieldHeight = height;
        }
        static List<Person> GenerateCops(int ammountToGenerate, int height, int width)
        {
            List<Person> cops = new List<Person>();
            for (int i = 1; i <= ammountToGenerate; i++)
            {
                int verticalPosition = rand.Next(0, height);
                int horizontalPosition = rand.Next(0, width);
                cops.Add(new Cop(verticalPosition, horizontalPosition, rand, i));
            }
            return cops;
        }

        static List<Person> GenerateRobbers(int ammountToGenerate, int height, int width)
        {
            List<Person> robbers = new List<Person>();
            for (int i = 1; i <= ammountToGenerate; i++)
            {
                int verticalPosition = rand.Next(0, height);
                int horizontalPosition = rand.Next(0, width);
                robbers.Add(new Robber(verticalPosition, horizontalPosition, rand, i));
            }
            return robbers;
        }
        static List<Person> GenerateRegularCitizens(int ammountToGenerate, int height, int width)
        {
            List<Person> regulars = new List<Person>();
            for (int i = 1; i <= ammountToGenerate; i++)
            {
                int verticalPosition = rand.Next(0, height);
                int horizontalPosition = rand.Next(0, width);
                regulars.Add(new Citizen(verticalPosition, horizontalPosition, rand, i));
            }
            return regulars;
        }        
    }
}
