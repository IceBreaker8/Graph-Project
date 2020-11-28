using Graph.GraphAlgorithms.PERT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using templateGraph;

namespace Graph.Controllers {
    class OrdonAlgorithm {

        private MainWindow mainWindow;

        private MenuItem MpmAlgo;
        private MenuItem Critical;
        public OrdonAlgorithm(MainWindow mainWindow) {
            this.mainWindow = mainWindow;
            try {
                MpmAlgo = mainWindow.FindName("PERT") as MenuItem;
                Critical = mainWindow.FindName("Critical") as MenuItem;
            }
            catch (Exception e) {
                MessageBox.Show(e.Message);
            }
            SaveEvents();
        }

        public void SaveEvents() {
            MpmAlgo.Click += MpmAlgoTrigger;
            Critical.Click += CriticalClick;
        }

        private void MpmAlgoTrigger(object sender, RoutedEventArgs e) {
            new PertAlgorithm(mainWindow);
        }

        private void CriticalClick(object sender, RoutedEventArgs e) {
            new CriticalPath(mainWindow);
        }
    }
}
