using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace templateGraph.InputOutputWindow {

    public partial class OutputText : Window {

        TextBlock txt;

        public OutputText(String title) {
            InitializeComponent();
            this.Title = title;
            txt = (TextBlock)this.FindName("TextB");

        }

        public void AddText(String text, FontWeight font, int FontSize) {
            txt.Inlines.Add(new Run(text) { FontWeight = font, FontSize = FontSize });



        }

        public void Print() {
            this.Show();
        }
    }
}
