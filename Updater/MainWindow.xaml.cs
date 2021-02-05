using Graph.Updater;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Updater {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {



        public MainWindow() {
            InitializeComponent();

            
            System.IO.File.Delete(@".\Graph.exe");
            string zipPath = @".\Graph.zip";
            string extractPath = @".\";
            ZipFile.ExtractToDirectory(zipPath, extractPath);
            System.IO.File.Delete(@".\Graph.zip");
            this.Close();

            Process.Start(@"Graph.exe");
        }
        

    }
}
