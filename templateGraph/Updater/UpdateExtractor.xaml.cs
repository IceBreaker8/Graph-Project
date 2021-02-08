using System;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;


namespace Graph.Updater {
    /// <summary>
    /// Interaction logic for UpdateExtractor.xaml
    /// </summary>
    public partial class UpdateExtractor : Window {
        

        ProgressBar progressBar;

        public UpdateExtractor() {
            InitializeComponent();
            //centering window
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            progressBar = FindName("progress") as ProgressBar;

            try {
                DownloadUpdate();

            } catch (Exception e) {
                MessageBox.Show(e.Message, "Alert", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }


        private void DownloadUpdate() {
            this.Show();
            Thread.Sleep(800);
            progressBar.Value = 0;
            try {

                WebClient webClient = new WebClient();
                webClient.DownloadFileAsync(new Uri("http://graphice.me/graph/25fg3v6xs42c13/Graph.zip"),
                    @".\Graph.zip");
                webClient.DownloadProgressChanged += (s, e) => {
                    progressBar.Value = e.ProgressPercentage;
                };
                webClient.DownloadFileCompleted += (s, e) => {
                    Thread.Sleep(1000);
                    new UpdateDownloader();

                    System.Windows.Application.Current.Shutdown();
                    Process.GetCurrentProcess().Kill();



                };
                
                webClient.Dispose();

            } catch (Exception e) {
                MessageBox.Show(e.Message, "Alert", MessageBoxButton.OK, MessageBoxImage.Warning);
            }


        }
    }
}
