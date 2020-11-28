using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using templateGraph.LinkManager;

namespace templateGraph.LinkManager {
    class CurvedArrow {

        public void MakeCurve(ref Point StartLocation, ref Point EndLocation, ref Path myPath
            , ref Point p2, ref Canvas Canv, ref Point Tloc, ref BezierSegment Q,
            ref PathSegmentCollection P, ref PathFigureCollection PF, ref PathFigure Pa) {
            //Fix points
            
            //
            //
            myPath = new Path();
            myPath.Stroke = Brushes.Black;
            myPath.StrokeThickness = 1;

            //
            Q = new BezierSegment();


            Q.Point1 = GetMidLoc(ref StartLocation, ref EndLocation, ref Tloc);
            Q.Point2 = p2;
            Q.Point3 = EndLocation;
            //
            P = new PathSegmentCollection();
            P.Add(Q);
            Pa = new PathFigure();
            Pa.StartPoint = StartLocation;
            Pa.Segments = P;
            //
            PF = new PathFigureCollection();
            PF.Add(Pa);
            //
            PathGeometry Pg = new PathGeometry {
                Figures = PF
            };

            //
            myPath.Data = Pg;
            Canv.Children.Add(myPath);


        }



        public Point GetMidLoc(ref Point startPoint, ref Point endPoint, ref Point Tloc) {
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

    }
}
