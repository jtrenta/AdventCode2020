 
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventCode2020Day8
{
    static class Constants {
        public const int NOP = 0;
        public const int ACC = 1;
        public const int JMP = 2;
    }

    public class Instruction {

        private int command = 0;
        private int value;
        private bool executed = false;

        public Instruction(string command, int value) {
            if(command == "acc") this.command = Constants.ACC;
            if(command == "jmp") this.command = Constants.JMP;
            this.value = value;
        }

        public void Execute(CodeSegment cs) {
            if(this.command == Constants.NOP) {
                cs.Index++;
            } else if(this.command == Constants.ACC) {
                cs.Index++;
                cs.Add(this.value);
            } else if(this.command == Constants.JMP) {
                cs.Index += value;
            }
            this.executed = true;
        }

        public void Reset() {
            executed = false;
        }

        public bool HasExecuted {
            get { 
                return executed;
            }
        }

        public int commandType {
            get {
                return command;
            }
            set {
                if(value == Constants.NOP || value == Constants.ACC || value == Constants.JMP)
                    command = value;
            }
        }

        public void Flip() {
            if(command == Constants.JMP) {
                command = Constants.NOP;
            } else if(command == Constants.NOP) {
                command = Constants.JMP;
            }
        }

    }

    public class CodeSegment {

        private string raw;
        private List<Instruction> Commands;
        private int accumulator = 0;
        private int index = 0;
        private bool terminated = false;

        public CodeSegment(string inputstring) {
            this.raw = inputstring;
            int temp;
            Commands = new List<Instruction>();
            foreach(string item in inputstring.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)) {
                item.Replace("+", "");
                int.TryParse(item.Split(' ')[1], out temp);
                Commands.Add(new Instruction(item.Split(' ')[0], temp));
            }
        }

        public void Reset() {
            accumulator = 0;
            index = 0;
            terminated = false;
            foreach(Instruction item in Commands) item.Reset();
        }

        public int Execute() {
            List<int> CommandsToChange = new List<int>();

            foreach(Instruction item in Commands) {
                if(item.commandType == Constants.NOP || item.commandType == Constants.JMP) {
                    CommandsToChange.Add(Commands.IndexOf(item));
                }
            }

            for(int i = 0;i < CommandsToChange.Count;i++) {

                Commands.ElementAt(CommandsToChange.ElementAt(i)).Flip();

                do {
                    Commands.ElementAt(index).Execute(this);
                } while(!terminated && !Commands.ElementAt(index).HasExecuted);

                if(terminated)
                    return accumulator;

                Commands.ElementAt(CommandsToChange.ElementAt(i)).Flip();
                this.Reset();

            }

            return accumulator;
        }

        public void Add(int value) {
            accumulator += value;
        }

        public int Index {
            get {
                return index;
            }

            set {
                if(value >= 0 && value < Commands.Count)
                    index = value;
                else
                    terminated = true;
            }
        }

    }

    class Program
    {
        public static void DoDay8() {

            string inputstring;
            int answer = 0;
            inputstring = System.IO.File.ReadAllText(@".\Input\DayEightInput.txt");
            CodeSegment Game = new CodeSegment(inputstring);
            answer = Game.Execute();
            System.Console.WriteLine("Accumulator ended at: {0}", answer);
            
        }


    }

}
