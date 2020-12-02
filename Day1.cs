using System;
using System.Collections.Generic;
namespace AdventCode2020Day1
{
    class Program
    {

        public static void DoDay1() {
            List<int> InputList;
            InputList = Day1Part1();
            Day1Part2(InputList);
        }

        static List<int> Day1Part1() {
            string line;
            int value = 0, result = 0;
            bool found = false;
            System.Console.WriteLine("Day1Part1");
            List<int> InputList = new List<int>();
            System.IO.StreamReader file = new System.IO.StreamReader(@"DayOneInput.txt");  
            while((line = file.ReadLine()) != null)  
            {  
                if(int.TryParse(line, out value)) {
                    InputList.Add(value);
                    if(!found) {
                        result = InputList.Find(item => (item == (2020-value)));
                        if(result > 0) {
                            System.Console.WriteLine("Answer is {0} * {1} = {2}", value, result, value * result);
                            found = true;
                        }
                    }
                } else {
                    System.Console.WriteLine("Invalid input detected, exiting.");
                    Environment.Exit(0);
                }
            }  
            file.Close();  
            return InputList;
        }

        static void Day1Part2(List<int> InputList) {
            int result = 0;
            bool found = false;
            System.Console.WriteLine("Day1Part2");
            for (int i = 0; i < InputList.Count; i++) {
                int item1 = InputList[i];
                for (int i1 = InputList.Count -1; i1 > i; i1--) {
                    int item2 = InputList[i1];
                    if (item1 != item2) {
                        result = InputList.Find(item => (item == (2020-item1-item2)));
                        if(result > 0) {
                            System.Console.WriteLine("Answer is {0} * {1} * {2} = {3}", item1, item2, result, item1 * item2 * result);
                            found = true;
                            break;
                        }
                    }
                }
                if(found) break;
            }
        }
    }



}
