using Graph.Controllers.AlgorithmController;
using Graph.GraphAlgorithms.PERT;
using Graph.Utils;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using templateGraph;

namespace Graph.Controllers {
    class CanvasAndClickCont {

        private MainWindow mainWindow;


        //MenuItems
        public static MenuItem Creation;
        public static MenuItem CancelAlgo;

        public static Dictionary<Button, VertexTupleBox> butDict =
            new Dictionary<Button, VertexTupleBox>();
        /* =================================== Button events ================================================ */
        public static Button StartButton;
        //button
        public static int maxButtonNameLength = 6;
        //other
        public static Control draggedItem;
        public static Point ItemRelativePosition;
        public static bool IsDragging;
        public static Point CreationCoords; // button creation coords

        public static bool didStart = false;
        public static Button VertexMenuButton;
        //Menus
        public static ContextMenu CanvMenu;
        public static ContextMenu VertexMenu;


        public static Relation firstRelation;

        public CanvasAndClickCont(MainWindow mainWindow, bool pass) {

            this.mainWindow = mainWindow;

            IsDragging = false;


            VertexMenu = mainWindow.FindResource("Vertex") as ContextMenu;
            CanvMenu = mainWindow.FindResource("cmButton") as ContextMenu;

            Creation = CanvMenu.Items[0] as MenuItem; // create button menu item
            CancelAlgo = CanvMenu.Items[1] as MenuItem;
            if (pass) {
                return;
            }

            Creation.Click += CreateButtonMenu;
            CancelAlgo.Click += AlgoCancel;
            mainWindow.Canv.MouseRightButtonDown += CanvMouseRightButtonUp;

        }

        public void ClearCanvasColor() {
            ColorCanvas(EAlgoMode.OFF);
            AlgoController.BellmanOnOnce = false;
            AlgoController.DijktraOnOnce = false;
            AlgoController.AlgoStarted = false;
            AlgoController.algorunning = false;

        }
        private void AlgoCancel(object sender, RoutedEventArgs e) {
            if (AlgoController.BellmanOnOnce || AlgoController.DijktraOnOnce) {
                ClearCanvasColor();

            } else if (PertAlgorithm.PertRunning) {
                ClearCanvasColor();
                foreach (Relation r in MainWindow.Relations) {
                    r.RestoreArrowColor();
                }
                PertAlgorithm.RemoveAlgorithmBoxes();
                PertAlgorithm.PertRunning = false;
            } else if (AlgoController.F != null) {
                AlgoController.F.Close();
            } else if (AlgoController.D != null) {
                AlgoController.D.Close();
            } else if (AlgoController.B != null) {
                AlgoController.B.Close();
            } else if (doubleClickOn) {
                ThrowVertexConnectionError();
            } else {
                MessageBox.Show("There are no algorithms to cancel!", "Alert",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }


        }

        private void CanvMouseRightButtonUp(object sender, MouseButtonEventArgs e) {

            if (Relation.ArrowMenu.IsOpen) {
                CanvMenu.IsOpen = false;
                return;
            }
            if (mainWindow.Vertices.Count != 0) {
                foreach (Button b in mainWindow.Vertices) {
                    if (b.IsMouseOver || b.IsMouseDirectlyOver) {
                        VertexMenu.IsOpen = true;
                        VertexMenuButton = b;
                        return;
                    }
                }
            }

            CreationCoords = e.GetPosition(mainWindow.Canv);
            CanvMenu.IsOpen = true;




        }
        public void CreateButton(Point location, string ButtonName) {
            Button newBtn = new Button();
            newBtn.Style = (Style)(mainWindow.Resources["CircleButton"]);
            newBtn.Width = 50;
            newBtn.Height = 50;
            newBtn.Content = ButtonName;
            //newBtn.FontWeight = FontWeights.Bold;
            newBtn.Margin = new Thickness(location.X, location.Y, 0, 0);
            newBtn.Background = Brushes.White;

            //right and left click
            newBtn.MouseDoubleClick += VertexMouseLeftDoubleClick;
            newBtn.MouseRightButtonUp += VertexRightClick;
            //dragging
            newBtn.PreviewMouseLeftButtonDown += VertexLeftClickDown;
            newBtn.PreviewMouseMove += VertexMouseDrag;
            newBtn.PreviewMouseLeftButtonUp += VertexMouseLeftClickUp;
            newBtn.IsHitTestVisible = true;
            //adding button
            mainWindow.Canv.Children.Add(newBtn);
            mainWindow.Vertices.Add(newBtn);
            // TODO

        }




        enum Mode {
            auto,
            restore
        }
        private void ThrowVertexConnectionError() {
            StartButton = null;
            didStart = false;
            doubleClickOn = false;
            AlgoController.AlgoStarted = false;
            MainWindow.Relations.Remove(firstRelation);
            firstRelation = null;
            SelectOtherButtons(null, Mode.restore);
            IsDragging = false;

            ColorCanvas(EAlgoMode.OFF);
        }

        public void ColorCanvas(EAlgoMode algomode) {
            if (algomode == EAlgoMode.ON)
                mainWindow.Canv.Background = new SolidColorBrush(Colors.LightGray);
            else
                mainWindow.Canv.Background = new SolidColorBrush(Colors.White);
        }
        private bool DoesExist(String s) {
            foreach (Button b in mainWindow.Vertices)
                if ((String)b.Content == s)
                    return true;
            return false;
        }
        private int VerticesConNum(Button b) {
            int num = 0;
            foreach (Relation R in MainWindow.Relations) {
                if (R.ConStart == b)
                    num++;
            }
            return num;
        }
        private void SelectOtherButtons(Button button, Mode Agree) {
            bool verif = false;
            if (Agree == Mode.auto) {
                foreach (Button b in mainWindow.Vertices) {
                    verif = false;
                    if (button == b) {
                        continue;
                    }

                    foreach (Relation r in MainWindow.Relations) {
                        if (r.ConStart == button && r.ConEnd == b) {
                            verif = true;
                            break;
                        }
                    }
                    if (verif == false) {
                        b.Background = Brushes.Gray;
                    }

                }
            } else if (Agree == Mode.restore) {
                foreach (Button b in mainWindow.Vertices) {

                    b.Background = Brushes.White;
                }
            }
        }
        private void VertexMouseLeftClickUp(object sender, MouseButtonEventArgs e) {
            if (l != null) {
                l.RemoveDashedLine();
            }
            if (!IsDragging)
                return;

            IsDragging = false;

        }
        private int GetMaxCon() {
            return mainWindow.Vertices.Count * (mainWindow.Vertices.Count - 1);
        }
        private void VertexMouseLeftDoubleClick(object sender, RoutedEventArgs e) {

            if (AlgoController.algorunning || AlgoController.AlgoStarted) {
                MessageBox.Show("You can't link while an algorithm is running!", "Alert",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (mainWindow.Vertices.Count < 2 || GetMaxCon() == MainWindow.Relations.Count) {
                MessageBox.Show("There are no other button to link to!", "Alert",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                IsDragging = false;
                return;
            }
            if (VerticesConNum((Button)sender) == mainWindow.Vertices.Count - 1) {
                MessageBox.Show("You can't make more connections!", "Alert",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                IsDragging = false;
                return;
            }

            ///color canvas
            AlgoController.AlgoStarted = true;

            Button b = (Button)sender;
            StartButton = b;

            firstRelation = new Relation(b);
            didStart = true;
            doubleClickOn = true;
            MainWindow.Relations.Add(firstRelation);
            SelectOtherButtons(b, Mode.auto);
            IsDragging = false;

        }
        private bool doubleClickOn = false;
        public static DashLine l;
        private void VertexMouseDrag(object sender, MouseEventArgs e) {
            if (l != null) {
                l.RemoveDashedLine();
            }
            if (!IsDragging)
                return;
            Button b = (Button)sender;
            if (MainWindow.Relations.Count != 0) {
                UpdateRelations(b);
            }
            Point buttonRelativePosition = e.GetPosition(b);

            if (e.LeftButton == MouseButtonState.Pressed) {
                bool horizontal = Keyboard.IsKeyDown(Key.H);
                bool vertical = Keyboard.IsKeyDown(Key.V);

                var margin = b.Margin;
                if (horizontal || !vertical) {
                    margin.Left = buttonRelativePosition.X + margin.Left - ItemRelativePosition.X;
                    if (horizontal) {
                        l = new DashLine(mainWindow);
                        if (l != null) {
                            Point relativePoint = b.TransformToAncestor(mainWindow)
                                  .Transform(new Point(0, 0));

                            Point start = new Point(0, b.Margin.Top + 23);
                            Point finish = new Point(b.Margin.Left, b.Margin.Top + 23);

                            Point begin = new Point(b.Margin.Left + 50, b.Margin.Top + 23);
                            Point end = new Point(b.Margin.Left + 2000, b.Margin.Top + 23);


                            l.DrawDashedLine(finish, start, end, begin);
                        }
                    }




                }
                if (vertical || !horizontal) {
                    margin.Top = buttonRelativePosition.Y + margin.Top - ItemRelativePosition.Y;
                    if (vertical) {
                        l = new DashLine(mainWindow);
                        if (l != null) {
                            Point relativePoint = b.TransformToAncestor(mainWindow)
                                  .Transform(new Point(0, 0));

                            Point start = new Point(b.Margin.Left + 25, 0);
                            Point finish = new Point(b.Margin.Left + 25, b.Margin.Top);

                            Point begin = new Point(b.Margin.Left + 25, b.Margin.Top + 50);
                            Point end = new Point(b.Margin.Left + 23, b.Margin.Top + 2000);


                            l.DrawDashedLine(finish, start, end, begin);
                        }

                    }
                }

                b.Margin = margin;
            }



            foreach (Button button in mainWindow.Vertices) {
                if (CanvasAndClickCont.butDict.ContainsKey(button))
                    CanvasAndClickCont.butDict[button].UpdatePosition();

            }
        }



        public void UpdateRelations(Button b) {
            foreach (Relation r in MainWindow.Relations) {
                if (r.ConStart == b || r.ConEnd == b)
                    if (r.linkType == Relation.LinkType.CurvedArrow) {
                        r.UpdateCurve();
                        r.UpdatePolygon();
                    } else {
                        r.UpdatePolygon();

                    }
            }
        }

        private void VertexRightClick(object sender, MouseButtonEventArgs e) {
            IsDragging = false;

        }
        private void VertexLeftClickDown(object sender, MouseButtonEventArgs e) {
            //is connection started


            if (StartButton == (Button)sender && didStart == true) {
                MessageBox.Show("You can't link a button to itself!", "Alert",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                ThrowVertexConnectionError();
                return;
            }
            if (((Button)sender).Background == Brushes.White && didStart == true) {
                MessageBox.Show("You are already connected to this vertex!", "Alert",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                ThrowVertexConnectionError();
                return;

            }

            if (MainWindow.Relations.Count != 0) {
                foreach (Relation r in MainWindow.Relations) {
                    if (r.Constarted == true && r.isConnected == false) {

                        InputBox inputDialog = new InputBox("Transition Number:");

                        if (inputDialog.ShowDialog() == true && inputDialog.Answer != "") {
                            try {
                                Int32.Parse(inputDialog.Answer);
                            } catch (Exception ex) {
                                MessageBox.Show(ex.Message, "Alert",
                                        MessageBoxButton.OK, MessageBoxImage.Warning);

                                ThrowVertexConnectionError();
                                return;
                            }
                            Button b = (Button)sender;
                            /* 
                             check if user wants directed arrow
                             
                             
                             */
                            if (CheckIfRelationExists(b, StartButton)) {
                                r.StartConnection(b, mainWindow,
                                    mainWindow.Canv, Int32.Parse(inputDialog.Answer), 
                                    Relation.LinkType.CurvedArrow);

                                r.isConnected = true;
                                didStart = false;
                                doubleClickOn = false;
                                SelectOtherButtons(null, Mode.restore);
                                AlgoController.AlgoStarted = false;
                                return;
                            } else {
                                r.StartConnection(b, mainWindow,
                                    mainWindow.Canv, Int32.Parse(inputDialog.Answer), 
                                    Relation.LinkType.DirectedArrow);

                                r.isConnected = true;
                                didStart = false;
                                doubleClickOn = false;
                                SelectOtherButtons(null, Mode.restore);
                                AlgoController.AlgoStarted = false;
                                return;
                            }

                        } else {

                            ThrowVertexConnectionError();
                            return;
                        }
                    }
                }
            }

            IsDragging = true;
            draggedItem = (Button)sender;
            ItemRelativePosition = e.GetPosition(draggedItem);



        }


        private bool CheckIfRelationExists(Button begin, Button end) {

            foreach (Relation R in MainWindow.Relations) {
                if (R.ConStart == begin && R.ConEnd == end || R.ConStart == end && R.ConEnd == begin) {
                    if (R.linkType != Relation.LinkType.CurvedArrow)
                        return true;
                }
            }
            return false;
        }
        private void CreateButtonMenu(object sender, RoutedEventArgs e) {
            string ButtonName;
            if (AlgoController.AlgoStarted || AlgoController.algorunning) {
                MessageBox.Show("You can't create a new vertex during an active algorithm!", "Alert",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            InputBox inputDialog = new InputBox("Vertex Name");
            if (inputDialog.ShowDialog() == true) {
                ButtonName = inputDialog.Answer;

                if (ButtonName.Length == 0) {
                    MessageBox.Show("You need to set a name to this vertex!", "Info",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                } else if (ButtonName.Length > maxButtonNameLength) {
                    MessageBox.Show("Your vertex name is too long (Max is " + maxButtonNameLength + ")!", "Info",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                    return;

                } else if (DoesExist(ButtonName)) {
                    MessageBox.Show("Button already exists!", "Info",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                } else {

                    //creating button
                    CreateButton(CreationCoords, ButtonName);

                }


            }
        }
    }
}
