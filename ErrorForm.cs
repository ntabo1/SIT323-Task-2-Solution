using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SIT323AssessmentTask2
{
    public partial class ErrorForm : Form
    {
        public ErrorForm()
        {
            InitializeComponent();
        }

        private void ErrorForm_Load(object sender, EventArgs e)
        {
            List<string> errors = SIT323AssessmentTask2Form.CompleteErrorList;

            StringBuilder strBuild = new StringBuilder();
            foreach(string str in errors)
            {
                strBuild.Append(str);
                strBuild.Append("\n\n");
            }
            string errorText = strBuild.ToString();
            label1.Text = errorText;
        }

    }
}
