using Paint.AffineTransform;
using Paint.Mouse;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Paint
{
    public class Canvas:Panel
    {
        public Canvas()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
            
        }

        private static float globalZoomNum = 1;

        public static float GlobalZoomNum
        {
            get { return globalZoomNum; }
            set { globalZoomNum = value; }
        }

        private static float scalNum = 1.5f;

        public static float ScalNum
        {
            get { return scalNum; }
            set { scalNum = value; }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            DrawShape(e.Graphics);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            this.Focus();
            if(e.Button == MouseButtons.Middle)
            {
                this.Cursor = Cursors.Hand;
                MouseShape.MouseStyle = MouseStyle.None;
                MouseShape.OldPoint = MouseShape.CurrentPoint;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            MouseShape.CurrentPoint = UserMatrix.GetMousePoint(e.X, e.Y);
            //HitRect 


            //SnapHitRect


            if(e.Button == MouseButtons.Middle)
            {
                MouseShape.MouseStyle = MouseStyle.None;
                Matrix matrix = new Matrix(1, 0, 0, 1, MouseShape.CurrentPoint.X - MouseShape.OldPoint.X, MouseShape.CurrentPoint.Y - MouseShape.OldPoint.Y);
                UserMatrix.initMatrix.Multiply(matrix, MatrixOrder.Prepend);
            }
            this.Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if(e.Button == MouseButtons.Middle)
            {
                this.Cursor = new Cursor("CursorBlank.cur");
                MouseShape.MouseStyle = MouseShape.LastMouseStyle;
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            if(e.Delta > 0)
            {
                ScalNum = 1.5f;
                GlobalZoomNum *= ScalNum;
            }
            else
            {
                ScalNum = 0.67f;
                GlobalZoomNum *= ScalNum;
            }

            UserMatrix.ScaleByPoint(ScalNum, MouseShape.CurrentPoint);
            this.Invalidate();
        }

        void DrawShape(Graphics g)
        {
            g.Clear(Color.Transparent);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            g.Transform = UserMatrix.initMatrix;

            GraphicsContainer drawContainer = g.BeginContainer();
            g.ScaleTransform(1 / GlobalZoomNum, 1 / GlobalZoomNum);

            CoordinateShape.Draw(g);
            MouseShape.Draw(g);
            g.EndContainer(drawContainer);
        }
    }
}
