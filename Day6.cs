using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventCode2020Day6
{
    class Questionaire {

        private string raw;
        private string[] eachRaw;
        private int dya = 0;
        private int cya = 0;

        public int distinctYesAnswers {
            get {
                return dya;
            }
        }

        public int commonYesAnswers {
            get {
                return cya;
            }
        }

        public Questionaire(string inputstring) {
            raw = inputstring;

            foreach(char c in raw.ToCharArray().Distinct()) {
                if((int)c >= (int)'a' && (int)c <= 'z') {
                    dya++;
                }
            }

            eachRaw = raw.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

            for(int i = (int)'a';i <= (int)'z';i++) {
                bool matched = true;
                foreach(string item in eachRaw) {
                    if(!item.ToCharArray().Contains((char)i)) {
                        matched = false;
                    }
                }
                if(matched) {
                    cya++;
                }
            }
        }

    }

    class Program
    {

        public static void DoDay6() {

            string inputstring;
            List<Questionaire> Questionaires;
            Questionaires = new List<Questionaire>();
            Questionaire newquestionaire;

            inputstring = System.IO.File.ReadAllText(@"DaySixInput.txt");

            foreach(string item in inputstring.Split(new string[] { Environment.NewLine + Environment.NewLine }, 
                                                                    StringSplitOptions.RemoveEmptyEntries)) {
                newquestionaire = new Questionaire(item);
                Questionaires.Add(newquestionaire);
            }

            System.Console.WriteLine("Questions with distinct yes answers: {0}", Questionaires.Sum(i => i.distinctYesAnswers));
            System.Console.WriteLine("Questions with common yes answers: {0}", Questionaires.Sum(i => i.commonYesAnswers));
            
        }

    }

}
