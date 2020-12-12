using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventCode2020Day5
{
    class BoardingPass  : IComparable<BoardingPass>{
        private string raw;
        private int[] row = { 0, 127 };
        private int[] col = {0, 7};
        public int seatID {
            get {
                for(int i = 0;i < 10;i++) {
                    if(i<7) {
                        if(raw.ToCharArray()[i] == 'F') {
                            row[1] = row[0] + ((row[1] - row[0] + 1) / 2) - 1;
                        } else if(raw.ToCharArray()[i] == 'B') {
                            row[0] = row[0] + ((row[1] - row[0] + 1) / 2);
                        }
                    } else {
                        if(raw.ToCharArray()[i] == 'L') {
                            col[1] = col[0] + ((col[1] - col[0] + 1) / 2) - 1;
                        } else if(raw.ToCharArray()[i] == 'R') {
                            col[0] = col[0] + ((col[1] - col[0] + 1) / 2);
                        }

                    }
                }
                return row[0] * 8 + col[0];
            }
        }

        public BoardingPass(string inputstring) {
            raw = inputstring;
        }

        public int CompareTo(BoardingPass other)
        {
            if (this.seatID < other.seatID)
            {
                return 1;
            }
            else if (this.seatID > other.seatID)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }

    }

    class Day5Data {

        private List<BoardingPass> BoardingPasses;
        private string raw;
        private string[] passstrings;
        public int maxseatID = 0;
        public Day5Data(string inputstring) {
            BoardingPasses = new List<BoardingPass>();
            BoardingPass newpass;
            raw = inputstring;
            passstrings = raw.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            foreach(string item in passstrings) {
                newpass = new BoardingPass(item);
                BoardingPasses.Add(newpass);
                //System.Console.WriteLine("SeatID: {0}", newpass.seatID);
                if(newpass.seatID > maxseatID) { 
                    maxseatID = newpass.seatID;
                }
            }
            System.Console.WriteLine("Max SeatID: {0}", maxseatID);
        }

        public int FindSeat() {
            BoardingPasses.Sort();
            int index = 1, seat = 0;
            while(index < BoardingPasses.Count) {
                seat = BoardingPasses.ElementAt(index).seatID - 1;
                if(!BoardingPasses.Exists(x => x.seatID == seat))
                    return seat;
                index++;
            }
            return 0;
        }
    }

    class Program
    {

        public static void DoDay5() {
            string passportstring = System.IO.File.ReadAllText(@".\Input\DayFiveInput.txt");
            Day5Data Passes = new Day5Data(passportstring);
            System.Console.WriteLine("Your seat: {0}", Passes.FindSeat());
        }

    }

}
