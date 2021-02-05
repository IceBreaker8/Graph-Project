using System;
using System.IO.Compression;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;

namespace Graph.Updater {
    class UpdateDownloader {


        public UpdateDownloader() {
            _ = Hold();
            StartUpdate();
            
        }

        private void StartUpdate() {
            Process.Start("Updater.exe");
        }
        public async Task Hold() {
            await Task.Delay(1000);
        }
    }
}
