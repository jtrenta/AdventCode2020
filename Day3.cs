using System.Collections.Generic;
using System.Linq;

namespace AdventCode2020Day3
{
    class MapRow {

        private string raw;
        private char[] rawArray {
            get {
                return raw.ToCharArray();
            }
        }

        public MapRow(string inputstring) {
            raw = inputstring;
        }

        public bool IsTree(int x) {
            var index = x % raw.Length;
            if(rawArray[index].Equals('#')) {
                return true;
            }
            return false;
        }

    }

    class Day3Data {

        private List<MapRow> Map;
        private int xpos = 0;
        private int ypos = 0;
        private int trees = 0;

        public int TreesHit {
            get {
                return trees;
            }
        }

        public int x {
            get {
                return xpos;
            }
        }

        public int y {
            get {
                return ypos;
            }
            set {
            }
        }

        public bool setPosition(int xVal, int yVal) {
            if(yVal < (Map.Count)) {
                ypos = yVal;
                xpos = xVal;
                if(Map.ElementAt(yVal).IsTree(xVal)) {
                    trees++;
                }
                return true;
            }
            return false;
        }

        public void AddRow(string inputstring) {
            Map.Add(new MapRow(inputstring));
        }

        public bool IsTree(int x, int y) {
            return Map.ElementAt(y).IsTree(x);
        }

        public bool AtBottom {
            get {
                return (y >= (Map.Count-1));
            }
        }

        public void resetData() {
            xpos = 0;
            ypos = 0;
            trees = 0;
        }

        public Day3Data() {
            Map = new List<MapRow>();
        }
    }

    class slope {

        public int x;
        public int y;
        public slope(int xVal, int yVal) {
            x = xVal;
            y = yVal;
        }

    }

    class Program
    {

        public static void DoDay3() {
            Day3Data MapData = new Day3Data();
            Day3Part1(MapData);
        }

        static void Day3Part1(Day3Data MapData) {
            string line;
            long trees = 1;
            List<slope> slopes = new List<slope>();
            slopes.Add(new slope(1, 1));
            slopes.Add(new slope(3, 1));
            slopes.Add(new slope(5, 1));
            slopes.Add(new slope(7, 1));
            slopes.Add(new slope(1, 2));
            System.IO.StreamReader file = new System.IO.StreamReader(@".\Input\DayThreeInput.txt");  
            while((line = file.ReadLine()) != null)  
            {
                MapData.AddRow(line);
            }

            foreach(slope item in slopes) {
                MapData.resetData();
                do {
                    if(!MapData.setPosition(MapData.x + item.x, MapData.y + item.y)) {
                        System.Console.WriteLine("Failed to set position ({0}, {1})",MapData.x, MapData.y);
                        break;
                    }
                }while(!MapData.AtBottom);
                System.Console.WriteLine("Trees Hit: {0}", MapData.TreesHit);
                trees = trees * MapData.TreesHit;
            }
            
            System.Console.WriteLine("Number of trees hit: {0}",trees);

        }
    }

}
