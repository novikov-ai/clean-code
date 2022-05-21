using System;
using System.IO;
using Autodesk.Revit.UI;

namespace CleanCode.Comments.ExternalCommands
{
    public class ButtonDataUtils
    {
        private const string PathPrefix = "pack://application:,,,/";
        private const string PathPostfix = ";component/Resources/";
        private const string ImageFormat = ".png";
        
        // 3.1 (3)
        /// <summary>
        /// Easy PushButton creation
        /// </summary>
        /// <param name="assembly">Current assembly</param>
        /// <param name="buttonData">ExternalCommand</param>
        /// <returns></returns>
        public static PushButtonData CreatePbData(string assembly, BasicCommand buttonData)
        {
            try
            {
                var type = buttonData.GetType();
                var assemblyName = type.Assembly.GetName().Name;

                // ...
                // business logic removed
                // ...

                return new PushButtonData(buttonData.Name, buttonData.Name, assembly, type.FullName)
                {
                    // ...
                    // business logic removed
                    // ...
                    
                    ToolTip = buttonData.Description,
                    LongDescription = buttonData.Version
                };
            }
            catch (Exception e)
            {
                TaskDialog.Show("Ошибка",
                    $@"Во время создания кнопки {buttonData.Name} {buttonData.Version} произошла ошибка.

{e.Message} | {e.StackTrace}");
                
                throw;
            }
        }

        static DateTime GetBuildDateTime(string path)
        {
            if (!File.Exists(path))
                return new DateTime();
            
            var fileInfo = new FileInfo(path);
            return fileInfo.LastWriteTime;
        }
    }
}