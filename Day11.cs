 
using System;
using System.Linq;

namespace AdventCode2020Day11
{

    public static class Constants {
        public const int Floor = 1;
        public const int Empty = 2;
        public const int Taken = 4;
    }

    class Map {
        string raw;
        private int[,] matrix;
        private int height;
        private int width;
        private int seatstaken;
        private int sightDistance;
        private int seatTolerance;

        public Map(string inputstring, int distance, int tolerance) {
            seatstaken = 0;
            this.raw = inputstring;
            this.sightDistance = distance;
            this.seatTolerance = tolerance;
            height = inputstring.Count(i => (i == '\n')) + 1;
            width = inputstring.IndexOf('\n');
            matrix = new int[height,width];
            int h = 0, w = 0;

            foreach(string item in raw.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)) {
                foreach(char c in item.ToCharArray()) {
                    matrix[h,w] = MapChar(c);
                    if(matrix[h,w++] == Constants.Taken) seatstaken++;
                }
                h++;
                w=0;
            }

        }

        private int MapChar(char c) {
            if(c == '.') return Constants.Floor;
            if(c == 'L') return Constants.Empty;
            if(c == '#') return Constants.Taken;
            return 0;
        }

        private char CharMap(int i) {
            if(i == Constants.Floor) return '.';
            if(i == Constants.Empty) return 'L';
            if(i == Constants.Taken) return '#';
            return ' ';
        }

        // Steps the whole map
        public bool Step() {
            bool returnValue = false;
            int[,] newMap = (int[,])matrix.Clone();
            for(int h = 0;h < height; h++) {
                for(int w = 0;w < width; w++) {
                    if(StepSeat(h, w, newMap) && !returnValue) returnValue = true;
                }
            }
            matrix = (int[,])newMap.Clone();
            return returnValue;
        }

        public int SeatsTaken {
            get {
                return seatstaken;
            }
        }

        public void PrintSeats() {
            string line = "";
            System.Console.WriteLine("\n");
            for(int h = 0;h < height; h++) {
                for(int w = 0;w < width; w++) {
                    line += CharMap(matrix[h,w]);
                }
                System.Console.WriteLine(line);
                line = "";
            }

        }

        // Checks a single direction by taking in how much to increment position by
        // So checking diagonally up and left would be incrementing height and width by -1
        // While checking straight up would increment height by 0 and width by -1, etc
        // To get step 1 results, limit the sight distance to 1
        private int CheckDirection(int heightInc, int widthInc, int height, int width) {
            int h = height + heightInc;
            int w = width + widthInc;
            int distance = 1;
            if(heightInc == 0 && widthInc == 0) return 0;
            while(  h >= 0 && h < this.height && 
                    w >= 0 && w < this.width && 
                    distance <= this.sightDistance &&
                    matrix[h,w] == Constants.Floor) {
                h += heightInc;
                w += widthInc;
                distance++;
            }
            if(h < 0 || h >= this.height || w < 0 || w >= this.width || distance > this.sightDistance) {
                return Constants.Floor;
            } 
            return matrix[h,w];
        }

        // Calculate how a single seat changes
        private bool StepSeat(int h, int w, int[,] newMap) {
            int nearbySeatsTaken = 0;
            bool returnValue = false;
            int seen = 0;

            if(matrix[h,w] == Constants.Floor) return false;
            
            //To check each direction, loop from -1 to 1 for both height and width
            for(int heightInc = -1;heightInc < 2;heightInc++) {
                for(int widthInc = -1;widthInc < 2;widthInc++) {
                    seen = CheckDirection(heightInc, widthInc, h, w);
                    if(seen == Constants.Taken) {
                        nearbySeatsTaken++;
                    }
                }
            }

            if(matrix[h,w] == Constants.Taken && nearbySeatsTaken >= this.seatTolerance) {
                returnValue = true;
                seatstaken--;
                newMap[h,w] = Constants.Empty;
            }

            if(matrix[h,w] == Constants.Empty && nearbySeatsTaken == 0) {
                returnValue = true;
                seatstaken++;
                newMap[h,w] = Constants.Taken;
            }

            return returnValue;
        }
    }

    class Program
    {
        public static void DoDay11() {

            string inputstring = System.IO.File.ReadAllText(@".\Input\DayElevenInput.txt");
            Map Seating = new Map(inputstring, 1, 4);
            int steps = 0;
            do{
                steps++;
                //Seating.PrintSeats();
            } while(Seating.Step() && steps < 10000000);

            System.Console.WriteLine("Answer Part 1: {0}\n", Seating.SeatsTaken);

            Seating = new Map(inputstring, int.MaxValue, 5);
            steps = 0;
            do{
                steps++;
                //Seating.PrintSeats();
            } while(Seating.Step() && steps < 10000000);

            System.Console.WriteLine("Answer Part 2: {0}\n", Seating.SeatsTaken);
            
        }

    }

}
