using Autodesk.Revit.UI;
using Newtonsoft.Json;
using System;
using System.IO;

namespace CleanCode.VariableBindingTimes.AutoSave
{
    public class Application : IExternalApplication
    {
        private const string ConfigPath = "../../config.txt";

        public Result OnStartup(UIControlledApplication application)
        {
            // ...
            // business logic removed
            // ...

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