using CleanCode.VariableNames3.Entities;
using Microsoft.Extensions.Logging;

namespace CleanCode.ClassNames
{
    public class SmsClient
    {
        private readonly ILogger<SmsClient> _logger;
        public SmsClient(ILogger<SmsClient> logger)
        {
            _logger = logger;
        }
        
        // 3.2 (5)
        // IsAverageMarkLowThan - CheckForLowAverageMark
        public bool CheckForLowAverageMark(Student student, int lowerMark)
        {
            _logger.LogTrace($"Checking student {student.Name} for average mark below 4.");
            
            bool isBelowAverage = false;

            var studentDiary = RetrievingData.GetHomeworksJournal(student.Attendance);

            foreach (var kvp in studentDiary)
            {
                int averageMark = 0;
                foreach (Homework homework in kvp.Value)
                {
                    averageMark += homework.Mark;
                }

                if (averageMark / kvp.Value.Count < lowerMark) // lowerMark = 4
                {
                    NotifyStudent(student, kvp.Key);
                    isBelowAverage = true;
                }
            }

            return isBelowAverage;
        }

        // 3.2 (6)
        // SendToStudent - NotifyStudent
        private void NotifyStudent(Student student, Discipline discipline)
        {
            _logger.LogInformation(@$"Student {student.Name} has average mark below 4 in {discipline.Name}.
Sending sms to student ({student.Telephone}).");
        }
    }
}
