using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventCode2020Day16
{

    static class Constants {
        public const int RANGES = 0;
        public const int TICKETS = 1;
    }

    class Rule {
        private string raw;
        private string name;
        private int[][] range;
        public int index = -1;

        public Rule(string inputstring) {
            this.raw = inputstring;
            string[] tempstring;
            tempstring = inputstring.Split(':');
            name = tempstring[0];
            tempstring = tempstring[1].Split(" or ");
            this.range = new int[2][];
            for(int i = 0;i<2;i++) {
                this.range[i] = new int[2];
                this.range[i] = Array.ConvertAll(tempstring[i].Split('-'), a => int.Parse(a));
            }
        } 

        public bool InRange(int value) {
            return (value >= range[0][0] && value <= range[0][1]) || (value >= range[1][0] && value <= range[1][1]); 
        }

        public string Raw {
            get {
                return raw;
            }
        }

        public string Name {
            get {
                return name;
            }
        }
    }

    class Ticket {
        private string raw;
        private List<(int value, bool valid)> values;
        public Ticket(string inputstring, List<Rule> rules) {
            values = new List<(int, bool)>();
            this.raw = inputstring;
            inputstring.Split(',').ToList().ForEach(x => values.Add((int.Parse(x), ValidValue(int.Parse(x), rules))));
        }

        public bool ValidValue(int value, List<Rule> rules) {
            foreach(Rule rule in rules) {
                if(rule.InRange(value)) {
                    return true;
                }
            }
            return false;
        }

        public long DepartureValue(List<Rule> rules) {
            long returnValue = 1;
            List<int> indexes = rules.Where(r => r.Name.StartsWith("departure")).Select(r => r.index).ToList();
            foreach(int i in indexes) {
                if(i < 0) {
                    System.Console.WriteLine("Invalid indexes found.");
                    break;
                } else {
                    returnValue *= values[i].value;
                }
            }
            return returnValue;
        }

        public List<int> InvalidValues(List<Rule> rules) {
            return values.Where(v => !v.valid).Select(v => v.value).ToList();
        }

        public int length {
            get {
                return values.Count;
            }
        }

        public int Value(int index) {
            return values[index].value;
        }

        public bool ValidRule(int index, Rule rule) {
            return rule.InRange(values[index].value);
        }

        public string Raw {
            get {
                return raw;
            }
        }

    }

    class Program
    {
        private static string inputstring;
        private static List<Rule> rules;
        private static List<Ticket> tickets;

        public static void DoDay16() {
            rules = new List<Rule>();
            tickets = new List<Ticket>();
            inputstring = System.IO.File.ReadAllText(@".\Input\DaySixteenInput.txt");
            int phase = Constants.RANGES;
            string[] inputstrings = inputstring.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            foreach(string item in inputstrings) {
                if(phase == Constants.RANGES) {
                    if(!item.StartsWith("your ticket:")) {
                        rules.Add(new Rule(item));
                    } else {
                        phase = Constants.TICKETS;
                    }
                } else if (phase == Constants.TICKETS && !item.StartsWith("nearby tickets:")) {
                    tickets.Add(new Ticket(item, rules));
                }
            }
            DoPart1();
            DoPart2();
        }

        public static void DoPart1() {
            List<int> invalidValues = new List<int>();
            tickets.ForEach(t => invalidValues.AddRange(t.InvalidValues(rules)));
            System.Console.WriteLine("Answer Part 1: {0}", invalidValues.Sum());

        }

        public static void DoPart2() {
            //Remove all invalid tickets
            tickets = (tickets.Where(ticket => ticket.InvalidValues(rules).Count == 0)).ToList();

            //Create list of indices and rules that are valid combinations
            List<(int index, Rule rule)> indices = new List<(int index, Rule rule)>();
            for(int i = 0;i < tickets[0].length;i++) {
                indices.AddRange(from Rule rule in rules
                                 where tickets.All(v => v.ValidRule(i, rule))
                                 select (i, rule));
            }

            //Order list by how many rules are valid for that index (count how many times that index appears)
            indices = indices.GroupBy(i => i.index).OrderBy(g => g.Count()).SelectMany(x=>x).ToList();

            //Should pass through list just once, as each time we remove rules the top index should only have one valid rule
            while(indices.Count > 0) {
                indices[0].rule.index = indices[0].index;
                indices.RemoveAll(n => n.rule == indices[0].rule);
            }

            System.Console.WriteLine("Answer Part 2: {0}", tickets[0].DepartureValue(rules));

        }

    }

}
