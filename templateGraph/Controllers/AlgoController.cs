using Graph.GraphAlgorithms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using templateGraph;
using templateGraph.GraphAlgorithms;

namespace Graph.Controllers.AlgorithmController {
    class AlgoController {
        private MainWindow mainWindow;




        //Algorithm Variables
        public static bool AlgoStarted = false;
        public static Button AlgoStart;
        public static Button AlgoEnd;


        public static Floyd F;
        public static Dijktra D;
        public static Bellman B;

        public static bool BellmanOnOnce = false;
        public static bool BellmanActive = false;

        public static bool IsBellmanAmeliorated = false;
        public static bool DijktraOnOnce = false;
        public static bool OneStartOnly = false;
        public static bool DijktraActive = false;

        public static bool BellmanAbsorbant = false;
        public static bool algorunning = false;


        public static bool firstStart = false;
        //MenuItems
        MenuItem DijktraButton;
        MenuItem BellmanButton;
        MenuItem FloydButton;
        MenuItem BellmanAmeButton;
        public AlgoController(MainWindow mainWindow) {
            this.mainWindow = mainWindow;
            DijktraButton = mainWindow.FindName("Dijktra") as MenuItem;
            BellmanButton = mainWindow.FindName("Bellman") as MenuItem;
            BellmanAmeButton = mainWindow.FindName("BellmanAme") as MenuItem;
            FloydButton = mainWindow.FindName("Floyd") as MenuItem;
            SaveEvents();
        }

        private void SaveEvents() {
            DijktraButton.Click += Dijktra;
            BellmanAmeButton.Click += AmelioratedBellman;
            BellmanButton.Click += Bellman;
            FloydButton.Click += Floyd;
        }

        private bool DijktraError() {
            foreach (Relation R in MainWindow.Relations) {
                if (R.getTransition() < 0) {
                    return true;
                }
            }
            return false;
        }



        public void RestoreColor() {
            foreach (Relation R in MainWindow.Relations) {
                R.RestoreArrowColor();
            }

        }
        private void Dijktra(object sender, RoutedEventArgs e) {
            if (mainWindow.Vertices.Count < 2) {
                MessageBox.Show("You need to construct at least 2 vertices to run an algorithm!", "Alert",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (AlgoStarted || algorunning) {
                MessageBox.Show("There is already another active algorithm!", "Alert",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (DijktraError()) {
                MessageBox.Show("You can't run Dijktra on a graph with negative weight(s)!", "Alert",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            algorunning = true;
            AlgoStarted = true;
            DijktraOnOnce = true;
            OneStartOnly = true;
            if (AlgoStart == null) {
                mainWindow.ColorCanvas(MainWindow.AlgoMode.ON);
                MessageBox.Show("Please select the starting vertex!", "Info",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }


        }
        private void Bellman(object sender, RoutedEventArgs e) {
            if (mainWindow.Vertices.Count < 2) {
                MessageBox.Show("You need to construct at least 2 vertices to run an algorithm!", "Alert",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (AlgoStarted || algorunning) {
                MessageBox.Show("There is already another active algorithm!", "Alert",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            //
            //TODO test for negative cycles

            //
            algorunning = true;
            AlgoStarted = true;
            BellmanOnOnce = true;
            OneStartOnly = true;
            if (AlgoStart == null) {
                mainWindow.ColorCanvas(MainWindow.AlgoMode.ON);
                MessageBox.Show("Please select the starting vertex!", "Info",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
        }

        private void AmelioratedBellman(object sender, RoutedEventArgs e) {
            if (AlgoStarted || algorunning) {
                mainWindow.ColorCanvas(MainWindow.AlgoMode.ON);
                MessageBox.Show("There is already another active algorithm!", "Alert",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            ////TEST NEGATIVE CYCLES
            //F = new Floyd(Relations, Vertices.Count, Vertices);
            //if (F.NegCycle()) {
            //    MessageBox.Show("You can't find a shortest path with the presence of a negative cycle!");
            //    F = null;
            //    return;
            //}
            //F = null;
            ////
            ///
            algorunning = true;
            IsBellmanAmeliorated = true;
            AlgoStarted = true;
            BellmanOnOnce = true;
            OneStartOnly = true;
            if (AlgoStart == null) {
                mainWindow.ColorCanvas(MainWindow.AlgoMode.ON);
                MessageBox.Show("Please select the starting vertex!", "Info",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
        }

        private void Floyd(object sender, RoutedEventArgs e) {
            if (mainWindow.Vertices.Count < 2) {
                MessageBox.Show("You need to construct at least 2 vertices to run an algorithm!", "Alert",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (AlgoStarted || algorunning) {
                MessageBox.Show("There is already another active algorithm!", "Alert",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            F = new Floyd(MainWindow.Relations, mainWindow.Vertices.Count, mainWindow.Vertices);
            if (F.NegCycle()) {
                MessageBox.Show("You can't find a shortest path with the presence of a negative cycle!", "Alert",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                F = null;
                return;
            }
            F.Show();
            AlgoStarted = true;
            algorunning = true;
            firstStart = true;

        }

        private void VertexEndTrigger(object sender, RoutedEventArgs e) {
            if (!firstStart) {
                MessageBox.Show("You need to select the start first!", "Alert",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!algorunning) {
                MessageBox.Show("An algorithm must be running first!", "Alert",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (DijktraActive) {
                if (CanvasAndClickCont.VertexMenuButton == AlgoStart) {
                    MessageBox.Show("This vertex can't be the start and the end at the same time!", "Alert",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }
            if (BellmanActive) {
                if (CanvasAndClickCont.VertexMenuButton == AlgoStart) {
                    MessageBox.Show("This vertex can't be the start and the end at the same time!", "Alert",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }
            if (AlgoEnd != null) {
                if (CanvasAndClickCont.VertexMenuButton == AlgoEnd) {
                    MessageBox.Show("You have already selected this button as an end vertex!", "Alert",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;

                }

                AlgoEnd.Background = Brushes.White;
                AlgoEnd.Foreground = Brushes.Black;

            }
            if (AlgoStart == CanvasAndClickCont.VertexMenuButton) {
                AlgoStart = null;
            }

            if (CanvasAndClickCont.VertexMenuButton == AlgoEnd) {
                MessageBox.Show("You have already selected this button as an end vertex!", "Info",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            RestoreColor();
            AlgoEnd = CanvasAndClickCont.VertexMenuButton;
            //Color it
            AlgoEnd.Background = Brushes.Crimson;
            AlgoEnd.Foreground = Brushes.White;
            if (AlgoStart != null) {
                if (F != null) {
                    F.ExtractPath(ref AlgoStart, ref AlgoEnd);
                    F.ColorPath();
                }
                if (D != null) {
                    D.ExtractPath(AlgoEnd);
                    D.ColorPath(ref AlgoEnd);
                }
                if (B != null) {
                    B.ExtractPath(AlgoEnd);
                    B.ColorPath(ref AlgoEnd);
                }

            }

        }
        
    }
}
