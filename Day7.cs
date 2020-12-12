using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventCode2020Day7
{
    public class Bag {

        private string adjective;
        private string color;
        private int num = 1;
        private List<Bag> allowedBags;

        public List<Bag> AllowedBags {
            get {
                return this.allowedBags;
            }
        }

        public bool Equals(Bag other) {
            if(other == null) return false;
            return (other.description == this.description);
        }

        public string description {
            get {
                return adjective + " " + color;
            }
        }

        public Bag(string adjective, string color, string num = "1") {
            this.adjective = adjective;
            this.color = color;
            int.TryParse(num, out this.num);
            allowedBags = new List<Bag>();
        }

        public void AddAllowedBag(Bag inputBag) {
            allowedBags.Add(inputBag);
        }

        public bool IsAllowed(Bag checkBag) {
            return allowedBags.Any(b => b.description == checkBag.description);
        }

        public int NumAllowed {
            get {
                return num;
            }
        }

    }

    public class Rule {

        private string raw;
        public Bag subject;

        public string description {
            get {
                return subject.description;
            }
        }

        public Rule(string inputstring) {
            this.raw = inputstring;
            string containsString;
            inputstring = inputstring.Replace("bags", "bag");
            subject = new Bag(inputstring.Split(' ')[0], raw.Split(' ')[1]);
            containsString = inputstring.Split(" contain ")[1];
            if(containsString != "no other bag.") {
                containsString.Replace(".", "");
                foreach(string item in containsString.Split(',')) {
                    subject.AddAllowedBag(new Bag(  item.Trim().Split(' ')[1],
                                                    item.Trim().Split(' ')[2],
                                                    item.Trim().Split(' ')[0]));
                }
            } 
        }

        public List<Bag> AllowedBags {
            get {
                return subject.AllowedBags;
            }
        }

        public bool IsAllowed(Bag checkBag) {
            return subject.IsAllowed(checkBag);
        }

        public bool IsAllowed(string desc) {
            Bag checkBag = new Bag(desc.Split(' ')[0], desc.Split(' ')[0]);
            return subject.IsAllowed(checkBag);
        }
    }

    class Program
    {
        public static List<Rule> Rules;

        public static void DoDay7() {

            string inputstring;
            Rules = new List<Rule>();
            Rule newRule;
            int numCombinations = 0;
            int numNecessary = 0;

            inputstring = System.IO.File.ReadAllText(@".\Input\DaySevenInput.txt");

            foreach(string item in inputstring.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)) {
                newRule = new Rule(item);
                Rules.Add(newRule);
            }

            Bag myBag = new Bag("shiny", "gold");

            foreach(Rule checkRule in Rules) {
                if(checkRule.description != myBag.description) {
                    if(RecursiveCheckRule(checkRule, myBag)) {
                        numCombinations++;
                    }
                }
            }

            Rule myRule = Rules.Find(r => r.description == myBag.description);

            numNecessary = CountBags(myRule, 1, 1) - 1;

            System.Console.WriteLine("Types of Bags that can contain your bag: {0}", numCombinations);
            System.Console.WriteLine("Number of Bags necessary: {0}", numNecessary);
            
        }

        static bool RecursiveCheckRule(Rule checkRule, Bag checkBag, int depth = 1) {
            if(depth > 50) return false;
            if(checkRule.IsAllowed(checkBag)) {
                return true;
            }
            foreach(Bag checkBags in checkRule.AllowedBags) {
                if(RecursiveCheckRule(Rules.Find(r => r.description == checkBags.description), checkBag, depth + 1)) 
                    return true;
            }
            return false;
        }

        // Recurse through rules, passing the rule, and the number of bags of that type the previouse rule allowed
        static int CountBags(Rule checkRule, int numAllowed, int depth = 1) {
            // In case something goes wrong, prevent infinite recursion
            if( depth > 1000) return 1;  

            // If this bag can't contain more bags, return the number of bags from previous rule
            if(checkRule.subject.AllowedBags.Count() == 0) {  
                return numAllowed;
            }

            int tempTotal = 0;

            // For each type of bag a rule define, multiply the number of bags the previous rule allowed
            // by the number of bags contained in that type of bag
            foreach(Bag checkBags in checkRule.AllowedBags) {
                tempTotal += CountBags( Rules.Find(r => r.description == checkBags.description), 
                                        checkBags.NumAllowed, 
                                        depth + 1) * numAllowed;
            }
            
            // Return the number of bags contained in this bag type, plus the number of bags of this type we passed
            return tempTotal + numAllowed;
        }

    }

}
