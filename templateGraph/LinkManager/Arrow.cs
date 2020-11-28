using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace templateGraph.LinkManager {
    class Arrow {
        private const double _maxArrowLengthPercent = 0.4; // factor that determines how the arrow is shortened for very short lines
        private const double _lineArrowLengthFactor = 2.41; // 15 degrees arrow:  = 1 / Math.Tan(15 * Math.PI / 180); 

        public PointCollection createArrow(ref Point EndLocation, ref bool CurveMade,
            ref Point Tloc, ref Point p2, ref Point startPoint, ref Point endPoint, double lineWidth) {
            if (CurveMade) {

                Vector d = endPoint - Tloc;

                Vector nd = d;
                nd.Normalize();

                Vector normalizedlineWidenVector = new Vector(-nd.Y, nd.X); // Rotate by 90 degrees
                Vector lineWidenVector = normalizedlineWidenVector * lineWidth * 0.18;

                double lineLength = d.Length;

                double defaultArrowLength = (lineWidth + 2) * _lineArrowLengthFactor;

                // Prepare usedArrowLength
                // if the length is bigger than 1/3 (_maxArrowLengthPercent) of the line length adjust the arrow length to 1/3 of line length

                double usedArrowLength;
                if (lineLength * _maxArrowLengthPercent < defaultArrowLength)
                    usedArrowLength = lineLength * _maxArrowLengthPercent;
                else
                    usedArrowLength = defaultArrowLength;

                // Adjust arrow thickness for very thick lines
                double arrowWidthFactor;
                if (lineWidth <= 1.5)
                    arrowWidthFactor = 3;
                else if (lineWidth <= 2.66)
                    arrowWidthFactor = 4;
                else
                    arrowWidthFactor = 1.5 * lineWidth;

                Vector arrowWidthVector = normalizedlineWidenVector * arrowWidthFactor * 1.4;


                // Now we have all the vectors so we can create the arrow shape positions
                var pointCollection = new PointCollection(5);

                Point endArrowCenterPosition = endPoint - (nd * usedArrowLength);
                //
                pointCollection.Add(endPoint);
                pointCollection.Add(endArrowCenterPosition + arrowWidthVector);
                pointCollection.Add(endArrowCenterPosition + lineWidenVector);
                pointCollection.Add(endArrowCenterPosition - lineWidenVector);
                pointCollection.Add(endArrowCenterPosition - arrowWidthVector);

                p2 = GetThirdPoint(EndLocation, Tloc, 8);

                return pointCollection;
            }
            else {

                Vector direction = endPoint - startPoint;

                Vector normalizedDirection = direction;
                normalizedDirection.Normalize();

                Vector normalizedlineWidenVector = new Vector(-normalizedDirection.Y, normalizedDirection.X); // Rotate by 90 degrees
                Vector lineWidenVector = normalizedlineWidenVector * lineWidth * 0.18;

                double lineLength = direction.Length;

                double defaultArrowLength = (lineWidth + 2) * _lineArrowLengthFactor;

                // Prepare usedArrowLength
                // if the length is bigger than 1/3 (_maxArrowLengthPercent) of the line length adjust the arrow length to 1/3 of line length

                double usedArrowLength;
                if (lineLength * _maxArrowLengthPercent < defaultArrowLength)
                    usedArrowLength = lineLength * _maxArrowLengthPercent;
                else
                    usedArrowLength = defaultArrowLength;

                // Adjust arrow thickness for very thick line
                double arrowWidthFactor;
                if (lineWidth <= 1.5)
                    arrowWidthFactor = 3;
                else if (lineWidth <= 2.66)
                    arrowWidthFactor = 4;
                else
                    arrowWidthFactor = 1.5 * lineWidth;

                Vector arrowWidthVector = normalizedlineWidenVector * arrowWidthFactor * 1.4;


                // Now we have all the vectors so we can create the arrow shape positions
                var pointCollection = new PointCollection(7);

                Point endArrowCenterPosition = endPoint - (normalizedDirection * usedArrowLength);

                pointCollection.Add(endPoint); // Start with tip of the arrow
                pointCollection.Add(endArrowCenterPosition + arrowWidthVector);
                pointCollection.Add(endArrowCenterPosition + lineWidenVector);
                pointCollection.Add(startPoint + lineWidenVector);
                pointCollection.Add(startPoint - lineWidenVector);
                pointCollection.Add(endArrowCenterPosition - lineWidenVector);
                pointCollection.Add(endArrowCenterPosition - arrowWidthVector);
                Tloc = getTLoc(startPoint, endPoint);




                return pointCollection;




            }



        }
        public Point GetThirdPoint(Point Tloc, Point endPoint, int width) {
            Vector direction = Tloc - endPoint;
            direction.Normalize();
            direction.X += width;
            direction.Y += width;
            return new Point(endPoint.X + direction.X, endPoint.Y + direction.Y);

        }
        public Point getTLoc(Point startPoint, Point endPoint) {
            if (startPoint.X >= endPoint.X && startPoint.Y <= endPoint.Y) {

                double x = (startPoint.X - endPoint.X) / 2 + endPoint.X;
                double y = (endPoint.Y - startPoint.Y) / 2 + startPoint.Y;
                return new Point(x, y);
            }
            else if (endPoint.X >= startPoint.X && endPoint.Y >= startPoint.Y) {

                double x = (endPoint.X - startPoint.X) / 2 + startPoint.X;
                double y = (endPoint.Y - startPoint.Y) / 2 + startPoint.Y;
                return new Point(x, y);


            }
            else if (endPoint.X >= startPoint.X && startPoint.Y >= endPoint.Y) {
                double x = (endPoint.X - startPoint.X) / 2 + startPoint.X;
                double y = (startPoint.Y - endPoint.Y) / 2 + endPoint.Y;
                return new Point(x, y);
            }
            else if (startPoint.X >= endPoint.X && startPoint.Y >= endPoint.Y) {
                double x = (startPoint.X - endPoint.X) / 2 + endPoint.X;
                double y = (startPoint.Y - endPoint.Y) / 2 + endPoint.Y;
                return new Point(x, y);
            }
            return new Point();

        }

    }


}
