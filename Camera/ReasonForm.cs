using Emgu.CV;
using Emgu.CV.Structure;
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
    public partial class ReasonForm : MetroForm
    {
        public ReasonForm(string goOCG, string leaveOCG, Image<Bgr, byte> GoFull, Image<Bgr, byte> LeaveFull, Image<Bgr,byte> goAvatar, Image<Bgr,byte> leaveAvatar, Image<Bgr,byte> goPlate, Image<Bgr,byte> leavePlate)
        {
            InitializeComponent();
        }
    }
}
