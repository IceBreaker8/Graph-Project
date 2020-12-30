using Graph.Controllers;
using Graph.Controllers.AlgorithmController;
using Graph.Vertex;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Navigation;
using templateGraph;
using templateGraph.VertexConnections;

namespace Graph.GraphAlgorithms.PERT {
    class PertAlgorithm {

        public Dictionary<Button, int> Ranks;
        private MainWindow mainWindow;
        public static Dictionary<Button, List<int>> Pert = new Dictionary<Button, List<int>>();

        public static bool PertRunning = false;
        public PertAlgorithm(MainWindow mainWindow) {
            if (mainWindow.Vertices.Count < 2) {
                MessageBox.Show("You need at least two vertices to start this algorithm!", "Alert"
                    , MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (AlgoController.algorunning || AlgoController.AlgoStarted) {
                MessageBox.Show("You are already running another algorithm!", "Alert"
                        , MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            Pert.Clear();
            this.mainWindow = mainWindow;
            VertexRank VR = new VertexRank(mainWindow.Vertices, MainWindow.Relations);
            this.Ranks = VR.Ranks;

            if (PertRunning) {
                RemoveAlgorithmBoxes();
            }
            if (CheckIfAlgorithmValid()) {
                InitList();
                CalculateTuples();
                DisplayBoxes();
                AlgoController.algorunning = true;
                AlgoController.AlgoStarted = true;
                PertRunning = true;
            }

        }



        public void InitList() {
            foreach (Button button in mainWindow.Vertices) {
                List<int> addons = new List<int>();
                addons.Add(0);
                addons.Add(0);
                Pert[button] = addons;
            }
        }

        public int GetMaxRank() {
            int max = 0;
            foreach (Button button in Ranks.Keys) {
                if (Ranks[button] > max) {
                    max = Ranks[button];
                }
            }
            return max;
        }


        public void CalculateTuples() {
            int maximum = 0;
            Button lastButton = null;
            //first one
            for (int firstRank = 1; firstRank <= GetMaxRank(); firstRank++) {
                foreach (Button button in Ranks.Keys) {
                    if (Ranks[button] == firstRank) {
                        List<Relation> prevRel = GetPred(button);
                        int max = 0;
                        foreach (Relation relation in prevRel) {
                            Button Start = relation.ConStart;
                            int Tran = relation.getTransition();
                            int total = Tran + Pert[Start][0];
                            if (total > max) {
                                Pert[button][0] = total;
                                max = total;
                                maximum = max;
                            }
                        }

                    }
                    lastButton = button;
                }
            }

            //second one
            Pert[lastButton][1] = maximum;
            for (int firstRank = GetMaxRank() - 1; firstRank >= 0; firstRank--) {
                foreach (Button button in Ranks.Keys) {
                    if (Ranks[button] == firstRank) {
                        List<Relation> prevRel = GetSucc(button);
                        int min = maximum;
                        foreach (Relation relation in prevRel) {
                            Button Start = relation.ConEnd;
                            int Tran = relation.getTransition();
                            int total = Pert[Start][1] - Tran;
                            if (total < min) {
                                Pert[button][1] = total;
                                min = total;
                            }
                        }

                    }
                }
            }


        }



        public void DisplayBoxes() {
            foreach (Button button in mainWindow.Vertices) {
                VertexTupleBox vtb = new VertexTupleBox(button, mainWindow, Pert[button][0]
                    , Pert[button][1]);
                CanvasAndClickCont.butDict[button] = vtb;
            }

        }

        public static void RemoveAlgorithmBoxes() {
            foreach (Button button in CanvasAndClickCont.butDict.Keys)
                CanvasAndClickCont.butDict[button].RemoveTuple();
        }
        public List<Relation> GetPred(Button button) {
            List<Relation> prevRel = new List<Relation>();
            foreach (Relation relation in MainWindow.Relations) {
                if (relation.ConEnd == button) {
                    prevRel.Add(relation);
                }
            }
            return prevRel;
        }

        public List<Relation> GetSucc(Button button) {
            List<Relation> prevRel = new List<Relation>();
            foreach (Relation relation in MainWindow.Relations) {
                if (relation.ConStart == button) {
                    prevRel.Add(relation);
                }
            }
            return prevRel;
        }


        public void SetAllVerticesPositions() {
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

        // ==============================================================================================//

        public bool CheckIfAlgorithmValid() {
            //check if there isnt a curved arrow TODOOOO
            if (!IfOnlyOneStart()) {
                MessageBox.Show("This algorithm only works with one starting vertex!", "Alert",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (!IfOnlyOneEnd()) {
                MessageBox.Show("This algorithm only works with one ending vertex!", "Alert",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (!HasCurvedArrow()) {
                MessageBox.Show("This algorithm only works with single directed arrows!", "Alert",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }

        private bool HasCurvedArrow() {
            foreach (Relation relation in MainWindow.Relations) {
                if (relation.CurveMade == true) {
                    return false;
                }
            }
            return true;
        }

        public bool IfOnlyOneStart() {
            int count = 0;
            foreach (Button b in mainWindow.Vertices) {
                if (GetPred(b).Count == 0) {
                    count++;
                }
            }

            return count == 1 ? true : false;
        }

        public bool IfOnlyOneEnd() {
            int count = 0;
            foreach (Button b in mainWindow.Vertices) {
                if (GetSucc(b).Count == 0) {
                    count++;
                }
            }

            return count == 1 ? true : false;
        }



    }
}
