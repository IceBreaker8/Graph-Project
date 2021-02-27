using Graph.Controllers.AlgorithmController;
using Graph.Serialization;
using Graph.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using templateGraph;


namespace Graph.Controllers {
    class FileController {

        private MenuItem New;
        private MenuItem Open;
        private MenuItem Save;
        private MenuItem Exit;
        private MainWindow mainWindow;

        public FileController(MainWindow mainWindow) {
            this.mainWindow = mainWindow;
            try {
                New = mainWindow.FindName("New") as MenuItem;
                Open = mainWindow.FindName("Open") as MenuItem;
                Save = mainWindow.FindName("Save") as MenuItem;
                Exit = mainWindow.FindName("Exit") as MenuItem;

            } catch (Exception e) {
                MessageBox.Show(e.Message, "Alert",
                    MessageBoxButton.OK, MessageBoxImage.Warning);

            }
            SaveEvents();

        }

        private void SaveEvents() {
            New.Click += New_Click;
            Save.Click += Save_Click;
            Open.Click += Open_Click;
            Exit.Click += Exit_Click;
            mainWindow.Closing += Window_Closing;
            mainWindow.KeyDown += Save_KeyDown;

        }

        private void Exit_Click(object sender, RoutedEventArgs e) {
            CloseAndCleanApp();
        }

        private void New_Click(object sender, RoutedEventArgs e) {
            NewData();
        }


        private void Save_Click(object sender, RoutedEventArgs e) {
            SaveData();

        }

        private void Open_Click(object sender, RoutedEventArgs e) {
            OpenData();

        }


        private GraphData SaveAllGraphData() { //stack all data into one object
            GraphData g = new GraphData(MainWindow.Relations, mainWindow.Vertices, mainWindow, mainWindow.Width,
                mainWindow.Height);
            return g;
        }

        private void clearCanvas() {
            MainWindow.Relations.Clear();
            mainWindow.Vertices.Clear();
            mainWindow.Canv.Children.Clear();
        }


        private void LoadAndSetVertices(GraphData objectToRead) {
            try {
                CanvasAndClickCont c = new CanvasAndClickCont(mainWindow, true);
                foreach (var b in objectToRead.getButtons()) {
                    c.CreateButton(new Point(b.Value.a, b.Value.b), b.Key);

                }
            } catch (Exception e) {
                MessageBox.Show(e.Message, "Alert",
                    MessageBoxButton.OK, MessageBoxImage.Warning);

            }

            mainWindow.UpdateLayout();
            //load relations
            List<List<string>> listToLoad = objectToRead.getMyList();

            foreach (var list in listToLoad) {

                Relation relation = new Relation(getButtonByName(list[0]));
                MainWindow.Relations.Add(relation);
                Relation.LinkType linkType;
                switch (list[2]) {
                    case "1":
                        linkType = Relation.LinkType.DirectedArrow;
                        break;
                    case "2":
                        linkType = Relation.LinkType.CurvedArrow;
                        break;
                    default:
                        linkType = Relation.LinkType.UndirectedArrow;
                        break;
                }
                
                relation.StartConnection(getButtonByName(list[1]), mainWindow, mainWindow.Canv,
                    Int32.Parse(list[3]), linkType);
                relation.isConnected = true;



            }

            mainWindow.UpdateLayout();
        }
        private Button getButtonByName(string name) {
            foreach (var button in mainWindow.Vertices) {
                if (button.Content.ToString().Equals(name)) {
                    return button;

                }
            }
            return null;
        }


        private void Save_KeyDown(object sender, KeyEventArgs e) {
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.S) {
                SaveData();
            } else if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.O) {
                OpenData();
            } else if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.N) {
                NewData();
            }
        }
        public bool AlreadySaved() {
            return SaveAllGraphData() == DataSaver.ReadFromBinaryFile<GraphData>(FileCreator.path);
        }

        /* ====================================== METHODS =======================================================*/

        private void SaveData() {
            if (AlgoController.AlgoStarted || AlgoController.algorunning) {
                MessageBox.Show("You can't save while an algorithm is running!", "Alert",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (FileCreator.path != null) {

                DataSaver.WriteToBinaryFile<GraphData>(FileCreator.path, SaveAllGraphData()); //save the data after its loaded
                return;
            }
            FileCreator.LoadDirAndCreateFile();

            if (FileCreator.path != null) {
                DataSaver.WriteToBinaryFile<GraphData>(FileCreator.path, SaveAllGraphData()); //save the data before loading (new save)
                MainWindow.main.Title = MainWindow.AppName + " - " + FileCreator.path;
                return;
            }
        }
        private void OpenData() {
            string copy = null;
            if (AlgoController.AlgoStarted || AlgoController.algorunning) {
                MessageBox.Show("You can't load a file while an algorithm is running!", "Alert",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            copy = FileCreator.path;

            FileCreator.LoadFile();
            if (FileCreator.path != null) {

                GraphData objectToRead = DataSaver.ReadFromBinaryFile<GraphData>(FileCreator.path);
                if (objectToRead != null) {
                    clearCanvas();
                    LoadAndSetVertices(objectToRead);
                    MainWindow.main.Title = MainWindow.AppName + " - " + FileCreator.path;

                } else {

                    FileCreator.path = copy;
                }

                
            }
            CanvasAndClickCont.doubleClickOn = false;

        }
        private void NewData() {
            if (AlgoController.AlgoStarted || AlgoController.algorunning) {
                MessageBox.Show("You can't create a new file while an algorithm is running!", "Alert",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (FileCreator.path != null) { // check if its already saved

                MessageBoxResult r = MessageBox.Show("Do you want to Save before creating new project?",
                    "Alert", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);

                if (r == MessageBoxResult.No) {
                    //do no stuff
                    MainWindow.main.Title = MainWindow.AppName;
                    clearCanvas();
                    FileCreator.path = null;
                    return;
                } else if (r == MessageBoxResult.Yes) {

                    DataSaver.WriteToBinaryFile<GraphData>(FileCreator.path, SaveAllGraphData());
                    clearCanvas();
                    MessageBox.Show("New File Created!",
                    "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    MainWindow.main.Title = MainWindow.AppName;
                    FileCreator.path = null;
                    return;
                } else {

                    return;
                }
                
            }
            if (mainWindow.Vertices.Count > 0) { //path is null
                MessageBoxResult r = MessageBox.Show("Do you want to Save before creating new project?",
                    "Alert", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);

                if (r == MessageBoxResult.No) {
                    //do no stuff
                    clearCanvas();
                    MessageBox.Show("New File Created!", "Info",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                } else if (r == MessageBoxResult.Yes) {
                    FileCreator.LoadDirAndCreateFile();

                    if (FileCreator.path != null) {
                        DataSaver.WriteToBinaryFile<GraphData>(FileCreator.path, SaveAllGraphData()); //save the data before loading (new save)
                                                                                                      //save the data before loading (new save)
                        clearCanvas();
                        MessageBox.Show("New file created!", "Info",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }

                } else {

                    return;
                }
            }
        }
        private void ExitData() {
            MessageBoxResult r = MessageBox.Show("Do you want to Save before exiting?",
                        "Alert", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);

            if (r == MessageBoxResult.No) {
                CloseAndCleanApp();
                return;
            } else if (r == MessageBoxResult.Yes) {
                SaveData();
                if (FileCreator.path != null) {
                    CloseAndCleanApp();
                } else {

                }
                return;
            }

        }
        private void CloseAndCleanApp() {
            App.Current.Shutdown();
            Process.GetCurrentProcess().Kill();
            if (AlgoController.F != null) {
                AlgoController.F.Close();
            }
            if (AlgoController.D != null) {
                AlgoController.D.Close();
            }
            if (AlgoController.B != null) {
                AlgoController.B.Close();
            }
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {

            MessageBoxResult r = MessageBox.Show("Do you want to Save before exiting?",
                    "Alert", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);

            if (r == MessageBoxResult.No) {
                CloseAndCleanApp();
                return;
            } else if (r == MessageBoxResult.Yes) {
                SaveData();
                if (FileCreator.path != null) {
                    CloseAndCleanApp();
                } else {
                    e.Cancel = true;
                }
                return;
            } else if (r == MessageBoxResult.Cancel) {
                e.Cancel = true;
            }

        }
    }

    /* ================ WINDOW CLOSING ===========================*/


}
