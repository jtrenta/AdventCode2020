 
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventCode2020Day10
{

    class Adapter : IComparable<Adapter>{
        private string raw;
        private int? joltage = null;
        public long NumPathsTo = 1;
        public Adapter NextAdapter;

        public int Joltage {
            get {
                return joltage == null ? 0 : (int)joltage;
            }
        }

        public Adapter(string inputstring) {
            this.raw = inputstring;
            joltage = int.Parse(this.raw);
        }

        public Adapter(int inputint) {
            this.raw = inputint.ToString();
            joltage = inputint;
        }

        public int CompareTo(Adapter other)
        {
            if (this.Joltage < other.Joltage)
            {
                return -1;
            }
            else if (this.Joltage > other.Joltage)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public bool CompatibleTo(Adapter other) {
            return (other.Joltage - this.Joltage) <= 3 && (other.Joltage - this.Joltage) > 0;
        }

        public bool CompatibleFrom(Adapter other) {
            return this.Joltage - other.Joltage <= 3 && (this.Joltage - other.Joltage) > 0;
        }

        public int Difference {
            get {
                if(NextAdapter == null) return -1;
                return NextAdapter.Joltage - this.Joltage;
            }
        }

    }

    class Chain {

        private string raw;
        private List<Adapter> Adapters;
        private int onesteps = 0;
        private int threesteps = 0;
        private long paths = 1;

        public Chain(string inputstring) {
            this.raw = inputstring;
            Adapters = new List<Adapter>();

            // Add the outlet as the first adapter
            Adapters.Add(new Adapter(0));

            foreach(string item in raw.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)) {
                Adapters.Add(new Adapter(item));
            }

            //Sort adapters so we can traverse from low to high
            Adapters.Sort();

            // Add the device as the final adapter
            Adapters.Add(new Adapter(Adapters.Max().Joltage + 3));

            for(int i = 0;i < Adapters.Count ;i++) {

                // Part 1
                // Ignore last adapter - there is no adapter for it to step to
                if(i < Adapters.Count - 1) {
                    Adapters[i].NextAdapter = Adapters[i+1];
                    if(Adapters[i].Difference == 1) this.onesteps++;
                    if(Adapters[i].Difference == 3) this.threesteps++;
                }

                //Part 2
                //Ignore first adapter - there is no adapter for it to step from
                if(i>0) {
                    //The number of paths leading to this adapter is the sum of the paths leading to any adapter 
                    //that can plug into the current adapter.
                    Adapters[i].NumPathsTo = Adapters.Where(a => (Adapters[i].CompatibleFrom(a))).ToList().Sum(b => b.NumPathsTo);
                }
            }
            paths = Adapters[Adapters.Count - 1].NumPathsTo;
        }

        public (int, int) Steps {
            get {
                return (onesteps, threesteps);
            }
        }

        public long Paths {
            get {
                return paths;
            }
        }

    }

    class Program
    {
        public static void DoDay10() {

            string inputstring = System.IO.File.ReadAllText(@"DayTenInput.txt");
            Chain InputData = new Chain(inputstring);
            System.Console.WriteLine("Answer Part 1: {0}\n", InputData.Steps.Item1*InputData.Steps.Item2);
            System.Console.WriteLine("Answer Part 2: {0}\n", InputData.Paths);
            
        }

    }

}
