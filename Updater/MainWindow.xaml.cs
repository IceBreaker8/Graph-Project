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



        ProgressBar progressBar;

        public MainWindow() {
            InitializeComponent();
            //centering window
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            progressBar = FindName("progress") as ProgressBar;

            try {
                DownloadUpdate();

            }
            catch (Exception e) {
                MessageBox.Show(e.Message, "Alert", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }


        private void DownloadUpdate() {
            this.Show();
            Thread.Sleep(800);
            progressBar.Value = 0;
            try {

                WebClient webClient = new WebClient();
                System.IO.File.Delete(@".\Graph.exe");
                
                webClient.DownloadProgressChanged += (s, e) => { 
                    progressBar.Value = e.ProgressPercentage;
                };
                webClient.DownloadFileCompleted += (s, e) => {
                    Thread.Sleep(1000); //for decoration ;)
                    string zipPath = @".\Graph.zip";
                    string extractPath = @".\";
                    ZipFile.ExtractToDirectory(zipPath, extractPath);
                    System.IO.File.Delete(@".\Graph.zip");
                    this.Close();
                    Process.Start(@"Graph.exe");
                    
                    

                };
                webClient.DownloadFileAsync(new Uri("http://graphice.me/graph/25fg3v6xs42c13/Graph.zip"),
                    @".\Graph.zip");
                webClient.Dispose();

            }
            catch (Exception e) {
                MessageBox.Show(e.Message, "Alert", MessageBoxButton.OK, MessageBoxImage.Warning);
            }


        }
        

    }
}
