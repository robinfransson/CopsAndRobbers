using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CopsAndRobbers
{
    class Hiscores
    {
        public static void GetScores()
        {
            
            //placerar highscores första bokstav 22 rutor från kanten till höger
            int hiscoreOffset = 22;
            //vem har blivit rånad mest
            int mostTimesRobbed = People.TownPeople.OfType<Citizen>().Max(x => x.TimesRobbed);
            Citizen mostRobbedCitizen = People.TownPeople.OfType<Citizen>().First(x => x.TimesRobbed == mostTimesRobbed);



            //vem har rånat flest

            int mostRobberies = People.TownPeople.OfType<Robber>().Max(x => x.PeopleRobbed);
            Robber mostRobberiesCommitted = People.TownPeople.OfType<Robber>().First(x => x.PeopleRobbed == mostRobberies);



            //vilken rånare som har blivit tagen flest gånger
            int mostTimesCaught = People.TownPeople.OfType<Robber>().Max(x => x.TimesCaught);
            Robber mostCaughtRobber = People.TownPeople.OfType<Robber>().First(x => x.TimesCaught == mostTimesCaught);

            //vilken rånare har mest items i sin inventory
            int mostCurrentItemsStolen = People.TownPeople.OfType<Robber>().Max(x => x.StolenGoods.Count);
            Robber robberWithMostitems = People.TownPeople.OfType<Robber>().First(x => x.StolenGoods.Count == mostCurrentItemsStolen);

            //vilken polis har mest items i sin inventory
            int mostItemsSiezed = People.TownPeople.OfType<Cop>().Max(x => x.SiezedItems.Count);
            Cop copWithMostItems = People.TownPeople.OfType<Cop>().First(x => x.SiezedItems.Count == mostItemsSiezed);


            //vilken polis som har tagit flest rånare
            int mostRobbersCaugh = People.TownPeople.OfType<Cop>().Max(x => x.RobbersBusted);
            Cop copWithMostRobbersCaught = People.TownPeople.OfType<Cop>().First(x => x.RobbersBusted == mostRobbersCaugh);

            //skriver ut highscores uppe till höger
            PrintHiScores(hiscoreOffset, mostRobbedCitizen, mostRobberiesCommitted, 
                mostCaughtRobber, robberWithMostitems, copWithMostItems, copWithMostRobbersCaught);
        }
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


        //färgar texten som konsollen ska skriva ut
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

                        Console.ForegroundColor = SelectColor(color); // sätter färgen till den valda (eller vit om talet var för högt)
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
                color = 15;
                return (ConsoleColor)color;
            }

            return (ConsoleColor)color;
        }
    }
}
