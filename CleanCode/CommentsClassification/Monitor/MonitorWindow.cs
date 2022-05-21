using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using Microsoft.Win32;
using CleanCode.CommentsClassification.Monitor.Models;

namespace CleanCode.CommentsClassification.Monitor
{
    public class MonitorWindow
    {
        private const string WaitingTitle = "downloading . . .";
        private const string ErrorTitle = "! connection problem !";
        private const string DoneTitle = "data fetched";

        public MonitorWindow()
        {
           // ...
           // business logic removed
           // ...
        }

        private async void BtnExport_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                // 10
                // TODO comments
                // todo: add other raw formats and design separate class for SaveFileDialog
                const string exportedFormat = "csv";
                var dialog = new SaveFileDialog()
                {
                    Filter = $"{exportedFormat} files (*.{exportedFormat})||All files (*.*)|*.*",
                    FileName = $"statistics_{DateTime.Now.ToString("yy-MM-dd")}.{exportedFormat}"
                };

                // ...
                // business logic removed
                // ...

                const char separator = ';';
                using (var sw = new StreamWriter(dialog.FileName))
                {
                    // ...
                    // business logic removed
                    // ...
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private delegate void GetDataLastDays(int days);

        private void BtnGetLast30_OnClick(object sender, RoutedEventArgs e)
        {
            UpdateData(30);
        }
        
        private void BtnGetLast180_OnClick(object sender, RoutedEventArgs e)
        {
            // 11
            // warn of consequences
            // use carefully, if UpdateData(...) takes more than 100 days it could take a while
            // and freeze UI 
            UpdateData(180);
        }

        private void UpdateData(int days)
        {
            // ...
            // business logic removed
            // ...
        }

        private void SetDisplayedStatistics(IEnumerable<(Statistic, Plugin, Panel)> statistics)
        {
            var grouped = statistics.GroupBy(t => t.Item2.Name);

            var uniqueRecords = new HashSet<DisplayedEntity>();

            foreach (var groupByPlugin in grouped)
            {
                foreach (var (st, pl, pn) in groupByPlugin)
                {
                    // 12
                    // magnification of used something
                    // symbol "!" used for handling errors
                    // if you want edit that, take care of changing Statistics creating logic
                    var displayedEntity = new DisplayedEntity
                    {
                        Name = pl.Name,
                        Version = pl.Version,
                        Panel = pn.Name,
                        Launches = 1,
                        Errors = st.Result.StartsWith("!") ? 1 : 0
                    };

                    if (uniqueRecords.Add(displayedEntity))
                    {
                        displayedEntity.UsersList.Add(st.User_Name);
                        continue;
                    }

                    uniqueRecords.TryGetValue(displayedEntity, out var recorderEntity);

                    recorderEntity.Launches++;
                    recorderEntity.Errors += displayedEntity.Errors;

                    recorderEntity.UsersList.Add(st.User_Name);
                }
            }

            // ...
            // business logic removed
            // ...
        }
    }
}