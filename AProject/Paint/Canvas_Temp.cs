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
    public class Canvas_Temp:Panel
    {
        public BufferedGraphics bGrp = null;
        public Canvas_Temp()
        {
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
            this.AutoScroll = true;
            this.AutoScrollMinSize = new Size(300, 300);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            bGrp = BufferedGraphicsManager.Current.Allocate(e.Graphics, ClientRectangle);
            DrawShap();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            this.Focus();
            if(e.Button == MouseButtons.Middle)
            {
                this.Cursor = Cursors.Hand;
                oldMouse = mousePoint;
            }
            if(e.Button == MouseButtons.Right)
            {
                initMatrix = new Matrix(1, 0, 0, -1, 0, 0);
                globalZoomNum = 1;
                this.Invalidate();
            }
        }

        PointF mousePoint = new PointF();
        PointF oldMouse = new PointF();
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            Matrix potTransMatris = new Matrix(1 / initMatrix.Elements[0], 0, 0, 1 / initMatrix.Elements[3], -initMatrix.Elements[4] / initMatrix.Elements[0], -initMatrix.Elements[5] / initMatrix.Elements[3]);
            PointF[] pot = new PointF[] { new PointF(e.X, e.Y) };
            potTransMatris.TransformPoints(pot);
            mousePoint = pot[0];

            if (e.Button == MouseButtons.Middle)
            {
                Matrix x = new Matrix(1, 0, 0, 1, mousePoint.X - oldMouse.X, mousePoint.Y - oldMouse.Y);

                initMatrix.Multiply(x, MatrixOrder.Prepend);
                this.Invalidate();
            }

            Console.WriteLine(string.Format("未变换,x:{0},y:{1}", e.X, e.Y));
            Console.WriteLine(string.Format("变换,x:{0},y:{1}", mousePoint.X, mousePoint.Y));
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
        }

        float scaleNum = 1;
        float globalZoomNum = 1;
        float maxZoomFactor = 200f;
        float minZoomFactor = 1 / 50f;
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            
            
            if (e.Delta > 0)
            {
                scaleNum = 1.5f;
                globalZoomNum *= 1.5f;
                if (globalZoomNum > maxZoomFactor)
                {
                    globalZoomNum = maxZoomFactor;
                    initMatrix.Elements[0] = globalZoomNum;
                    initMatrix.Elements[3] = -globalZoomNum;
                }
            }
            else
            {
                scaleNum = 0.67f;
                globalZoomNum *= 0.67f;
                if (globalZoomNum < minZoomFactor)
                {
                    globalZoomNum = minZoomFactor;
                    initMatrix.Elements[0] = globalZoomNum;
                    initMatrix.Elements[3] = -globalZoomNum;
                }
            }


            //Matrix grpTransMatrix = new Matrix(scaleNum, 0, 0, scaleNum, (1 - scaleNum) * e.X, (1 - scaleNum) * e.Y);
            if (globalZoomNum < maxZoomFactor && globalZoomNum > minZoomFactor)
            {
                Matrix grpTransMatrix = new Matrix(scaleNum, 0, 0, scaleNum, 0, 0);
                initMatrix.Multiply(grpTransMatrix, MatrixOrder.Prepend);
            }
            this.Invalidate();
        }
        public Matrix initMatrix = new Matrix(1, 0, 0, -1, 0, 0);

        float penWidth = 2f;

        public void DrawShap()
        {
            Graphics g = bGrp.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.Clear(Color.Black);
            Rectangle rectangle = this.ClientRectangle;

            var initContainer = g.BeginContainer();
            g.Transform = initMatrix;// new Matrix(1, 0, 0, -1, rectangle.Width / 2, rectangle.Height / 2);
            //g.TranslateTransform(rectangle.Width / 2, rectangle.Height / 2);
            float r = 150;
            RectangleF rect = new RectangleF(-r, -r, 2 * r, 2 * r);
            g.DrawEllipse(Pens.Red, rect);

            GraphicsContainer drawContainer = g.BeginContainer();
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            penWidth = 2/globalZoomNum;
            Pen pen = new Pen(Color.White, penWidth);
                      
            g.TranslateTransform(r / 2, 0);
            g.DrawEllipse(pen, -r / 2, -r / 2, r, r);
            g.EndContainer(drawContainer);

            drawContainer = g.BeginContainer();
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.TranslateTransform(-r / 2, 0);
            g.DrawEllipse(Pens.Yellow, -r / 2, -r / 2, r, r);
            g.EndContainer(drawContainer);

            drawContainer = g.BeginContainer();
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.TranslateTransform(0, -r / 2);
            g.DrawEllipse(Pens.Blue, -r / 2, -r / 2, r, r);
            g.EndContainer(drawContainer);

            drawContainer = g.BeginContainer();
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.TranslateTransform(0, r / 2);
            g.DrawEllipse(Pens.Purple, -r / 2, -r / 2, r, r);
            g.EndContainer(drawContainer);

            g.EndContainer(initContainer);
            g.DrawRectangle(Pens.Red, 0, 0, 50, 50);
            bGrp.Render();
        }
    }
}
