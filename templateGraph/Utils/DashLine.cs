using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using templateGraph;

namespace Graph.Utils {


    class DashLine {

        public static Image image;
        private MainWindow mainwindow;
        public static bool IsDrawn = false;

        public DashLine(MainWindow main) {
            this.mainwindow = main;


        }
        public void DrawDashedLine(Point start, Point finish, Point begin, Point end) {
            RemoveDashedLine();
            DrawingVisual drawingVisual = new DrawingVisual();
            DrawingContext drawingContext = drawingVisual.RenderOpen();
            var pen = new Pen(new SolidColorBrush(Color.FromRgb(169, 169, 169)), 2);
            pen.DashStyle = DashStyles.Dash;
            drawingContext.DrawLine(pen, start, finish);
            drawingContext.DrawLine(pen, begin, end);


            drawingContext.Close();

            RenderTargetBitmap bmp = new RenderTargetBitmap(2000, 2000, 0, 0, PixelFormats.Pbgra32);
            bmp.Render(drawingVisual);

            image = new Image();
            image.Source = bmp;

            mainwindow.Canv.Children.Add(image);
        }

        public void RemoveDashedLine() {
            if (image != null) {
                IsDrawn = false;
                mainwindow.Canv.Children.Remove(image);
            }
        }

    }
}
