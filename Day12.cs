 
using System;
using System.Collections.Generic;

namespace AdventCode2020Day12
{

    class Position {
        private int[] coords;

        public Position(int x, int y) {
            coords = new int[2] { x, y };
        }

        public int[] Coords {
            get { return coords; }
            set { coords = value; }
        }

        public int X {
            get { return coords[0]; }
            set { coords[0] = value; }
        }

        public int Y {
            get { return coords[1]; }
            set { coords[1] = value; }
        }
    }

    class Ship {

        private Position Part1ShipPosition;
        private Position ShipPosition;
        private Position WaypointPosition;
        private char heading;
        private int headingIndex;
        private Dictionary<char,int[]> headings = new Dictionary<char,int[]>{
            { 'E', new int[] {  1,  0,  0 } },
            { 'S', new int[] {  0, -1,  1 } },
            { 'W', new int[] { -1,  0,  2 } },
            { 'N', new int[] {  0,  1,  3 } }
        };

        public Ship(int x, int y, int w, int z, char heading) {
            Part1ShipPosition = new Position(x, y);
            ShipPosition = new Position(x, y);
            WaypointPosition = new Position(w, z);
            this.heading = heading;
            this.headingIndex = headings[this.heading][2];
        }

        //Always positive modulus
        private int Mod(int a, int m) {
            int r = a % m;
            return r < 0 ? (m + r) : r;
        }

        private char Rotate(int amount) {
            int iterations = Mod(Mod(-1 * amount, 360) / 90, 4);
            headingIndex = Mod(headingIndex + iterations, 4);
            WaypointPosition.Coords = new int[] {
                    (int)Math.Round((Math.Cos(amount * Math.PI/180) * WaypointPosition.X) + (-1 * Math.Sin(amount * Math.PI/180) * WaypointPosition.Y)), 
                    (int)Math.Round((Math.Sin(amount * Math.PI/180) * WaypointPosition.X) + (Math.Cos(amount * Math.PI/180) * WaypointPosition.Y)) 
                };
            return "ESWN".ToCharArray()[headingIndex];
        }

        private void Rotate2(int amount) {
        }

        public bool Move(char direction, int distance) {
            if(direction == 'L') {
                heading = Rotate(distance);
            } else if(direction == 'R') {
                heading = Rotate(distance * -1);
            } else if(direction == 'F') {
                direction = this.heading;
                Part1ShipPosition.X += distance * headings[direction][0];
                Part1ShipPosition.Y += distance * headings[direction][1];
                ShipPosition.X += WaypointPosition.X * distance;
                ShipPosition.Y += WaypointPosition.Y * distance;
            } else {
                Part1ShipPosition.X += distance * headings[direction][0];
                Part1ShipPosition.Y += distance * headings[direction][1];
                WaypointPosition.X += distance * headings[direction][0];
                WaypointPosition.Y += distance * headings[direction][1];
            }
            return true;
        }

        public int ManhattanDistance {
            get {
                return Math.Abs(ShipPosition.X) + Math.Abs(ShipPosition.Y);
            }
        }

        public int Part1ManhattanDistance {
            get {
                return Math.Abs(Part1ShipPosition.X) + Math.Abs(Part1ShipPosition.Y);
            }
        }

    }

    class Program
    {
        public static void DoDay12() {

            string inputstring = System.IO.File.ReadAllText(@".\Input\DayTwelveInput.txt");
            Ship TheLoveBoat = new Ship(0, 0, 10, 1, 'E');
            foreach(string item in inputstring.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)) {
                TheLoveBoat.Move(item.ToCharArray()[0], int.Parse(item.Substring(1)));
            }

            System.Console.WriteLine("Answer Part 1: {0}\n", TheLoveBoat.Part1ManhattanDistance);
            System.Console.WriteLine("Answer Part 2: {0}\n", TheLoveBoat.ManhattanDistance);

        }

    }

}
