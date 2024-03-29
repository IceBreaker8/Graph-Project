﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows;

namespace Graph.Updater {


    public class UpdateChecker {

        public static string mainLink = "https://icebreaker8.github.io/graphICE-website/";

        public static string version = "0.1.3";
        public static string VersionLink = mainLink + "graph/25fg3v6xs42c13/version.JSON";

        public static bool CheckForUpdate(bool verif) {
            try {

                using (var web = new WebClient()) {

                    var json = web.
                        DownloadString(VersionLink);
                    var JSONObj = new JavaScriptSerializer().Deserialize<Dictionary<string, string>>(json);


                    string currentVersion = JSONObj["version"];
                    if (currentVersion != version) { // need to update 
                        UpdateWindow.RunUpdateWindow(verif);
                        return true;
                    }
                }



            } catch (Exception) {
                //MessageBox.Show("Connection error, Couldn't search for updates!", "Alert"
                //  , MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            return false;
        }

    }
}
