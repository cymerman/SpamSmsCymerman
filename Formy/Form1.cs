using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpamSmsLicencjat
{
    public partial class StartForm : Form
    {
        public StartForm()
        {
            InitializeComponent();
        }

        private void naiveBayesButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            NaiveBayesForm nb = new NaiveBayesForm();
            nb.ShowDialog();
        }

        private void logisticRegressionButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            LogisticRegression lr = new LogisticRegression();
            lr.ShowDialog();
        }
    }
}
