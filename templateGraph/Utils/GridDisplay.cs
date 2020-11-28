using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Graph.Utils {
    class GridDisplay {

        public static int minCanvWidthError = 15;
        public static int minCanvHeightError = 15;

        public static double widthError = 70.0d;
        public static double heightError = 80.0d;


        Grid MyGrid;

        
        private int rows;
        private int columns;
        public GridDisplay(String title, int rows, int columns) {
            MyGrid = new Grid();
            this.rows = rows;
            this.columns = columns;


            MyGrid.Margin = new Thickness(12, 12, 12, 12);

            for (int x = 0; x < columns; x++) {
                ColumnDefinition c = new ColumnDefinition();
                c.Width = GridLength.Auto;
                MyGrid.ColumnDefinitions.Add(c);
            }
            for (int y = 0; y < rows; y++) {
                RowDefinition r = new RowDefinition();
                r.Height = GridLength.Auto;

                MyGrid.RowDefinitions.Add(r);
            }
           
            
        }

        public string TableValue(int row, int column, int rows) {
            int i = row + column * rows;
            return ((TextBox)MyGrid.Children[i]).Text;
        }

        public Grid getGrid() {
            return this.MyGrid;
        }

        public void AddCell(int i, int j, Run run1, Run run2) {
            TextBlock tb = new TextBlock();


            tb.FontSize = 16;
            tb.Inlines.Add(run1);
            if (run2 != null) {
                tb.Inlines.Add(run2);

            }

            tb.TextAlignment = TextAlignment.Center;
            tb.Margin = new Thickness(8, 0, 8, 0);


            Border B = new Border();
            B.Child = tb;
            B.BorderThickness = new Thickness(0.8);
            B.BorderBrush = Brushes.Black;

            Grid.SetColumn(B, j);
            Grid.SetRow(B, i);
            MyGrid.Children.Add(B);

        }
    }
}
