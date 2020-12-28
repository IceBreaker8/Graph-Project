using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Graph.Updater {
    class UpdateWindow {

        
        private short DelayTime = 1;
        public UpdateWindow(bool verif) {
            if (verif)
                _ = Delay(0);
            else
                _ = Delay(DelayTime);

        }

        private async Task Delay(short delay) {
            try {
                await Task.Delay(delay * 1000);
                MessageBoxResult result = 
                    MessageBox.Show("There is a new version available for download, would you like to download it now?", "Update"
                    , MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (result == MessageBoxResult.Yes) {
                    //redirect link
                    new UpdateDownloader();
                    System.Windows.Application.Current.Shutdown();
                    Process.GetCurrentProcess().Kill();
                    
                    
                    // close app
                }
                else if (result == MessageBoxResult.No) {
                    //cancel
                }
            }
            catch (Exception e) {
                MessageBox.Show(e.Message, "Alert"
                    , MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }
    }
}
