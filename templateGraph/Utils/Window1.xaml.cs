using Graph.Updater;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Graph.Utils {
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window {

        private readonly string CompanyName = "IceDev";
        private readonly string DevName = "Frikha Ahmad";
        private readonly string gmail = "graphice.company@gmail.com";


        public Window1() {
            InitializeComponent();

            //centering window
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

            Version = this.FindName("Version") as TextBlock;
            Company = this.FindName("Company") as TextBlock;
            Developer = this.FindName("Developer") as TextBlock;
            Email = this.FindName("Email") as TextBlock;

            Version.Text = UpdateChecker.version + "v";
            Company.Text = CompanyName;
            Developer.Text = DevName;
            Email.Text = gmail;

        }
    }
}
