using Graph.Controllers;
using Graph.Controllers.AlgorithmController;
using Graph.MongoDB;
using Graph.Updater;
using Graph.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace templateGraph {


    public partial class MainWindow : Window {


        public static bool TestActive = false;



        //buttons management here
        public List<Button> Vertices = new List<Button>();
        public static List<Relation> Relations = new List<Relation>();

        public static string AppName = "GraphICE";
        public static MainWindow main;



        public MainWindow() {


            //Maybe use splash screen?
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            main = this;
            CheckForUpdate();
            //centering window
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            //registering events
            InitializeEvents();



        }
        private void CheckForUpdate() {
            try {
                UpdateChecker.CheckForUpdate(false);
            } catch (Exception e) {
                MessageBox.Show(e.Message);
            }
        }
        private void InitializeEvents() {
            new HelpController(this);
            new FileController(this);
            new VertexRankController(this);
            new AlgoController(this);
            new ButtonController(this);
            new CanvasAndClickCont(this, false);
            new OrdonAlgorithm(this);
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e) {
            Task.Run(() => Connector.EstablishConnection());
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
            ColorCanvas(EAlgoMode.OFF);
        }


        public void ColorCanvas(EAlgoMode algomode) {
            if (algomode == EAlgoMode.ON)
                this.Canv.Background = new SolidColorBrush(Colors.LightGray);
            else
                this.Canv.Background = new SolidColorBrush(Colors.White);
        }


    }

}
