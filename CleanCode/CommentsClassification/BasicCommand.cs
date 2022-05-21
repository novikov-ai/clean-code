using System;
using System.Windows;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace CleanCode.CommentsClassification
{
    public abstract class BasicCommand
    {
        // ...
        // business logic removed
        // ...
        
        private bool _collectStats = true;

        private static Window _window = null;
        private bool _isWindowModal = true;

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                // ...
                // business logic removed
                // ...
            }
            catch (Exception e)
            {
                // ...
                // business logic removed
                // ...
            }
            finally
            {
                if (_isWindowModal)
                {
                    // ...
                    // business logic removed
                    // ...

                    try
                    {
                        if (_collectStats)
                        {
                            // ...
                            // business logic removed
                            // ...
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            return Result.Succeeded;
        }

        // 1
        // informative comment
        /// <summary>
        /// Use this method to set up modeless window.
        /// </summary>
        /// <param name="window"></param>
        protected void SetUpModeless(Window window)
        {
            _isWindowModal = false;

            if (_window is null)
            {
                _window = window;
                
                // ...
                // business logic removed
                // ...
            }
            else
            {
                // ...
                // business logic removed
                // ...

                _window.WindowState = WindowState.Normal;
            }
        }

        // 2
        // informative comment
        /// <summary>
        /// Use this method to turn off statistics module.
        /// </summary>
        protected void TurnOffStatistics()
        {
            _collectStats = false;
        }
    }
}