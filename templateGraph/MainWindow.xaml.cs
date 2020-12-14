using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Graph.Controllers;
using Graph.Controllers.AlgorithmController;

namespace templateGraph {


    public partial class MainWindow : Window {

        //buttons management here
        public List<Button> Vertices = new List<Button>();
        public static List<Relation> Relations = new List<Relation>();

        

        public MainWindow() {

            InitializeComponent();


            //centering window
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            //registering events
            new FileController(this);
            new VertexRankController(this);
            new AlgoController(this);
            new ButtonController(this);
            new CanvasAndClickCont(this, false);
            new OrdonAlgorithm(this);
        }

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
            ColorCanvas(AlgoMode.OFF);
        }
        public enum AlgoMode {
            ON, OFF
        }

        public void ColorCanvas(AlgoMode algomode) {
            if (algomode == AlgoMode.ON)
                this.Canv.Background = new SolidColorBrush(Colors.LightGray);
            else
                this.Canv.Background = new SolidColorBrush(Colors.White);
        }


    }

}
