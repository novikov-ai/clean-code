using System.Linq;
using CleanCode.VariableNames3.Entities;
using Microsoft.Extensions.Logging;

namespace CleanCode.VariableNames3
{
    public class EmailManager
    {
        private readonly ILogger<EmailManager> _logger;
        public EmailManager(ILogger<EmailManager> logger)
        {
            _logger = logger;
        }

        public bool IsSkippedClassesMoreThan(Student student, int numberOfSkippedClasses)
        {
            _logger.LogTrace($"Checking student {student.Name} for skipping the classes.");

            // 7.1 (2) skipped - isSkippedClasses
            bool isSkippedClasses = false;

            var studentSkippedClasses = student.Attendance.Where(vst => !vst.Attended).ToHashSet();
            var skippedDisciplineList = DataAccess.RetrievingData.GetStudentJournalFromVisits(studentSkippedClasses);

            foreach (var kvp in skippedDisciplineList)
            {
                if (kvp.Value.Count > numberOfSkippedClasses) // numberOfSkippedClasses = 3
                {
                    SendToLecturerAndStudent(kvp.Key.Lecturer, student);
                    isSkippedClasses = true;
                }
            }

            return isSkippedClasses;
        }

        private void SendToLecturerAndStudent(Lecturer lecturer, Student student)
        {
            _logger.LogInformation(@$"Student {student.Name} has skipped more than 3 lessons.
Sending email to lecturer ({lecturer.Email}) and student({student.Email}).");
        }
    }
}
