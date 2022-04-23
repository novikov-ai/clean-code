using System;
using System.IO;

namespace CleanCode.VariableNames4
{
    static class AocDay2
    {
        public static void ShowResult()
        {
            string rawInput = File.ReadAllText("input.txt");
            string[] allPoliciesAndPasswords = rawInput.Split('\n');

            // (10)
            // pass - currentPassword
            string currentPassword = " "; // current password

            // (11)
            // passwords - validPasswordsCount
            int validPasswordsCount = 0;

            foreach (string policyAndPassword in allPoliciesAndPasswords)
            {
                // (12)
                // array - splitPolicyAndPassword
                string[] splitPolicyAndPassword = policyAndPassword.Split(' ');
                
                string[] lengthPolicy = splitPolicyAndPassword[0].Split('-');

                var positionOne = Convert.ToInt32(lengthPolicy[0]) - 1;
                var positionTwo = Convert.ToInt32(lengthPolicy[1]) - 1;

                var givenLetter = splitPolicyAndPassword[1][0];

                currentPassword = splitPolicyAndPassword[2];

                if (currentPassword[positionOne] == givenLetter)
                {
                    if (currentPassword[positionTwo] != givenLetter)
                        validPasswordsCount++;
                }
                else
                {
                    if (currentPassword[positionTwo] == givenLetter)
                        validPasswordsCount++;
                }
            }
            Console.WriteLine("Day 02: " + validPasswordsCount);
        }
    }
}