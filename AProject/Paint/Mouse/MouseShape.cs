using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paint.Mouse
{
    public enum MouseStyle
    {
        None,
        Normal,
        Draw,
        Edit,
        Point
    }

    public class MouseShape
    {
        public static MouseStyle MouseStyle { get; set; }
        public static MouseStyle LastMouseStyle { get; set; }

        public static PointF OldPoint { get; set; }
        public static PointF CurrentPoint { get; set; }

        public static RectangleF HitRect { get; set; }

        public static void Draw(Graphics g)
        {
            g.TranslateTransform(CurrentPoint.X * Canvas.GlobalZoomNum, CurrentPoint.Y * Canvas.GlobalZoomNum);
            MouseShape.MouseStyle = MouseStyle.Normal;
            switch (MouseStyle)
            {
                case MouseStyle.Normal:
                    g.DrawLine(Pens.White, -40, 0, 40, 0);
                    g.DrawLine(Pens.White, 0, -40, 0, 40);
                    g.DrawRectangle(Pens.White, -3, -3, 6, 6);
                    break;
                case MouseStyle.Draw:
                    g.DrawLine(Pens.White, -40, 0, 40, 0);
                    g.DrawLine(Pens.White, 0, -40, 0, 40);
                    break;
                case MouseStyle.Edit:
                    g.DrawRectangle(Pens.Red, -3, -3, 6, 6);
                    break;
                case MouseStyle.None:
                    break;
                case MouseStyle.Point:
                    g.DrawEllipse(Pens.White, -3, -3, 6, 6);
                    break;
            }
        }
    }
}
