using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CopsAndRobbers
{
    class GameField
    {
        public static int GameHeight { get; set; }
        public static int GameWidth { get; set; }
        public static char[,] PlayingField { get; set; }
        public static void GeneratePlayingField(int height, int width)
        {
            PlayingField = new char[height, width];
            ResetField(); // sätter alla tecken till mellanslag
        }

        public static void ResetField()
        {
            int verticalPosition = PlayingField.GetUpperBound(0);
            int horizontalPosition = PlayingField.GetUpperBound(1);

            for (int i = 0; i <= verticalPosition; i++)
            {
                for (int j = 0; j <= horizontalPosition; j++)
                {
                    PlayingField[i, j] = ' ';
                }
            }

        }        //lägger till en bokstav på fältet där personen står
        public static void AddMarker(int verticalPos, int horizontalPos, char letter)
        {
            PlayingField[verticalPos, horizontalPos] = letter;
        }
        // om det inte har varit någon person på samma plats så ska de vanliga bokstäverna läggas till i arrayen
        public static void RegularLocationMarkers(Person person)
        {
            try
            {

                if (person is Cop)
                {
                    AddMarker(person.VerticalPosition, person.HorizontalPosition, 'P');
                }
                else if (person is Robber)
                {
                    AddMarker(person.VerticalPosition, person.HorizontalPosition, 'R');
                }
                else
                {
                     AddMarker(person.VerticalPosition, person.HorizontalPosition, 'I');
                }

            }catch(Exception e)
            {
                MessageBox.Show($"PERSON: {person.VerticalPosition}, {person.HorizontalPosition}\n" +
                    $"GAMEFIELD: {PlayingField.GetUpperBound(0)}, {PlayingField.GetUpperBound(1)}\n" +
                    $"{e.ToString()}");
            }
        }
    }
}
