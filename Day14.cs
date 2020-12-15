using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventCode2020Day14
{

    class Line {
        private string raw;
        private string command;
        private long address;
        private long value;
        private long and;
        private long or;
        private string mask;

        public Line(string command, string bitmask, long address, long value) {
            this.raw = command + " " + bitmask + " " + address.ToString() + " " + value.ToString();
            this.command = command;
            this.mask = bitmask;
            if(bitmask != null) {
                while(bitmask.Length < 64) bitmask = "X" + bitmask;
                and = Convert.ToInt64(bitmask.Replace('X', '1'), 2);
                or = Convert.ToInt64(bitmask.Replace('X', '0'), 2);
            } else {
                this.address = address;
                this.value = value;
            }
        }

        public long Address {
            get {
                return address;
            }
        }

        public long Value {
            get {
                return value;
            }
        }

        public string Raw {
            get {
                return raw;
            }
        }

        public string Mask {
            get {
                return mask;
            }
        }

        public string Command {
            get {
                return command;
            }
        }

        public long AND {
            get {
                return and;
            }
        }

        public long OR {
            get {
                return or;
            }
        }
    }

    class DockingProgram {
        private long AND;
        private long OR;
        private long mem;
        string mask;
        private long setValue = 0;
        private List<Line> commands;
        private Dictionary<long, long> memories;

        public DockingProgram(string[] inputstrings) {
            commands = new List<Line>();
            memories = new Dictionary<long, long>();
            foreach(string item in inputstrings) {
                if(item.Substring(0, 3) == "mem") {
                    commands.Add(new Line("mem", null, long.Parse(item.Substring(4).Split(']')[0]), long.Parse(item.Split(" = ")[1])) );
                } else if(item.Split(" = ")[0] == "mask") {
                    commands.Add(new Line("mask", item.Split(" = ")[1], 0, 0));
                }
            }
        }

        public void ExecutePart1() {
            foreach(Line command in commands) {
                if(command.Command == "mem") {
                    memories[command.Address] = (command.Value | OR) & AND;
                } else if(command.Command == "mask") {
                    AND = command.AND;
                    OR = command.OR;
                }
            }

        }

        public void ExecutePart2() {
            string initialAddress;
            string bm;
            foreach(Line command in commands) {
                //System.Console.WriteLine("Line {0}:{1}", num++, command.Raw);
                if(command.Command == "mem") {
                    bm = mask.Replace('X', '0');
                    initialAddress = Convert.ToString(command.Address | Convert.ToInt64(bm,2),2).PadLeft(36,'0');
                    bm = mask.Replace('1', '0');
                    setValue = command.Value;
                    AddAddresses(initialAddress.ToCharArray(), bm.ToCharArray(), 0);

                } else if(command.Command == "mask") {
                    mask = command.Mask;
                }
            }
            return;
        }

        private void AddAddresses(char[] a, char[] bm, int start) {
            char c;
            char d;
            bool found = false;
            for(int i = start;i < bm.Length;i++) {
                if(bm[i] == 'X') {
                    found = true;
                    c = bm[i];
                    d = a[i];
                    bm[i] = '0';
                    a[i]='0';
                    AddAddresses(a, bm, i);
                    a[i] = '1';
                    AddAddresses(a, bm, i);
                    a[i] = d;
                    bm[i] = c;
                }
            }
            if(!found) {
                mem = Convert.ToInt64(new string(a),2);
                memories[mem] = setValue;
            }
        }

        public long value {
            get {
                return memories.Sum(m => m.Value);
            }
        }

    }

    class Program
    {
        private static string inputstring;

        public static void DoDay14() {

            inputstring = System.IO.File.ReadAllText(@".\Input\DayFourteenInput.txt");
            string[] inputstrings = inputstring.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            DockingProgram program = new DockingProgram(inputstrings);
            DoPart1(program);
            program = new DockingProgram(inputstrings);
            DoPart2(program);
        }

        public static void DoPart1(DockingProgram program) {
            program.ExecutePart1();
            System.Console.WriteLine("Answer Part 1: {0}", program.value);
        }

        public static void DoPart2(DockingProgram program) {
            program.ExecutePart2();
            System.Console.WriteLine("Answer Part 2: {0}", program.value);
        }

    }

}
