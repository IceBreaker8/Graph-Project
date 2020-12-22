using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Graph.Updater {
    class UpdateWindow {


        private short DelayTime = 1;
        public UpdateWindow() {
            _ = Delay();
        }

        private async Task Delay() {
            try {
                await Task.Delay(DelayTime * 1000);
                MessageBoxResult result = MessageBox.Show("There is a new version available for download, would you like to download it now?", "Update"
                    , MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (result == MessageBoxResult.Yes) {
                    //redirect link
                    System.Diagnostics.Process.Start("http://graphice.me");
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
