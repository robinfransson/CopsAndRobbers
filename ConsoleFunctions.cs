using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CopsAndRobbers
{
    class ConsoleFunctions
    {
        public static void ResetDrawnGameMap(char[,] gameField)
        {
            int height = gameField.GetUpperBound(0); // hur hög spelskärmen är
            int width = gameField.GetUpperBound(1); //hur bred spelskärmen är
            for (int i = 0; i <= height; i++)
            {
                for (int j = 0; j <= width; j++)
                {
                    if (gameField[i, j] != ' ') // där det inte är ett mellanslag i arrayen ska det i fönstret ersättas med ett mellanslag ( ta bort bokstaven från skärmen)
                    {
                        PrintAtLocation(i, j, ' ');
                    }
                }
            }
        }


        //sätter konsollens pekare till vald rad och kolumn och skriver någonting
        public static void PrintInfo(int line, int column, string toPrint)
        {
            Console.SetCursorPosition(column, line);
            Console.Write(toPrint);
        }

        public static void ShowLocations()
        {
            for (int verticalLocation = 0; verticalLocation <= GameField.GameHeight; verticalLocation++)
            {
                for (int horizontalLocation = 0; horizontalLocation <= GameField.GameWidth; horizontalLocation++)
                {
                    char letter = GameField.PlayingField[verticalLocation, horizontalLocation];
                    if (letter != ' ') // om det inte är ett mellanslag ska bokstaven skrivas ut på positionen
                    {
                        // skickar med höjden som användaren i början skrev in för återställa positionen på konsollens markör
                        PrintAtLocation(verticalLocation, horizontalLocation, letter);

                    }
                }
            }
            foreach (Robber prisoner in Prison.Jail)
            {
                PrintAtLocation(prisoner.VerticalPosition, prisoner.HorizontalPosition, 'F');
            }

            Console.SetWindowPosition(0, 0);

        }


        //ritar en pil brevid personen för att visa vem det hände något med
        public static void PrintWho(Person p)
        {
            int hPos = p.HorizontalPosition;
            int vPos = p.VerticalPosition;
            if (p.HorizontalPosition + 3 > GameField.GameWidth) // kollar om det får plats att skriva en pil till höger
            {
                Console.SetCursorPosition(hPos -3, vPos); // pilen ska skrivas ut till vänster om personen
                Console.Write("-->");
                Thread.Sleep(1000);
                for (int i = 1; i < 4; i++)
                {
                    char letterBeforeArrow = GameField.PlayingField[vPos, hPos - i]; //tar bort pilen och återställer till det som var innan
                    PrintAtLocation(vPos, hPos - i, letterBeforeArrow);
                }
                Thread.Sleep(1000);

            }
            else
            {
                Console.SetCursorPosition(hPos + 1, vPos);
                Console.Write("<--"); //höger om personen
                Thread.Sleep(1000);
                for (int i = 3; i > 0; i--)
                {
                    char letterBeforeArrow = GameField.PlayingField[vPos, hPos + i];//tar bort pilen och återställer till det som var innan
                    PrintAtLocation(vPos, hPos + i, letterBeforeArrow);
                }
                Thread.Sleep(1000);
            }
            Console.SetCursorPosition(0, GameField.GameHeight + 1);
        }

        public static void PrintInfoStatScreen(string[] toPrint, int offsetFromTop, int middlePositionLeft)
        {
            for (int i = 0; i < toPrint.Length; i++)
            {
                string myLine;
                if (toPrint[i].Length < 25)
                {
                    myLine = "|";
                    myLine += toPrint[i];
                    while (myLine.Length < 25) // set till att alla rader har samma längd, har dom inte det så lägger programmet till ett mellanslag
                    {
                        myLine += " ";
                    }
                    myLine += "|"; //avslutar raden med ett |
                }
                else
                {
                    myLine = toPrint[i]; //annars skrivs raden ut direkt
                }
                //offsetFromTop är hur långt ifrån fönstrets höjd det ska skrivas ut
                PrintInfo(offsetFromTop + i, middlePositionLeft, myLine);


            }
            Console.SetCursorPosition(0, GameField.GameHeight+1);
            Thread.Sleep(5000);
        }

        //här återställer programmet "loot" rutan och skriver ut det som var innan
        public static void ResetPartialScreen(int verticalStart, int horizontalStart, int verticalEnd, int horizontalEnd)
        {
            for(int i = verticalStart; i <= verticalEnd; i++)
            {
                for(int j = horizontalStart; j <= horizontalEnd; j++)
                {
                    char letterToPrint = GameField.PlayingField[i, j];

                    PrintAtLocation(i, j, letterToPrint);
                }
            }
        }

        static void PrintAtLocation(int vertical, int horizontal, char letter)
        { // default location är alltså samma som 2d-arrayens vertikala längd, dvs, den raden längst ner
            Console.SetCursorPosition(horizontal, vertical);
            if (letter == 'P') //polis
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
            }
            else if (letter == 'F') // fånge
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else if (letter == 'R') // rånare
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else if (letter == 'H') // någon blir rånad
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
            }
            else if (letter == 'I') // invånare
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }
            else // arresterad
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
            }
            Console.Write(letter);
            Console.SetCursorPosition(0, GameField.GameHeight+1); //återställer inmatningsplats till sista raden
            Console.ResetColor();
        }
    }
}
