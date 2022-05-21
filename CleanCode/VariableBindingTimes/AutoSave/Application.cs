using Autodesk.Revit.UI;
using Newtonsoft.Json;
using System;
using System.IO;

namespace CleanCode.VariableBindingTimes.AutoSave
{
    public class Application : IExternalApplication
    {
        // (2)
        // compiling binding
        // why: config file has only one relative path, which perfectly stored in a constant,
        // but it also easy to maintain if you need to move file somewhere else
        private const string ConfigPath = "../../config.txt";

        public Result OnStartup(UIControlledApplication application)
        {
            // ...
            // business logic removed
            // ...

            // (3)
            // binding while the program is running
            // why: every application user has his own specific settings, which could change behavior of the executing program during the process
            UserConfig config = GetConfigFile();

            FileWatcher fileWatcher = new FileWatcher(config.AutoSaveInterval, config.AutoSyncInterval);
            fileWatcher.Run();

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            // ...
            // business logic removed
            // ...

            return Result.Succeeded;
        }
        
        static UserConfig GetConfigFile()
        {
            UserConfig userConfig = null;

            // (2)
            // compiling binding
            if (File.Exists(ConfigPath))
            {
                try
                {
                    userConfig = JsonConvert.DeserializeObject<UserConfig>(File.ReadAllText(ConfigPath));

                    if (userConfig is null)
                        throw new Exception();
                    
                    UserConfig.UpdateUserTabs(userConfig);
                }
                catch
                {
                    TaskDialog.Show("Info", "place_holder");
                    userConfig = null;
                }
            }
            else
                TaskDialog.Show("Warning", "place_holder");

            return userConfig ?? InitialConfig(userConfig);
        }

        static UserConfig InitialConfig(UserConfig userConfig = null)
        {
            userConfig ??= UserConfig.GetDefaultConfigFile();

            string jsonString = JsonConvert.SerializeObject(userConfig);
            try
            {
                File.WriteAllText(ConfigPath, jsonString);
            }
            catch (Exception e)
            {
                TaskDialog.Show("Error", $@"Error happened: {e.Message}");
            }

            return userConfig;
        }

        // ...
        // business logic removed
        // ...
    }
}