using Graph.Updater;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using templateGraph;

namespace Graph.Controllers {
    class HelpController {
        private MainWindow mainWindow;
        //buttons
        private MenuItem Update;
        private MenuItem Tutorial;
        private MenuItem Report;

        //email address
        private string emailAddress = "graphice.company@gmail.com";

        public HelpController(MainWindow mainWindow) {
            this.mainWindow = mainWindow;
            try {
                Update = mainWindow.FindName("Update") as MenuItem;
                Tutorial = mainWindow.FindName("Tutorial") as MenuItem;
                Report = mainWindow.FindName("Report") as MenuItem;


            }
            catch (Exception e) {
                MessageBox.Show(e.Message);
            }
            SaveEvents();

        }

        private void SaveEvents() {
            Update.Click += CheckForUpdate;
            Tutorial.Click += TutorialRedirect;
            Report.Click += ReportRedirect;
        }

        private void CheckForUpdate(object sender, RoutedEventArgs e) {
            if (new UpdateChecker().CheckForUpdate(true) == false) {
                MessageBox.Show("You are running the latest GraphICE version " + UpdateChecker.version, "Notice",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }


        }
        private void TutorialRedirect(object sender, RoutedEventArgs e) {
            try {
                System.Diagnostics.Process.Start("http://graphice.me/#tutorial");

            }
            catch (Exception) {
                MessageBox.Show("Connection error", "Notice",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
            }


        }

        private void ReportRedirect(object sender, RoutedEventArgs e) {
            MessageBox.Show("Please consider sending bug reports to this email address: " + emailAddress,
                "Notice", MessageBoxButton.OK, MessageBoxImage.Information);


        }
    }
}
