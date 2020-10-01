using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CopsAndRobbers
{
    class People
    {
        public static List<Person> TownPeople { get; set;}
        public static int RobbersIngame
        {
            get
            {
                return TownPeople.Where(x => x is Robber).Count();
            }
        }
        public static void MoveAround()
        {
            foreach (Person person in TownPeople)
            {
                person.Move();
            }
        }

        public static void CheckCollision(out int robbed, out int caught)
        {
            robbed = 0;
            caught = 0;
            //räknar igenom alla personer och sätter deras vanliga bokstav först, så det inte är halvfyllt när kollisioner ska kollas..
            //det var jag inte nöjd med helt enkelt :P 
            foreach (Person p in TownPeople)
            {
                GameField.RegularLocationMarkers(p);
            }
            foreach(Robber prisoner in Prison.Jail)
            {
                GameField.AddMarker(prisoner.VerticalPosition, prisoner.HorizontalPosition, 'F');
            }

            ConsoleFunctions.ShowLocations();
            List<Person> listOfAlreadyCheckedPersons = new List<Person>();
            foreach (Person person in TownPeople)
            {
                List<Cop> copsAtLocation = CopsOnSpot(TownPeople, person);
                List<Robber> robbersAtLocation = RobbersOnSpot(TownPeople, person);
                List<Citizen> citizensAtLocation = CitizensOnSpot(TownPeople, person);
                if (listOfAlreadyCheckedPersons.Contains(person)) // om personen redan har kollats kan vi kolla nästa direkt
                {
                    continue;
                }
                if (robbersAtLocation.Any() && citizensAtLocation.Any()) // om det finns en eller flera rånare/vanliga medborgare ska rånarna ta en sak av varje medborgare
                {
                    Event.TryRobbing(robbersAtLocation, citizensAtLocation, out int robbedPeople);
                    robbed += robbedPeople;
                }
                if (copsAtLocation.Any() && robbersAtLocation.Any()) // och om en polis eller rånare möts ska polisen ta hans saker
                {
                    Event.TryToCatchRobber(robbersAtLocation, copsAtLocation, out int caughtRobbers);
                    caught += caughtRobbers;
                }

                listOfAlreadyCheckedPersons.AddRange(copsAtLocation);
                listOfAlreadyCheckedPersons.AddRange(robbersAtLocation);
                listOfAlreadyCheckedPersons.AddRange(citizensAtLocation);

            }

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

    }
}
