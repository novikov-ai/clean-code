using System;
using System.ComponentModel;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace CleanCode.Comments.ExternalCommands
{
    public abstract class BasicCommand : IExternalCommand, ICommandInfo
    {
        // 3.1 (1)
        /// <summary>
        /// Displayed name of RibbonItem for user.
        /// </summary>
        public virtual string Name => "Безымянный";

        /// <summary>
        /// Displayed tooltip of RibbonItem for user.
        /// </summary>
        public virtual string Description => "Описание отсутствует";

        /// <summary>
        /// Image of the RibbonItem at custom RibbonPanel.
        /// </summary>
        public virtual string Picture => "unknown_32x32";

        /// <summary>
        /// Version of the RibbonItem. Displayed like long description.
        /// </summary>
        public virtual string Version => "1.0";

        // ...
        // business logic removed
        // ...

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                // ...
                // business logic removed
                // ...

                RunFunc(commandData);

                return Result.Succeeded;
            }
            catch (Exception e)
            {
                // ...
                // business logic removed
                // ...

                return Result.Failed;
            }
        }
        
        protected virtual void RunFunc(ExternalCommandData commandData)
        {
        }

        // ...
        // business logic removed
        // ...
    }
}