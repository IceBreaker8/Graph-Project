using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using templateGraph;

namespace Graph.Updater {
    class UpdateWindow {


        public static short DelayTime = 1;
        
        public static void RunUpdateWindow(bool verif) {
            _ = Delay(verif ? 0 : DelayTime);
        }
        public static async Task Delay(int delay) {
            try {
                await Task.Delay(delay * 1000);
                MessageBoxResult result =
                    MessageBox.Show("There is a new version available for download, would you like to download it now?", "Update"
                    , MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (result == MessageBoxResult.Yes) {
                    //redirect link
                    UpdateExtractor ue = new UpdateExtractor();
                    ue.Show();

                } else if (result == MessageBoxResult.No) {
                    //cancel
                }
            } catch (Exception e) {
                MessageBox.Show(e.Message, "Alert"
                    , MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }
    }
}
