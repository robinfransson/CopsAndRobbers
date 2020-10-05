using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CopsAndRobbers
{
    class Prison
    {
        public static List<Robber> Jail = new List<Robber>();

        public static int Prisoners
        {
            get
            {
                return Jail.Count;
            }
        }

        public static int NextRelease()
        {
            if(Jail.Any())
            {
                int prisonerReleaseTimer = Jail.Max(prisoner => prisoner.TimeInPrison);
                return PrisonTimer - prisonerReleaseTimer;
            }
            return 0;
        }
        public static int PrisonTimer { get; set; }

        static void RemoveRobberFromField()
        {
            if(Jail.Any())
            {
                foreach(Robber prisoner in Jail)
                {
                    if(People.TownPeople.Contains(prisoner))
                    {
                        People.TownPeople.Remove(prisoner); 
                    }
                }
            }

        }
        public static void CheckPrison()
        {
            RemoveRobberFromField();
            List<Robber> toRelease = Jail.Where(prisoner => prisoner.TimeInPrison >= PrisonTimer).ToList(); // om det finns någon som suttit klart i fängelset
            if (toRelease.Any())
            {
                foreach (Robber releasedRobber in toRelease)
                {
                    releasedRobber.TimeInPrison = 0;
                    Jail.Remove(releasedRobber); //så ska han släppas tillbaks
                    People.TownPeople.Add(releasedRobber);
                }
            }
        }

        //lägger på 5 i tid på varje fånge, vid 100 släpps den 
        public static void PrisonTimerTick()
        {
            foreach (Robber prisoner in Jail)
            {
                prisoner.TimeInPrison += 5;
            }
        }

        public static void PutRobberInJail(Robber r)
        {
            Jail.Add(r);
        }


    }
}
