using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventCode2020Day2
{
    class Day2Data {
        private int minimum = 0;
        private int maximum = 0;
        private char letter = ' ';
        private string password = "";
        private string raw;
        private string[] arguments;
        private int lettercount = 0;

        public char[] passwordArray {
            get {
                return password.ToCharArray();
            }
        }

        public bool validdayone {
            get {
                return (lettercount >= minimum && lettercount <= maximum);
            }
        }

        public bool validdaytwo {
            get {
                return ((passwordArray[minimum-1].Equals(letter) && !passwordArray[maximum-1].Equals(letter))
                    || (!passwordArray[minimum-1].Equals(letter) && passwordArray[maximum-1].Equals(letter)));
            }
        }

        public string input {
            get {
                return raw;
            }
            set {
                raw = value;
                arguments = raw.Split(' ');
                int.TryParse(arguments[0].Split('-')[0], out minimum);
                int.TryParse(arguments[0].Split('-')[1], out maximum);
                letter = arguments[1].ToCharArray()[0];
                password = arguments[2];
                lettercount = 0;
                for(int i = 0;i<password.Length;i++) {
                    if(passwordArray[i].Equals(letter)) {
                        lettercount++;
                    }
                }
            }
        }
        public Day2Data(string inputstring) {
            input = inputstring;
        }
    }

    class Program
    {

        public static void DoDay2() {
            List<Day2Data> InputList;
            InputList = Day2Part1();
            Day2Part2(InputList);
        }

        static List<Day2Data> Day2Part1() {
            string line;
            Day2Data inputdata;
            List<Day2Data> inputlist = new List<Day2Data>();
            System.IO.StreamReader file = new System.IO.StreamReader(@"DayTwoInput.txt");  
            while((line = file.ReadLine()) != null)  
            {
                inputdata = new Day2Data(line);
                inputlist.Add(inputdata);
            }
            System.Console.WriteLine("Lines read: {0}", inputlist.Count());
            System.Console.WriteLine("Number of valid passwords is {0}", (from item in inputlist where item.validdayone select item).Count());
            return inputlist;
        }

        static void Day2Part2(List<Day2Data> inputlist) {
            System.Console.WriteLine("Number of valid passwords is {0}", (from item in inputlist where item.validdaytwo select item).Count());
        }
    }

}
