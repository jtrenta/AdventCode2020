using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventCode2020Day4
{
    class AdventPassport {
        
        public int? BirthYear = null;
        public int? IssueYear = null;
        public int? ExpirationYear = null;
        public int? Height = null;
        public string HairColor = null;
        public string EyeColor = null;
        public int? PassportID = null;
        public int? CountryID = null;

        public bool valid {
            get {
                return ( BirthYear.HasValue &&
                    IssueYear.HasValue &&
                    ExpirationYear.HasValue &&
                    Height != null &&
                    HairColor != null &&
                    EyeColor != null &&
                    PassportID.HasValue
                );
            }
        }

        public AdventPassport(string inputstring) {
            inputstring = inputstring.Replace("\r\n", " ");
            inputstring = inputstring.Replace("\n", " ");
            inputstring = inputstring.Replace("\r", " ");
            int tempint = 0;
            bool parsed = false;
            string key;
            string value;
            foreach(string item in inputstring.Split(" ")) {
                if(item.Length > 0) {
                    key = item.Split(":")[0];
                    value = item.Split(":")[1];
                    switch(key) {
                    case "byr":// (Birth Year)
                        parsed = int.TryParse(value, out tempint);
                        if (parsed) {
                            if(tempint >= 1920 && tempint <= 2002) {
                                BirthYear = tempint;
                            }
                        }
                        break;
                    case "iyr":// (Issue Year)
                        parsed = int.TryParse(value, out tempint);
                        if (parsed) {
                            if(tempint >= 2010 && tempint <= 2020) {
                                IssueYear = tempint;
                            }
                        }
                        break;
                    case "eyr":// (Expiration Year)
                        parsed = int.TryParse(value, out tempint);
                        if (parsed) {
                            if(tempint >= 2020 && tempint <= 2030) {
                                ExpirationYear = tempint;
                            }
                        }
                        break;
                    case "hgt":// (Height)
                        if(value.Length > 3) {
                            parsed = int.TryParse(value.Substring(0, value.Length - 2), out tempint);
                            if(parsed) {
                                if(value.Substring(value.Length - 2) == "in") {
                                    if(tempint >= 59 && tempint <= 76) {
                                        Height = tempint;
                                    }
                                } else if(value.Substring(value.Length - 2) == "cm") {
                                    if(tempint >= 150 && tempint <= 193) {
                                        Height = tempint;
                                    }
                                }
                            }
                        }
                        break;
                    case "hcl":// (Hair Color)
                        bool invalid = false;
                        if(value[0] == '#' && value.Length == 7) {
                            foreach(char c in value.Substring(1).ToCharArray()) {
                                if(!(((int)c >= (int)'0' && (int)c <= '9') || ((int)c >= 'a' && (int)c <= 'f')) ) {
                                    invalid = true;
                                    break;
                                }
                            }
                        } else {
                            invalid = true;
                        }
                        if(!invalid) HairColor = value;
                        break;
                    case "ecl":// (Eye Color)
                        if( value.Length == 3 && (
                            value == "amb" ||
                            value == "blu" ||
                            value == "brn" ||
                            value == "gry" ||
                            value == "grn" ||
                            value == "hzl" ||
                            value == "oth")) {
                            EyeColor = value;
                        }
                        break;
                    case "pid":// (Passport ID)
                        parsed = int.TryParse(value, out tempint);
                        if (parsed && value.Length == 9) {
                            PassportID = tempint;
                        }
                        break;
                    case "cid":// (Country ID)
                        parsed = int.TryParse(value, out tempint);
                        //if (parsed) {
                            CountryID = tempint;
                        //}
                        break;
                    }
                    tempint = 0;
                }
            }
            //parse
        }
    }

    class Day4Data {

        private List<AdventPassport> Passports;
        private string raw;
        private string[] passportstrings;
        public int validpassports = 0;

        public Day4Data(string inputstring) {
            Passports = new List<AdventPassport>();
            AdventPassport newpassport;
            raw = inputstring;
            passportstrings = raw.Split(new string[] { Environment.NewLine + Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            foreach(string item in passportstrings) {
                newpassport = new AdventPassport(item);
                Passports.Add(newpassport);
                if(newpassport.valid) { 
                    validpassports++; 
                    //System.Console.WriteLine("Valid passport: {0}", item);                    
                }
            }

        }
    }

    class Program
    {

        public static void DoDay4() {
            string passportstring = System.IO.File.ReadAllText(@"DayFourInput.txt");
            Day4Data Passports = new Day4Data(passportstring);
            System.Console.WriteLine("Valid passports: {0}", Passports.validpassports);
        }

    }

}
