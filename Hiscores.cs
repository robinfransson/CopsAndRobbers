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
            


            int i = 0;
            int placeOnRight = Console.WindowWidth - offset;
            while (i <= 10)
            {
                if (i == 0)
                {
                    ConsoleFunctions.ColoredText("[C10]Most robbed", placeOnRight, i);
                    ConsoleFunctions.ColoredText($"[C6]ID:[C10] {mostRobbed.ID}[C15], score:[C12] {mostRobbed.TimesRobbed}", placeOnRight, i + 1);
                }
                else if (i == 2)
                {
                    ConsoleFunctions.ColoredText("[C10]Most robberies", placeOnRight, i);
                    ConsoleFunctions.ColoredText($"[C6]ID:[C10] {mostRobberies.ID}[C15], score:[C12] {mostRobberies.PeopleRobbed}", placeOnRight, i + 1);
                }
                else if (i == 4)
                {
                    ConsoleFunctions.ColoredText("[C10]Most caught", placeOnRight, i);
                    ConsoleFunctions.ColoredText($"[C6]ID:[C10] {mostTimesCaught.ID}[C15], score:[C12] {mostTimesCaught.TimesCaught}", placeOnRight, i + 1);
                }
                else if (i == 6)
                {
                    ConsoleFunctions.ColoredText("[C10]Most items [C12](robber)", placeOnRight, i);
                    ConsoleFunctions.ColoredText($"[C6]ID:[C10] {robberMostItems.ID}[C15], score:[C12] {robberMostItems.StolenGoods.Count}", placeOnRight, i + 1);
                }
                else if (i == 8)
                {
                    ConsoleFunctions.ColoredText("[C10]Most items [C9](cop)", placeOnRight, i);
                    ConsoleFunctions.ColoredText($"[C6]ID:[C10] {copMostItems.ID}[C15], score:[C12] {copMostItems.SiezedItems.Count}", placeOnRight, i + 1);
                }
                else if (i == 10)
                {
                    ConsoleFunctions.ColoredText("[C10]Most robbers busted", placeOnRight, i);
                    ConsoleFunctions.ColoredText($"[C6]ID:[C10] {mostRobbersBusted.ID}[C15], score:[C12] {mostRobbersBusted.RobbersBusted}", placeOnRight, i + 1);
                }
                i += 2; // plussar på med 2, så nästa utskrift hamnar under  id, score (dvs 2 rader under den förra)
            }
        }


        
    }
}
