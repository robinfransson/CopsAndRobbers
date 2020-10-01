using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace CopsAndRobbers
{
    class Program
    {
        static void Main()
        {
            int numberOfRobbedCitizens = 0;
            int numberOfRobbersCaught = 0;
            Console.CursorVisible = false;
            Initialize.Start();
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
                Hiscores.GetScores(); //hämtar highscores
                Prison.CheckPrison(); //kollar fängelset om det är någon rånare som ska läggas till i spelplanen
                People.MoveAround(); //alla utom dom i fängelset ska röra sig


                People.CheckCollision(out int robbed, out int caught); // kollar om någon står på samma plats som någon annan, och skickar tillbaks hur många som blivit rånade/tagna

                ConsoleFunctions.ShowLocations(); //visar vart alla står
                numberOfRobbedCitizens += robbed;
                numberOfRobbersCaught += caught;
                ConsoleFunctions.PrintInfo(GameField.GameHeight+2, 0, $"Number of Robbers caught: {numberOfRobbersCaught}\n" +
                    $"Number of Citizens robbed: {numberOfRobbedCitizens}, Robbers in prison: {Prison.Prisoners}\n" +
                    $"Number of robbers not in prison: {People.RobbersIngame}");
                Thread.Sleep(300);
                ConsoleFunctions.ResetDrawnGameMap(GameField.PlayingField); // istället för console clear skriver programmet ett " " där det finns en bokstav (mindre flicker)
                GameField.ResetField(); // nollställer 2d arrayen så de gamla bokstäverna inte skrivs ut igen
            }
        }
    }
}
