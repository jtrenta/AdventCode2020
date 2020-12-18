using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventCode2020Day18
{

    class Equation 
    {
        private string raw;
        private long? value;
        private List<long> terms;
        private List<char> ops;

        public Equation(string inputstring, int Part) {
            this.raw = inputstring;
            if(Part == 1) {
                InitializePart1(inputstring);
            } else {
                InitializePart2(inputstring);
            }
            
        }

        private void InitializePart1(string inputstring) {
            int i = 0, j;
            value = null;
            char op = '+';
            while(i < inputstring.Length) {
                if(inputstring[i] == '(') {

                    j = i + 1;
                    int countPar = 1;

                    while(countPar >= 1) {
                        if(inputstring[j] == '(') countPar++;
                        else if(inputstring[j] == ')') countPar--;
                        j++;
                    }

                    if(value == null) {
                        value = new Equation(inputstring.Substring(i+1, j - i - 2), 1).Value;
                    } else {
                        value = Calc((long)value, new Equation(inputstring.Substring(i+1, j - i - 2), 1).Value, op);
                    }

                    i = j;

                } else {

                    if(inputstring[i] == '*' || inputstring[i] == '+') { 
                        op = inputstring[i];
                    } else if((int)inputstring[i] >= (int)'0' && (int)inputstring[i] <= (int)'9') {

                        j = inputstring.Substring(i).Split(' ')[0].Length;

                        if(value == null) {
                            value = int.Parse(inputstring.Substring(i, j));
                        } else {
                            value = Calc((long)value, int.Parse(inputstring.Substring(i, j)), op);
                        }

                        i += j;
                    }
                    i++;
                }
            }
        }

        private void InitializePart2(string inputstring) {
            int i = 0, j;
            int t = 1;
            terms = new List<long>();
            ops = new List<char>();

            value = null;
            terms.Add(0);
            ops.Add('+');

            while(i < inputstring.Length) {
                if(inputstring[i] == '(') {

                    j = i + 1;
                    int countPar = 1;

                    while(countPar >= 1) {
                        if(inputstring[j] == '(') countPar++;
                        else if(inputstring[j] == ')') countPar--;
                        j++;
                    }

                    terms.Add(new Equation(inputstring.Substring(i+1, j - i - 2), 2).Value);
                    i = j;

                } else {

                    if(inputstring[i] == '*' || inputstring[i] == '+') { 
                        ops.Add(inputstring[i]);
                        t++;
                    } else if((int)inputstring[i] >= (int)'0' && (int)inputstring[i] <= (int)'9') {
                        j = inputstring.Substring(i).Split(' ')[0].Length;
                        terms.Add(int.Parse(inputstring.Substring(i, j)));
                        i += j;
                    }
                    
                    i++;
                }
            }

            // To impliment order of operations, do all addition first
            i = 0;
            while(i < ops.Count()) {
                if(ops[i] == '+') {
                    terms[i] = terms[i] + terms[i+1];
                    terms.RemoveAt(i+1);
                    ops.RemoveAt(i);
                } else {
                    i++;
                }
            }

            // Now do multiplication
            i = 0;
            while(i < ops.Count()) {
                if(ops[i] == '*') {
                    terms[i] = terms[i] * terms[i+1];
                    terms.RemoveAt(i+1);
                    ops.RemoveAt(i);
                } else {
                    i++;
                }
            }
            value = terms[0];
        }

        private long Calc(long val1, long val2, char op) {
            if(op == '+') return val1 + val2;
            if(op == '*') return val1 * val2;
            else {
                System.Console.WriteLine("Invalid inputs to Calc {0} {1} {2}", val1, val2, op);
                return 0;
            }
        }

        public long Value { get { return value == null ? 0 : (long)value; } }
        public string Raw { get { return raw; } }

    }
 
    class Program
    {
        private static string inputstring;

        public static void DoDay18() {

            inputstring = System.IO.File.ReadAllText(@".\Input\DayEighteenInput.txt");
            string[] inputstrings = inputstring.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            DoPart1(inputstrings);
            DoPart2(inputstrings);

        }

        public static void DoPart1(string[] inputstrings) {
            Equation equation;
            long answer = 0;

            foreach(string item in inputstrings) {
                equation = new Equation(item, 1);
                //System.Console.WriteLine("{0} = {1}", equation.Raw, equation.Value);
                answer += equation.Value;
            }

            System.Console.WriteLine("Part 1 Answer: {0}", answer);
        }

        public static void DoPart2(string[] inputstrings) {
            Equation equation;
            long answer = 0;
            
            foreach(string item in inputstrings) {
                equation = new Equation(item, 2);
                //System.Console.WriteLine("{0} = {1}", equation.Raw, equation.Value);
                answer += equation.Value;
            }

            System.Console.WriteLine("Part 2 Answer: {0}", answer);

        }


    }

}
