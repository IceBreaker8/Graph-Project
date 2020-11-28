﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace templateGraph {
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class InputBox : Window {

        public InputBox(string question, string defaultAnswer = "") {
            InitializeComponent();
            lblQuestion.Content = question;
            txtAnswer.Text = defaultAnswer;
            txtAnswer.Focus();

        }

        private void btnDialogOk_Click(object sender, RoutedEventArgs e) {
            this.DialogResult = true;
        }

        private void Window_ContentRendered(object sender, EventArgs e) {
            txtAnswer.SelectAll();
        }

        public string Answer {
            get {
                return txtAnswer.Text;
            }
        }
    }
}
