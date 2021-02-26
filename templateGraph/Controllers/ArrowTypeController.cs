using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using templateGraph;

namespace Graph.Controllers {
    class ArrowTypeController {


        //Menuitems 
        public static MenuItem DirectedArrow;
        public static MenuItem UndirectedArrow;



        public ArrowTypeController() {
            DirectedArrow = MainWindow.main.FindName("DirectedArrow") as MenuItem;
            UndirectedArrow = MainWindow.main.FindName("UndirectedArrow") as MenuItem;
            
            SaveEvents();

        }

        private void SaveEvents() {
            DirectedArrow.Click += DirectedArrowEvent;
            UndirectedArrow.Click += UndirectedArrowEvent;
            DirectedArrow.IsCheckable = false;
        }



        private void DirectedArrowEvent(object sender, RoutedEventArgs e) {
            UndirectedArrow.IsCheckable = false;
            if (UndirectedArrow.IsChecked) {
                UndirectedArrow.IsChecked = false;
                DirectedArrow.IsChecked = true;
            }
            
            
        }

        private void UndirectedArrowEvent(object sender, RoutedEventArgs e) {
            DirectedArrow.IsCheckable = false;
            if (DirectedArrow.IsChecked) {
                DirectedArrow.IsChecked = false;
                UndirectedArrow.IsChecked = true;
            }
            

        }

    }
}