using Graph.Controllers;
using Graph.Controllers.AlgorithmController;
using Graph.GraphAlgorithms.PERT;
using Graph.LinkManager;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using templateGraph.LinkManager;

namespace templateGraph {
    [Serializable]
    public class Relation {

        private Window w;
        //Polygon arrow
        private Polygon polygon;
        private Brush Color = Brushes.Black;
        private Brush BorderColor = Brushes.White;


        //Polygon textblock location
        private Point Tloc;




        //relations attributes
        public bool Constarted = false;
        public bool isConnected = false;
        public Button ConStart;
        public Button ConEnd;
        private int transition;
        private Point StartLocation;
        private Point EndLocation;

        //canvas
        private Canvas Canv;
        public Point p2;

        //textblock
        private TextBlock T;
        //caracteristics
        private Brush TextColor = Brushes.Gray;
        private FontWeight TextFontWeight = FontWeights.Bold;
        private int TextFontSize = 13;


        //constructor
        public Relation(Button b1) {
            this.ConStart = b1;
            this.Constarted = true;

        }
        public int getTransition() {
            return this.transition;
        }
        public void DestructRelation() {
            if (polygon != null) {
                this.Canv.Children.Remove(this.polygon);
            }
            if (T != null) {
                this.Canv.Children.Remove(this.T);
            }
            if (myPath != null) {
                this.Canv.Children.Remove(this.myPath);
            }
        }
        public Button GetStartButton() {
            return this.ConStart;
        }
        public Button GetEndButton() {
            return this.ConEnd;
        }

        public enum LinkType {

            DirectedArrow,
            CurvedArrow,
            UndirectedArrow

        }
        public LinkType linkType;
        public void StartConnection(Button b2, Window MainWindow, Canvas Canv,
            int transition, LinkType linkType) {
            this.linkType = linkType;
            this.transition = transition;
            this.Canv = Canv;
            this.w = MainWindow;
            this.ConEnd = b2;
            this.isConnected = true;
            if (linkType == LinkType.CurvedArrow) {
                FixPoints();
                new CurvedArrow().MakeCurve(ref StartLocation, ref EndLocation, ref myPath
            , ref p2, ref Canv, ref Tloc, ref Q, ref P, ref PF, ref Pa);
                CreateTextOnArrow();
                CreateArrow("yes");
                UpdateCurve();
            } else if (linkType == LinkType.DirectedArrow) {
                CreateArrow("yes");

            } else {
                CreateArrow("yes");
            }
        }


        public Point GetLocation(Button b) {
            Point relativePoint = b.TransformToAncestor(w)
                          .Transform(new Point(0, 0));
            return relativePoint;
        }

        public Point CircleCenter(Button b) {
            Point p1 = GetLocation(b);
            Point p2 = new Point(-25, -6); // X = -25   Y = -6
            Vector V = Point.Subtract(p1, p2);
            Point result = new Point();
            result.X = V.X;
            result.Y = V.Y;
            return result;
        }



        public void FixPoints() {
            Vector direction = GetLocation(ConStart) - GetLocation(ConEnd);
            direction.Normalize();
            //start
            this.StartLocation = CircleCenter(ConStart);
            this.StartLocation.X -= (direction * 25).X;
            this.StartLocation.Y -= (direction * 25).Y;
            //end
            this.EndLocation = CircleCenter(ConEnd);
            this.EndLocation.X += (direction * 25).X;
            this.EndLocation.Y += (direction * 25).Y;
        }

        public void UpdatePolygon() {
            this.Canv.Children.Remove(this.polygon);
            CreateArrow("no");
        }
        public void CreateArrow(String s) {
            if (linkType != LinkType.CurvedArrow) {
                FixPoints();
            }
            if (linkType == LinkType.UndirectedArrow) {
                var points = new LineUnd().createArrow(ref EndLocation, ref Tloc, ref p2,
                    ref StartLocation, ref EndLocation, 3);
                this.polygon = new Polygon();
                this.polygon.Points = points;
                this.polygon.Fill = Color;
                this.polygon.IsHitTestVisible = true;

                this.Canv.Children.Add(this.polygon);
                Canvas.SetZIndex(polygon, 0);
                if (linkType != LinkType.CurvedArrow) {
                    if (s == "yes") {
                        CreateTextOnArrow();
                    }
                    UpdateTextOnArrow();
                }
            } else {
                var points = new Arrow().createArrow(ref EndLocation, ref linkType, ref Tloc, ref p2,
                    ref StartLocation, ref EndLocation, 3);
                this.polygon = new Polygon();
                this.polygon.Points = points;
                this.polygon.Fill = Color;
                this.polygon.IsHitTestVisible = true;

                this.Canv.Children.Add(this.polygon);
                Canvas.SetZIndex(polygon, 0);
                if (linkType != LinkType.CurvedArrow) {
                    if (s == "yes") {
                        CreateTextOnArrow();
                    }
                    UpdateTextOnArrow();
                }
            }



        }







        private void CreateTextOnArrow() {
            T = new TextBlock();
            T.Text = this.transition.ToString();
            T.Foreground = TextColor;
            T.FontSize = TextFontSize;
            T.FontWeight = TextFontWeight;

            Canvas.SetLeft(T, Tloc.X);
            Canvas.SetTop(T, Tloc.Y);
            Canv.Children.Add(T);
            Panel.SetZIndex(T, 999);
            UpdateTextOnArrow();
            AddEvents();

        }

        private void UpdateTextOnArrow() {
            T.Text = this.transition.ToString();
            Canvas.SetLeft(T, Tloc.X);
            Canvas.SetTop(T, Tloc.Y);
        }

        public void ColorArrow(SolidColorBrush color) {
            this.Color = color;
            UpdatePolygon();
            if (myPath != null) {
                myPath.Stroke = Color;
            }
        }

        public void RestoreArrowColor() {
            this.Color = Brushes.Black;
            UpdatePolygon();
            if (myPath != null) {
                myPath.Stroke = Color;
            }
        }


        BezierSegment Q;
        PathSegmentCollection P;
        PathFigureCollection PF;
        PathFigure Pa;
        Path myPath;

        private Point GetMidLoc(Point startPoint, Point endPoint) {
            Vector v = startPoint - endPoint;

            v.Normalize();
            v.X = -v.X;
            v *= 60;
            //-
            double x1 = (endPoint.X - startPoint.X) / 2 + startPoint.X;
            double y1 = (endPoint.Y - startPoint.Y) / 2 + startPoint.Y;
            Tloc = new Point(x1, y1);
            Tloc.X += v.X;
            Tloc.Y += v.Y;
            Point n = new Point(Tloc.X, Tloc.Y);
            n.X += v.X;
            n.Y += v.Y + 30;
            return n;

        }


        public void UpdateCurve() {
            FixPoints();
            Q.Point1 = GetMidLoc(StartLocation, EndLocation);
            Q.Point2 = p2;
            Q.Point3 = EndLocation;
            myPath.Stroke = Color;
            myPath.StrokeThickness = 1;
            Pa.StartPoint = StartLocation;
            UpdateTextOnArrow();

        }

        // arrow event

        private void AddEvents() {
            T.MouseEnter += OnMouseEnterArrow;
            T.MouseLeave += OnMouseLeaveArrow;
            T.MouseRightButtonDown += OnMouseRightClickButtonDown;
        }

        private void OnMouseEnterArrow(object sender, RoutedEventArgs e) {
            if (PertAlgorithm.PertRunning || AlgoController.algorunning || AlgoController.AlgoStarted) {
                return;
            }
            polygon.Fill = Brushes.Red;
            if (myPath != null)
                myPath.Stroke = Brushes.Red;
            foreach (Relation r1 in MainWindow.Relations) {
                if (r1.polygon == polygon) {
                    foreach (Relation r2 in MainWindow.Relations) {
                        if (r2.ConStart == r1.ConEnd && r1.ConStart == r2.ConEnd) {
                            r2.polygon.Fill = Brushes.Red;
                        }
                    }
                }
            }
        }

        private void OnMouseLeaveArrow(object sender, RoutedEventArgs e) {
            if (PertAlgorithm.PertRunning || AlgoController.algorunning || AlgoController.AlgoStarted) {
                return;
            }
            polygon.Fill = Brushes.Black;
            if (myPath != null)
                myPath.Stroke = Brushes.Black;
            foreach (Relation r1 in MainWindow.Relations) {
                if (r1.polygon == polygon) {
                    foreach (Relation r2 in MainWindow.Relations) {
                        if (r2.ConStart == r1.ConEnd && r1.ConStart == r2.ConEnd) {
                            r2.polygon.Fill = Brushes.Black;
                        }
                    }
                }
            }
        }

        public static ContextMenu ArrowMenu = new ContextMenu();
        private void OnMouseRightClickButtonDown(object sender, RoutedEventArgs e) {
            ArrowMenu = new ContextMenu();
            //Change Transition
            MenuItem transition = new MenuItem();
            transition.Header = "Change Transition";
            transition.Click += TransitionClickEvent;
            //Delete Relation items
            MenuItem delete = new MenuItem();
            delete.Header = "Delete Relation";
            delete.Click += DeleteClickEvent;

            ArrowMenu.Items.Add(transition);
            ArrowMenu.Items.Add(delete);

            //disable main context menu
            CanvasAndClickCont.CanvMenu.IsOpen = false;
            ArrowMenu.IsOpen = true;


        }
        private void TransitionClickEvent(object sender, RoutedEventArgs e) {
            if (AlgoController.AlgoStarted) {
                MessageBox.Show("You can't change the transition while an algorithm is running!");
                return;
            }
            InputBox inputDialog = new InputBox("Please Change Transition Number:");
            if (inputDialog.ShowDialog() == true && inputDialog.Answer != "") {
                this.transition = Int32.Parse(inputDialog.Answer);
                UpdateTextOnArrow();
                if (this.linkType == LinkType.UndirectedArrow) {
                    foreach (Relation r in MainWindow.Relations) {
                        if (r.ConStart == this.ConStart && r.ConEnd == this.ConEnd
                            || r.ConEnd == this.ConStart && r.ConStart == this.ConEnd) {
                            r.transition = Int32.Parse(inputDialog.Answer);
                            r.UpdateTextOnArrow();
                            break;
                        }
                    }
                }
            }
        }
        private void DeleteClickEvent(object sender, RoutedEventArgs e) {
            if (AlgoController.AlgoStarted) {
                MessageBox.Show("You can't delete this relation while an algorithm is running!");
                return;
            }
            DestroyRelation(this);
            this.DestructRelation();
            if (this.linkType == LinkType.UndirectedArrow) {
                foreach (Relation r in MainWindow.Relations) {
                    if (r.ConStart == this.ConStart && r.ConEnd == this.ConEnd
                        || r.ConEnd == this.ConStart && r.ConStart == this.ConEnd) {
                        DestroyRelation(r);
                        r.DestructRelation();
                        break;
                    }
                }
            }

        }
        public void DestroyRelation(Relation relation) {
            MainWindow.Relations.Remove(relation);
        }
    }
}


