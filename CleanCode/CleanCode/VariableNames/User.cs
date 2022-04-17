using System;

namespace CleanCode.VariableNames
{
    public class User
    {
        public int Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Picture { get; set; }
        public string Title { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public DateTime DateBirth { get; set; }
        public DateTime DateRegister { get; set; }
    }
}