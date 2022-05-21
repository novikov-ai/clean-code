using System;

namespace CleanCode.VariableBindingTimes.Engineering
{
    public class LinesConnectorWindow
    {
        private string UserInput { get; set; }

        private int _connectionDirectionX;
        public int ConnectionDirectionX
        {
            get => _connectionDirectionX;
            set => _connectionDirectionX = value is 0 or 1 ? value : 0; 
        }
        
        // ...
        // business logic removed
        // ...

        public void ShowUserDialog()
        {
            UserInput = Console.ReadLine();
            
            if (ConnectionDirectionX == 1)
            {
                // ...
                // business logic removed
                // ...
            }
            
            // ...
            // business logic removed
            // ...
        }
    }
}