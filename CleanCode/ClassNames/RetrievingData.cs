using System;
using System.Collections.Generic;
using System.Linq;
using CleanCode.VariableNames3.DataAccess;
using CleanCode.VariableNames3.Entities;

namespace CleanCode.ClassNames
{
    public static class RetrievingData
    {
        // 3.2 (7)
        // GetStudentJournalFromVisits - GetHomeworksJournal
        public static Dictionary<Discipline, List<Homework>> GetHomeworksJournal(HashSet<Lecture> visits)
        {
            var journal = new Dictionary<Discipline, List<Homework>>();

            foreach (Lecture visit in visits)
            {
                if (!journal.ContainsKey(visit.Discipline))
                    journal.Add(visit.Discipline, new List<Homework>());

                journal.TryGetValue(visit.Discipline, out List<Homework> homeworks);
                homeworks?.Add(visit.Homework);
            }

            return journal;
        }
        public static Dictionary<Student, List<Lecture>> GetStudentsOfDiscipline(ICrud<Student> repository, Discipline discipline)
        {
            var studentsOfDiscipline = new Dictionary<Student, List<Lecture>>();

            var students = repository.Read();
            foreach (var student in students)
            {
                var visits = student.Attendance.Where(vst => vst.Homework.Discipline.Name == discipline.Name).ToList();
                studentsOfDiscipline.Add(student, visits);
            }

            return studentsOfDiscipline;
        }
    }
}
