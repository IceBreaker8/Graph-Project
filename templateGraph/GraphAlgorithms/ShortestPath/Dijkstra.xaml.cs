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

namespace Graph.GraphAlgorithms {
    /// <summary>
    /// Interaction logic for Dijktra.xaml
    /// </summary>
    public partial class Dijkstra : Window {

        List<Relation> relations = new List<Relation>();
        List<Button> buttons = new List<Button>();
        private Button AlgoStart;

        public Canvas canv1;
        public double canvHeight;
        public double canvWidth;

        public Dijkstra(List<Relation> relations, int size, List<Button> buttons, Button AlgoStart) {
            InitializeComponent();

            this.relations = relations;
            this.buttons = new List<Button>(buttons);
            this.size = size;
            this.AlgoStart = AlgoStart;
            actualHeight = this.size;
            PredsString = new String[size + 1, size + 1];
            outputTable = new String[size + 1, size + 1];
            intDistance = new int[size + 1, size + 1];

            canv1 = (Canvas)this.FindName("TextB");

            InitDijktra();





        }



        public String[,] outputTable;
        public int[,] intDistance;
        String[,] PredsString;
        public int size;

        private Button GetButtonByName(String ButtonName) {
            foreach (Button B in buttons) {
                if (B.Content.ToString() == ButtonName) {
                    return B;
                }
            }
            return null;
        }
        private int FindDistance(Button start, Button end) {
            foreach (Relation R in relations) {
                if (R.ConStart == start && R.ConEnd == end) {
                    return R.getTransition();
                }
            }
            return INF;
        }
        private int INF = int.MaxValue;
        private int count = 0;
        private Button NextMin;

        private void InitDijktra() {
            outputTable[0, 0] = "S";
            outputTable[0, 1] = AlgoStart.Content.ToString();
            outputTable[1, 0] = AlgoStart.Content.ToString();
            buttons.Remove(AlgoStart);
            for (int i = 2; i < size + 1; i++) {
                outputTable[0, i] = buttons[count++].Content.ToString();
            }
            outputTable[1, 1] = "0";
            intDistance[1, 1] = 0;

            for (int i = 2; i <= size; i++) {
                Button b = GetButtonByName(outputTable[0, i]);
                intDistance[1, i] = FindDistance(AlgoStart, b);
                if (intDistance[1, i] == INF) {
                    outputTable[1, i] = "INF";
                }
                else {
                    outputTable[1, i] = intDistance[1, i].ToString();
                    PredsString[1, i] = AlgoStart.Content.ToString();

                }
            }
            AlreadyMarked.Add(AlgoStart);
            //Real dijktra
            for (int i = 2; i <= size; i++) {
                if (VerifyINF(i - 1)) {
                    this.actualHeight = i - 1;
                    buttons.Add(AlgoStart);
                    Print();
                    return;
                }
                outputTable[i, 1] = "0";
                intDistance[i, 1] = 0;
                NextMin = GetNextMin(i - 1);
                outputTable[i, 0] = NextMin.Content.ToString();
                int RealMin = GetRealMin(i - 1, NextMin);
                for (int j = 2; j <= size; j++) {
                    PredsString[i, j] = "";
                    if (outputTable[0, j] == outputTable[i, 0]) {
                        intDistance[i, j] = intDistance[i - 1, j];
                        outputTable[i, j] = outputTable[i - 1, j];
                        PredsString[i, j] = PredsString[i - 1, j];
                        continue;
                    }
                    int RealDis = FindDistance(NextMin, GetButtonByName(outputTable[0, j]));
                    if (RealDis == INF) {
                        intDistance[i, j] = intDistance[i - 1, j];
                        outputTable[i, j] = outputTable[i - 1, j];
                        PredsString[i, j] = PredsString[i - 1, j];
                        continue;
                    }
                    int dis = RealDis + RealMin;
                    if (intDistance[i - 1, j] < dis) {
                        intDistance[i, j] = intDistance[i - 1, j];
                        outputTable[i, j] = intDistance[i - 1, j].ToString();
                        PredsString[i, j] = PredsString[i - 1, j];
                    }
                    else if (intDistance[i - 1, j] > dis) {
                        intDistance[i, j] = dis;
                        outputTable[i, j] = intDistance[i, j].ToString();
                        PredsString[i, j] += NextMin.Content.ToString();
                    }
                    else {
                        intDistance[i, j] = intDistance[i - 1, j];
                        outputTable[i, j] = intDistance[i - 1, j].ToString();
                        PredsString[i, j] = PredsString[i - 1, j] + "," + NextMin.Content.ToString();

                    }

                }
                AlreadyMarked.Add(NextMin);
            }
            Print();
            buttons.Add(AlgoStart);
        }

        private int GetRealMin(int line, Button B) {
            for (int i = 2; i <= size; i++) {
                if (B.Content.ToString() == outputTable[0, i]) {
                    return intDistance[line, i];
                }
            }
            return 0;
        }
        private List<Button> AlreadyMarked = new List<Button>();

        public int actualHeight;
        public bool VerifyINF(int line) {
            int num = buttons.Count - AlreadyMarked.Count + 1;
            int count = 0;
            for (int j = 1; j <= size; j++) {
                if (outputTable[line, j] == "INF" && !AlreadyMarked.Contains(GetButtonByName(outputTable[0, j]))) {
                    count++;
                }
            }
            if (count == num) {
                return true;
            }
            return false;



        }
        private Button GetNextMin(int line) {
            int min = 0;
            Button b = null;
            for (int i = 2; i <= size; i++) {
                if (AlreadyMarked.Contains(GetButtonByName(outputTable[0, i]))) {
                    continue;
                }
                min = intDistance[line, i];
                b = GetButtonByName(outputTable[0, i]);
            }

            for (int i = 2; i <= size; i++) {
                if (AlreadyMarked.Contains(GetButtonByName(outputTable[0, i]))) {
                    continue;
                }
                if (intDistance[line, i] < min) {
                    min = intDistance[line, i];
                    b = GetButtonByName(outputTable[0, i]);
                }
            }
            return b;
        }

        private void Print() {
            Canvas canv1 = (Canvas)this.FindName("TextB");

            GridDisplay grid = new GridDisplay("Dijktra", this.actualHeight + 1, this.size + 1);

            for (int i = 0; i <= this.actualHeight; i++) {
                for (int j = 0; j <= size; j++) {
                    if (i == 0 || j == 0) {
                        grid.AddCell(i, j, new Run(outputTable[i, j]) { FontWeight = FontWeights.Bold }, null);
                    }
                    else {
                        if (j >= 2) {
                            grid.AddCell(i, j, new Run(outputTable[i, j]),
                                new Run(PredsString[i, j]) {
                                    BaselineAlignment
                            = BaselineAlignment.Subscript,
                                    FontSize = 12
                                });

                        }
                        else {
                            grid.AddCell(i, j, new Run(outputTable[i, j]), null);
                        }
                    }
                }
            }
            Grid g = grid.getGrid();


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
                        if (outputTable[actualHeight, j] == "INF") {

                            return;
                        }
                        if (PredsString[actualHeight, j].Contains(",")) {
                            if (PredsString[actualHeight, j].ToString()
                                .Substring(0, PredsString[actualHeight, j].ToString().IndexOf(","))
                                == AlgoStart.Content.ToString()) {

                                result.Add(AlgoStart.Content.ToString());
                                return;
                            }
                            next = PredsString[actualHeight, j].ToString()
                                .Substring(0, PredsString[actualHeight, j].ToString().IndexOf(","));
                            result.Add(next);
                        }
                        else {
                            if (PredsString[actualHeight, j].ToString() == AlgoStart.Content.ToString()) {

                                result.Add(AlgoStart.Content.ToString());
                                return;
                            }
                            next = PredsString[actualHeight, j].ToString();
                            result.Add(next);
                        }

                    }

                }
            }

        }
        public void ColorPath(ref Button algoEnd) {
            if (result.Count == 1) {
                MessageBox.Show("Unreachable!", "Alert", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                    }
                }
            }

        }


        public void RestoreColor() {
            foreach (Relation R in relations) {
                R.RestoreArrowColor();
            }

        }

        public void Window_Closed(object sender, EventArgs e) {
            MainWindow wnd = (MainWindow)Application.Current.MainWindow;
            wnd.AlgoAftermath();

        }
    }
}
