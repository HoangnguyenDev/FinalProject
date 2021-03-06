﻿using Emgu.CV;
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
        public ReasonForm(string GoOCG, string LeaveOCG, Image<Bgr, byte> GoFull, Image<Bgr, byte> LeaveFull, Image<Bgr,byte> GoAvatar, Image<Bgr,byte> LeaveAvatar, Image<Bgr,byte> GoPlate, Image<Bgr,byte> LeavePlate)
        {
            InitializeComponent();
            if(LeaveAvatar!= null)
                img_Face_Out.Image = LeaveAvatar.Bitmap;
            if(LeaveFull !=null)
                img_Full_Out.Image = LeaveFull.Bitmap;
            if(LeavePlate !=null)
                img_Plate_Out.Image = LeavePlate.Bitmap;
            lXeRa.Text = LeaveOCG;

            lXeVao.Text = GoOCG;
            if(GoAvatar !=null)
                img_Face_In.Image = GoAvatar.Bitmap;
            if(GoFull != null)
                img_Full_In.Image = GoFull.Bitmap;
            if(GoPlate.Bitmap != null)
                img_Plate_In.Image = GoPlate.Bitmap;

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

        }
    }
}
