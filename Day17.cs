using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventCode2020Day17
{
    class Cube : IEquatable<Cube>{
        public (int x, int y, int z, int w) coords;

        public Cube((int x, int y, int z, int w) coords) {
            this.coords = coords;
        }

        public bool Near((int x, int y, int z, int w) other) {
            return (other.x >= coords.x - 1 && other.x <= coords.x+1 &&
                    other.y >= coords.y - 1 && other.y <= coords.y+1 &&
                    other.z >= coords.z - 1 && other.z <= coords.z+1 &&
                    other.w >= coords.w - 1 && other.w <= coords.w+1 &&
                    other != this.coords);
        }

        public bool Equals(Cube other) {
            return other.coords == coords;
        }

    }

    class Grid {
        private int cycles;
        private Dictionary<(int x, int y, int z, int w), Cube> cubes;
        private int size;

        public Grid(string[] inputstrings) {
            cubes = new Dictionary<(int x, int y, int z, int w), Cube>();
            size = inputstrings.Length - 1;
            for(int y = 0;y <= size;y++) {
                for(int x = 0;x <= size;x++) {
                    if(inputstrings[y][x] == '#') {
                        cubes.Add((x, y, 0, 0), new Cube((x, y, 0, 0)));
                    }
                }
            }
            cycles = 0;
        }

        public void RunCycle() {
            cycles++;
            int count = 0;
            (int x, int y, int z, int w) coords;
            Dictionary<(int x, int y, int z, int w), Cube> newCubes = new Dictionary<(int x, int y, int z, int w), Cube>();
            for(int x = 0 - cycles;x <= cycles + size;x++) {
                for(int y = 0 - cycles;y <= cycles + size;y++) {
                    for(int z = 0 - cycles;z <= cycles;z++) {
                        for(int w = 0 - cycles;w <= cycles;w++) {
                            coords = (x,y,w,z);
                            count = cubes.Values.Count(c => c.Near(coords));
                            if(cubes.GetValueOrDefault(coords, null) != null) {
                                if(count == 2 || count == 3) {
                                    newCubes.Add(coords, new Cube(coords));
                                }
                            } else {
                                if(count == 3) {
                                    newCubes.Add(coords, new Cube(coords));
                                }
                            }
                        }
                    }
                }
            }
            cubes = newCubes;
        }

        public int Cycle { get { return cycles; } }

        public int ActiveCubes {
            get {
                return cubes.Count();
            }
        }

        public void PrintSpace() { 
            for(int w = 0 - cycles;w <= cycles;w++) {
                for(int z = 0 - cycles;z <= cycles;z++) {
                    System.Console.WriteLine("z={0}, w={1}",z, w);
                    for(int y = 1 - cycles;y <= cycles + size;y++) {
                        System.Console.Write("{0,2}:",y);
                        for(int x = 1 - cycles;x <= cycles + size;x++) {
                            System.Console.Write("{0}", cubes.GetValueOrDefault((x,y,z,w),null) == null ? '.' : '#');
                        }
                        System.Console.WriteLine();
                    }
                }
            }
        }
    }
 
    class Program
    {
        private static string inputstring;
        private static Grid grid;

        public static void DoDay17() {
            inputstring = System.IO.File.ReadAllText(@".\Input\DaySeventeenInput.txt");
            string[] inputstrings = inputstring.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            grid = new Grid(inputstrings);
            //DoPart1();
            DoPart2();
        }

        public static void DoPart1() {
            //Part 1 
            //System.Console.WriteLine("Answer Part 2: {0}", tickets[0].DepartureValue(rules));

        }

        public static void DoPart2() {

            for(int i = 0;i < 6; i++) {
                grid.RunCycle();
                //grid.PrintSpace();
            }
            System.Console.WriteLine("Answer Part 2: {0}", grid.ActiveCubes);

        }


    }

}
