using CleanCode.VariableNames3.Entities;
using Microsoft.Extensions.Logging;

namespace CleanCode.VariableNames3
{
    public class SmsManager
    {
        private readonly ILogger<SmsManager> _logger;
        public SmsManager(ILogger<SmsManager> logger)
        {
            _logger = logger;
        }
        public bool IsAverageMarkLowThan(Student student, int lowerMark)
        {
            _logger.LogTrace($"Checking student {student.Name} for average mark below 4.");

            // 7.1 (3) lessAverage - isBelowAverage
            bool isBelowAverage = false;

            var studentDiary = DataAccess.RetrievingData.GetStudentJournalFromVisits(student.Attendance);

            foreach (var kvp in studentDiary)
            {
                int averageMark = 0;
                foreach (Homework homework in kvp.Value)
                {
                    averageMark += homework.Mark;
                }

                if (averageMark / kvp.Value.Count < lowerMark) // lowerMark = 4
                {
                    SendToStudent(student, kvp.Key);
                    isBelowAverage = true;
                }
            }

            return isBelowAverage;
        }

        private void SendToStudent(Student student, Discipline discipline)
        {
            _logger.LogInformation(@$"Student {student.Name} has average mark below 4 in {discipline.Name}.
Sending sms to student ({student.Telephone}).");
        }
    }
}
