namespace CleanCode.VariableBindingTimes.AutoSave
{
    public class FileWatcher
    {
        private readonly int _saveIntervalMinutes;
        private readonly int _syncIntervalMinutes;
        
        public FileWatcher(int saveIntervalMinutes, int syncIntervalMinutes)
        {
            _saveIntervalMinutes = saveIntervalMinutes;
            _syncIntervalMinutes = syncIntervalMinutes;
        }

        public void Run()
        {
            // ...
            // business logic removed
            // ...
        }
    }
}