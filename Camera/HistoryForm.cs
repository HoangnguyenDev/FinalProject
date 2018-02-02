using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Camera
{
    public partial class HistoryForm : MetroForm
    {
        public HistoryForm()
        {
            InitializeComponent();
        }

        private void HistoryForm_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'admin_dangkythitoeicDataSet.GoLeave' table. You can move, or remove it, as needed.
            this.goLeaveTableAdapter.Fill(this.admin_dangkythitoeicDataSet.GoLeave);

        }
    }
}
