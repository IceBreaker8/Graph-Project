using Graph.Serialization;
using Graph.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using templateGraph;

namespace Graph.GraphAlgorithms.PERT {
    class VertexTupleBox {

        private double topAddon = 25;
        private MainWindow mainWindow;
        private Grid g;
        private Button button;
        public VertexTupleBox(Button button, MainWindow mainWindow, long x = 0, long y = 0) {
            Pos pos = GetLocation(button);
            this.button = button;
            Pos boxLoc = new Pos(pos.a, pos.b - topAddon, pos.c, pos.d);
            this.mainWindow = mainWindow;


            mainWindow.Canv.Children.Add(MakeGrid(boxLoc, x, y));


        }

        public void RemoveTuple() {
            if (mainWindow.Canv.Children.Contains(g))
                mainWindow.Canv.Children.Remove(g);
        }

        public void UpdatePosition() {
            Pos pos = GetLocation(button);
            g.Margin = new Thickness(pos.a, pos.b - topAddon, pos.c, pos.d);

        }
        public Grid MakeGrid(Pos pos, long x, long y) {
            GridDisplay grid = new GridDisplay("", 1, 2);
            grid.AddCell(0, 0, new Run(x.ToString()) {
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.Blue
            }, null);
            grid.AddCell(0, 1, new Run(y.ToString()) { 
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.Red 
            }, null);
            g = grid.getGrid();
            g.Margin = new Thickness(pos.a, pos.b, pos.c, pos.d);
            return g;
        }
        public Pos GetLocation(Button button) {
            Pos relativePoint = new Pos(button.Margin.Left, button.Margin.Top,
                button.Margin.Right, button.Margin.Bottom);
            return relativePoint;
        }
    }
}
