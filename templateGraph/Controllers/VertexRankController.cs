using Graph.Vertex;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using templateGraph;
using templateGraph.VertexConnections;

namespace Graph.Controllers {
    class VertexRankController {

        private MainWindow mainWindow;
        //menuitems
        private MenuItem SortVertices;
        private MenuItem PositionVertices;
        private MenuItem DisplayRank;
        public VertexRankController(MainWindow mainWindow) {
            this.mainWindow = mainWindow;
            try {
                SortVertices = mainWindow.FindName("SortVertices") as MenuItem;
                PositionVertices = mainWindow.FindName("PositionVertices") as MenuItem;
                DisplayRank = mainWindow.FindName("DisplayRanks") as MenuItem;
            }
            catch(Exception e) {
                MessageBox.Show(e.Message);
            }
            SaveEvents();
        }

        private void SaveEvents() {
            SortVertices.Click += SortVerticesByRank;
            PositionVertices.Click += PositioningVertices;
            DisplayRank.Click += DisplayRanks;
        }
        private void DisplayRanks(object sender, RoutedEventArgs e) {
            if (mainWindow.Vertices.Count == 0) {
                MessageBox.Show("There are no vertices!", "Alert",
                    MessageBoxButton.OK, MessageBoxImage.Warning);

                return;
            }
            if (MainWindow.Relations.Count == 0) {
                MessageBox.Show("There are no connections between vertices!", "Alert",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            VertexRank VR = new VertexRank(mainWindow.Vertices, MainWindow.Relations);
            VR.DisplayRanks();

        }

        private void SortVerticesByRank(object sender, RoutedEventArgs e) {
            if (mainWindow.Vertices.Count == 0) {
                MessageBox.Show("There are no vertices!", "Alert",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (MainWindow.Relations.Count == 0) {
                MessageBox.Show("There are no connections between vertices!", "Alert",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            VertexRank VR = new VertexRank(mainWindow.Vertices, MainWindow.Relations);
            VR.SortVerticesByRanks(ref mainWindow.Vertices);
            MessageBox.Show("Vertices are sorted by ranks!", "Alert",
                    MessageBoxButton.OK, MessageBoxImage.Warning);

        }

        private void PositioningVertices(object sender, RoutedEventArgs e) {
            if (mainWindow.Vertices.Count < 1) {
                MessageBox.Show("You need at least 1 vertex to use this!", "Alert",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            Button button = mainWindow.Vertices[0];
            VertexPosManager vs = new VertexPosManager(mainWindow, mainWindow.Width, mainWindow.Height);

            vs.setAllVerticesPos(mainWindow.Vertices, MainWindow.Relations);

            mainWindow.UpdateLayout();

            foreach (var vertex in mainWindow.Vertices) {

                UpdateRelations(vertex);

            }


        }
        public void UpdateRelations(Button b) {
            foreach (Relation r in MainWindow.Relations) {
                if (r.ConStart == b || r.ConEnd == b)
                    if (r.CurveMade) {
                        r.UpdateCurve();
                        r.UpdatePolygon();
                    }
                    else {
                        r.UpdatePolygon();

                    }
            }
        }
    }
}
