using Graph.Controllers.AlgorithmController;
using Graph.GraphAlgorithms;
using Graph.GraphAlgorithms.PERT;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using templateGraph;
using Graph.Utils;

namespace Graph.Controllers {
    class ButtonController {
        private MainWindow mainWindow;

        //MenuItems
        private MenuItem Rename;
        private MenuItem Delete;
        private MenuItem Start;
        private MenuItem End;


        public ButtonController(MainWindow mainWindow) {
            this.mainWindow = mainWindow;
            try {
                ContextMenu VertexMenu = mainWindow.FindResource("Vertex") as ContextMenu;
                ContextMenu CanvMenu = mainWindow.FindResource("cmButton") as ContextMenu;


                Rename = VertexMenu.Items[0] as MenuItem;

                Delete = VertexMenu.Items[1] as MenuItem;
                // 2 is for the separator
                Start = VertexMenu.Items[3] as MenuItem;

                End = VertexMenu.Items[4] as MenuItem;

            }
            catch (Exception e) {
                MessageBox.Show(e.Message);
            }

            SaveEvents();
        }

        private void SaveEvents() {
            try {
                Rename.Click += RenameVertex;
                Delete.Click += VertexDeletion;
                Start.Click += VertexStartTrigger;
                End.Click += VertexEndTrigger;
            }
            catch (Exception e) {
                MessageBox.Show(e.Message);
            }

        }
        /* ========================= RENAME =================================================*/
        private void RenameVertex(object sender, RoutedEventArgs e) {
            if (AlgoController.AlgoStarted || AlgoController.algorunning) {
                MessageBox.Show("You can't rename a vertex during an active algorithm", "Alert",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            InputBox inputDialog = new InputBox("Rename Vertex:");
            if (inputDialog.ShowDialog() == true && inputDialog.Answer != "") {
                if (ButtonAlreadyExists(inputDialog.Answer)) {
                    MessageBox.Show("You already have that vertex!", "Alert",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                CanvasAndClickCont.VertexMenuButton.Content = inputDialog.Answer;
            }
        }
        private bool ButtonAlreadyExists(string s) {
            foreach (Button b in mainWindow.Vertices) {
                if (b.Content.ToString().Equals(s)) {
                    return true;
                }
            }
            return false;
        }
        /* ==================== DELETION ================================*/
        private void VertexDeletion(object sender, RoutedEventArgs e) {
            if (AlgoController.AlgoStarted || AlgoController.algorunning) {
                MessageBox.Show("You can't delete a vertex during an active algorithm", "Alert",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            RemoveRelationOnVertexDeletion(CanvasAndClickCont.VertexMenuButton);
            mainWindow.Vertices.Remove(CanvasAndClickCont.VertexMenuButton);
            mainWindow.Canv.Children.Remove(CanvasAndClickCont.VertexMenuButton);

        }
        private void RemoveRelationOnVertexDeletion(Button b) {
            List<Relation> a = new List<Relation>();
            foreach (Relation R in MainWindow.Relations) {
                if (R.ConStart == b || R.ConEnd == b) {
                    a.Add(R);
                }
            }
            foreach (Relation R in a) {
                R.DestructRelation();
                MainWindow.Relations.Remove(R);
            }

        }
        /* ============================= START ======================================*/
        private void VertexStartTrigger(object sender, RoutedEventArgs e) {
            if (PertAlgorithm.PertRunning) {
                MessageBox.Show("You can't do that for this algorithm!", "Alert",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!AlgoController.algorunning) {
                MessageBox.Show("An algorithm must be running first!", "Alert",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (AlgoController.OneStartOnly && AlgoController.AlgoStart != null) {
                MessageBox.Show("You can only choose one start for this algorithm!", "Alert",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (AlgoController.AlgoStart != null) {
                if (CanvasAndClickCont.VertexMenuButton == AlgoController.AlgoStart) {
                    MessageBox.Show("You have already selected this button as a start vertex!", "Alert",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;

                }

                if (!AlgoController.DijktraActive) {
                    AlgoController.AlgoStart.Background = Brushes.White;
                    AlgoController.AlgoStart.Foreground = Brushes.Black;
                }
                if (!AlgoController.BellmanActive) {
                    AlgoController.AlgoStart.Background = Brushes.White;
                    AlgoController.AlgoStart.Foreground = Brushes.Black;
                }

            }
            if (AlgoController.AlgoEnd == CanvasAndClickCont.VertexMenuButton) {
                AlgoController.AlgoEnd = null;
            }

            if (AlgoController.BellmanOnOnce) {
                AlgoController.firstStart = true;
                ClearCanvasColor();
                RestoreColor();
                AlgoController.AlgoStart = CanvasAndClickCont.VertexMenuButton;
                //Color it
                AlgoController.AlgoStart.Background = Brushes.Green;
                AlgoController.AlgoStart.Foreground = Brushes.White;
                //algo
                AlgoController.B = new Bellman(MainWindow.Relations,
                   mainWindow.Vertices.Count, mainWindow.Vertices, AlgoController.AlgoStart,
                   AlgoController.IsBellmanAmeliorated);
                if (AlgoController.BellmanAbsorbant) {
                    AlgoAftermath();
                    return;
                }
                AlgoController.B.Show();

                AlgoController.BellmanOnOnce = false;
                AlgoController.BellmanActive = true;
                return;

            }
            if (AlgoController.DijktraOnOnce) {
                AlgoController.firstStart = true;
                ClearCanvasColor();
                RestoreColor();
                AlgoController.AlgoStart = CanvasAndClickCont.VertexMenuButton;
                //Color it
                AlgoController.AlgoStart.Background = Brushes.Green;
                AlgoController.AlgoStart.Foreground = Brushes.White;
                //algo
                AlgoController.D = new Dijkstra(MainWindow.Relations, mainWindow.Vertices.Count,
                    mainWindow.Vertices, AlgoController.AlgoStart);
                AlgoController.D.Show();

                AlgoController.DijktraOnOnce = false;
                AlgoController.DijktraActive = true;
                return;
            }

            RestoreColor();
            AlgoController.AlgoStart = CanvasAndClickCont.VertexMenuButton;
            //Color it
            AlgoController.AlgoStart.Background = Brushes.Green;
            AlgoController.AlgoStart.Foreground = Brushes.White;
            if (AlgoController.AlgoEnd != null) {
                if (AlgoController.F != null) {
                    AlgoController.F.ExtractPath(ref AlgoController.AlgoStart, ref AlgoController.AlgoEnd);
                    AlgoController.F.ColorPath();
                }
                if (AlgoController.D != null) {
                    AlgoController.D.ExtractPath(AlgoController.AlgoEnd);
                    AlgoController.D.ColorPath(ref AlgoController.AlgoEnd);
                }
                if (AlgoController.B != null) {
                    AlgoController.B.ExtractPath(AlgoController.AlgoEnd);
                    AlgoController.B.ColorPath(ref AlgoController.AlgoEnd);
                }

            }
        }
        
        public void ClearCanvasColor() {
            ColorCanvas(EAlgoMode.OFF);
            AlgoController.BellmanOnOnce = false;
            AlgoController.DijktraOnOnce = false;
            AlgoController.AlgoStarted = false;

        }
        public void ColorCanvas(EAlgoMode algomode) {
            if (algomode == EAlgoMode.ON)
                mainWindow.Canv.Background = new SolidColorBrush(Colors.LightGray);
            else
                mainWindow.Canv.Background = new SolidColorBrush(Colors.White);
        }

        private void VertexEndTrigger(object sender, RoutedEventArgs e) {
            if (PertAlgorithm.PertRunning) {
                MessageBox.Show("You can't do that for this algorithm!", "Alert",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!AlgoController.firstStart) {
                MessageBox.Show("You need to select the start first!", "Alert",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!AlgoController.algorunning) {
                MessageBox.Show("An algorithm must be running first!", "Alert",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (AlgoController.DijktraActive) {
                if (CanvasAndClickCont.VertexMenuButton == AlgoController.AlgoStart) {
                    MessageBox.Show("This vertex can't be the start and the end at the same time!", "Alert",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }
            if (AlgoController.BellmanActive) {
                if (CanvasAndClickCont.VertexMenuButton == AlgoController.AlgoStart) {
                    MessageBox.Show("This vertex can't be the start and the end at the same time!", "Alert",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }
            if (AlgoController.AlgoEnd != null) {
                if (CanvasAndClickCont.VertexMenuButton == AlgoController.AlgoEnd) {
                    MessageBox.Show("You have already selected this button as an end vertex!", "Alert",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;

                }

                AlgoController.AlgoEnd.Background = Brushes.White;
                AlgoController.AlgoEnd.Foreground = Brushes.Black;

            }
            if (AlgoController.AlgoStart == CanvasAndClickCont.VertexMenuButton) {
                AlgoController.AlgoStart = null;
            }

            if (CanvasAndClickCont.VertexMenuButton == AlgoController.AlgoEnd) {
                MessageBox.Show("You have already selected this button as an end vertex!", "Info",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            RestoreColor();
            AlgoController.AlgoEnd = CanvasAndClickCont.VertexMenuButton;
            //Color it
            AlgoController.AlgoEnd.Background = Brushes.Crimson;
            AlgoController.AlgoEnd.Foreground = Brushes.White;
            if (AlgoController.AlgoStart != null) {
                if (AlgoController.F != null) {
                    AlgoController.F.ExtractPath(ref AlgoController.AlgoStart, ref AlgoController.AlgoEnd);
                    AlgoController.F.ColorPath();
                }
                if (AlgoController.D != null) {
                    AlgoController.D.ExtractPath(AlgoController.AlgoEnd);
                    AlgoController.D.ColorPath(ref AlgoController.AlgoEnd);
                }
                if (AlgoController.B != null) {
                    AlgoController.B.ExtractPath(AlgoController.AlgoEnd);
                    AlgoController.B.ColorPath(ref AlgoController.AlgoEnd);
                }

            }

        }



        /* =================================================================================== */
        public void AlgoAftermath() {
            if (AlgoController.AlgoStart != null) {
                AlgoController.AlgoStart.Background = Brushes.White;
                AlgoController.AlgoStart.Foreground = Brushes.Black;

            }
            if (AlgoController.AlgoEnd != null) {
                AlgoController.AlgoEnd.Background = Brushes.White;
                AlgoController.AlgoEnd.Foreground = Brushes.Black;

            }
            AlgoController.firstStart = false;
            AlgoController.algorunning = false;
            AlgoController.IsBellmanAmeliorated = false;
            AlgoController.BellmanAbsorbant = false;
            AlgoController.AlgoStart = null;
            AlgoController.AlgoEnd = null;
            AlgoController.AlgoStarted = false;
            AlgoController.OneStartOnly = false;
            AlgoController.DijktraOnOnce = false;
            AlgoController.DijktraActive = false;
            AlgoController.BellmanActive = false;
            AlgoController.BellmanOnOnce = false;
            AlgoController.F = null;
            AlgoController.D = null;
            AlgoController.B = null;
            foreach (Relation R in MainWindow.Relations) {
                R.RestoreArrowColor();

            }
            mainWindow.ColorCanvas(EAlgoMode.OFF);
        }
        public void RestoreColor() {
            foreach (Relation R in MainWindow.Relations) {
                R.RestoreArrowColor();
            }

        }

    }

}
