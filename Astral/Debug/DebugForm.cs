﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Astral.Debug
{
    public partial class DebugForm : Form
    {
        public DebugForm()
        {
            InitializeComponent();
            this.Update();
        }

        public void SetImage(Bitmap image)
        {
            PictureBox.Invoke(() =>
            {
                PictureBox.Image = image;
                PictureBox.Refresh();
            });
        }
    }
}
