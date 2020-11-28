
using Graph.Controllers;
using Graph.Serialization;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using templateGraph;
using templateGraph.VertexConnections;

namespace Graph.Vertex {
    class VertexPosManager {


        //maximum vertex position 735   344 
        //maximum window size => 800 450.4 
        //difference => 65 106.4


        private double windowWidth;
        private double windowHeight;

        private Visual visual;


        public VertexPosManager(Visual visual, double width, double height) {
            this.windowHeight = height - 106.4;
            this.windowWidth = width - 65;
            this.visual = visual;
        }

        public Pos GetLocation(Button button) {
            Pos relativePoint = new Pos(button.Margin.Left, button.Margin.Top,
                button.Margin.Right, button.Margin.Bottom);
            return relativePoint;
        }

        public void setLocation(Button button, Point location) {
            button.Margin = new Thickness(location.X, location.Y, 0, 0);


        }

        public void setLocationY(Button button, double y) {
            button.Margin = new Thickness(button.Margin.Left, y, 0, 0);


        }
        public void setLocationX(Button button, double x) {
            button.Margin = new Thickness(x, button.Margin.Top, 0, 0);


        }


        public void printSize() {
            MessageBox.Show(windowWidth + "  " + windowHeight);
        }

        public void setToBorder(Button button) {
            setLocation(button, new Point(windowWidth, windowHeight));
        }


        public void setAllVerticesPos(List<Button> vertices, List<Relation> relations) {
            VertexRank vr = new VertexRank(vertices, relations);
            vr.SortVerticesByRanks(ref vertices);
            Dictionary<Button, int> Ranks = vr.getRanks();
            //rank number
            int rankNum = vr.RankNumber();
            //width division
            double widthMultiplier = (windowWidth / (rankNum + 1));

            //setting buttons width positions
            foreach (var button in vertices) {
                double buttonWidthPos = widthMultiplier * (Ranks[button] + 1);
                setLocationX(button, buttonWidthPos);
            }
            //check each line of rank for how many buttons with the same rank
            for (int rank = 0; rank < rankNum; rank++) {
                List<Button> toArrange = new List<Button>();
                foreach (var item in Ranks) {
                    if (item.Value == rank) {
                        toArrange.Add(item.Key);
                    }
                }
                //number of buttons per rank line
                int numButtonPerLine = toArrange.Count;
                double heightMultiplier = (windowHeight / (numButtonPerLine + 1));
                for (int altitude = 0; altitude < toArrange.Count; altitude++) {
                    double buttonHeightPos = heightMultiplier * (altitude + 1);
                    setLocationY(toArrange[altitude], buttonHeightPos);
                }
            }
            //update boxes locations
            foreach (Button b in vertices) {
                if (CanvasAndClickCont.butDict.ContainsKey(b))
                    CanvasAndClickCont.butDict[b].UpdatePosition();

            }

        }

    }
}
