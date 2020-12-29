using Graph.Updater;
using Graph.Utils;
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
        private MenuItem PatchNotes;
        private MenuItem About;

        //email address
        private string emailAddress = "graphice.company@gmail.com";

        public HelpController(MainWindow mainWindow) {
            this.mainWindow = mainWindow;
            try {
                Update = mainWindow.FindName("Update") as MenuItem;
                Tutorial = mainWindow.FindName("Tutorial") as MenuItem;
                Report = mainWindow.FindName("Report") as MenuItem;
                PatchNotes = mainWindow.FindName("Patch") as MenuItem;
                About = mainWindow.FindName("About") as MenuItem;
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
            PatchNotes.Click += Patching;
            About.Click += Abouting;
        }


        private void Abouting(object sender, RoutedEventArgs e) {
            Window1 w = new Window1();
            w.Show();
        }
        private void Patching(object sender, RoutedEventArgs e) {

            try {
                System.Diagnostics.Process.Start("http://graphice.me/#updates");

            }
            catch (Exception) {
                MessageBox.Show("Connection error!", "Notice",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
            }

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
                MessageBox.Show("Connection error!", "Notice",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
            }


        }

        private void ReportRedirect(object sender, RoutedEventArgs e) {
            MessageBox.Show("Please consider sending bug reports to this email address: " + emailAddress,
                "Notice", MessageBoxButton.OK, MessageBoxImage.Information);


        }
    }
}
