using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CopsAndRobbers
{
    class Event
    {
        public static void TryRobbing(List<Robber> robbers, List<Citizen> citizens, out int robbedCitizens)
        {
            robbedCitizens = 0;

            foreach (Citizen citizen in citizens)
            {
                bool multipleRobbersCanRobTheSameCitizen = true;
                if (!citizen.Belongings.Any()) // om inte medborgaren har något på sig kan han inte bli rånad
                {
                    continue;
                }
                if (multipleRobbersCanRobTheSameCitizen)
                {

                    foreach (Robber r in robbers)
                    {
                        if (citizens.First().Equals(citizen))
                        {
                            GameField.AddMarker(citizen.VerticalPosition, citizen.HorizontalPosition, 'H');
                            ConsoleFunctions.ShowLocations();
                        }
                        r.StealFrom(citizen);
                        r.PeopleRobbed++;
                        robbedCitizens++;
                        citizen.TimesRobbed++;

                        Event.SomeoneRobbed(citizen, r);
                    }
                }
                else
                {

                    if (citizens.First().Equals(citizen))
                    {
                        GameField.AddMarker(citizen.VerticalPosition, citizen.HorizontalPosition, 'H');
                        ConsoleFunctions.ShowLocations();
                    }
                    Robber robber = robbers[Initialize.rand.Next(robbers.Count)]; // en slumpad rånare rånar personen
                    robber.StealFrom(citizen);
                    robber.PeopleRobbed++;
                    citizen.TimesRobbed++;
                    robbedCitizens++;

                    Event.SomeoneRobbed(citizen, robber);
                }
            }
        }
        static void SomeoneBusted(Cop cop, Robber robber)
        {
            int copClocks = cop.SiezedItems.OfType<Item>().Where(x => x.ItemName == "Clock").Count();
            int copCash = cop.SiezedItems.OfType<Item>().Where(x => x.ItemName == "Cash").Count();
            int copKeys = cop.SiezedItems.OfType<Item>().Where(x => x.ItemName == "Keys").Count();
            int copPhone = cop.SiezedItems.OfType<Item>().Where(x => x.ItemName == "Phone").Count();
            int offsetFromTop = 4;
            int middlePositionLeft = GameField.GameWidth / 2;
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
            int lastLine = toPrint.Length;
            int lastColumn = middlePositionLeft + toPrint[0].Length + 1; //när looten ska försvinna behöver den funktionen veta hur långa raderna var, (jag har gjort alla raden lika långa)


            ConsoleFunctions.PrintWho(robber);
            ConsoleFunctions.PrintInfoStatScreen(toPrint, offsetFromTop, middlePositionLeft);
            Thread.Sleep(5000);
            Prison.PrisonTimerTick();
            Prison.PutRobberInJail(robber); // råraren hamnar i finkan, men får inte sin tid plussad då han egentligen inte har suttit inne än
            ConsoleFunctions.ResetPartialScreen(offsetFromTop, middlePositionLeft, offsetFromTop + lastLine, offsetFromTop+lastColumn);
            Console.SetCursorPosition(0, GameField.GameHeight+1);
        }
        public static void SomeoneRobbed(Citizen citizen, Robber robber)
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
            int middlePositionLeft = GameField.GameWidth / 2;
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

            int lastLine = offsetFromTop + toPrint.Length;
            int lastColumn = middlePositionLeft + toPrint[0].Length + 1;

            ConsoleFunctions.PrintWho(citizen); //visar en pil brevid personen som det hände något med

            ConsoleFunctions.PrintInfoStatScreen(toPrint, offsetFromTop, middlePositionLeft);


            Thread.Sleep(5000);
            ConsoleFunctions.ResetPartialScreen(offsetFromTop, middlePositionLeft, lastLine, lastColumn);
            Prison.PrisonTimerTick();
            Console.SetCursorPosition(0, GameField.GameHeight+1);

        }
        public static void TryToCatchRobber(List<Robber> robbers, List<Cop> cops, out int robbersCaught)
        {
            robbersCaught = 0;
            foreach (Robber robber in robbers)
            {
                if (!robber.StolenGoods.Any()) // om rånaren inte har något i sin inventory är han ännu inte en rånare (eller har avtjänat sitt straff)
                {
                    continue;
                }
                Cop cop = cops[Initialize.rand.Next(cops.Count)];
                cop.TakeGoodsFromRobber(robber); // en slumpad polis tar rånarens saker (om det är flera på samma ställe)
                robber.TimesCaught++;
                cop.RobbersBusted++;
               GameField.AddMarker(robber.VerticalPosition, robber.HorizontalPosition, 'A');

                ConsoleFunctions.ShowLocations();
                SomeoneBusted(cop, robber);
                //RobberCaughtPrint(gameField, cop, robber, width, height); //gammal kod!
                robbersCaught++;
            }
        }
    }
}
