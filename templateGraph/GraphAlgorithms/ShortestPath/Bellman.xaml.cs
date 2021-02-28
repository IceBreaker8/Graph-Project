using Graph.Controllers.AlgorithmController;
using Graph.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using templateGraph;
using templateGraph.VertexConnections;

namespace Graph.GraphAlgorithms {
    /// <summary>
    /// Interaction logic for Bellman.xaml
    /// </summary>
    public partial class Bellman : Window {

        private List<Relation> relations;
        private List<Button> buttons;
        private Button algoStart;

        public String[,] outputTable;
        public int[,] intDistance;
        String[,] PredsString;
        public int size;

        private bool BellmanAmeliorated;

        public Bellman(List<Relation> relations, int count, List<Button> vertices, Button algoStart,
            bool BellmanAmeliorated) {
            InitializeComponent();

            this.BellmanAmeliorated = BellmanAmeliorated;
            this.relations = new List<Relation>(relations);
            this.size = count;
            this.buttons = new List<Button>(vertices);
            this.algoStart = algoStart;
            PredsString = new String[size + 2, size + 1];
            outputTable = new String[size + 2, size + 1];
            intDistance = new int[size + 2, size + 1];
            InitBellman();
        }
        private Button GetButtonByName(String ButtonName) {
            foreach (Button B in buttons) {
                if (B.Content.ToString() == ButtonName) {
                    return B;
                }
            }
            return null;
        }
        private int INF = 99999;
        private int FindDistance(Button start, Button end) {
            foreach (Relation R in relations) {
                if (R.ConStart == start && R.ConEnd == end) {
                    return R.getTransition();
                }
            }
            return INF;
        }
        private bool CheckPreviousLine(int line, int predLine) {
            for (int i = 2; i <= size; i++) {
                if (outputTable[line, i] != outputTable[predLine, i]) {
                    return false;
                }
            }
            return true;

        }
        private bool CheckIfPred(String start, String end) {
            foreach (Relation R in relations) {
                if (R.ConStart.Content.ToString() == start
                    && R.ConEnd.Content.ToString() == end) {
                    return true;
                }
            }
            return false;

        }

        private int getTransition(String start, String end) {
            foreach (Relation R in relations) {
                if (R.ConStart.Content.ToString() == start
                    && R.ConEnd.Content.ToString() == end) {
                    return R.getTransition();
                }
            }
            return INF;

        }

        private void InitBellman() {
            if (BellmanAmeliorated) {
                VertexRank VR = new VertexRank(buttons, relations);
                VR.SortVerticesByRanks(ref buttons);
            }
            int k = 0;
            buttons.Remove(algoStart);
            outputTable[0, 0] = "k";
            outputTable[0, 1] = algoStart.Content.ToString();
            int c = 0;
            for (int j = 2; j < size + 1; j++) {
                outputTable[0, j] = buttons[c++].Content.ToString();

            }
            outputTable[1, 0] = "0";

            outputTable[1, 1] = "0";
            intDistance[1, 1] = 0;

            for (int j = 2; j <= size; j++) {
                intDistance[1, j] = INF;
                outputTable[1, j] = "INF";
                PredsString[1, j] = "";
            }

            //Algorithm
            while (true) {
                k++;
                bool disInf = false;
                outputTable[k + 1, 0] = k.ToString();
                outputTable[k + 1, 1] = "0";
                intDistance[k + 1, 1] = 0;

                for (int j = 2; j <= size; j++) {
                    String Button = outputTable[0, j]; //B
                    //min
                    int minT = intDistance[k, j]; //min distance of B
                    String PredMin = PredsString[k, j];
                    for (int f = 1; f <= size; f++) {
                        disInf = false;
                        if (outputTable[0, f] == Button)
                            continue;
                        if (intDistance[k, f] == INF)
                            disInf = true;

                        if (CheckIfPred(outputTable[0, f], Button)) {
                            int transition = getTransition(outputTable[0, f], Button); //12
                            //getTransition
                            if (disInf == false) {
                                if (transition + intDistance[k, f] < minT) {
                                    PredMin = outputTable[0, f];
                                    minT = transition + intDistance[k, f];

                                } else if (transition + intDistance[k, f] == minT &&
                                      PredsString[k, j] == "") {
                                    PredMin += "," + outputTable[0, f];


                                } else if (transition + intDistance[k, f] == minT &&
                                      !PredsString[k, j].Contains(outputTable[0, f])) {
                                    PredMin = PredsString[k, j] + "," + outputTable[0, f];


                                }
                            }

                            //
                            if (BellmanAmeliorated && f < j) {
                                if (intDistance[k + 1, f] == INF)
                                    continue;
                                if (transition + intDistance[k + 1, f] < minT) {
                                    PredMin = outputTable[0, f];
                                    minT = transition + intDistance[k + 1, f];

                                } else if (transition + intDistance[k + 1, f] == minT &&
                                      PredsString[k, j] == "" &&
                                      !PredMin.Contains(outputTable[0, f])) {
                                    PredMin += "," + outputTable[0, f];


                                } else if (transition + intDistance[k + 1, f] == minT &&
                                      !PredsString[k, j].Contains(outputTable[0, f]) &&
                                      !PredMin.Contains(outputTable[0, f])) {
                                    PredMin = PredsString[k, j] + "," + outputTable[0, f];


                                }

                            }
                        }


                    }
                    PredsString[k + 1, j] = PredMin;
                    if (minT == INF) {
                        outputTable[k + 1, j] = "INF";
                    } else {
                        outputTable[k + 1, j] = minT.ToString();
                    }
                    intDistance[k + 1, j] = minT;

                }

                if (CheckPreviousLine(k + 1, k)) {
                    break;
                } else {
                    if (k == size) {
                        if (!CheckPreviousLine(k + 1, k)) {
                            MessageBox.Show("Absorbant!", "Alert", MessageBoxButton.OK, MessageBoxImage.Warning);
                            AlgoController.BellmanAbsorbant = true;
                            return;
                        }
                    }
                }
            }
            this.kHeight = k + 1;
            buttons.Add(algoStart);
            Print();
        }


        private int kHeight;
        private GridDisplay to;
        private void Print() {
            Canvas canv1 = (Canvas)this.FindName("TextB");

            if (BellmanAmeliorated) {
                to = new GridDisplay("Bellman Ameliorated", this.kHeight + 1, this.size + 1);
            } else {
                to = new GridDisplay("Bellman", this.kHeight + 1, this.size + 1);
            }
            for (int i = 0; i <= this.kHeight; i++) {
                for (int j = 0; j <= size; j++) {
                    if (i == 0 || j == 0) {
                        to.AddCell(i, j, new Run(outputTable[i, j]) { FontWeight = FontWeights.Bold }, null);
                    } else {
                        if (j >= 2) {
                            to.AddCell(i, j, new Run(outputTable[i, j]),
                                new Run(PredsString[i, j]) {
                                    BaselineAlignment
                            = BaselineAlignment.Subscript,
                                    FontSize = 12
                                });

                        } else {
                            to.AddCell(i, j, new Run(outputTable[i, j]), null);
                        }
                    }
                }
            }

            Grid g = to.getGrid();

            canv1.Children.Add(g);

            this.Show();

            this.Width = g.ActualWidth + GridDisplay.widthError;
            this.Height = g.ActualHeight + GridDisplay.heightError;
            canv1.Width = g.ActualWidth + GridDisplay.minCanvWidthError;
            canv1.Height = g.ActualHeight + GridDisplay.minCanvHeightError;
            CenterWindowOnScreen();

        }
        private void CenterWindowOnScreen() {
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            double windowWidth = this.Width;
            double windowHeight = this.Height;
            this.Left = (screenWidth / 2) - (windowWidth / 2);
            this.Top = (screenHeight / 2) - (windowHeight / 2);
        }


        List<String> result;
        private Button end;
        public void ExtractPath(Button end) {
            this.end = end;
            result = new List<String>();
            result.Add(end.Content.ToString());
            String next = end.Content.ToString();

            while (true) {
                for (int j = 2; j <= size; j++) {
                    if (next == outputTable[0, j]) {
                        if (outputTable[kHeight, j] == "INF") {

                            return;
                        }
                        if (PredsString[kHeight, j].Contains(",")) {
                            if (PredsString[kHeight, j].ToString()
                                .Substring(0, PredsString[kHeight, j].ToString().IndexOf(","))
                                == algoStart.Content.ToString()) {

                                result.Add(algoStart.Content.ToString());
                                return;
                            }
                            next = PredsString[kHeight, j].ToString()
                                .Substring(0, PredsString[kHeight, j].ToString().IndexOf(","));
                            result.Add(next);
                        } else {
                            if (PredsString[kHeight, j].ToString() == algoStart.Content.ToString()) {

                                result.Add(algoStart.Content.ToString());
                                return;
                            }
                            next = PredsString[kHeight, j].ToString();
                            result.Add(next);
                        }

                    }

                }
            }

        }
        public void RestoreColor() {
            foreach (Relation R in relations) {
                R.RestoreArrowColor();
            }
        }
        public void ColorPath(ref Button algoEnd) {
            if (result.Count == 1) {
                MessageBox.Show("Unreachable!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                end.Background = Brushes.White;
                end.Foreground = Brushes.Black;
                algoEnd = null;
                return;
            }
            RestoreColor();
            for (int i = result.Count - 1; i >= 1; i--) {
                foreach (Relation R in relations) {
                    if (R.ConStart.Content.ToString() == result[i]
                        && R.ConEnd.Content.ToString() == result[i - 1]) {
                        R.ColorArrow(Brushes.Red);


                        foreach (Relation r2 in MainWindow.Relations) {
                            if (r2.ConStart == R.ConEnd && R.ConStart == r2.ConEnd) {
                                if (r2.linkType == Relation.LinkType.UndirectedArrow)
                                    r2.ColorArrow(Brushes.Red);
                            }
                        }


                    }
                }
            }

        }
        public void Window_Closed(object sender, EventArgs e) {
            MainWindow wnd = (MainWindow)Application.Current.MainWindow;
            wnd.AlgoAftermath();

        }

    }
}
