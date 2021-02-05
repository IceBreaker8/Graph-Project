
using Graph.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace templateGraph.GraphAlgorithms {

    public partial class Floyd : Window {

        List<Relation> relations = new List<Relation>();
        private bool checkCyclesOnly = false;
        public Floyd(List<Relation> relations, int size, List<Button> buttons, bool checkCyclesOnly) {
            InitializeComponent();
            this.checkCyclesOnly = checkCyclesOnly;
            if (checkCyclesOnly)
                HideWindow();
            gridHeight = 0;
            InitFloyd(relations, size, buttons);
            this.relations = relations;


        }

        public void HideWindow() {
            this.Visibility = Visibility.Hidden;
            this.Width = 0;
            this.Height = 0;
            WindowStyle = WindowStyle.None;
            ShowInTaskbar = false;
            ShowActivated = false;
        }



        public String[,] distance;
        public int[,] intDistance;
        public int size;
        public String[,] P;


        //window items


        Dictionary<Button, List<Relation>> Dict = new Dictionary<Button, List<Relation>>();
        public void InitFloyd(List<Relation> relations, int size, List<Button> buttons) {
            if (size < 1) {
                MessageBox.Show("You can't execute this algorithm!", "Alert", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            this.size = size;
            distance = new String[size + 1, size + 1];
            P = new String[size + 1, size + 1];
            intDistance = new int[size + 1, size + 1];
            distance[0, 0] = " ";
            for (int i = 1; i < size + 1; i++) {
                distance[i, 0] = buttons[i - 1].Content.ToString();
                distance[0, i] = buttons[i - 1].Content.ToString();
                P[i, 0] = buttons[i - 1].Content.ToString();
                P[0, i] = buttons[i - 1].Content.ToString();
            }
            foreach (Relation R in relations) {
                if (!Dict.ContainsKey(R.ConStart)) {
                    Dict.Add(R.ConStart, new List<Relation>());
                }
                Dict[R.ConStart].Add(R);

            }
            //construction of distance array
            ConstructArray(buttons, relations);
            IsCalculated = false;
        }



        private int INF = 9999;
        private void ConstructArray(List<Button> buttons, List<Relation> relations) {
            for (int i = 1; i < this.size + 1; i++) {
                for (int j = 1; j < this.size + 1; j++) {
                    bool verif = false;
                    if (i == j) {
                        distance[i, j] = "0";
                        intDistance[i, j] = 0;
                        continue;
                    }
                    foreach (Relation R in relations) {
                        if (R.ConStart == buttons[i - 1] && R.ConEnd == buttons[j - 1]) {
                            distance[i, j] = R.getTransition().ToString();
                            intDistance[i, j] = R.getTransition();
                            verif = true;
                            break;
                        }
                    }
                    if (verif == false) {
                        distance[i, j] = "INF";
                        intDistance[i, j] = INF;
                    }

                }
            }

            for (int i = 1; i < this.size + 1; i++) {
                for (int j = 1; j < this.size + 1; j++) {

                    if (distance[i, j] == "INF") {
                        P[i, j] = "0";
                    } else {
                        P[i, j] = buttons[i - 1].Content.ToString();
                    }
                }
            }
            PrintMatrix();
            PrintP();

            FloydWarshall();

            canv.Height = gridHeight;




        }

        private Canvas canv;

        private double gridHeight = 0;

        private GridDisplay to;
        public void PrintMatrix() {
            canv = (Canvas)this.FindName("text");

            to = new GridDisplay("Dijktra", this.size + 1, this.size + 1);

            for (int i = 0; i < size + 1; i++) {
                for (int j = 0; j < size + 1; j++) {
                    if (i == 0 || j == 0) {
                        to.AddCell(i, j, new Run(distance[i, j] + "") { FontWeight = FontWeights.Bold }, null);
                    } else if (intDistance[i, j] == INF) {
                        to.AddCell(i, j, new Run("INF"), null);
                    } else {
                        to.AddCell(i, j, new Run(intDistance[i, j] + ""), null);
                    }
                }

            }

            Grid g = to.getGrid();
            Canvas.SetTop(g, gridHeight);
            canv.Children.Add(g);
            if (!IsCalculated) {
                if (!checkCyclesOnly) {
                    this.Show();
                    WidthToSize = g.ActualWidth;
                    canv.Width = WidthToSize * 2 + 200;
                    this.Width = WidthToSize * 2 + 300;
                    IsCalculated = true;
                    CenterWindowOnScreen();
                }
                
            }


        }


        public void PrintP() {

            to = new GridDisplay("Floyd", this.size + 1, this.size + 1);


            for (int i = 0; i < size + 1; i++) {
                for (int j = 0; j < size + 1; j++) {
                    if (i == 0 || j == 0) {
                        to.AddCell(i, j, new Run(P[i, j] + "") { FontWeight = FontWeights.Bold }, null);

                    } else {
                        to.AddCell(i, j, new Run(P[i, j] + ""), null);
                    }

                }

            }

            Grid g = to.getGrid();
            Canvas.SetTop(g, gridHeight);
            canv.Children.Add(g);



            Canvas.SetLeft(g, WidthToSize + 100);



            gridHeight += (this.size + 1) * 20 + GridDisplay.heightError;

        }

        private void CenterWindowOnScreen() {
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            double windowWidth = this.Width;
            double windowHeight = this.Height;
            this.Left = (screenWidth / 2) - (windowWidth / 2);
            this.Top = (screenHeight / 2) - (windowHeight / 2);
        }

        private double WidthToSize = 0;
        private bool IsCalculated = false;
        public void FloydWarshall() {

            for (int k = 1; k < size + 1; k++) {
                for (int i = 1; i < size + 1; i++) {
                    for (int j = 1; j < size + 1; j++) {
                        if (intDistance[i, k] == INF || intDistance[k, j] == INF)
                            continue;
                        if (intDistance[i, k] + intDistance[k, j] < intDistance[i, j]) {
                            intDistance[i, j] = intDistance[i, k] + intDistance[k, j];
                            P[i, j] = P[k, j];
                        }
                    }
                }
                PrintMatrix();
                PrintP();
            }


        }



        public String[] preds;
        public int BeginIndex;
        public int EndIndex;
        public int realSize;
        public void ExtractPath(ref Button begin, ref Button end) {
            realSize = 0;
            preds = new String[size];
            for (int i = 0; i < size + 1; i++) {
                if (P[i, 0] == begin.Content.ToString()) {
                    BeginIndex = i;
                    break;
                }
            }
            for (int i = 1; i < size + 1; i++) {
                if (P[0, i] == end.Content.ToString()) {
                    EndIndex = i;
                    break;
                }
            }
            int j = 0;
            preds[j++] = end.Content.ToString();


            while (true) {
                String pred = P[BeginIndex, EndIndex];
                if (pred == "0") {
                    MessageBox.Show("Unreachable!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    end.Background = Brushes.White;
                    end.Foreground = Brushes.Black;
                    begin.Background = Brushes.White;
                    begin.Foreground = Brushes.Black;
                    end = null;
                    begin = null;
                    RestoreArrowColors();
                    return;
                }
                if (pred == begin.Content.ToString()) {
                    preds[j++] = begin.Content.ToString();
                    realSize = j;
                    return;
                }
                for (int i = 1; i < size + 1; i++) {
                    if (P[0, i] == pred) {
                        EndIndex = i;
                        preds[j++] = pred;
                        break;
                    }
                }


            }

        }
        public void ColorPath() {
            RestoreArrowColors();
            for (int i = realSize - 1; i >= 1; i--) {
                foreach (Relation R in relations) {
                    if (R.ConStart.Content.ToString() == preds[i]
                        && R.ConEnd.Content.ToString() == preds[i - 1]) {
                        R.ColorArrow(Brushes.Red);
                    }
                }
            }
        }
        public void RestoreArrowColors() {
            foreach (Relation R in relations) {
                R.RestoreArrowColor();
            }

        }

        public bool NegCycle() {
            for (int i = 1; i < size + 1; i++)
                if (intDistance[i, i] < 0)
                    return true;
            return false;
        }
        public void Window_Closed(object sender, EventArgs e) {
            MainWindow wnd = (MainWindow)Application.Current.MainWindow;
            wnd.AlgoAftermath();

        }


        public void ColorNegCycle() {
            //later

        }
    }

}

