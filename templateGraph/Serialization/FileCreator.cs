using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Graph.Utils {
    class FileCreator {


        public static string path;
        public static void LoadDirAndCreateFile() {


            try {


                SaveFileDialog saveFileDialog = new SaveFileDialog();
                if (saveFileDialog.ShowDialog() == DialogResult.OK) {
                    path = saveFileDialog.FileName;
                }

            }
            catch (Exception e) {
                MessageBox.Show(e.Message);
            }

        }

        public static void LoadFile() {
            
            var filePath = string.Empty;
            using (OpenFileDialog openFileDialog = new OpenFileDialog()) {
                
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = false;

                if (openFileDialog.ShowDialog() == DialogResult.OK) {
                    //Get the path of specified file
                    path = openFileDialog.FileName;

                    //Read the contents of the file into a stream
                    var fileStream = openFileDialog.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream)) {
                        //FILE CONTENT HERE TODO
                    }
                }
            }
        }
    }
}
