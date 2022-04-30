using System;
using System.IO;
using System.Collections.Generic;

namespace CleanCode.ConstantNames
{
    static class AocDay4
    {
        private const int PassportValidOccurrences = 7;
        private const int DateLength = 4;

        private const string BirthYearCode = "byr";
        private const int BirthYearMin = 1920;
        private const int BirthYearMax = 2002;

        private const string IssueYearCode = "iyr";
        private const int IssueYearMin = 2010;
        private const int IssueYearMax = 2020;

        private const string ExpirationYearCode = "eyr";
        private const int ExpirationYearMin = 2020;
        private const int ExpirationYearMax = 2030;

        private const string HeightCode = "hgt";

        private const string HeightUnitMetric = "cm";
        private const int HeightMetricMin = 150;
        private const int HeightMetricMax = 193;

        private const string HeightUnit = "in";
        private const int HeightMin = 59;
        private const int HeightMax = 76;

        private const string HairColorCode = "hcl";
        private const string EyeColorCode = "ecl";
        private const string PassportIdCode = "pid";
        private const int PassportIdNumbersCount = 9;

        private const string CountryIdCode = "cid";

        private static readonly List<string> PassportCodes = new List<string> 
            {
                BirthYearCode, IssueYearCode, ExpirationYearCode,
                HeightCode, HairColorCode, EyeColorCode,
                PassportIdCode
            };

        private static readonly List<char> ValidChars =
            new List<char> {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f'};

        private static readonly List<string> Colors =
            new List<string> {"amb", "blu", "brn", "gry", "grn", "hzl", "oth"};

        public static void ShowResult()
        {
            string input = File.ReadAllText("input.txt");

            string[] passports = input.Split('\n');

            int valid = 0;
            int correct = 0;

            // (1)
            // prev: List<string> data = new List<string> { "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid" };

            foreach (string item in passports)
            {
                if (item == "")
                {
                    // (2)
                    // prev: if (correct == 7)
                    if (correct == PassportValidOccurrences)
                        valid++;

                    correct = 0;
                    continue;
                }

                string[] passport = item.Split(' ');
                foreach (string key in passport)
                {
                    string[] keys = key.Split(':');

                    if (PassportCodes.Contains(keys[0]))
                    {
                        switch (keys[0])
                        {
                            // (3)
                            // prev: case "byr":
                            case BirthYearCode:
                            {
                                // (4)
                                // prev: if (keys[1].Length == 4)
                                if (keys[1].Length == DateLength)
                                {
                                    Int32.TryParse(keys[1], out var res);
                                    // (5)
                                    // prev: if (res >= 1920 && res <= 2002)
                                    if (res >= BirthYearMin && res <= BirthYearMax)
                                        correct++;
                                }

                                break;
                            }

                            // (6)
                            // prev: case "iyr":
                            case IssueYearCode:
                            {
                                if (keys[1].Length == DateLength)
                                {
                                    Int32.TryParse(keys[1], out var res);
                                    // (7)
                                    // prev: if (res >= 2010 && res <= 2020)
                                    if (res >= IssueYearMin && res <= IssueYearMax)
                                        correct++;
                                }

                                break;
                            }

                            case ExpirationYearCode:
                            {
                                if (keys[1].Length == DateLength)
                                {
                                    Int32.TryParse(keys[1], out var res);
                                    // (8)
                                    // if (res >= 2020 && res <= 2030)
                                    if (res >= ExpirationYearMin && res <= ExpirationYearMax)
                                        correct++;
                                }

                                break;
                            }

                            // (9)
                            // prev: case "hgt":
                            case HeightCode:
                            {
                                // (10)
                                // prev: if (keys[1].EndsWith("cm"))
                                if (keys[1].EndsWith(HeightUnitMetric))
                                {
                                    string heightNum = keys[1].Replace(HeightUnitMetric, "");
                                    Int32.TryParse(heightNum, out var res);
                                    if (res >= HeightMetricMin && res <= HeightMetricMax)
                                        correct++;
                                }
                                else if (keys[1].EndsWith(HeightUnit))
                                {
                                    string heightNum = keys[1].Replace(HeightUnit, "");
                                    Int32.TryParse(heightNum, out var res);
                                    if (res >= HeightMin && res <= HeightMax)
                                        correct++;
                                }

                                break;
                            }

                            // (11)
                            // prev: case "hcl":
                            case HairColorCode:
                            {
                                if (keys[1].Length == PassportValidOccurrences && keys[1].StartsWith('#'))
                                {
                                    bool invalid = false;

                                    for (int i = 1; i < keys[1].Length; i++)
                                    {
                                        if (!ValidChars.Contains(keys[1][i]))
                                        {
                                            invalid = true;
                                            break;
                                        }
                                    }

                                    if (!invalid)
                                        correct++;
                                }

                                break;
                            }

                            case EyeColorCode:
                            {
                                // (12)
                                // prev: List<string> Colors = new List<string> { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" };
                                if (Colors.Contains(keys[1]))
                                    correct++;

                                break;
                            }

                            case PassportIdCode:
                            {
                                if (keys[1].Length == PassportIdNumbersCount)
                                {
                                    bool invalid = false;
                                    foreach (char ch in keys[1])
                                    {
                                        if (!ValidChars.Contains(ch))
                                        {
                                            invalid = true;
                                            break;
                                        }
                                    }

                                    if (!invalid)
                                        correct++;
                                }

                                break;
                            }
                        }
                    }
                }
            }

            if (correct == PassportValidOccurrences)
                valid++;

            // byr(Birth Year) - [1920 - 2002]
            // iyr(Issue Year) - [2010 - 2020]
            // eyr(Expiration Year) - [2020 - 2030]
            // hgt(Height) - [150-193cm / 59-76in]
            // hcl(Hair Color) - [#0-9 6times / #a-f 6times]
            // ecl(Eye Color) - [amb blu brn gry grn hzl oth]
            // pid(Passport ID) - [012345678]
            // cid(Country ID) - optional

            // byr(Birth Year) - four digits; at least 1920 and at most 2002.
            // iyr(Issue Year) - four digits; at least 2010 and at most 2020.
            // eyr(Expiration Year) - four digits; at least 2020 and at most 2030.
            // hgt(Height) - a number followed by either cm or in:
            // If cm, the number must be at least 150 and at most 193.
            // If in, the number must be at least 59 and at most 76.
            // hcl(Hair Color) - a # followed by exactly six characters 0-9 or a-f.
            // ecl(Eye Color) - exactly one of: amb blu brn gry grn hzl oth.
            // pid(Passport ID) - a nine - digit number, including leading zeroes.
            // cid(Country ID) - ignored, missing or not.

            Console.WriteLine("Day 04: " + valid);
        }
    }
}