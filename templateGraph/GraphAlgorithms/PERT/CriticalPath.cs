using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using templateGraph;
using templateGraph.VertexConnections;

namespace Graph.GraphAlgorithms.PERT {
    class CriticalPath {
        private MainWindow mainWindow;
        //criticalPath color
        SolidColorBrush color = Brushes.DarkOrange;

        public CriticalPath(MainWindow mainWindow) {
            this.mainWindow = mainWindow;
            if (PertAlgorithm.PertRunning) {
                SearchPath();
            }
            else {
                MessageBox.Show("You need to start the MPM algorithm first!", "Alert"
                    , MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void SearchPath() {
            List<Button> criticals = new List<Button>();
            foreach (Button b in PertAlgorithm.Pert.Keys) {
                if (PertAlgorithm.Pert[b][0] == PertAlgorithm.Pert[b][1]) {
                    criticals.Add(b);
                }
            }

            //get starting vertex 
            Button startingVertex = null;
            Button endingVertex = null;
            VertexRank r = new VertexRank(mainWindow.Vertices, MainWindow.Relations);
            r.SearchForRanks();

            foreach (Button b in r.getRanks().Keys) {
                if (r.getRanks()[b] == 0) {
                    startingVertex = b;
                }
            }
            foreach (Button b in r.getRanks().Keys) {
                if (r.getRanks()[b] == r.RankNumber() - 1) {
                    endingVertex = b;
                }
            }
            List<List<Button>> allpaths = new List<List<Button>>();
            storeAllPaths(startingVertex.Content.ToString(), endingVertex.Content.ToString(),
                allpaths);
            List<List<Button>> criticalPaths = new List<List<Button>>();
            FindCriticalPaths(criticalPaths, allpaths);
            //check if sum of weights are equal to the end weight TODO
            int maxSum = PertAlgorithm.Pert[endingVertex][0];

            List<List<Button>> toRemove = new List<List<Button>>();
            foreach (var path in criticalPaths) {
                int sum = 0;
                for (int i = 0; i < path.Count - 1; i++) {
                    sum += GetRelation(path[i], path[i + 1]).getTransition();
                }
                if (sum != maxSum) {
                    toRemove.Add(path);
                }
            }
            foreach (var l in toRemove) {
                criticalPaths.Remove(l);
            }
            ColorCriticalPath(criticalPaths);
        }



        private void ColorCriticalPath(List<List<Button>> criticalPaths) {
            foreach (var path in criticalPaths) {
                for (int i = 0; i < path.Count - 1; i++) {
                    GetRelation(path[i], path[i + 1]).ColorArrow(color);
                }

            }
        }
        public Relation GetRelation(Button start, Button end) {
            foreach (var relation in MainWindow.Relations) {
                if (relation.ConStart == start && relation.ConEnd == end) {
                    return relation;
                }
            }
            return null;
        }

        private void FindCriticalPaths(List<List<Button>> criticalPaths, List<List<Button>> allpaths) {
            foreach (var l in allpaths) {
                bool verif = true;
                foreach (Button b in l) {
                    if (PertAlgorithm.Pert[b][0] != PertAlgorithm.Pert[b][1])
                        verif = false;

                }
                if (verif)
                    criticalPaths.Add(l);

            }
        }

        public List<string> GetSuccors(string button) {
            List<string> succ = new List<string>();
            foreach (Relation r in MainWindow.Relations) {
                if (r.ConStart.Content.ToString().Equals(button)) {
                    succ.Add(r.ConEnd.Content.ToString());
                }
            }
            return succ;
        }

        public void storeAllPaths(string s, string d, List<List<Button>> allpaths) {
            Dictionary<string, bool> isVisited = new Dictionary<string, bool>();
            foreach (Button button in mainWindow.Vertices) {
                isVisited[button.Content.ToString()] = false;
            }
            List<string> pathList = new List<string>();

            // add source to path[] 
            pathList.Add(s);

            // Call recursive utility 
            printAllPathsUtil(s, d, isVisited, pathList, allpaths);
        }
        private void printAllPathsUtil(string u, string d,
                                   Dictionary<string, bool> isVisited,
                                   List<string> localPathList,
                                   List<List<Button>> allpaths) {

            if (u.Equals(d)) {
                allpaths.Add(extractFromStringPaths(localPathList));
                // if match found then no need 
                // to traverse more till depth 
                return;
            }

            // Mark the current node 
            isVisited[u] = true;

            // Recur for all the vertices 
            // adjacent to current vertex 
            foreach (string i in GetSuccors(u)) {
                if (!isVisited[i]) {//!isVisited[i]) {
                    // store current node 
                    // in path[] 
                    localPathList.Add(i);
                    printAllPathsUtil(i, d, isVisited,
                                      localPathList, allpaths);

                    // remove current node 
                    // in path[] 
                    localPathList.Remove(i);
                }
            }

            // Mark the current node 
            isVisited[u] = false;
        }

        private List<Button> extractFromStringPaths(List<string> localPathList) {
            List<Button> newlist = new List<Button>();
            foreach (string s in localPathList) {
                newlist.Add(GetButtonByName(s));
            }
            return newlist;
        }
        public Button GetButtonByName(string button) {
            foreach (Button b in mainWindow.Vertices) {
                if (b.Content.ToString().Equals(button)) {
                    return b;
                }
            }
            return null;
        }
    }
}
