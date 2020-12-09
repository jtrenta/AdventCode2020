 
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventCode2020Day9
{

    class XMAS {

        private Queue<int> data;
        private int size;
        private List<int> fullList;
        private List<(int, int, int)> ValidNumbers;
        private int? weakness = null;
        private int? answer = null;

        public XMAS(string inputstring, int size) {

            ValidNumbers = new List<(int, int, int)>();
            data = new Queue<int>();
            fullList = new List<int>();
            this.size = size;

            foreach(string item in inputstring.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)) {
                fullList.Add(int.Parse(item));
                if(!this.Add(int.Parse(item))) {
                    break;
                }
            }                
        }

        public int? Weakness {
            get {
                return this.weakness;
            }
        }

        public int? Answer {
            get {

                if(weakness == null) {
                    answer = null;
                }

                int end = 0, 
                    start = 0, 
                    test = fullList.ElementAt(0);

                while(end < fullList.Count && answer == null) {
                    if(test > weakness) {
                        test -= fullList.ElementAt(start++);
                    } else if(test < weakness) {
                        test += fullList.ElementAt(++end);
                    } else {
                        var answerList = fullList.GetRange(start, end - start);
                        answer = answerList.Min() + answerList.Max();
                    }
                }

                return answer;
            }
        }

        private void Recalculate(int? numToRemove, int numAdded) {

            if(numToRemove != null)
                ValidNumbers.RemoveAll(v => (v.Item2 == numToRemove || v.Item3 == numToRemove));

            foreach(int item in data) {
                if(item != numAdded) {
                    ValidNumbers.Add((item + numAdded, item, numAdded));
                }
            }

        }

        public bool Add(int num) {

            if(Valid(num)) {

                data.Enqueue(num);

                if(data.Count > size) {
                    Recalculate(data.Dequeue(), num);
                } else {
                    Recalculate(null, num);
                }

                return true;

            } else {

                weakness = num;
                return false;

            }
        }

        public bool Valid(int num) {
            return ((data.Count < size) || (ValidNumbers.Where(v => v.Item1 == num).Count() > 0));
        }

    }

    class Program
    {
        public static void DoDay9() {

            string inputstring = System.IO.File.ReadAllText(@"DayNineInput.txt");
            XMAS EncryptedData = new XMAS(inputstring, 25);
            int? weakness = EncryptedData.Weakness;
            int? answer = EncryptedData.Answer;
            System.Console.WriteLine("Weakness: {0}", weakness);
            System.Console.WriteLine("Answer: {0}", answer);
            
        }

    }

}
