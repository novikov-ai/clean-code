using System.Linq;
using CleanCode.VariableNames3.Entities;
using Microsoft.Extensions.Logging;
using CleanCode.ClassNames;

namespace CleanCode.ClassNames
{
    public class EmailClient
    {
        private readonly ILogger<EmailClient> _logger;
        public EmailClient(ILogger<EmailClient> logger)
        {
            _logger = logger;
        }

        
        // 3.2 (3)
        // IsSkippedClassesMoreThan - CheckForSkippedClasses
        public bool CheckForSkippedClasses(Student student, int numberOfSkippedClasses)
        {
            _logger.LogTrace($"Checking student {student.Name} for skipping the classes.");
            
            bool isSkippedClasses = false;

            var studentSkippedClasses = student.Attendance.Where(vst => !vst.Attended).ToHashSet();
            var skippedDisciplineList = RetrievingData.GetHomeworksJournal(studentSkippedClasses);

            foreach (var kvp in skippedDisciplineList)
            {
                if (kvp.Value.Count > numberOfSkippedClasses) // numberOfSkippedClasses = 3
                {
                    NotifyLecturerAndStudent(kvp.Key.Lecturer, student);
                    isSkippedClasses = true;
                }
            }

            return isSkippedClasses;
        }

        // 3.2 (4)
        // SendToLecturerAndStudent - NotifyLecturerAndStudent        
        private void NotifyLecturerAndStudent(Lecturer lecturer, Student student)
        {
            _logger.LogInformation(@$"Student {student.Name} has skipped more than 3 lessons.
Sending email to lecturer ({lecturer.Email}) and student({student.Email}).");
        }
    }
}
