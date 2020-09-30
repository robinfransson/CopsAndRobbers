using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace CopsAndRobbers
{
    class Program
    {

        static readonly Random rand = new Random();
        static int numberOfRobbedCitizens = 0;
        static int numberOfRobbersCaught = 0;
        
        
        
        
        
        
        
        
        
        
        
        

        //efter x antal händelser som användaren väljer kommer tjuven tillbaks, plussar på 5 på varje fånge varje gång spelet stannar
        static int whenToReleaseFromPrison;

        static List<Robber> prison = new List<Robber>();
        
        static void AskUserAboutPrisoners()
        {
            Console.Write("Hur många händelser ska det vara innan rånarna släpps fria från fängelset? ");
            whenToReleaseFromPrison = VerifyIfInt(Console.ReadLine()) * 5; // plussar på 5 i tid på varje fånge när det händer något, därav gånger 5
        }
        static void Main()
        {

            //inställningar
            Console.CursorVisible = false;

            //placerar highscores första bokstav 22 rutor från kanten till höger
            int hiscoreOffset = 22;

            //sätter fönsterstorleken 
            AskUserGameSize(out int height, out int width);
            AskUserHowManyCitizens(out int numberOfRobbers, out int numberOfCops, out int numberOfCitizens);
            AskUserAboutPrisoners();
            Console.Clear();

            //genererar en 2d array med samma storlek som det användaren skrev in eller max storlek
            char[,] gameField = GeneratePlayingField(height, width);



            //genererar människor i staden
            List<Person> people = CreateListOfPeople(numberOfCops, numberOfRobbers, numberOfCitizens, height, width);


            //lite info
            Console.WriteLine("Om en rånare blir tagen av en polis blir det ett 'A'nhållen\n" +
                              "Om en invånare blir rånad blir det ett 'H'old-up\n" +
                              "Om rånare blir tagen blir det ett stillastående 'F'ånge\n" +
                              "Efter 20 händelser kommer rånaren tillbaks in i spelet\n" +
                              "'P'oliser, 'I'nvånare, 'R'ånare\n" +
                              "Tryck vart som helst för att fortsätta..");
            Console.ReadKey();
            Console.Clear();

            while (true)
            {

                //vem har blivit rånad mest
                int mostTimesRobbed = people.OfType<Citizen>().Max(x => x.TimesRobbed);
                Citizen mostRobbedCitizen = people.OfType<Citizen>().First(x => x.TimesRobbed == mostTimesRobbed);



                //vem har rånat flest
                
                int mostRobberies = people.OfType<Robber>().Max(x => x.PeopleRobbed);
                Robber mostRobberiesCommitted = people.OfType<Robber>().First(x => x.PeopleRobbed == mostRobberies);



                //vilken rånare som har blivit tagen flest gånger
                int mostTimesCaught = people.OfType<Robber>().Max(x => x.TimesCaught);
                Robber mostCaughtRobber = people.OfType<Robber>().First(x => x.TimesCaught == mostTimesCaught);

                //vilken rånare har mest items i sin inventory
                int mostCurrentItemsStolen = people.OfType<Robber>().Max(x => x.StolenGoods.Count);
                Robber robberWithMostitems = people.OfType<Robber>().First(x => x.StolenGoods.Count == mostCurrentItemsStolen);

                //vilken polis har mest items i sin inventory
                int mostItemsSiezed = people.OfType<Cop>().Max(x => x.SiezedItems.Count);
                Cop copWithMostItems = people.OfType<Cop>().First(x => x.SiezedItems.Count == mostItemsSiezed);


                //vilken polis som har tagit flest rånare
                int mostRobbersCaugh = people.OfType<Cop>().Max(x => x.RobbersBusted);
                Cop copWithMostRobbersCaught = people.OfType<Cop>().First(x => x.RobbersBusted == mostRobbersCaugh);

                //skriver ut highscores uppe till höger
                PrintHiScores(hiscoreOffset, mostRobbedCitizen, mostRobberiesCommitted, mostCaughtRobber, robberWithMostitems, copWithMostItems, copWithMostRobbersCaught);

                int numberOfRobbersIngame = people.Where(x => x is Robber).ToList().Count;


                people = CheckPrison(people); //kollar fängelset om det är någon rånare som ska tas bort från spelplanen


                MoveAround(people, height, width); //alla utom dom i fängelset ska röra sig


                gameField = CheckCollision(people, gameField, width, height); // kollar om någon står på samma plats som någon annan


                //ShowLocations(gameField, height); // ritar upp vart alla står

                Console.WriteLine($"Number of Robbers caught: {numberOfRobbersCaught}\n" +
                    $"Number of Citizens robbed: {numberOfRobbedCitizens}, Robbers in prison: {prison.Count}\n" +
                    $"Number of robbers not in prison: {numberOfRobbersIngame}");

                Thread.Sleep(300);
                ResetDrawnGameMap(gameField); // istället för console clear skriver vi ett " " där det finns en bokstav (mindre flicker)
                gameField = ResetField(gameField); // nollställer 2d arrayen så de gamla bokstäverna inte skrivs ut igen
            }
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
            while(height < 25 || height > maxHeight)
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
            gameFieldWidth = width;
            gameFieldHeight = height;
        }
        static void AskUserHowManyCitizens(out int numberOfRobbers, out int numberOfCops, out int numberOfCitizens)
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

        //offset är hur många bokstäver/rutor brevid själva "spelfältet" hiscores ska skrivas ut
        static void PrintHiScores(int offset, Citizen mostRobbed, Robber mostRobberies,
            Robber mostTimesCaught, Robber robberMostItems, Cop copMostItems, Cop mostRobbersBusted)
        {
            //färgernas koder
            //Black 0 
            //DarkBlue 1
            //DarkGreen 2
            //DarkCyan 3
            //DarkRed 4
            //DarkMagenta 5
            //DarkYellow 6
            //Gray 7
            //DarkGray 8
            //Blue 9
            //Green 10
            //Cyan 11
            //Red 12
            //Magenta 13
            //Yellow 14
            //White 15


            int i = 0;
            int placeOnRight = Console.WindowWidth - offset;
            while (i <= 10)
            {
                if (i == 0)
                {
                    ColoredText("[C10]Most robbed", placeOnRight, i);
                    ColoredText($"[C6]ID:[C10] {mostRobbed.ID}[C15], score:[C12] {mostRobbed.TimesRobbed}", placeOnRight, i + 1);
                }
                else if (i == 2)
                {
                    ColoredText("[C10]Most robberies", placeOnRight, i);
                    ColoredText($"[C6]ID:[C10] {mostRobberies.ID}[C15], score:[C12] {mostRobberies.PeopleRobbed}", placeOnRight, i + 1);
                }
                else if (i == 4)
                {
                    ColoredText("[C10]Most caught", placeOnRight, i);
                    ColoredText($"[C6]ID:[C10] {mostTimesCaught.ID}[C15], score:[C12] {mostTimesCaught.TimesCaught}", placeOnRight, i + 1);
                }
                else if (i == 6)
                {
                    ColoredText("[C10]Most items [C12](robber)", placeOnRight, i);
                    ColoredText($"[C6]ID:[C10] {robberMostItems.ID}[C15], score:[C12] {robberMostItems.StolenGoods.Count}", placeOnRight, i + 1);
                }
                else if (i == 8)
                {
                    ColoredText("[C10]Most items [C9](cop)", placeOnRight, i);
                    ColoredText($"[C6]ID:[C10] {copMostItems.ID}[C15], score:[C12] {copMostItems.SiezedItems.Count}", placeOnRight, i + 1);
                }
                else if (i == 10)
                {
                    ColoredText("[C10]Most robbers busted", placeOnRight, i);
                    ColoredText($"[C6]ID:[C10] {mostRobbersBusted.ID}[C15], score:[C12] {mostRobbersBusted.RobbersBusted}", placeOnRight, i + 1);
                }
                i += 2; // plussar på med 2, så nästa utskrift hamnar under  id, score (dvs 2 rader under den förra)
            }
        }

        static void ColoredText(string colorThis, int horizontalPos, int verticalPos)
        {
            int nextplacement = 0;
            Regex regex = new Regex(@"(\[[C]\d{1,2}\])"); // regex matchar input strängen i formated [CXX] där XX är siffror
            string[] split = regex.Split(colorThis); // splittrar strängen där regex hittar en träff
            for (int i = 0; i < split.Length; i++)
            {

                Console.SetCursorPosition(horizontalPos + nextplacement, verticalPos); // plussar på den gamla strängens längd så de inte hamnar på samma ställe
                if (split[i].Contains("["))
                {
                    string matchInteger = new Regex(@"\d{1,2}").Match(split[i]).Value; // försöker hitta 1 eller 2 siffror i strängen
                    bool parseSuccess = Int32.TryParse(matchInteger, out int color); //gör om strängen till en int
                    if (parseSuccess)
                    {

                        Console.ForegroundColor = SelectColor(color); // sätter färgen till den valda
                    }

                }
                else
                {
                    nextplacement += split[i].Length; // nästa text ska inte hamna över den gamla så vi behöver spara hur lång den förra utskriften var
                    Console.Write(split[i]); // skriver ut strängen i
                    Console.ResetColor();
                }
            }
        }


        static ConsoleColor SelectColor(int color)
        {
            if (color > 15) // det finns bara 15 färger att välja på, har man skrivit högre blir det vitt
            {

                return (ConsoleColor)color;
            }

            return (ConsoleColor)color;
        }
        //skapar en 2d char array
        static char[,] GeneratePlayingField(int height, int width)
        {
            char[,] field = new char[height, width];
            field = ResetField(field); // sätter alla tecken till mellanslag
            return field;
        }
        static void ResetDrawnGameMap(char[,] gameField)
        {
            int height = gameField.GetUpperBound(0);
            int width = gameField.GetUpperBound(1);
            for (int i = 0; i <= height; i++)
            {
                for (int j = 0; j <= width; j++)
                {
                    if (gameField[i, j] != ' ')
                    {
                        PrintAtLocation(0, i, j, ' ');
                    }
                }
            }
        }
        static List<Person> RemoveRobberFromGameField(List<Person> people)
        {
            if (prison.Any()) // om det finns någon i fängelset ska vi kolla så att samma person inte också är med i spelet
            {
                foreach (Robber prisoner in prison)
                {
                    if (people.Contains(prisoner))
                    {
                        people.Remove(prisoner);
                    }
                }
            }
            return people;
        }
        static List<Person> CheckPrison(List<Person> people)
        {
            people = RemoveRobberFromGameField(people);
            List<Robber> toRelease = prison.Where(prisoner => prisoner.TimeInPrison >= whenToReleaseFromPrison).ToList(); // om det finns någon som suttit klart i fängelset
            if (toRelease.Any())
            {
                foreach (Robber releasedRobber in toRelease)
                {
                    releasedRobber.TimeInPrison = 0;
                    prison.Remove(releasedRobber); //så ska han släppas tillbaks
                    people.Add(releasedRobber);
                }
            }
            return people;
        }

        //lägger på 5 i tid på varje fånge, vid 100 släpps den 
        static void PrisonTimer()
        {
            foreach (Robber prisoner in prison)
            {
                prisoner.TimeInPrison += 5;
            }
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

        static char[,] CheckCollision(List<Person> persons, char[,] gameField, int width, int height)
        {
            //räknar igenom alla personer och sätter deras vanliga bokstav först, så det inte är halvfyllt när kollisioner ska kollas..
            //det var jag inte nöjd med helt enkelt :P 
            foreach(Person p in persons)
            {
                gameField = PrintRegularLocationMarkers(p, gameField);
            }
            ShowLocations(gameField, height);
            List<Person> listOfAlreadyCheckedPersons = new List<Person>();
            foreach (Person person in persons)
            {
                List<Cop> copsAtLocation = CopsOnSpot(persons, person);
                List<Robber> robbersAtLocation = RobbersOnSpot(persons, person);
                List<Citizen> citizensAtLocation = CitizensOnSpot(persons, person);
                if (listOfAlreadyCheckedPersons.Contains(person)) // om personen redan har kollats kan vi kolla nästa direkt
                {
                    continue;
                }
                if (robbersAtLocation.Any() && citizensAtLocation.Any()) // om det finns en eller flera rånare/vanliga medborgare ska rånarna ta en sak av varje medborgare
                {
                    gameField = TryRobbing(robbersAtLocation, citizensAtLocation, gameField, width, height);
                }
                if (copsAtLocation.Any() && robbersAtLocation.Any()) // och om en polis eller rånare möts ska polisen ta hans saker
                {
                    gameField = TryToCatchRobber(robbersAtLocation, copsAtLocation, gameField, width, height);
                }
                //else
                //{
                //    gameField = PrintRegularLocationMarkers(person, gameField);
                //}

                listOfAlreadyCheckedPersons.AddRange(copsAtLocation);
                listOfAlreadyCheckedPersons.AddRange(robbersAtLocation);
                listOfAlreadyCheckedPersons.AddRange(citizensAtLocation);


            }
            return gameField;

        }










        static void SomeoneBusted(char[,] gameField, Cop cop, Robber robber, int height, int width)
        {
            int copClocks = cop.SiezedItems.OfType<Item>().Where(x => x.ItemName == "Clock").Count();
            int copCash = cop.SiezedItems.OfType<Item>().Where(x => x.ItemName == "Cash").Count();
            int copKeys = cop.SiezedItems.OfType<Item>().Where(x => x.ItemName == "Keys").Count();
            int copPhone = cop.SiezedItems.OfType<Item>().Where(x => x.ItemName == "Phone").Count();
            int offsetFromTop = 4;
            int middlePositionLeft = width / 2;
            string[] toPrint = { "========================",
                                " A cop has busted ", 
                                " a robber! ", 
                                " This is his current ", 
                                " inventory. ", "========================",
                               $" Clock x {copClocks}", 
                               $" Cash x {copCash}", 
                               $" Keys x {copKeys}", 
                               $" Phone x {copPhone}", 
                                 " ", 
                                 "========================"
            };
            PrintWho(robber, width);
            PrintInfoStatScreen(toPrint, offsetFromTop, middlePositionLeft);
            Thread.Sleep(5000);
            PrisonTimer();
            Console.Clear();
            ShowLocations(gameField, height);
            Console.SetCursorPosition(0, height);
            }

            static char[,] TryRobbing(List<Robber> robbers, List<Citizen> citizens, char[,] gameField, int width, int height)
            {
            gameField = ResetField(gameField);
            
            foreach (Citizen citizen in citizens)
            {
                if (citizens.First().Equals(citizen))
                    {
                    gameField = AddLetterToGameField(gameField, citizen.VerticalPosition, citizen.HorizontalPosition, 'H');
                    ShowLocations(gameField, height);
                }
                bool multipleRobbersCanRobTheSameCitizen = true;
                if (!citizen.Belongings.Any()) // om inte medborgaren har något på sig kan han inte bli rånad
                {
                    continue;
                }
                if(multipleRobbersCanRobTheSameCitizen)
                {

                    foreach (Robber r in robbers)
                    {
                        r.StealFrom(citizen);
                        r.PeopleRobbed++;
                        numberOfRobbedCitizens++;
                        citizen.TimesRobbed++;

                        SomeoneRobbed(citizen, r, height, width);
                    }
                }
                else
                {

                    Robber robber = robbers[rand.Next(robbers.Count)]; // en slumpad rånare rånar personen
                    robber.StealFrom(citizen);
                    robber.PeopleRobbed++;
                    citizen.TimesRobbed++;
                    numberOfRobbedCitizens++;

                    SomeoneRobbed(citizen, robber, height, width);
                }

                //CitizenRobbedPrint(gameField, citizen, robber, height, width); //min gamla metod.. (finns längst ner i filen)
            }
            return gameField;
        }
        static void SomeoneRobbed(Citizen citizen, Robber robber, int height, int width)
        {
            int robberClocks = robber.StolenGoods.OfType<Item>().Where(x => x.ItemName == "Clock").Count();
            int robberCash = robber.StolenGoods.OfType<Item>().Where(x => x.ItemName == "Cash").Count();
            int robberKeys = robber.StolenGoods.OfType<Item>().Where(x => x.ItemName == "Keys").Count();
            int robberPhone = robber.StolenGoods.OfType<Item>().Where(x => x.ItemName == "Phone").Count(); 
            int citizenClocks = citizen.Belongings.OfType<Item>().Where(x => x.ItemName == "Clock").Count();
            int citizenCash = citizen.Belongings.OfType<Item>().Where(x => x.ItemName == "Cash").Count();
            int citizenKeys = citizen.Belongings.OfType<Item>().Where(x => x.ItemName == "Keys").Count();
            int citizenPhone = citizen.Belongings.OfType<Item>().Where(x => x.ItemName == "Phone").Count();
            int offsetFromTop = 4;
            int middlePositionLeft = width / 2;
            
            string[] toPrint = { "========================",
                                 " A robber has stolen ", 
                                 " from a citizen! ", 
                                 " This is their current ", 
                                 " inventories. ", 
                                 "======ROBBER============",
                                $" Clock x {robberClocks}", 
                                $" Cash x {robberCash}", 
                                $" Keys x {robberKeys}", 
                                $" Phone x {robberPhone}", 
                                 " ",  
                                 "======CITIZEN===========",
                                $" Clock x {citizenClocks}", 
                                $" Cash x {citizenCash}", 
                                $" Keys x {citizenKeys}", 
                                $" Phone x {citizenPhone}", 
                                 " ", 
                                "========================"
            };

            PrintWho(citizen, width);

            PrintInfoStatScreen(toPrint, offsetFromTop, middlePositionLeft);


            Thread.Sleep(5000);
            PrisonTimer();
            Console.Clear();
            Console.SetCursorPosition(0, height);

        }

        static void PrintInfo(int line, int column, string toPrint)
        {
            Console.SetCursorPosition(column, line);
            Console.Write(toPrint);
        }

        //skriver ut infon som vi samlat ihop i 
        static void PrintInfoStatScreen(string[] toPrint, int offsetFromTop, int middlePositionLeft)
        {
            for (int i = 0; i < toPrint.Length; i++)
            {
                string myLine;
                if (toPrint[i].Length < 25)
                {
                    myLine = "|";
                    myLine += toPrint[i];
                    while (myLine.Length < 25)
                    {
                        myLine += " "; 
                    }
                    myLine += "|";
                }
                else
                {
                    myLine = toPrint[i];
                }

                PrintInfo(offsetFromTop + i, middlePositionLeft, myLine);
            }
        }



        //om det var en eller flera poliser och en eller flera rånare på samma plats ska det här hända
        static char[,] TryToCatchRobber(List<Robber> robbers, List<Cop> cops, char[,] gameField, int width, int height)
        {
            foreach (Robber robber in robbers)
            {
                if (!robber.StolenGoods.Any()) // om rånaren inte har något i sin inventory är han ännu inte en rånare (eller har avtjänat sitt straff)
                {
                    continue;
                }
                prison.Add(robber);
                Cop cop = cops[rand.Next(cops.Count)];
                cop.TakeGoodsFromRobber(robber); // en slumpad polis tar rånarens saker (om det är flera på samma ställe)
                robber.TimesCaught++;
                cop.RobbersBusted++;
                gameField = AddLetterToGameField(gameField, robber.VerticalPosition, robber.HorizontalPosition, 'A');

                ShowLocations(gameField, height);
                SomeoneBusted(gameField, cop, robber, height, width);
                //RobberCaughtPrint(gameField, cop, robber, width, height); //gammal kod!
                numberOfRobbersCaught++;
            }
            return gameField;
        }


        //skapar en lista av typen Cop där alla poliser med samma position som person kommer med
        static List<Cop> CopsOnSpot(List<Person> persons, Person person)
        {
            List<Cop> citizens = persons.OfType<Cop>().Where(x => x.HorizontalPosition == person.HorizontalPosition && x.VerticalPosition == person.VerticalPosition).ToList();
            return citizens;
        }


        //skapar en lista av typen Robber där alla rånare med samma position som person kommer med
        static List<Robber> RobbersOnSpot(List<Person> persons, Person person)
        {
            List<Robber> citizens = persons.OfType<Robber>().Where(x => x.HorizontalPosition == person.HorizontalPosition && x.VerticalPosition == person.VerticalPosition).ToList();
            return citizens;
        }




        //samma som ovan fast för typen Citizen
        static List<Citizen> CitizensOnSpot(List<Person> persons, Person person)
        {
            List<Citizen> citizens = persons.OfType<Citizen>().Where(x => x.HorizontalPosition == person.HorizontalPosition && x.VerticalPosition == person.VerticalPosition).ToList();
            return citizens;
        }


        // om det inte har varit någon person på samma plats så ska de vanliga bokstäverna läggas till i arrayen
        static char[,] PrintRegularLocationMarkers(Person person, char[,] gameField)
        {
            if (person is Cop)
            {
                gameField = AddLetterToGameField(gameField, person.VerticalPosition, person.HorizontalPosition, 'P');
            }
            else if (person is Robber)
            {
                gameField = AddLetterToGameField(gameField, person.VerticalPosition, person.HorizontalPosition, 'R');
            }
            else
            {
                gameField = AddLetterToGameField(gameField, person.VerticalPosition, person.HorizontalPosition, 'I');
            }

            return gameField;
        }




        //lägger till en bokstav på fältet
        static char[,] AddLetterToGameField(char[,] gameField, int verticalPos, int horizontalPos, char letter)
        {
            gameField[verticalPos, horizontalPos] = letter;
            return gameField;
        }



        // sätter alla chars till mellanslag igen för att återställa "brädet"
        static char[,] ResetField(char[,] gameField)
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



        //varje person ska röra sig, skickar även med höjd och bredd för att kunna räkna ut om personen är vid en vägg
        static void MoveAround(List<Person> people, int height, int width)
        {
            foreach (Person person in people)
            {
                person.Move(height, width);
            }
        }

        // skickar med instansen av Random för att annars kommer (nästan) alla få samma position och move direction
        static List<Person> GenerateCops(int ammountToGenerate, int height, int width)
        {
            List<Person> cops = new List<Person>();
            for (int i = 1; i <= ammountToGenerate; i++)
            {
                int verticalPosition = rand.Next(height);
                int horizontalPosition = rand.Next(width);
                cops.Add(new Cop(verticalPosition, horizontalPosition, rand, i));
            }
            return cops;
        }

        static List<Person> GenerateRobbers(int ammountToGenerate, int height, int width)
        {
            List<Person> robbers = new List<Person>();
            for (int i = 1; i <= ammountToGenerate; i++)
            {
                int verticalPosition = rand.Next(height);
                int horizontalPosition = rand.Next(width);
                robbers.Add(new Robber(verticalPosition, horizontalPosition, rand, i));
            }
            return robbers;
        }
        static List<Person> GenerateRegularCitizens(int ammountToGenerate, int height, int width)
        {
            List<Person> regulars = new List<Person>();
            for (int i = 1; i <= ammountToGenerate; i++)
            {
                int verticalPosition = rand.Next(height);
                int horizontalPosition = rand.Next(width);
                regulars.Add(new Citizen(verticalPosition, horizontalPosition, rand, i));
            }
            return regulars;
        }

        static void ShowLocations(char[,] gameField, int height)
        {
            for (int verticalLocation = 0; verticalLocation <= gameField.GetUpperBound(0); verticalLocation++)
            {
                for (int horizontalLocation = 0; horizontalLocation <= gameField.GetUpperBound(1); horizontalLocation++)
                {
                    char letter = gameField[verticalLocation, horizontalLocation];
                    if (letter != ' ') // om det inte är ett mellanslag ska bokstaven skrivas ut på positionen
                    {
                        // skickar med höjden som användaren i början skrev in för återställa positionen på konsollens markör
                        PrintAtLocation(height, verticalLocation, horizontalLocation, letter);
                    }
                }
            }
            foreach (Robber prisoner in prison)
            {
                PrintAtLocation(height, prisoner.VerticalPosition, prisoner.HorizontalPosition, 'F');
            }
        }


        //ritar en pil brevid personen för att visa vem det hände något med
        static void PrintWho(Person p, int width)
        {
            if (p.HorizontalPosition + 3 > width)
            {
                Console.SetCursorPosition(p.HorizontalPosition - 3, p.VerticalPosition);
                Console.Write("-->");
            }
            else
            {
                Console.SetCursorPosition(p.HorizontalPosition + 1, p.VerticalPosition);
                Console.Write("<--");
            }
            Thread.Sleep(1500);
            Console.Clear();
        }









        static void PrintAtLocation(int defaultlocation, int vertical, int horizontal, char letter)
        { // default location är alltså samma som 2d-arrayens vertikala längd, dvs, den raden längst ner
            Console.SetCursorPosition(horizontal, vertical);
            if (letter == 'P')
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
            }
            else if (letter == 'F')
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else if (letter == 'R')
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else if (letter == 'H')
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }
            Console.Write(letter);
            Console.SetCursorPosition(0, defaultlocation);
            Console.ResetColor();
        }
    }
}












//skriver ut i konsollen vad personerna har för saker
//static void ListItems(Person person)
//{
//
//    if (person is Cop cop)
//    {
//        foreach (Item item in cop.SiezedItems)
//        {
//            Console.Write(item.ItemName + "\n");
//        }
//    }
//    else if (person is Robber robber)
//    {
//        foreach (Item goods in robber.StolenGoods)
//        {
//            Console.Write(goods.ItemName + "\n");
//        }
//    }
//    else
//    {
//        Citizen citizen = (Citizen)person;
//        foreach (Item item in citizen.Belongings)
//        {
//            Console.Write(item.ItemName + "\n");
//        }
//    }
//}











//static void CitizenRobbedPrint(char[,] gameField, Citizen citizen, Robber robber, int height, int width)
//{
//    ShowLocations(gameField, height);
//    Console.WriteLine("SOMEONE GOT ROBBED");
//    Console.WriteLine($"ROBBED TIMES: {citizen.TimesRobbed}");
//    Console.WriteLine($"The Robber with id {robber.ID} now has robbed {robber.PeopleRobbed} and has this in his inventory:");
//    ListItems(robber);
//    Console.WriteLine($"\nThe citizen with id {citizen.ID} now has: ");
//    ListItems(citizen);
//    PrisonTimer();
//    Thread.Sleep(1000);
//    Console.Clear();
//
//
//}


//// hämtar vilken rånare som blev tagen, och bredden för att pilen inte ska skrivas utanför skärmen
//static void RobberCaughtPrint(char[,] gameField, Cop cop, Robber robber, int height, int width)
//{
//    PrintWho(robber, width);
//    Console.WriteLine($"A ROBBER WITH ID {robber.ID} GOT BUSTED \nBusted times: {robber.TimesCaught}");
//    Console.WriteLine($"The cop with id {cop.ID} who busted him have busted {cop.RobbersBusted}, and now has these items: ");
//    ListItems(cop); // skriver ut vad polisen har för 
//    Thread.Sleep(1000);
//    PrisonTimer();
//    Console.Clear();
//    ShowLocations(gameField, height); // visar
//}


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