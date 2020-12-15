 
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventCode2020Day13
{


    class Bus : IComparable<Bus>{
        private int cycle;
        private long currentTime;
        private long nextPickup = long.MaxValue;
        private int index;

        public Bus(int input, long time, int index) {
            cycle = input;
            SetCurrentTime(time);
            this.index = index;
        }

        public int Index {
            get {
                return index;
            }
        }

        public long NextPickup {
            get {
                return nextPickup;
            }
        }

        public void SetCurrentTime(long time) {
            currentTime = time;
            nextPickup = (cycle - time % cycle) % cycle;
        }

        public int CompareTo(Bus other)
        {
            if      (this.nextPickup < other.NextPickup)    return -1;
            else if (this.nextPickup > other.NextPickup)    return  1;
            else                                            return  0;
        }

        public int BusID {
            get {
                return cycle;
            }
        }
    }

    class Program
    {
        private static int time;
        private static string inputstring;

        public static void DoDay13() {

            inputstring = System.IO.File.ReadAllText(@".\Input\DayThirteenInput.txt");
            string[] intputstrings = inputstring.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            
            time = int.Parse(intputstrings[0]);
            List<Bus> Buses;
            for(int j = 1;j < intputstrings.Count();j++) {
                Buses = new List<Bus>();
                int i = 0;
                foreach(string item in intputstrings[j].Replace('x', '1').Split(',')) {
                    Buses.Add(new Bus(int.Parse(item), time, i++));
                }
                if(j == 1) DoPart1(Buses);
                DoPart2(Buses);
            }

        }

        public static void DoPart1(List<Bus> Buses) {
            List<Bus> iBuses = Buses.ToList();
            iBuses.Sort();
            iBuses.RemoveAll(b => b.BusID == 1);
            System.Console.WriteLine("Answer Part 1: {0}", iBuses[0].BusID * iBuses[0].NextPickup);
        }

        public static void DoPart2(List<Bus> Buses) {
            List<Bus> BusesByCycleTime = Buses.ToList();
            BusesByCycleTime.RemoveAll(b => b.BusID == 1);
            long timeincrement = BusesByCycleTime[0].BusID;
            long time = timeincrement;
            bool done = false;
            int i=0;
            while(!done) {
                if((time + BusesByCycleTime[i+1].Index) % BusesByCycleTime[i+1].BusID == 0) {
                    timeincrement *= BusesByCycleTime[i+1].BusID;
                    i++;
                }
                if(i == (BusesByCycleTime.Count - 1))
                    done = true;
                else
                    time += timeincrement;
            }
            System.Console.WriteLine("Answer Part 2: {0}", time);

        }

    }

}
