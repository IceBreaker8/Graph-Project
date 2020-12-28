using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows;

namespace Graph.Updater {


    public class UpdateChecker {

        public static string version = "0.0.1";
        public static string VersionLink = "https://raw.githubusercontent.com/IceBreaker8" +
            "/graphICE-website/main/version.JSON?token=AOISHLE2YQI3NT2ZFS53UXK75MLQO";

        public bool CheckForUpdate(bool verif) {
            try {
                var json = new WebClient().
                    DownloadString(VersionLink);
                var JSONObj = new JavaScriptSerializer().Deserialize<Dictionary<string, string>>(json);


                string currentVersion = JSONObj["version"];
                if (currentVersion != version) { // need to update 
                    new UpdateWindow(verif);
                    return true;
                }
            }
            catch (Exception) {
                MessageBox.Show("Connection error!", "Alert"
                    , MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            return false;
        }

    }
}
