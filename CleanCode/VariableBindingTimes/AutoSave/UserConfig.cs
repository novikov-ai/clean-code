namespace CleanCode.VariableBindingTimes.AutoSave
{
    public class UserConfig
    {
        public int AutoSaveInterval { get; set; }
        public int AutoSyncInterval { get; set; }

        // ...
        // business logic removed
        // ...

        public static UserConfig GetDefaultConfigFile()
        {
            return new UserConfig
            {
                AutoSaveInterval = 30,
                AutoSyncInterval = 45
            };
        }

        public static void UpdateUserTabs(UserConfig config)
        {
            // ...
            // business logic removed
            // ...
        }
    }
}