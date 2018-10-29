using Paint.Mouse;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Paint
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void canvas1_MouseMove(object sender, MouseEventArgs e)
        {
            statusPoint.Text = string.Format("{0},{1}", MouseShape.CurrentPoint.X, MouseShape.CurrentPoint.Y);
        }
    }
}
