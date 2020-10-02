using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CopsAndRobbers
{
    class ConsoleFunctions
    {
        public static void ResetDrawnGameMap()
        {
            int height = GameField.GameHeight; // hur hög spelskärmen är
            int width = GameField.GameWidth; //hur bred spelskärmen är

            for (int i = 0; i <= height; i++)
            {
                for (int j = 0; j <= width; j++)
                {
                    if (GameField.PlayingField[i, j] != ' ') // där det inte är ett mellanslag i arrayen ska det i fönstret ersättas med ett mellanslag ( ta bort bokstaven från skärmen)
                    {
                        PrintCharAtLocation(j, i, ' ', 0, GameField.GameHeight + 1);
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
                        ShowEntityOnScreen(verticalLocation, horizontalLocation, letter);

                    }
                }
            }
            foreach (Robber prisoner in Prison.Jail)
            {
                ShowEntityOnScreen(prisoner.VerticalPosition, prisoner.HorizontalPosition, 'F');
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
                    PrintCharAtLocation(hPos - i, vPos, letterBeforeArrow, 0, GameField.GameHeight + 1);
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
                    PrintCharAtLocation(hPos + i,vPos,  letterBeforeArrow, 0, GameField.GameHeight + 1);
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

                    PrintCharAtLocation(j, i, letterToPrint, 0, GameField.GameHeight + 1);
                }
            }
        }
        static ConsoleColor GetEntityColor(char entity)
        {
            switch(entity)
            {
                case 'P':
                    return SelectColor(9);
                case 'F':
                    return SelectColor(10);
                case 'R':
                    return SelectColor(12);
                case 'I':
                    return SelectColor(14);
                case 'H':
                    return SelectColor(2);
                case 'A':
                    return SelectColor(13);
                default: // default blir vit
                    return SelectColor(15);
            }
        }
        static void ShowEntityOnScreen(int vertical, int horizontal, char letter)
        { // default location är alltså samma som 2d-arrayens vertikala längd, dvs, den raden längst ner
            Console.ForegroundColor = GetEntityColor(letter);
            PrintCharAtLocation(horizontal, vertical, letter, 0, GameField.GameHeight + 1);
            Console.ResetColor();
        }

        //färgar texten som konsollen ska skriva ut
        /// <summary>
        /// Colors a string with individual colors\n
        /// usage: [CX] or [CXX] where XX is a number between 0 to 15 where the colors are as following (if the number provided is higher than 15 it will be set to 15).:
        /// //färgernas koder
        ///Black 0 
        ///DarkBlue 1
        ///DarkGreen 2
        ///DarkCyan 3
        ///DarkRed 4
        ///DarkMagenta 5
        ///DarkYellow 6
        ///Gray 7
        ///DarkGray 8
        ///Blue 9
        ///Green 10
        ///Cyan 11
        ///Red 12
        ///Magenta 13
        ///Yellow 14
        ///White 15
        /// </summary>
        /// <returns>
        /// A colored string at the position provided
        /// </returns>
        /// <param name="colorThis">String to color</param>
        /// <param name="horizontalPos">Horizontal position on the screen to start printing at</param>
        /// <param name="verticalPos">Vertical position on the screen to start printing at</param>
        public static void ColoredText(string colorThis, int horizontalPos, int verticalPos)
        {
            int nextplacement = 0;
            Regex regex = new Regex(@"(\[[C]\d{1,2}\])"); // regex matchar input strängen i formated [CXX] där XX är siffror
            string[] split = regex.Split(colorThis); // splittrar strängen där regex hittar en träff
            for (int i = 0; i < split.Length; i++)
            {
                // plussar på den gamla strängens längd så de inte hamnar på samma ställe
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

                    
                    PrintStringAtLocation(horizontalPos + nextplacement, verticalPos, split[i], 0, GameField.GameHeight + 1);
                    nextplacement += split[i].Length;// nästa text ska inte hamna över den gamla så vi behöver spara hur lång den förra utskriften var
                    Console.ResetColor();
                }

            }
        }

        public static void PrintCharAtLocation(int horizontalPosition, int verticalPosition, char letterToWrite, int resetCursorPositionLeft, int resetCursorPositionUp)
        {
            Console.SetCursorPosition(horizontalPosition, verticalPosition); // sätter inmatningsplats till de angivna koordinaterna
            Console.Write(letterToWrite);
            Console.SetCursorPosition(resetCursorPositionLeft, resetCursorPositionUp); //återställer inmatningsplats till den angivna raden+kolumn 
            Console.ResetColor();
        }
        public static void PrintStringAtLocation(int horizontalPosition, int verticalPosition, string textToWrite, int resetCursorPositionLeft, int resetCursorPositionUp)
        {
            Console.SetCursorPosition(horizontalPosition, verticalPosition); // sätter inmatningsplats till de angivna koordinaterna
            Console.Write(textToWrite);
            Console.SetCursorPosition(resetCursorPositionLeft, resetCursorPositionUp); //återställer inmatningsplats till den angivna raden+kolumn 
            Console.ResetColor();
        }
        /// <summary>
        /// Returns the ConsoleColor of the number provided
        /// </summary>
        /// <param name="color">The color to select from ConsoleColor</param>
        /// <returns>Returns the ConsoleColor in the enum of ConsoleColor, see Colored text for codes.</returns>
        public static ConsoleColor SelectColor(int color)
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
