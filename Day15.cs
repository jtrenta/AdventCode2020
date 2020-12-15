using System;
using System.Collections.Generic;

namespace AdventCode2020Day15
{

    class Program
    {
        private static string inputstring;

        public static void DoDay15() {

            Dictionary<int, int> Nodes = new Dictionary<int, int>();;
            int turn = 1;
            int lastNum = 0;
            int part1Target = 2020;
            int part2Target = 30000000;
            int prevNumTurn = 0;
            int newNum = 0;

            inputstring = System.IO.File.ReadAllText(@".\Input\DayFifteenInput.txt");
            string[] inputstrings = inputstring.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            foreach(string item in inputstrings[0].Split(',')) {
                lastNum = int.Parse(item);
                Nodes.Add(lastNum, turn++);
            }

            prevNumTurn = turn - 1;

            while(turn <= part1Target) {
                newNum = turn - prevNumTurn - 1;
                prevNumTurn = Nodes.GetValueOrDefault(newNum, turn);
                Nodes[newNum] = turn++;
                lastNum = newNum;
            }
            System.Console.WriteLine("Answer Part 1: {0}", lastNum);

            while(turn <= part2Target) {
                newNum = turn - prevNumTurn - 1;
                prevNumTurn = Nodes.GetValueOrDefault(newNum, turn);
                Nodes[newNum] = turn++;
                lastNum = newNum;
            }
            System.Console.WriteLine("Answer Part 2: {0}", lastNum);
        }

    }

}
