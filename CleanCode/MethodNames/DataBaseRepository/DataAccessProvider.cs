using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace CleanCode.MethodNames.DataBaseRepository
{
    public class DataAccessProvider
    {
        private string _jsonDataFile;

        public DataAccessProvider(string jsonFile)
        {
            _jsonDataFile = jsonFile;
        }
        
        // (11)
        // ChangeDataBase - ChangeDataFile
        public void ChangeDataFile(string jsonFile)
        {
            _jsonDataFile = jsonFile;
        }
       
        // (12)
        // GetStudentsListFromJson - GetStudents
        public List<Student> GetStudents()
        {
            if (string.IsNullOrEmpty(_jsonDataFile))
                return null;

            return JsonSerializer.Deserialize<List<Student>>(File.ReadAllText(_jsonDataFile));
        }
    }
}